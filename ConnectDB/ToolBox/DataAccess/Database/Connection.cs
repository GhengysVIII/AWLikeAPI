using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.DataAccess.Database
{
    public class Connection
    {
        private string _ConnectionString;

        protected string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        public Connection(string ConnectionString)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ArgumentException("Connection can't be null or empty!");

            this.ConnectionString = ConnectionString;

            using (SqlConnection c = CreateConnection())
            {
                c.Open();
            }
        }

        public object ExecuteScalar(Command Command)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    object o = cmd.ExecuteScalar();
                    return (o is DBNull) ? null : o;
                }
            }
        }

        public int ExecuteNonQuery(Command Command)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command Command, Func<SqlDataReader, TResult> Selector)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            yield return Selector(reader);
                        }
                    }
                }
            }
        }

        private SqlCommand CreateCommand(Command Command, SqlConnection Connection)
        {
            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandText = Command.Query;
            cmd.CommandType = (Command.IsStoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;

            foreach (KeyValuePair<string, object> kvp in Command.Parameters)
            {
                SqlParameter Parameter = new SqlParameter();
                Parameter.ParameterName = kvp.Key;
                Parameter.Value = (kvp.Value == null) ? DBNull.Value : kvp.Value;

                cmd.Parameters.Add(Parameter);
            }

            return cmd;
        }

        private SqlConnection CreateConnection()
        {
            SqlConnection c = new SqlConnection();
            c.ConnectionString = ConnectionString;

            return c;
        }        
    }
}
