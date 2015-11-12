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
namespace xProxy
{

    public struct ListenEntry
    {

        public Listener listener;

        public Guid guid;

        public override bool Equals(object obj)
        {
            return ((ListenEntry)obj).guid.Equals(guid);
        }
    }

    public class Proxy
    {
        public static IProxyService service;
        public static Proxy proxy;
        public static System.Timers.Timer timer;
        public static void Main()
        {
            try
            {
                proxy = new Proxy();
                ProxyServiceCallBack callback = new ProxyServiceCallBack(proxy);
                DuplexChannelFactory<IProxyService> channel = new DuplexChannelFactory<IProxyService>(callback, "ProxyClient");
                service = channel.CreateChannel();
                timer = new System.Timers.Timer(Settings.Default.HeartBeatSpan * 1000);
                timer.Elapsed+=timer_Elapsed;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("��������˳�...");
                Console.ReadLine();
            }
            proxy.Start();
        }

        private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            service.HeartBeatMessage(proxy.IP);
        }

        public Proxy()
        {

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
            //IP = GetIP();
            IP = "192.168.1.29";
            if (IP == string.Empty)
                return;

            int[] arr = getRandomNum(2, 10000, 30000);
            httpport = arr[0];
            sock5port = arr[1];

            RegisterEntiy entity = new RegisterEntiy();
            // HTTP����
            Listener listener = null;
            listener = CreateListener("xProxy.Http.HttpListener", string.Format("host:{0};int:{1}", IP, httpport));
            if (listener != null)
            {
                try
                {
                    listener.Start();
                    AddListener(listener);
                    entity.Ip = IP;
                    entity.HttpPort = httpport;
                    entity.SocketPort = 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine("HTTP��������ʧ�ܣ�" + e.Message);
                }
            }
            //socks5 ����      
            UserList.Clear();
            string Name = "test";
            string Pwd = "1";
            UserList.AddItem(Name, Pwd);
            listener = CreateListener("xProxy.Socks.SocksListener", string.Format("host:{0};int:{1};authlist", IP, sock5port));
            if (listener != null)
            {
                try
                {
                    listener.Start();
                    AddListener(listener);
                    entity.Ip = IP;
                    entity.SocketPort = sock5port;
                    entity.UName = Name;
                    entity.UPwd = Pwd;
                }
                catch (Exception e)
                {
                    Console.WriteLine("SOCKS5��������ʧ�ܣ�" + e.Message);
                }
            }
            if (entity.Ip != null)
            {
                try
                {
                    if (!service.RegisterProxy(entity))
                    {
                        Console.WriteLine("�������ע�����ʧ�ܣ������ԣ�");
                    }
                    else
                    {
                        Console.WriteLine("�������ע�����ɹ���");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ע�����ʧ�ܣ�" + e.Message);
                }
            }
        }
        public void Restart()
        {
           timer.Stop();
           service.CancelProxy(IP);
           Console.WriteLine("��ע����ǰ����IP");
           Stop();
           Console.WriteLine("��ֹͣ��ǰ����");
           Dial();
           Console.WriteLine("��������...");
           Start();
        }

        private void Dial()
        {
            Console.WriteLine("Dialing...");
        }
        public void Cancel()
        {
            System.Environment.Exit(0);
        }
        public void Start()
        {
            // Initialize some objects
            StartTime = DateTime.Now;
            LoadListeners();
            timer.Start();
            // Start the proxy
            string command;
            Console.WriteLine("\r\n  �ɹ��Ƽ� Proxy\r\n  ~~~~~~~~~~~~~~~~~~\r\n\r\n  �˳������루exit)");
            Console.Write("\r\n>");
            command = Console.ReadLine().ToLower();
            while (!command.Equals("exit"))
            {
                Console.Write("\r\n>");
                command = Console.ReadLine().ToLower();
            }
            Stop();
            try
            {
                service.CancelProxy(IP);
            }
            catch (Exception e)
            {
                Console.WriteLine("ע��ʧ�ܣ�");
                Console.WriteLine("��������˳�...");
                Console.ReadKey();
            }
            Console.WriteLine("Goodbye...");
        }

        protected void ShowDelListener()
        {
            Console.WriteLine("Please enter the ID of the listener you want to delete:\r\n (use the 'listlisteners' command to show all the listener IDs)");
            string id = Console.ReadLine();
            if (id != "")
            {
                try
                {
                    ListenEntry le = new ListenEntry();
                    le.guid = new Guid(id);
                    if (!Listeners.Contains(le))
                    {
                        Console.WriteLine("Specified ID not found in list!");
                        return;
                    }
                    else
                    {
                        this[Listeners.IndexOf(le)].Dispose();
                        Listeners.Remove(le);

                    }
                }
                catch
                {
                    Console.WriteLine("Invalid ID tag!");
                    return;
                }
                Console.WriteLine("Listener removed from the list.");
            }
        }

        protected void ShowListeners()
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Console.WriteLine(((ListenEntry)Listeners[i]).listener.ToString());
                Console.WriteLine("  id: " + ((ListenEntry)Listeners[i]).guid.ToString("N"));
            }
        }

        protected void ShowAddListener()
        {
            Console.WriteLine("Please enter the full class name of the Listener object you're trying to add:\r\n (ie. xProxy.Http.HttpListener)");
            string classtype = Console.ReadLine();
            if (classtype == "")
                return;
            else if (Type.GetType(classtype) == null)
            {
                Console.WriteLine("The specified class does not exist!");
                return;
            }
            Console.WriteLine("Please enter the construction parameters:");
            string construct = Console.ReadLine();
            object listenObject = CreateListener(classtype, construct);
            if (listenObject == null)
            {
                Console.WriteLine("Invalid construction string.");
                return;
            }
            Listener listener;
            try
            {
                listener = (Listener)listenObject;
            }
            catch
            {
                Console.WriteLine("The specified object is not a valid Listener object.");
                return;
            }
            try
            {
                listener.Start();
                AddListener(listener);
            }
            catch
            {
                Console.WriteLine("Error while staring the Listener.\r\n(Perhaps the specified port is already in use?)");
                return;
            }

        }

        public void Stop()
        {
            for (int i = 0; i < ListenerCount; i++)
            {
                Console.WriteLine(this[i].ToString() + " stopped.");
                this[i].Dispose();
            }
            Listeners.Clear();
        }

        public void AddListener(Listener newItem)
        {
            if (newItem == null)
                throw new ArgumentNullException();
            ListenEntry le = new ListenEntry();
            le.listener = newItem;
            le.guid = Guid.NewGuid();
            while (Listeners.Contains(le))
            {
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

        public Listener CreateListener(string type, string cpars)
        {
            try
            {
                string[] parts = cpars.Split(';');
                object[] pars = new object[parts.Length];
                string oval = null, otype = null;
                int ret;
                // Start instantiating the objects to give to the constructor
                for (int i = 0; i < parts.Length; i++)
                {
                    ret = parts[i].IndexOf(':');
                    if (ret >= 0)
                    {
                        otype = parts[i].Substring(0, ret);
                        oval = parts[i].Substring(ret + 1);
                    }
                    else
                    {
                        otype = parts[i];
                    }
                    switch (otype.ToLower())
                    {
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
            }
            catch
            {
                return null;
            }
        }

        protected ArrayList Listeners
        {
            get
            {
                return m_Listeners;
            }
        }

        internal int ListenerCount
        {
            get
            {
                return Listeners.Count;
            }
        }

        internal virtual Listener this[int index]
        {
            get
            {
                return ((ListenEntry)Listeners[index]).listener;
            }
        }

        protected DateTime StartTime
        {
            get
            {
                return m_StartTime;
            }
            set
            {
                m_StartTime = value;
            }
        }


        private DateTime m_StartTime;
        private ArrayList m_Listeners = new ArrayList();
    }
}
