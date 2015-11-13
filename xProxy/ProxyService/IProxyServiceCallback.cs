using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ProxyService
{
     [ServiceContract]
    public interface IProxyServiceCallback
    {
         [OperationContract(IsOneWay = true)]
         void Restart();

         [OperationContract(IsOneWay = true)]
         void Exit();

         [OperationContract]
         bool IsActive();

    }
}