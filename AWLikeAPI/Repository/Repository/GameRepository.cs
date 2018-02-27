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

            return (Db.ExecuteNonQuery(cmd)>0);
        }

        public GamePOCO Get(int Id)
        {
            Command cmd = new Command($"SELECT * FROM {TableName} WHERE Id = @Id");

            cmd.AddParameter("Id", Id);

            return Db.ExecuteReader(cmd, Selector).SingleOrDefault();
        }

        
        public IEnumerable<GamePOCO> GetAllGameInfoAvailable()
        {
            Command cmd = new Command($"select g.Id, u.Username, g.Name, g.MapID from Game as g join Rel_User_Game as r on g.Id = r.Ref_Game join [User] as u on r.Ref_User = u.Id where g.InGame = 0");

            List<GamePOCO> list = new List<GamePOCO>();

            foreach (dynamic game in Db.ExecuteReader(cmd, Selector))
            {
                var elem = list.FirstOrDefault(item => item.Id == game.Id);
                if (elem == null)
                {
                    GamePOCO g = new GamePOCO {  };
                }
                else
                {
                    elem.UserList.Add(new UserPOCO { });
                }
                //list.Add(game);
            }

            return list;
        }
    }
}
