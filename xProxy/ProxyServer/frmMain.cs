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
        public frmMain()
        {
            InitializeComponent();
            this.FormClosing += frmMain_FormClosing;
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
                    host = null;
                }
                else
                {
                    btnStart.Text = "停止服务";
                    host.Open();
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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in ProxyService.ProxyService.CallBackDic.Values)
            {
                item.Restart();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in ProxyService.ProxyService.CallBackDic.Values)
            {
                item.Exit();
            }
        }

    }
}
