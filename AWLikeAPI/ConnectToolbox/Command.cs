using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Connect
{
    public class Command
    {
        internal IDictionary<string, object> Parameters { get; private set; }
        internal string Query { get; private set; }
        internal bool IsStoredProcedure { get; private set; }

        public Command(string Query) : this(Query, false)
        {

        }

        public Command(string Query, bool IsStoredProcedure)
        {
            if (string.IsNullOrWhiteSpace(Query))
                throw new ArgumentException("Query can't be empty or null");

            Parameters = new Dictionary<string, object>();
            this.Query = Query;
            this.IsStoredProcedure = IsStoredProcedure;
        }

        public void AddParameter(string ParameterName, object Value)
        {
            Parameters.Add(ParameterName, Value);
        }
    }
}
