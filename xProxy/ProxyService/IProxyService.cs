using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProxyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IProxyService
    {

        [OperationContract]
        void RegisterProxy(RegisterEntiy regInfo,string type);

        [OperationContract]
        void HeartBeatMessage(HeartBeatEntity heartBeatInfo);

        [OperationContract]
        void CancelProxy(string ip,string port);
    }


    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class RegisterEntiy
    {
        string ip = string.Empty;
        int port=0;
        string uname = string.Empty;

        [DataMember]
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        [DataMember]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        [DataMember]
        public string UName
        {
            get { return uname; }
            set { uname = value; }
        }
        string upwd = string.Empty;

        [DataMember]
        public string UPwd
        {
            get { return upwd; }
            set { upwd = value; }
        }
    }

    [DataContract]
    public class HeartBeatEntity
    {
        string id;
        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
