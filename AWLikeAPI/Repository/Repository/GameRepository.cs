using AWLike.DAL.Entity;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.BaseRepository;
using ToolBox.Connect;

namespace AWLike.Repository
{
    public class GameRepository : BaseRepository<int, GamePOCO>
    {
        #region Singleton
        private static GameRepository _Instance;
        public static GameRepository Instance
        {
            get { return _Instance ?? (_Instance = new GameRepository()); }
        }
        #endregion

        private GameRepository() : base("Game")
        { }

        protected override GamePOCO Selector(SqlDataReader Data)
        {
            return new GamePOCO()
            {
                Id = (int)Data["Id"],
                Name = Data["Name"].ToString(),
                MapID = (int)Data["MapID"],
                UserTurnNumb = (int)Data["UserTurnNumb"]
            };
        }

        public override int Insert(GamePOCO Entity)
        {
            Command cmd = new Command($"INSERT INTO {TableName}(Name, MapID, UserTurnNumb) OUTPUT inserted.id VALUES(@Name, @MapID, @UserTurnNumb)");
            cmd.AddParameter("Name", Entity.Name);
            cmd.AddParameter("MapID", Entity.MapID);
            cmd.AddParameter("UserTurnNumb", Entity.UserTurnNumb);

            return (int)Db.ExecuteScalar(cmd);
        }

        public override bool Update(GamePOCO Entity)
        {
            Command cmd = new Command($"UPDATE {TableName} set (Name= @Name , MapID= @MapID, UserTurnNumb= @UserTurnNumb) where Id = @Id ");
            cmd.AddParameter("Id", Entity.Id);
            cmd.AddParameter("Name", Entity.Name);
            cmd.AddParameter("MapID", Entity.MapID);
            cmd.AddParameter("UserTurnNumb", Entity.UserTurnNumb);

            return (Db.ExecuteNonQuery(cmd) > 0);
        }

        public override GamePOCO Get(int Id)
        {
            Command cmd = new Command($"SELECT * FROM {TableName} WHERE Id = @Id");

            cmd.AddParameter("Id", Id);

            return Db.ExecuteReader(cmd, Selector).SingleOrDefault();
        }


        public IEnumerable<GamePOCO> GetAllGameInfoAvailable()
        {
            Command cmd = new Command(@"SELECT G.Id as GameID,J.NumberOfPlayer, G.Name as GameName, G.MapID, G.InGame, U.Id as UserID,U.Username FROM Game AS G LEFT JOIN
                                        (SELECT R.Ref_Game, COUNT(*) as NumberOfPlayer
                                        FROM Rel_User_Game as R
                                        GROUP bY R.Ref_Game) as J ON J.Ref_Game = G.Id
                                        LEFT JOIN Rel_User_Game as R ON G.Id = R.Ref_Game
                                        LEFT JOIN[User] as U ON R.Ref_User = U.Id
                                        where G.InGame = 0
                                        Order By GameID");

            List<GamePOCO> list = new List<GamePOCO>();

            foreach (dynamic gamePlayerDBItem in Db.ExecuteReader(cmd, (data) => { return data; }))
            {
                GamePOCO Item = list.FirstOrDefault(item => item.Id == (int)gamePlayerDBItem["GameId"]);
                if (Item == null)
                {
                    GamePOCO g = new GamePOCO
                    {
                        Id = gamePlayerDBItem["GameId"],
                        Name = ((string)gamePlayerDBItem["GameName"]).ToString(),
                        MapID = (int)gamePlayerDBItem["MapID"],
                        UserList = new List<UserPOCO>(),
                        NumberOfPlayer = (int)gamePlayerDBItem["NumberOfPlayer"]

                    };
                    Item = g;
                    list.Add(g);
                    
                }

                Item.UserList.Add(new UserPOCO() {
                    Id = gamePlayerDBItem["UserID"],
                    Username = gamePlayerDBItem["Username"],
                    Email = "",
                    Password = "",
                    
                });
                
                //var ItemList = list.FirstOrDefault(item => item.Id == gamePlayerDBItem.Id);
                //if (ItemList == null)
                //{
                //    GamePOCO g = new GamePOCO { };
                //}
                //else
                //{
                //    ItemList.UserList.Add(new UserPOCO { });
                //}
                ////list.Add(game);
            }

            return list;
        }
    }
}
