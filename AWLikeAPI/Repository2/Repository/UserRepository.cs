using AWLike.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.BaseRepository;
using ToolBox.Connect;

namespace NetFlax.DAL.Repository
{
    public class UserRepository : BaseRepository<int, UserPOCO>
    {
        #region Singleton
        private static UserRepository _Instance;
        public static UserRepository Instance
        {
            get { return _Instance ?? (_Instance = new UserRepository()); }
        }
        #endregion

        private UserRepository() : base("User")
        { }

        protected override UserPOCO Selector(SqlDataReader Data)
        {
            return new UserPOCO()
            {
                Id = (int)Data["Id"],
                Username = Data["Username"].ToString(),
                Password = Data["Password"].ToString(),
                Email = Data["Email"].ToString()
            };
        }

        public override int Insert(UserPOCO Entity)
        {
            Command cmd = new Command($"INSERT INTO {TableName}(Mail, Username, Password) OUTPUT inserted.id VALUES(@Mail, @Username, @Password)");
            cmd.AddParameter("Email", Entity.Email);
            cmd.AddParameter("Username", Entity.Username);
            cmd.AddParameter("Password", Entity.Password);

            return (int)Db.ExecuteScalar(cmd);
        }

        public override bool Update(UserPOCO Entity)
        {
            Command cmd = new Command($"UPDATE {TableName} set (Mail= @Email , Username= @Username, Password= @Password) where Id = @Id ");
            cmd.AddParameter("Id", Entity.Id);
            cmd.AddParameter("Email", Entity.Email);
            cmd.AddParameter("Username", Entity.Username);
            cmd.AddParameter("Password", Entity.Password);

            return (Db.ExecuteNonQuery(cmd)>0);
        }

        public UserPOCO Get(string Username, string Password)
        {
            Command cmd = new Command($"SELECT * FROM {TableName} WHERE (Email LIKE @Username OR Username LIKE @Username) AND Pwd LIKE @Password;");

            cmd.AddParameter("Username", Username);
            cmd.AddParameter("Password", Password);

            return Db.ExecuteReader(cmd, Selector).SingleOrDefault();
        }
    }
}
