using System.Web.Services;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Configuration;

namespace WebService
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://xgtechproxyuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        public static string connectionStringManager = ConfigurationManager.ConnectionStrings["DBConnectString"].ToString();
        //public static string connectionStringManager = "connectionString=\"Server=localhost;Database=test;Uid=root;Pwd=root;charset=utf8\" providerName=\"MySql.Data.MySqlClient\"";
        /// <summary>
        /// a valid database connectionstring
        /// </summary>
        public static string ConnectionStringManager
        {
            get { return connectionStringManager; }
        }

        //<add name="DBConnectString" connectionString="Server=192.168.1.2;Database=dbname;Uid=root;Pwd=111;charset=utf8" providerName="MySql.Data.MySqlClient"/>
        [WebMethod]
        public int HttpToDb(string ip,string port)
        {
            int iret = 0;
            MySqlCommand sqlcom = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(ConnectionStringManager);
            conn.Open();
            try
            {
                sqlcom.CommandText = "INSERT INTO Thttp(IP,Port)" +
                        " VALUES(@IP,@Port);";
                MySqlParameter[] commandParameters = new MySqlParameter[]{
            new MySqlParameter("@IP",ip),
            new MySqlParameter("@Port",port)
          
                };
                iret = MySqlHelper.ExecuteNonQuery(conn, sqlcom.CommandText, commandParameters);
            }
            catch
            {
                iret = -1;
            }
             conn.Close();
             return iret;
          
        }

        [WebMethod]
        public int DelHttp(string ip, string port)
        {
            int iret = 0;
            if (ip == string.Empty || port == string.Empty)
                return iret;
            MySqlCommand sqlcom = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(ConnectionStringManager);
            conn.Open();
            try
            {
                
                sqlcom.CommandText = "Delete From Thttp where IP = @IP,Port=@Port;";
                MySqlParameter[] commandParameters = new MySqlParameter[]{
            new MySqlParameter("@IP",ip),
            new MySqlParameter("@Port",port)
          
                };
                iret = MySqlHelper.ExecuteNonQuery(conn, sqlcom.CommandText, commandParameters);
            }
            catch
            {
                iret = -1;
            }
            conn.Close();
            return iret;

        }
        [WebMethod]
        public int Socks5ToDb(string ip,string port,string name,string pwd)
        {
            int iret = 0;
            MySqlCommand sqlcom = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(ConnectionStringManager);
            conn.Open();
            try
            {
              
                sqlcom.CommandText = "INSERT INTO TSocks(IP,Port,UName,UPwd)" +
                        " VALUES(@IP,@Port,@UName,@UPwd);";
                MySqlParameter[] commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@IP",ip),
                    new MySqlParameter("@Port",port),
                    new MySqlParameter("@UName",name),
                    new MySqlParameter("@UPwd",pwd)
                };
                iret = MySqlHelper.ExecuteNonQuery(conn, sqlcom.CommandText, commandParameters);
            }
            catch
            {
                iret = -1;

            }

            conn.Close();
            return iret;
        }

        [WebMethod]
        public int DelSocks5(string ip, string port)
        {
            int iret = 0;
            if (ip == string.Empty || port == string.Empty)
                return iret;
            MySqlCommand sqlcom = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(ConnectionStringManager);
            conn.Open();
            try
            {

                sqlcom.CommandText = "DELETE From TSocks where IP = @IP,Port=@Port;";
                MySqlParameter[] commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@IP",ip),
                    new MySqlParameter("@Port",port)
                };
                iret = MySqlHelper.ExecuteNonQuery(conn, sqlcom.CommandText, commandParameters);
            }
            catch
            {
                iret = -1;

            }
            conn.Close();
            return iret;
        }

    }
}
