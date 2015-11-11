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
            if (host == null)
            {
                host = new ServiceHost(typeof(ProxyService.ProxyService));
                host.Open();
            }

        }
    }
}
