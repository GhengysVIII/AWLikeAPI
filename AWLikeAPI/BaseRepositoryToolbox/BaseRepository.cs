using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ToolBox.Connect;

namespace Toolbox.BaseRepository
{    
    public abstract class BaseRepository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        private const string _ConnectionString = @"Server=DESKTOP-ULK5KPB\SQLEXPRESS;Database=AWLike;User ID=AWLike;Password=AWLike;";
        private string _TableName;
        private Connection _Db;

        public string ConnectionString
        {
            get { return _ConnectionString; }
        }

        public string TableName
        {
            get { return _TableName; }
            private set { _TableName = value; }
        }

        public Connection Db
        {
            get { return _Db; }
            private set { _Db = value; }
        }

        public BaseRepository(string TableName)
        {
            this.TableName = "[" + TableName + "]";
            this.Db = new Connection(ConnectionString);
        }

        public virtual bool Delete(TKey Id)
        {
            Command cmd = new Command($"DELETE FROM {TableName} WHERE Id = @Id");
            cmd.AddParameter("Id", Id);

            int nbRow = Db.ExecuteNonQuery(cmd);
            return nbRow == 1;
        }

        public virtual bool Delete(TEntity Entity)
        {
            return Delete(Entity.Id);
        }

        protected abstract TEntity Selector(SqlDataReader Data);

        public virtual TEntity Get(TKey Id)
        {
            Command cmd = new Command($"SELECT * FROM {TableName} WHERE Id = @Id");
            cmd.AddParameter("Id", Id);

            return Db.ExecuteReader(cmd, Selector).SingleOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            Command cmd = new Command($"SELECT * FROM {TableName}");

            return Db.ExecuteReader(cmd, Selector);
        }

        public abstract TKey Insert(TEntity Entity);

        public abstract bool Update(TEntity Entity);
    }
}
