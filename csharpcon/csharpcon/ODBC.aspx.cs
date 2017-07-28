using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace csharpcon
{
    public partial class ODBC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string myConnectionString;
              myConnectionString = "DRIVER={MySQL ODBC 5.3 unicode Driver}; Server=seal-db1.mysql.database.azure.com; Port=3306; Database=employees; Uid=sean@seal-db1; Pwd=Password1; sslverify=0; Option=3;";
          //  myConnectionString = "DNS=sysdsn; Uid=sean@seal-db1; Pwd=Insignia1103; sslverify=0; Option=3;";
            OdbcConnection MyConnection = null;
            try
            {
                MyConnection = new OdbcConnection(myConnectionString);
                MyConnection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            OdbcDataAdapter adapter = new OdbcDataAdapter("select * from employees.employees limit 0,100", MyConnection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            GridView1.DataSource = dataSet.Tables[0];
            GridView1.DataBind();
            
            MyConnection.Close();
        }
    }
}
