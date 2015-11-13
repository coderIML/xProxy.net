using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
namespace ProxyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class ProxyService : IProxyService
    {
        static ProxyService instance;
        public static ProxyService Instance { get { return instance; } }
        public static DataManger dataManager;
        public static Dictionary<string, IProxyServiceCallback> CallBackDic = new Dictionary<string, IProxyServiceCallback>();
        public static event Action<RegisterEntiy> Registering;
        public static event Action<string> Canceling;
        public ProxyService()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProxyDbString"].ConnectionString;
            if (dataManager == null)
            {
                dataManager = new DataManger(connStr);
            }
            if (instance == null)
            {
                instance = this;
            }
        }

        public bool RegisterProxy(RegisterEntiy regInfo)
        {
            IProxyServiceCallback callbackInstance = OperationContext.Current.GetCallbackChannel<IProxyServiceCallback>();
            if (!CallBackDic.Keys.Contains(regInfo.Ip))
            {
                if (dataManager.AddProxy(regInfo))
                {
                    CallBackDic.Add(regInfo.Ip, callbackInstance);
                    OnRegistering(regInfo);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {

                return false;
            }
        }

        public bool HeartBeatMessage(string ip)
        {
            return dataManager.HeartBeat(ip);
        }

        public void CancelProxy(string ip)
        {
            if (CallBackDic.ContainsKey(ip))
            {
                CallBackDic.Remove(ip);
            }
            dataManager.DeleteProxy(ip);
            OnCanceling(ip);
        }

        void OnRegistering(RegisterEntiy regInfo)
        {
            if (Registering != null)
            {
                Registering(regInfo);
            }
        }

        void OnCanceling(string id)
        {
            if (Canceling != null)
            {
                Canceling(id);
            }
        }
    }
}
