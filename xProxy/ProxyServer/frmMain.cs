using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using ProxyService;
using System.ServiceModel.Description;

namespace ProxyServer
{
    public partial class frmMain : Form
    {
        ServiceHost host;
        System.Timers.Timer isActiveTimer = new System.Timers.Timer();
        System.Timers.Timer dialTimer = new System.Timers.Timer();
        int count = 0;
        BindingList<RegisterEntiy> list = new BindingList<RegisterEntiy>();
        object syncRoot = new object();
        public frmMain()
        {
            InitializeComponent();
            this.FormClosing += frmMain_FormClosing;
            ProxyService.ProxyService.Registering += ProxyService_Registering;
            ProxyService.ProxyService.Canceling += ProxyService_Canceling;
            isActiveTimer.Elapsed += isActiveTimer_Elapsed;
            dialTimer.Elapsed += dialTimer_Elapsed;
            registerEntiyBindingSource.DataSource = list;
        }

        void dialTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (syncRoot)
            {
                foreach (var item in ProxyService.ProxyService.CallBackDic)
                {
                    try
                    {
                        item.Value.Restart();
                    }
                    catch
                    {
                        this.Invoke(new Action(() => { ProxyService.ProxyService.Instance.CancelProxy(item.Key); }));
                    }
                }
            }
        }

        void isActiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (syncRoot)
            {
                foreach (var item in ProxyService.ProxyService.CallBackDic)
                {
                    try
                    {
                        item.Value.IsActive();
                    }
                    catch
                    {
                        this.Invoke(new Action(() => { ProxyService.ProxyService.Instance.CancelProxy(item.Key); }));
                    }
                }
            }
        }

        void ProxyService_Canceling(string obj)
        {
            labelOnline.Text = (--count).ToString();
            richOutPut.SelectionColor = Color.Black;
            richOutPut.AppendText(DateTime.Now.ToShortTimeString()+"：");
            richOutPut.SelectionColor = Color.Red;
            string str = string.Format("{0}已注销\n",obj);
            richOutPut.AppendText(str);
            var item=list.Where(x => x.Ip == obj).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
            }
        }

        void ProxyService_Registering(RegisterEntiy obj)
        {
            labelOnline.Text = (++count).ToString();
            richOutPut.SelectionColor = Color.Black;
            richOutPut.AppendText(DateTime.Now.ToShortTimeString()+"：");
            richOutPut.SelectionColor = Color.Green;
            string str = string.Format("{0}已登录\n", obj.Ip);
            richOutPut.AppendText(str);
            list.Add(obj);
        }

        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            try
            {
                if (host == null)
                {
                    host = new ServiceHost(typeof(ProxyService.ProxyService));
                }
                if (host.State == CommunicationState.Opened)
                {
                    btnStart.Text = "启动服务";
                    host.Close();
                    isActiveTimer.Stop();
                    dialTimer.Stop();
                    host = null;
                }
                else
                {
                    btnStart.Text = "停止服务";
                    host.Open();
                    if (Settings.Default.AutoDialing)
                    {
                        dialTimer.Interval = Settings.Default.AutoDialingTime*1000;
                        dialTimer.Start();
                    }
                    if (Settings.Default.CheckActive)
                    {
                        isActiveTimer.Interval = Settings.Default.CheckActiveTime*1000;
                        isActiveTimer.Start();
                    }
                    ProxyService.ProxyService.CallBackDic.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.Message);
                host = null;
            }
            btnStart.Enabled = true;
        }

        private void btnDial_Click(object sender, EventArgs e)
        {
            var item = (RegisterEntiy)registerEntiyBindingSource.Current;
            ProxyService.ProxyService.CallBackDic[item.Ip].Restart();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var item = (RegisterEntiy)registerEntiyBindingSource.Current;
            try
            {
                ProxyService.ProxyService.CallBackDic[item.Ip].Exit();
            }
            catch (Exception ex)
            {

            }
            ProxyService.ProxyService.Instance.CancelProxy(item.Ip);
        }

    }
}
