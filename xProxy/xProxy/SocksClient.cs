

using System;
using System.Net;
using System.Net.Sockets;
using xProxy;
using xProxy.Socks.Authentication;

namespace xProxy.Socks {


public sealed class SocksClient : Client {
	
	public SocksClient(Socket ClientSocket, DestroyDelegate Destroyer, AuthenticationList AuthList) : base(ClientSocket, Destroyer) {
		this.AuthList = AuthList;
	}
	
	internal SocksHandler Handler {
		get {
			return m_Handler;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_Handler = value;
		}
	}
	
	public bool MustAuthenticate {
		get {
			return m_MustAuthenticate;
		}
		set {
			m_MustAuthenticate = value;
		}
	}
	public override void StartHandshake() {
		try {
			ClientSocket.BeginReceive(Buffer, 0, 1, SocketFlags.None, new AsyncCallback(this.OnStartSocksProtocol), ClientSocket);
		} catch {
			Dispose();
		}
	}
	
	private void OnStartSocksProtocol(IAsyncResult ar) {
		int Ret;
		try {
			Ret = ClientSocket.EndReceive(ar);
			if (Ret <= 0) {
				Dispose();
				return;
			}
			if (Buffer[0] == 4) { //SOCKS4 Protocol
				if (MustAuthenticate) {
					Dispose();
					return;
				} else {
					Handler = new Socks4Handler(ClientSocket, new NegotiationCompleteDelegate(this.OnEndSocksProtocol));
				}
			} else if(Buffer[0] == 5) { //SOCKS5 Protocol
				if (MustAuthenticate && AuthList == null) {
					Dispose();
					return;
				}
				Handler = new Socks5Handler(ClientSocket, new NegotiationCompleteDelegate(this.OnEndSocksProtocol), AuthList);
			} else {
				Dispose();
				return;
			}
			Handler.StartNegotiating();
		} catch {
			Dispose();
		}
	}
	
	private void OnEndSocksProtocol(bool Success, Socket Remote) {
		DestinationSocket = Remote;
		if (Success)
			StartRelay();
		else
			Dispose();
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
		try {
			if (Handler != null)
				return Handler.Username + " (" + ((IPEndPoint)ClientSocket.LocalEndPoint).Address.ToString() +") connected to " + DestinationSocket.RemoteEndPoint.ToString();
			else
				return "SOCKS connection from " + ((IPEndPoint)ClientSocket.LocalEndPoint).Address.ToString();
		} catch {
			return "Incoming SOCKS connection";
		}
	}
	private AuthenticationList m_AuthList;
	private bool m_MustAuthenticate = false;
	private SocksHandler m_Handler;
}

}
