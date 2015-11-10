using System;
using System.Net;
using System.Net.Sockets;
using xProxy;
using xProxy.Socks.Authentication;

namespace xProxy.Socks {


public sealed class SocksListener : Listener {
	
	public SocksListener(int Port) : this(IPAddress.Any, Port, null) {}
	
	public SocksListener(IPAddress Address, int Port) : this(Address, Port, null) {}
	
	public SocksListener(int Port, AuthenticationList AuthList) : this(IPAddress.Any, Port, AuthList) {}
	
	public SocksListener(IPAddress Address, int Port, AuthenticationList AuthList) : base(Port, Address) {
		this.AuthList = AuthList;
	}
	
	public override void OnAccept(IAsyncResult ar) {
		try {
			Socket NewSocket = ListenSocket.EndAccept(ar);
			if (NewSocket != null) {
				SocksClient NewClient = new SocksClient(NewSocket, new DestroyDelegate(this.RemoveClient), AuthList);
				AddClient(NewClient);
				NewClient.StartHandshake();
			}
		} catch {}
		try {
			//Restart Listening
			ListenSocket.BeginAccept(new AsyncCallback(this.OnAccept), ListenSocket);
		} catch {
			Dispose();
		}
	}
	
	private AuthenticationList AuthList {
		get {
			return m_AuthList;
		}
		set {
			m_AuthList = value;
		}
	}
	
	public override string ToString() {
		return "SOCKS service on " + Address.ToString() + ":" + Port.ToString();
	}
	
	public override string ConstructString {
		get {
			if (AuthList == null)
				return "host:" + Address.ToString() + ";int:" + Port.ToString()+ ";null";
			else
				return "host:" + Address.ToString() + ";int:" + Port.ToString()+ ";authlist";
		}
	}
	private AuthenticationList m_AuthList;
}

}
