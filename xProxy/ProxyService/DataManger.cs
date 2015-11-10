using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProxyService
{
    public class DataManger
    {
        SqlConnection conn;
        public DataManger(string connStr)
        {
            conn = new SqlConnection(connStr);
            conn.Open();
        }
        public void SaveToHttp(RegisterEntiy regInfo)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO THttp VALUES(@ip,@port)";
            SqlParameter ip = new SqlParameter("@ip", regInfo.Ip);
            SqlParameter port = new SqlParameter("@port", regInfo.Port);
            cmd.Parameters.Add(ip);
            cmd.Parameters.Add(port);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
            }
        }
        public void SaveToSocket(RegisterEntiy regInfo)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO TSocket VALUES(@ip,@port,@uname,@upwd)";
            SqlParameter ip = new SqlParameter("@ip", regInfo.Ip);
            SqlParameter port = new SqlParameter("@port", regInfo.Port);
            SqlParameter uname = new SqlParameter("@uname", regInfo.UName);
            SqlParameter upwd = new SqlParameter("@upwd", regInfo.UPwd);
            cmd.Parameters.Add(ip);
            cmd.Parameters.Add(port);
            cmd.Parameters.Add(uname);
            cmd.Parameters.Add(upwd);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
            }
        }
        public void DeleteProxy(string ip, string port)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM THttp WHERE Ip=@ip and Port=@port;DELETE FROM TSocket WHERE Ip=@ip and Port=@port";
            SqlParameter Ip = new SqlParameter("@ip", ip);
            SqlParameter Port = new SqlParameter("@port", port);
            cmd.Parameters.Add(ip);
            cmd.Parameters.Add(port);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}