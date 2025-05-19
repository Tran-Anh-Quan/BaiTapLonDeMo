using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLonDeMo.Connect
{
    public class ConnectSQL
    {
        public  static SqlConnection ConnectDB()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QLHD;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
