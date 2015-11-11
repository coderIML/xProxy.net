using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Net.Sockets;
using System.Security.Cryptography;
using xProxy;
using xProxy.Http;
using xProxy.Socks;
using xProxy.Socks.Authentication;
using ProxyService;
using System.ServiceModel;
namespace xProxy {
	
	public struct ListenEntry {
		
		public Listener listener;
		
		public Guid guid;
		
		public override bool Equals(object obj) {
			return ((ListenEntry)obj).guid.Equals(guid);
		}
	}
	
	public class Proxy {
		
		public static void Main() {
			try {
                using (ChannelFactory<IProxyService> channel = new ChannelFactory<IProxyService>("ProxyClient"))
                {
                    var proxy = channel.CreateChannel();
                    RegisterEntiy info = new RegisterEntiy();
                    info.HttpPort = 100;
                    info.Ip = GetIP();
                    info.SocketPort = 0;
                    info.UName = "fewef";
                    info.UPwd = "123";
                    bool re=proxy.RegisterProxy(info);
                }
				Proxy prx = new Proxy();
				prx.Start();
			} catch {
				Console.WriteLine("The program ended abnormally!");
			}
		}
		
		public Proxy() {
		
		}
        public int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //����ѭ���ж��Ƿ����ظ�
                {
                    tmp = ra.Next(minValue, maxValue); //���������ȡ��
                    getNum(arrNum, tmp, minValue, maxValue, ra);//�ݹ�:���ȡ���������ֺ���ȡ�õ��������ظ������������ȡ��
                }
                n++;
            }
            return tmp;
        }
        public int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                tmp = ra.Next(minValue, maxValue); //���ȡ��
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //ȡ��ֵ����������
            }
            return arrNum;
        }
        int httpport
        {
            get;
            set;
        }
        int sock5port
        {
            get;
            set;
        }

        string IP
        {
            get;
            set;
        }
        private static string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); 
                int start = all.IndexOf("����IP��ַ�ǣ�[") + 9;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }
        private void LoadListeners()
        {

            string oldip = IP;
            string oldHttpPort = httpport.ToString();
            string oldSocksPort = sock5port.ToString();
            IP = GetIP();
            if (IP == string.Empty)
                return;

            int[] arr = getRandomNum(2, 10000, 30000);
            httpport = arr[0];
            sock5port = arr[1];

            // HTTP����
            Listener listener = null;
            listener = CreateListener("xProxy.Http.HttpListener",string.Format("host:{0};int:{1}",IP,httpport));
            if (listener != null)
            {
                try
                {
                    listener.Start();
                }
                catch { }
                AddListener(listener);
                //ds.HttpToDb(IP, httpport.ToString());
                //ds.DelHttp(oldip, oldHttpPort);
            }
            
            
            //socks5 ����      
            UserList.Clear();
            string Name = "test";
            string Pwd = "1";
            UserList.AddItem(Name, Pwd);
            listener = CreateListener("xProxy.Socks.SocksListener",string.Format( "host:{0};int:{1};authlist",IP,sock5port));
            if (listener != null)
            {
                try
                {
                    listener.Start();
                }
                catch { }
                AddListener(listener);
                //ds.Socks5ToDb(IP, sock5port.ToString(), Name,Pwd);
                //ds.DelSocks5(oldip, oldSocksPort);
            }
        }
		public void Start() {
			// Initialize some objects
			StartTime = DateTime.Now;
            LoadListeners();
			// Start the proxy
			string command;
            Console.WriteLine("\r\n  �ɹ��Ƽ� Proxy\r\n  ~~~~~~~~~~~~~~~~~~\r\n\r\n  �˳������루exit)");
			Console.Write("\r\n>");
			command = Console.ReadLine().ToLower();
			while (!command.Equals("exit")) {
				Console.Write("\r\n>");
				command = Console.ReadLine().ToLower();
			}
			Stop();
			Console.WriteLine("Goodbye...");
		}

		protected void ShowDelListener() {
			Console.WriteLine("Please enter the ID of the listener you want to delete:\r\n (use the 'listlisteners' command to show all the listener IDs)");
			string id = Console.ReadLine();
			if (id != "") {
				try {
					ListenEntry le = new ListenEntry();
					le.guid = new Guid(id);
					if (!Listeners.Contains(le)) {
						Console.WriteLine("Specified ID not found in list!");
						return;
					} else {
						this[Listeners.IndexOf(le)].Dispose();
						Listeners.Remove(le);
						
					}
				} catch {
					Console.WriteLine("Invalid ID tag!");
					return;
				}
				Console.WriteLine("Listener removed from the list.");
			}
		}

		protected void ShowListeners() {
			for(int i = 0; i < Listeners.Count; i++) {
				Console.WriteLine(((ListenEntry)Listeners[i]).listener.ToString());
				Console.WriteLine("  id: " + ((ListenEntry)Listeners[i]).guid.ToString("N"));
			}
		}

		protected void ShowAddListener() {
			Console.WriteLine("Please enter the full class name of the Listener object you're trying to add:\r\n (ie. xProxy.Http.HttpListener)");
			string classtype = Console.ReadLine();
			if (classtype == "")
				return;
			else if(Type.GetType(classtype) == null) {
				Console.WriteLine("The specified class does not exist!");
				return;
			}
			Console.WriteLine("Please enter the construction parameters:");
			string construct = Console.ReadLine();
			object listenObject = CreateListener(classtype, construct);
			if (listenObject == null) {
				Console.WriteLine("Invalid construction string.");
				return;
			}
			Listener listener;
			try {
				listener = (Listener)listenObject;
			} catch {
				Console.WriteLine("The specified object is not a valid Listener object.");
				return;
			}
			try {
				listener.Start();
				AddListener(listener);
			} catch {
				Console.WriteLine("Error while staring the Listener.\r\n(Perhaps the specified port is already in use?)");
				return;
			}
			
		}
		
		public void Stop() {
			for (int i = 0; i < ListenerCount; i++) {
				Console.WriteLine(this[i].ToString() + " stopped.");
				this[i].Dispose();
			}
			Listeners.Clear();
		}
		
		public void AddListener(Listener newItem) {
			if (newItem == null)
				throw new ArgumentNullException();
			ListenEntry le = new ListenEntry();
			le.listener = newItem;
			le.guid = Guid.NewGuid();
			while (Listeners.Contains(le)) {
				le.guid = Guid.NewGuid();
			}
			Listeners.Add(le);
			Console.WriteLine(newItem.ToString() + " started.");
		}

        private AuthenticationList m_UserList = new AuthenticationList();
        internal AuthenticationList UserList
        {
            get
            {
                return m_UserList;
            }
        }

		public Listener CreateListener(string type, string cpars) {
			try {
				string [] parts = cpars.Split(';');
				object [] pars = new object[parts.Length];
				string oval = null, otype = null;
				int ret;
				// Start instantiating the objects to give to the constructor
				for(int i = 0; i < parts.Length; i++) {
					ret = parts[i].IndexOf(':');
					if (ret >= 0) {
						otype = parts[i].Substring(0, ret);
						oval = parts[i].Substring(ret + 1);
					} else {
						otype = parts[i];
					}
					switch (otype.ToLower()) {
						case "int":
							pars[i] = int.Parse(oval);
							break;
						case "host":
							pars[i] = Dns.Resolve(oval).AddressList[0];
							break;
						case "authlist":
							pars[i] = UserList;
							break;
						case "null":
							pars[i] = null;
							break;
						case "string":
							pars[i] = oval;
							break;
						case "ip":
							pars[i] = IPAddress.Parse(oval);
							break;
						default:
							pars[i] = null;
							break;
					}
				}
				return (Listener)Activator.CreateInstance(Type.GetType(type), pars);
			} catch {
				return null;
			}
		}
		
		protected ArrayList Listeners {
			get {
				return m_Listeners;
			}
		}
		
		internal int ListenerCount {
			get {
				return Listeners.Count;
			}
		}
		
		internal virtual Listener this[int index] {
			get {
				return ((ListenEntry)Listeners[index]).listener;
			}
		}
		
		protected DateTime StartTime {
			get {
				return m_StartTime;
			}
			set {
				m_StartTime = value;
			}
		}
		

		private DateTime m_StartTime;
		private ArrayList m_Listeners = new ArrayList();
	}
}
