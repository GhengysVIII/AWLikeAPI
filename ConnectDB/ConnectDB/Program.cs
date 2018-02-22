using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ToolBox.DataAccess.Database;

namespace ConnectDB
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ConnectionString = @"Server=DESKTOP-G62985H\TFTIC;Database=Ado;Trusted_Connection=True;";

            Contact NewContact = new Contact() { Nom = "Strimelle", Prenom = "Aurélien" };

            Connection c = new Connection(ConnectionString);
            Command cmd = new Command("select ID, Nom, Prenom, DateNaiss from V_Contact where ID = @ID;");
            cmd.AddParameter("ID", 9);

            foreach (Contact co in c.ExecuteReader(cmd, (dr) => new Contact() { ID = (int)dr["ID"], Nom = (string)dr["Nom"], Prenom = (string)dr["Prenom"], DateNaiss = (dr["DateNaiss"] is DBNull) ? null : (DateTime?)dr["DateNaiss"] }))
            {
                Console.WriteLine($"{co.Nom} {co.Prenom}");
            }

            Console.ReadLine();
        }
    }
}
