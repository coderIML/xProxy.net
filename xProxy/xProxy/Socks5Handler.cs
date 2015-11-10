using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using xProxy;
using xProxy.Socks.Authentication;

namespace xProxy.Socks {

internal sealed class Socks5Handler : SocksHandler {
	
	public Socks5Handler(Socket ClientConnection, NegotiationCompleteDelegate Callback, AuthenticationList AuthList) : base(ClientConnection, Callback) {
		this.AuthList = AuthList;
	}
	
	public Socks5Handler(Socket ClientConnection, NegotiationCompleteDelegate Callback) : this(ClientConnection, Callback, null) {}
	
	protected override bool IsValidRequest(byte [] Request) {
		try {
			return (Request.Length == Request[0] + 1);
		} catch {
			return false;
		}
	}
	
	protected override void ProcessRequest(byte [] Request) {
		try {
			byte Ret = 255;
			for (int Cnt = 1; Cnt < Request.Length; Cnt++) {
				if (Request[Cnt] == 0 && AuthList == null) { //0 = No authentication
					Ret = 0;
					AuthMethod = new AuthNone();
					break;
				} else if (Request[Cnt] == 2 && AuthList != null) { //2 = user/pass
					Ret = 2;
					AuthMethod = new AuthUserPass(AuthList);
					if (AuthList != null)
						break;
				}
			}
			Connection.BeginSend(new byte[]{5, Ret}, 0, 2, SocketFlags.None, new AsyncCallback(this.OnAuthSent), Connection);
		} catch {
			Dispose(false);
		}
	}
	
	private void OnAuthSent(IAsyncResult ar) {
		try {
			if (Connection.EndSend(ar) <= 0 || AuthMethod == null) {
				Dispose(false);
				return;
			}
			AuthMethod.StartAuthentication(Connection, new AuthenticationCompleteDelegate(this.OnAuthenticationComplete));
		} catch {
			Dispose(false);
		}
	}
	
	private void OnAuthenticationComplete(bool Success) {
		try {
			if (Success) {
				Bytes = null;
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
			} else {
				Dispose(false);
			}
		} catch {
			Dispose(false);
		}
	}
	
	private void OnRecvRequest(IAsyncResult ar) {
		try {
			int Ret = Connection.EndReceive(ar);
			if (Ret <= 0) {
				Dispose(false);
				return;
			}
			AddBytes(Buffer, Ret);
			if (IsValidQuery(Bytes))
				ProcessQuery(Bytes);
			else
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
		} catch {
			Dispose(false);
		}
	}
	
	private bool IsValidQuery(byte [] Query) {
		try {
			switch(Query[3]) {
				case 1: //IPv4 address
					return (Query.Length == 10);
				case 3: //Domain name
					return (Query.Length == Query[4] + 7);
				case 4: //IPv6 address
					//Not supported
					Dispose(8);
					return false;
				default:
					Dispose(false);
					return false;
			}
		} catch {
			return false;
		}
	}
	
	private void ProcessQuery(byte [] Query) {
		try {
			switch(Query[1]) {
				case 1: //CONNECT
					IPAddress RemoteIP = null;
					int RemotePort = 0;
					if (Query[3] == 1) {
						RemoteIP = IPAddress.Parse(Query[4].ToString() + "." + Query[5].ToString() + "." + Query[6].ToString() + "." + Query[7].ToString());
						RemotePort = Query[8] * 256 + Query[9];
					} else if( Query[3] == 3) {
						RemoteIP = Dns.Resolve(Encoding.ASCII.GetString(Query, 5, Query[4])).AddressList[0];
						RemotePort = Query[4] + 5;
						RemotePort = Query[RemotePort] * 256 + Query[RemotePort + 1];
					}
					RemoteConnection = new Socket(RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					RemoteConnection.BeginConnect(new IPEndPoint(RemoteIP, RemotePort), new AsyncCallback(this.OnConnected), RemoteConnection);
					break;
				case 2: //BIND
					byte [] Reply = new byte[10];
					long LocalIP = Listener.GetLocalExternalIP().Address;
					AcceptSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					AcceptSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
					AcceptSocket.Listen(50);
					Reply[0] = 5;  //Version 5
					Reply[1] = 0;  //Everything is ok :)
					Reply[2] = 0;  //Reserved
					Reply[3] = 1;  //We're going to send a IPv4 address
					Reply[4] = (byte)(Math.Floor((double)(LocalIP % 256)));  //IP Address/1
					Reply[5] = (byte)(Math.Floor((double)(LocalIP % 65536) / 256));  //IP Address/2
					Reply[6] = (byte)(Math.Floor((double)(LocalIP % 16777216) / 65536));  //IP Address/3
					Reply[7] = (byte)(Math.Floor((double)LocalIP / 16777216));  //IP Address/4
					Reply[8] = (byte)(Math.Floor((double)((IPEndPoint)AcceptSocket.LocalEndPoint).Port / 256));  //Port/1
					Reply[9] = (byte)(((IPEndPoint)AcceptSocket.LocalEndPoint).Port % 256);  //Port/2
					Connection.BeginSend(Reply, 0, Reply.Length, SocketFlags.None, new AsyncCallback(this.OnStartAccept), Connection);
					break;
				case 3: //ASSOCIATE
                    
                    RemotePort = 0;
					if (Query[3] == 1) {
						RemoteIP = IPAddress.Parse(Query[4].ToString() + "." + Query[5].ToString() + "." + Query[6].ToString() + "." + Query[7].ToString());
						RemotePort = Query[8] * 256 + Query[9];
                    }
                    else if (Query[3] == 3)
                    {
                        RemoteIP = Dns.Resolve(Encoding.ASCII.GetString(Query, 5, Query[4])).AddressList[0];
                        RemotePort = Query[4] + 5;
                        RemotePort = Query[RemotePort] * 256 + Query[RemotePort + 1];
                    }
					LocalIP = Listener.GetLocalExternalIP().Address;
                   // RemoteConnection = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Udp);

                    RemoteConnection = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    RemoteConnection.Bind(new IPEndPoint(IPAddress.Any, RemotePort+1));

                    Reply = new byte[10];
					Reply[0] = 5;  //Version 5
					Reply[1] = 0;  //Everything is ok :)
					Reply[2] = 0;  //Reserved
					Reply[3] = 1;  //We're going to send a IPv4 address
					Reply[4] = (byte)(Math.Floor((double)(LocalIP % 256)));  //IP Address/1
					Reply[5] = (byte)(Math.Floor((double)(LocalIP % 65536) / 256));  //IP Address/2
					Reply[6] = (byte)(Math.Floor((double)(LocalIP % 16777216) / 65536));  //IP Address/3
					Reply[7] = (byte)(Math.Floor((double)LocalIP / 16777216));  //IP Address/4
					Reply[8] = (byte)(Math.Floor((double)RemotePort/ 256));  //Port/1
					Reply[9] = (byte)(RemotePort % 256);  //Port/2
					Connection.BeginSend(Reply, 0, Reply.Length, SocketFlags.None, new AsyncCallback(this.OnStartAccept), Connection);
					break;
				default:
					Dispose(7);
					break;
			}
		} catch (Exception e){
            Console.WriteLine(e.Message.ToString());
			Dispose(1);
		}
	}
	
	private void OnConnected(IAsyncResult ar) {
		try {
			RemoteConnection.EndConnect(ar);
			Dispose(0);
		} catch {
			Dispose(1);
		}
	}
	
	protected override void OnAccept(IAsyncResult ar) {
		try {
			RemoteConnection = AcceptSocket.EndAccept(ar);
			AcceptSocket.Close();
			AcceptSocket = null;
			Dispose(0);
		} catch {
			Dispose(1);
		}
	}
	
	protected override void Dispose(byte Value) {
		byte [] ToSend;
		try {
			ToSend = new byte[]{5, Value, 0, 1,
						(byte)(((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 256),
						(byte)(Math.Floor((double)(((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 65536) / 256)),
						(byte)(Math.Floor((double)(((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 16777216) / 65536)),
						(byte)(Math.Floor((double)((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address / 16777216)),
						(byte)(Math.Floor((double)((IPEndPoint)RemoteConnection.LocalEndPoint).Port / 256)),
						(byte)(((IPEndPoint)RemoteConnection.LocalEndPoint).Port % 256)};
		} catch {
			ToSend = new byte[] {5, 1, 0, 1, 0, 0, 0, 0, 0, 0};
		}
		try {
			Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, (AsyncCallback)(ToSend[1] == 0 ? new AsyncCallback(this.OnDisposeGood) : new AsyncCallback(this.OnDisposeBad)), Connection);
		} catch {
			Dispose(false);
		}
	}
	
	private AuthBase AuthMethod {
		get {
			return m_AuthMethod;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_AuthMethod = value;
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
	private AuthenticationList m_AuthList;
	private AuthBase m_AuthMethod;
}

}
