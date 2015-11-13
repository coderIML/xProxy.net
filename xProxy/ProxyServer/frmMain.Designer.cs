namespace ProxyServer
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.labelOnline = new System.Windows.Forms.Label();
            this.lab1 = new System.Windows.Forms.Label();
            this.richOutPut = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnDial = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.ipDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.httpPortDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.socketPortDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uPwdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registerEntiyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.registerEntiyBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richOutPut, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(627, 312);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ipDataGridViewTextBoxColumn,
            this.httpPortDataGridViewTextBoxColumn,
            this.socketPortDataGridViewTextBoxColumn,
            this.uNameDataGridViewTextBoxColumn,
            this.uPwdDataGridViewTextBoxColumn});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.registerEntiyBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(126, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(498, 186);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.labelOnline);
            this.panel1.Controls.Add(this.lab1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(117, 306);
            this.panel1.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(13, 206);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(81, 78);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // labelOnline
            // 
            this.labelOnline.AutoSize = true;
            this.labelOnline.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOnline.ForeColor = System.Drawing.Color.Red;
            this.labelOnline.Location = new System.Drawing.Point(43, 104);
            this.labelOnline.Name = "labelOnline";
            this.labelOnline.Size = new System.Drawing.Size(21, 21);
            this.labelOnline.TabIndex = 1;
            this.labelOnline.Text = "0";
            // 
            // lab1
            // 
            this.lab1.AutoSize = true;
            this.lab1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab1.Location = new System.Drawing.Point(3, 45);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(85, 19);
            this.lab1.TabIndex = 0;
            this.lab1.Text = "在线数：";
            // 
            // richOutPut
            // 
            this.richOutPut.BackColor = System.Drawing.SystemColors.Control;
            this.richOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richOutPut.ForeColor = System.Drawing.Color.Red;
            this.richOutPut.Location = new System.Drawing.Point(126, 195);
            this.richOutPut.Name = "richOutPut";
            this.richOutPut.ReadOnly = true;
            this.richOutPut.Size = new System.Drawing.Size(498, 114);
            this.richOutPut.TabIndex = 2;
            this.richOutPut.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDial,
            this.btnCancel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // btnDial
            // 
            this.btnDial.Name = "btnDial";
            this.btnDial.Size = new System.Drawing.Size(100, 22);
            this.btnDial.Text = "拨号";
            this.btnDial.Click += new System.EventHandler(this.btnDial_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 22);
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ipDataGridViewTextBoxColumn
            // 
            this.ipDataGridViewTextBoxColumn.DataPropertyName = "Ip";
            this.ipDataGridViewTextBoxColumn.HeaderText = "Ip地址";
            this.ipDataGridViewTextBoxColumn.Name = "ipDataGridViewTextBoxColumn";
            this.ipDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // httpPortDataGridViewTextBoxColumn
            // 
            this.httpPortDataGridViewTextBoxColumn.DataPropertyName = "HttpPort";
            this.httpPortDataGridViewTextBoxColumn.HeaderText = "Http端口";
            this.httpPortDataGridViewTextBoxColumn.Name = "httpPortDataGridViewTextBoxColumn";
            this.httpPortDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // socketPortDataGridViewTextBoxColumn
            // 
            this.socketPortDataGridViewTextBoxColumn.DataPropertyName = "SocketPort";
            this.socketPortDataGridViewTextBoxColumn.HeaderText = "Socket端口";
            this.socketPortDataGridViewTextBoxColumn.Name = "socketPortDataGridViewTextBoxColumn";
            this.socketPortDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uNameDataGridViewTextBoxColumn
            // 
            this.uNameDataGridViewTextBoxColumn.DataPropertyName = "UName";
            this.uNameDataGridViewTextBoxColumn.HeaderText = "用户名";
            this.uNameDataGridViewTextBoxColumn.Name = "uNameDataGridViewTextBoxColumn";
            this.uNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uPwdDataGridViewTextBoxColumn
            // 
            this.uPwdDataGridViewTextBoxColumn.DataPropertyName = "UPwd";
            this.uPwdDataGridViewTextBoxColumn.HeaderText = "密码";
            this.uPwdDataGridViewTextBoxColumn.Name = "uPwdDataGridViewTextBoxColumn";
            this.uPwdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // registerEntiyBindingSource
            // 
            this.registerEntiyBindingSource.DataSource = typeof(ProxyService.RegisterEntiy);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 312);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "ProxyServer";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.registerEntiyBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lab1;
        private System.Windows.Forms.RichTextBox richOutPut;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label labelOnline;
        private System.Windows.Forms.DataGridViewTextBoxColumn ipDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn httpPortDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn socketPortDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uPwdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource registerEntiyBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnDial;
        private System.Windows.Forms.ToolStripMenuItem btnCancel;
    }
}

