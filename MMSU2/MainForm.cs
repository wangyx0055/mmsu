using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace MMSU
{
	public class MainForm : Form
	{
		private ArrayList macs = new ArrayList();
		private ArrayList allMacs = new ArrayList();
		private ArrayList iplist = new ArrayList();
        private IContainer components;
		private Button button6;
		private Button button5;
		private Button button4;
		private Button button3;
		private ListBox listBox2;
		private Button button2;
		private ListBox listBox1;
        private Label label1;
        private Label label2;
        private Label label3;
		private Button button1;
		public MainForm()
		{
			this.InitializeComponent();
			base.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
		}
		private void Button1Click(object sender, EventArgs e)
		{
			this.listBox1.Items.Clear();
			this.macs.Clear();
			this.allMacs.Clear();
			this.allMacs = libGetAdaptersInfo.GetAdapters();
			foreach (object current in this.allMacs)
			{
				if (((Macs)current).IPAddress != "0.0.0.0")
				{
					this.macs.Add(current);
				}
			}
			foreach (object current2 in this.macs)
			{
				if (((Macs)current2).IPAddress == "0.0.0.0")
				{
					this.macs.Remove(current2);
				}
				this.listBox1.Items.Add(current2);
			}
		}
		private void Button2Click(object sender, EventArgs e)
		{
			string text = "";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "D:\\";
			openFileDialog.Filter = "All files (*.*)|*.*|加速程序 files (*.exe)|*.exe";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				text = openFileDialog.FileName;
			}
			foreach (object current in this.listBox2.Items)
			{
				if (("\"" + text + "\"").Equals(current))
				{
					MessageBox.Show("此程序已经添加");
					return;
				}
			}
			if (text != "")
			{
				this.listBox2.Items.Add("\"" + text + "\"");
			}
		}
		private void Button3Click(object sender, EventArgs e)
		{
			if (this.listBox1.SelectedIndex == -1 || this.listBox2.SelectedIndex == -1)
			{
				MessageBox.Show("选择网卡和应用程序");
				return;
			}
            Macs macs = this.listBox1.SelectedItem as Macs;
            string fileName = "ForceBindIP.exe";
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.UseShellExecute = false;
            processStartInfo.FileName = fileName;
            
            for (int j = listBox2.SelectedIndex; j <= listBox2.Items.Count - 1; j++)
            {
                listBox2.SelectedIndex = j;
                processStartInfo.Arguments = "-i " + macs.IPAddress + " " + this.listBox2.SelectedItem.ToString();
                this.Text = macs.ToString() + " " + this.listBox2.SelectedItem.ToString();
                Process.Start(processStartInfo);
               
            }
		}
		private void Button4Click(object sender, EventArgs e)
		{
			if (this.macs.Count < 2)
			{
				MessageBox.Show("提速只能用于2个以上网络连接");
				return;
			}
			int num = 0;
			foreach (object current in this.macs)
			{
				if (((Macs)current).IPAddress != null)
				{
					num++;
				}
			}
			if (num < this.macs.Count)
			{
				MessageBox.Show("网卡配置出错，请尝试关闭网卡并重新开启");
				return;
			}
			Routes routes = new Routes();
			int i = 1;
			for (int j = 1; j < 255; j++)
			{
				if (j != 127 && j != 192)
				{
					if (j != 224)
					{
						while (i <= this.macs.Count)
						{
							object obj = this.macs[i - 1];
							obj.ToString();
							routes.createIpForwardEntry(Routes.IPToInt(j + ".0.0.0"), Routes.IPToInt("255.0.0.0"), Routes.IPToInt(((Macs)obj).DefaultIPGateway), Routes.IPToInt2(((Macs)obj).InterfaceIndex), Routes.countMetric(this.macs.Count, i));
							i++;
							j++;
							if (i > this.macs.Count)
							{
								i = 1;
							}
							if (j == 255)
							{
								break;
							}
						}
					}
				}
			}
			this.button6.Enabled = true;
			this.button4.Enabled = false;
		}
		private void Button5Click(object sender, EventArgs e)
		{
			if (this.listBox2.SelectedIndex == -1)
			{
				MessageBox.Show("没有选择应用程序");
				return;
			}
			this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
			this.iplist.Clear();
		}
		private void Button6Click(object sender, EventArgs e)
		{
			int num = 0;
			foreach (object current in this.macs)
			{
				Routes routes = new Routes();
				routes.createIpForwardEntry(Routes.IPToInt("0.0.0.0"), Routes.IPToInt("0.0.0.0"), Routes.IPToInt(((Macs)current).DefaultIPGateway), Routes.IPToInt2(((Macs)current).InterfaceIndex), 20 + num);
				num += 5;
			}
			Routes routes2 = new Routes();
			int i = 1;
			for (int j = 1; j < 255; j++)
			{
				if (j != 127 && j != 192)
				{
					if (j != 224)
					{
						while (i <= this.macs.Count)
						{
							object obj = this.macs[i - 1];
							routes2.deleteIpForwardEntry(Routes.IPToInt(j + ".0.0.0"), Routes.IPToInt("255.0.0.0"), Routes.IPToInt(((Macs)obj).DefaultIPGateway), Routes.IPToInt2(((Macs)obj).InterfaceIndex));
							i++;
							j++;
							if (i > this.macs.Count)
							{
								i = 1;
							}
							if (j == 255)
							{
								break;
							}
						}
					}
				}
			}
			this.button4.Enabled = true;
			this.button6.Enabled = false;
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.button6.Enabled)
			{
				int num = 0;
				foreach (object current in this.macs)
				{
					Routes routes = new Routes();
					routes.createIpForwardEntry(Routes.IPToInt("0.0.0.0"), Routes.IPToInt("0.0.0.0"), Routes.IPToInt(((Macs)current).DefaultIPGateway), Routes.IPToInt2(((Macs)current).InterfaceIndex), 20 + num);
					num += 5;
				}
				int i = 1;
				for (int j = 1; j < 255; j++)
				{
					if (j != 127 && j != 192)
					{
						if (j != 224)
						{
							while (i <= this.macs.Count)
							{
								object obj = this.macs[i - 1];
								Routes routes2 = new Routes();
								routes2.deleteIpForwardEntry(Routes.IPToInt(j + ".0.0.0"), Routes.IPToInt("255.0.0.0"), Routes.IPToInt(((Macs)obj).DefaultIPGateway), Routes.IPToInt2(((Macs)obj).InterfaceIndex));
								i++;
								j++;
								if (i > this.macs.Count)
								{
									i = 1;
								}
								if (j == 255)
								{
									break;
								}
							}
						}
					}
				}
			}
			switch (MessageBox.Show("确定要退出吗?", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1))
			{
			case DialogResult.Yes:
				break;
			case DialogResult.No:
				e.Cancel = true;
				break;
			default:
				return;
			}
		}
		private void Button7Click(object sender, EventArgs e)
		{
			MessageBox.Show("[请确定是管理员身份]\n指定匹配功能：\n1.用<检测网卡>检测出当前的网卡，支持2个以上的网卡\n2.指定应用程序使用指定的网卡，先选择网卡，再选择应用程序，最后点击<开始匹配>\nP2P下载提速功能:\n1.用<检测网卡>检测出当前的网卡，支持2个以上的网卡\n2.点击<P2P下载提速>。\n\n如果在使用当中出现问题上不去网，请重新启动计算机", "帮助");
		}
		private void Button8Click(object sender, EventArgs e)
		{
			MessageBox.Show("Firefox\nChrome\n迅雷精简版\n这几款软件配合使用能足够满足一般需求。", "推荐配合使用");
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(70, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "检测网卡";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.LemonChiffon;
            this.listBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(364, 88);
            this.listBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(70, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "选择应用";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.Color.LemonChiffon;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(12, 164);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(364, 88);
            this.listBox2.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(70, 258);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "开始匹配";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(167, 135);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "删除应用";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5Click);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(280, 258);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(96, 23);
            this.button6.TabIndex = 8;
            this.button6.Text = "停止提速";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(167, 258);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "P2P下载提速";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "第一步：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "第二步：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "第三步：";
            // 
            // MainForm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(389, 286);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网速叠加工具 QQ群:365628167  by sysorem";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

       
	}
}
