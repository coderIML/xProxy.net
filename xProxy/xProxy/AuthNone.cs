

using System.Net.Sockets;
using xProxy.Socks.Authentication;

namespace xProxy.Socks.Authentication {

internal sealed class AuthNone : AuthBase {
	public AuthNone() {}
	
	internal override void StartAuthentication(Socket Connection, AuthenticationCompleteDelegate Callback) {
		Callback(true);
	}
}

}
