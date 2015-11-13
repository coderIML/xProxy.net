using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProxyService
{
    public class DataManger : IDisposable
    {
        SqlConnection conn;
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }
        public DataManger(string connStr)
        {
            conn = new SqlConnection(connStr);
            conn.Open();
        }
        public bool AddProxy(RegisterEntiy info)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO TProxyClient VALUES(@Ip,@HttpPort,@SocketPort,@UName,@UPwd,@Time)";
            SqlParameter ip = new SqlParameter("@Ip", info.Ip);
            SqlParameter httpPort = new SqlParameter("@HttpPort", info.HttpPort);
            SqlParameter socketPort = new SqlParameter("@SocketPort", info.SocketPort);
            SqlParameter uName = new SqlParameter("@UName", info.UName);
            SqlParameter uPwd = new SqlParameter("@UPwd", info.UPwd);
            SqlParameter time = new SqlParameter("@Time", DateTime.Now.Ticks);
            cmd.Parameters.Add(ip);
            cmd.Parameters.Add(httpPort);
            cmd.Parameters.Add(socketPort);
            cmd.Parameters.Add(uName);
            cmd.Parameters.Add(uPwd);
            cmd.Parameters.Add(time);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                cmd.Dispose();
                return false;
            }
            cmd.Dispose();
            return true;
        }
        public void DeleteProxy(string ip)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM TProxyClient WHERE Ip=@ip";
            SqlParameter Ip = new SqlParameter("@ip", ip);
            cmd.Parameters.Add(Ip);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public bool HeartBeat(string ip)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE TProxyClient SET Time=@time WHERE Ip=@ip";
            SqlParameter Ip = new SqlParameter("@ip", ip);
            SqlParameter time = new SqlParameter("@time", DateTime.Now.Ticks);
            cmd.Parameters.Add(Ip);
            cmd.Parameters.Add(time);
            int count = 0;
            try
            {
                count = cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }
            if (count == 0)
                return false;
            else
                return true;
        }
    }
}