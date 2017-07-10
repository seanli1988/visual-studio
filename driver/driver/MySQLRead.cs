using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Data.Odbc;


namespace driver
{
    class MySQLRead
    {
        // Obtain connection string information from the portal
        //

        static void Main(string[] args)
        {

            var conn = new OdbcConnection("DRIVER={MySQL ODBC 5.3 unicode Driver}; Server=seal-db1.mysql.database.azure.com; Port=3306; Database=employees; Uid=sean@seal-db1; Pwd=Insignia1103; sslverify=0; Option=3;MULTI_STATEMENTS=1");


            Console.Out.WriteLine("Opening connection");
            conn.Open();

            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM inventory;";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(
                    string.Format(
                        "Reading from table=({0}, {1}, {2})",
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString()
                        )
                    );
            }

            Console.Out.WriteLine("Closing connection");
            conn.Close();

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
    }
}

