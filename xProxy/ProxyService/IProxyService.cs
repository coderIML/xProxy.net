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
        bool RegisterProxy(RegisterEntiy regInfo);

        [OperationContract]
        bool HeartBeatMessage(string ip);

        [OperationContract]
        void CancelProxy(string ip);
    }


    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class RegisterEntiy
    {
        [DataMember]
        public string Ip { get; set; }
        [DataMember]
        public int HttpPort { get; set; }
        [DataMember]
        public int SocketPort { get; set; }
        [DataMember]
        public string UName { get; set; }
        [DataMember]
        public string UPwd { get; set; }
    }
}
