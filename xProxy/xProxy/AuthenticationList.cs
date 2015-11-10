

using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Specialized;

namespace xProxy.Socks.Authentication {


public class AuthenticationList {

	public AuthenticationList() {}
	
	public void AddItem(string Username, string Password) {
		if (Password == null)
			throw new ArgumentNullException();
		AddHash(Username, Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Password))));
	}
	
	public void AddHash(string Username, string PassHash) {
		if (Username == null || PassHash == null)
			throw new ArgumentNullException();
		if (Listing.ContainsKey(Username)) {
			Listing[Username] = PassHash;
		} else {
			Listing.Add(Username, PassHash);
		}
	}
	
	public void RemoveItem(string Username) {
		if (Username == null)
			throw new ArgumentNullException();
		Listing.Remove(Username);
	}
	
	public bool IsItemPresent(string Username, string Password) {
		return IsHashPresent(Username, Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Password))));
	}
	
	public bool IsUserPresent(string Username) {
		return Listing.ContainsKey(Username);
	}
	
	public bool IsHashPresent(string Username, string PassHash) {
		return Listing.ContainsKey(Username) && Listing[Username].Equals(PassHash);
	}
	
	protected StringDictionary Listing {
		get {
			return m_Listing;
		}
	}
	
	public string[] Keys {
		get {
			ICollection keys = Listing.Keys;
			string [] ret = new string[keys.Count];
			keys.CopyTo(ret, 0);
			return ret;
		}
	}
	
	public string[] Hashes {
		get {
			ICollection values = Listing.Values;
			string [] ret = new string[values.Count];
			values.CopyTo(ret, 0);
			return ret;
		}
	}

	public void Clear() {
		Listing.Clear();
	}

	private StringDictionary m_Listing = new StringDictionary();
}

}