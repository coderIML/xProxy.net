using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace xProxy
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = true)]
    class ProxyServiceCallBack:ProxyService.IProxyServiceCallback
    {
        #region IProxyServiceCallback 成员
        Proxy proxy;
        public ProxyServiceCallBack(Proxy proxy)
        {
            this.proxy = proxy;
        }
        public void Restart()
        {
            proxy.Restart();
        }

        public void Exit()
        {
            proxy.Cancel();
        }

        public bool IsActive()
        {
            return true;
        }

        #endregion
    }
}
