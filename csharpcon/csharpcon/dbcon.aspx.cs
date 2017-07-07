using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace csharpcon
{
    public partial class dbcon : System.Web.UI.Page
    {


        protected void Button1_Click(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //string constr = "DRIVER={MySQL ODBC 5.3 UNICODE Driver}; Server=seal-db1.mysql.database.azure.com; Port=3306; Database=employees; Uid=sean@seal-db1; Pwd=Insignia1103; sslverify=0; Option=3;";
            //string constr = "Server=seal-db1.mysql.database.azure.com;Uid=sean@seal-db1; Pwd=Insignia1103;";


            MySql.Data.MySqlClient.MySqlConnection conn = null;
            string myConnectionString;
            myConnectionString = "Server=seal-db1.mysql.database.azure.com;Uid=sean@seal-db1; Pwd=Insignia1103;";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            using (MySqlCommand cmd = new MySqlCommand("select * from employees.employees limit 0,100"))
            {
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = conn;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
            conn.Close();


        }
    }
}