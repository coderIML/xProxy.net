

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using xProxy;

namespace xProxy.Socks {

internal sealed class Socks4Handler : SocksHandler {
	
	public Socks4Handler(Socket ClientConnection, NegotiationCompleteDelegate Callback) : base(ClientConnection, Callback) {}
	
	protected override bool IsValidRequest(byte [] Request) {
		try {
			if (Request[0] != 1 && Request[0] != 2) { //CONNECT or BIND
				Dispose(false);
			} else {
				if (Request[3] == 0 && Request[4] == 0 && Request[5] == 0 && Request[6] != 0) { //Use remote DNS
					int Ret = Array.IndexOf(Request, (byte)0, 7);
					if (Ret > -1)
						return Array.IndexOf(Request, (byte)0, Ret + 1) != -1;
				} else {
					return Array.IndexOf(Request, (byte)0, 7) != -1;
				}
			}
		} catch {}
		return false;
	}
	
	protected override void ProcessRequest(byte [] Request) {
		int Ret;
		try {
			if (Request[0] == 1) { // CONNECT
				IPAddress RemoteIP;
				int RemotePort = Request[1] * 256 + Request[2];
				Ret = Array.IndexOf(Request, (byte)0, 7);
				Username = Encoding.ASCII.GetString(Request, 7, Ret - 7);
				if (Request[3] == 0 && Request[4] == 0 && Request[5] == 0 && Request[6] != 0) {// Use remote DNS
					Ret = Array.IndexOf(Request, (byte)0, Ret + 1);
					RemoteIP = Dns.Resolve(Encoding.ASCII.GetString(Request, Username.Length + 8, Ret - Username.Length - 8)).AddressList[0];
				} else { //Do not use remote DNS
					RemoteIP = IPAddress.Parse(Request[3].ToString() + "." + Request[4].ToString() + "." + Request[5].ToString() + "." + Request[6].ToString());
				}
				RemoteConnection = new Socket(RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				RemoteConnection.BeginConnect(new IPEndPoint(RemoteIP, RemotePort), new AsyncCallback(this.OnConnected), RemoteConnection);
			} else if (Request[0] == 2) { // BIND
				byte [] Reply = new byte[8];
				long LocalIP = Listener.GetLocalExternalIP().Address;
				AcceptSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				AcceptSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
				AcceptSocket.Listen(50);
				RemoteBindIP = IPAddress.Parse(Request[3].ToString() + "." + Request[4].ToString() + "." + Request[5].ToString() + "." + Request[6].ToString());
				Reply[0] = 0;  //Reply version 0
				Reply[1] = 90;  //Everything is ok :)
				Reply[2] = (byte)(Math.Floor((double)((IPEndPoint)AcceptSocket.LocalEndPoint).Port / 256));  //Port/1
				Reply[3] = (byte)(((IPEndPoint)AcceptSocket.LocalEndPoint).Port % 256);  //Port/2
                Reply[4] = (byte)(Math.Floor((double)(LocalIP % 256)));  //IP Address/1
                Reply[5] = (byte)(Math.Floor((double)(LocalIP % 65536) / 256));  //IP Address/2
                Reply[6] = (byte)(Math.Floor((double)(LocalIP % 16777216) / 65536));  //IP Address/3
                Reply[7] = (byte)(Math.Floor((double)(LocalIP / 16777216)));  //IP Address/4
				Connection.BeginSend(Reply, 0, Reply.Length, SocketFlags.None, new AsyncCallback(this.OnStartAccept), Connection);
			}
		} catch {
			Dispose(91);
		}
	}
	
	private void OnConnected(IAsyncResult ar) {
		try {
			RemoteConnection.EndConnect(ar);
			Dispose(90);
		} catch {
			Dispose(91);
		}
	}
	
	protected override void Dispose(byte Value) {
		byte [] ToSend;
		try {
            ToSend = new byte[]{0, Value, (byte)(Math.Floor((double)((IPEndPoint)RemoteConnection.RemoteEndPoint).Port / 256)),
		                                   (byte)(((IPEndPoint)RemoteConnection.RemoteEndPoint).Port % 256),
		                                   (byte)(Math.Floor((double)(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 256))),
		                                   (byte)(Math.Floor((double)(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 65536) / 256)),
		                                   (byte)(Math.Floor((double)(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 16777216) / 65536)),
		                                   (byte)(Math.Floor((double)((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address / 16777216))};
		} catch {
			ToSend = new byte[]{0, 91, 0, 0, 0, 0, 0, 0};
		}
		try {
			Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, (AsyncCallback)(ToSend[1] == 90 ? new AsyncCallback(this.OnDisposeGood) : new AsyncCallback(this.OnDisposeBad)), Connection);
		} catch {
			Dispose(false);
		}
	}
	
	protected override void OnAccept(IAsyncResult ar) {
		try {
			RemoteConnection = AcceptSocket.EndAccept(ar);
			AcceptSocket.Close();
			AcceptSocket = null;
			if (RemoteBindIP.Equals(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address))
				Dispose(90);
			else
				Dispose(91);
		} catch {
			Dispose(91);
		}
	}
}

}
