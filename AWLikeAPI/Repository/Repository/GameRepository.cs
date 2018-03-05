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

        public IEnumerable<GamePOCO> GetAllGameIn(int UserID)
        {
            Command cmd = new Command(@"SELECT G.Id as GameID, g.UserTurnNumb ,
                                          gameIn.NumberOfPlayer, G.Name as GameName, 
                                          G.MapID, G.InGame, U.Id as UserID,U.Username, 
                                          r.Ref_TurnPosition as TurnPosition FROM Game as g
                                        INNER JOIN(SELECT r.Ref_Game, count(*) as NumberOfPlayer FROM Game as g
                                            INNER JOIN Rel_User_Game as r ON g.Id = r.Ref_Game
                                            WHERE g.InGame = @UserID 
                                            GROUP BY R.Ref_Game) as gameIn on gameIn.Ref_Game = g.Id
                                        INNER JOIN Rel_User_Game as r ON g.Id = r.Ref_Game
                                        INNER JOIN[User] as u ON u.Id = r.Ref_User");
            cmd.AddParameter("UserID", UserID);
            List<GamePOCO> list = new List<GamePOCO>();

            foreach (SqlDataReader gamePlayerDBItem in Db.ExecuteReader(cmd,(x)=>x))
            {
                GamePOCO item = list.FirstOrDefault((x) => x.Id == (int)gamePlayerDBItem["GameId"]);
                if (item == null)
                {
                    item = new GamePOCO()
                    {
                        Id = (int)gamePlayerDBItem["GameId"],
                        MapID = (int)gamePlayerDBItem["MapID"],
                        Name = gamePlayerDBItem["GameName"].ToString(),
                        NumberOfPlayers = (int)gamePlayerDBItem["NumberOfPlayer"],
                        UserList = new List<UserPOCO>(),
                        UserTurnNumb = (int)gamePlayerDBItem["UserTurnNumb"]
                    };

                    list.Add(item);
                }

                item.UserList.Add(new UserPOCO()
                {
                    Id = (int)gamePlayerDBItem["UserID"],
                    Username = gamePlayerDBItem["UserID"].ToString(),
                    TurnPosition = (int)gamePlayerDBItem["TurnPosition"]

                });


            }

            return list;

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

            foreach (SqlDataReader gamePlayerDBItem in Db.ExecuteReader(cmd, (data) => { return data; }))
            {
                GamePOCO Item = list.FirstOrDefault(item => item.Id == (int)gamePlayerDBItem["GameId"]);
                if (Item == null)
                {
                    Item = new GamePOCO
                    {
                        Id = (int)gamePlayerDBItem["GameId"],
                        Name = ((string)gamePlayerDBItem["GameName"]).ToString(),
                        MapID = (int)gamePlayerDBItem["MapID"],
                        UserList = new List<UserPOCO>(),
                        NumberOfPlayers = gamePlayerDBItem["NumberOfPlayer"] == DBNull.Value ? 0 : (int)gamePlayerDBItem["NumberOfPlayer"]

                    };
                    list.Add(Item);
                    
                }

                if (gamePlayerDBItem["UserID"] != DBNull.Value)
                {
                    Item.UserList.Add(new UserPOCO()
                    {
                        Id = (int)gamePlayerDBItem["UserID"],
                        Username = (string)gamePlayerDBItem["Username"],
                        Email = "",
                        Password = "",

                    });

                }

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
