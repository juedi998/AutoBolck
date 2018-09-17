using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AdBolck.FormManage
{
    public partial class AutoBolck : Form
    {

        FiddlerClass ss = new FiddlerClass();
        RegistryKey Regy_Key;
        static object Auto_Url = null;
        static int Labeleft = 0;
        private bool isMouseDown = false;  //记录鼠标是否被按下
        private Point position;  //记录鼠标位置
        public AutoBolck()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            //this.Region = new Region(SetAlltypeWindow(this, @"C:\Users\Naruto\Desktop\UI.jpg", Color.FromArgb(0, 0, 0, 0)));
            InitializeComponent();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Regy_Key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (Auto_Url == null&& Regy_Key.GetValue("AutoConfigURL")!=null) {
                Auto_Url = Regy_Key.GetValue("AutoConfigURL");
            }
            if (this.checkBox1.CheckState == CheckState.Checked)
            {
                DialogResult dr = MessageBox.Show("开启广告过滤功能将会接管当前系统代理设置，是否继续？", "开启全局广告过滤", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Regy_Key.SetValue("ProxyEnable", 1);
                    Regy_Key.SetValue("ProxyServer", "127.0.0.1:8887");
                    Regy_Key.SetValue("AutoConfigURL", 0);
                    //激活代理设置
                    InternetSetOption(0, 39, IntPtr.Zero, 0);
                    InternetSetOption(0, 37, IntPtr.Zero, 0);
                }
                else {
                    this.checkBox1.Checked = false;
                }
                
            }
            else {
                Regy_Key.SetValue("AutoConfigURL", Auto_Url!=null?Auto_Url:"");
                Regy_Key.SetValue("ProxyEnable", 0);
                //激活代理设置
                InternetSetOption(0, 39, IntPtr.Zero, 0);
                InternetSetOption(0, 37, IntPtr.Zero, 0);
            }



        }

        private void AutoBolck_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Hi，感谢您使用本程序，当前程序的广告过滤系统仅支持过滤爱奇艺、优酷、腾讯视频、乐视TV、芒果TV等节目预播前广告哟，更多过滤规则正处于开发阶段，请谅解，如您有兴趣参与我们的开源计划方案欢迎造访：https://github.com/juedi998/AdBolck，本程序仅供学习与研究之用，请遵循网站的相关协议，一旦开启广告过滤功能即表明您已授权本程序进行对目标站点过滤，如您不确定请关闭本程序！最后祝您畅游愉快！",  "温馨提示：", MessageBoxButtons.OK,MessageBoxIcon.Information);
            Labeleft = label2.Left;
            ss.FiddlerProxy();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Left -= 20;
            if (label2.Right < 0)
            {
                label2.Text = "";
                label2.Left = Labeleft;
                timer1.Enabled = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //Labeleft = label2.Left;
            //if (label2.Text != null) {
            //    timer1.Interval = 200;
            //    timer1.Enabled = true;
            //}
        }

        [DllImport(@"wininet",
        SetLastError = true,
        CharSet = CharSet.Auto,
        EntryPoint = "InternetSetOption",
        CallingConvention = CallingConvention.StdCall)]
        public static extern bool InternetSetOption
        (
        int hInternet,
        int dmOption,
        IntPtr lpBuffer,
        int dwBufferLength
        );

        private void AutoBolck_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = -e.X;
                int y = -e.Y;
                position = new Point(x, y);
                isMouseDown = true;
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.CheckState == CheckState.Checked)
            {
                MessageBox.Show("退出程序之前请关闭全局代理哟，否则网页将无法访问！", "关闭程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                this.Close();
                Application.Exit();
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本程序目前暂不支持最大化，感谢您的支持！", "窗体最大化", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void AutoBolck_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point newPosition = Control.MousePosition;
                newPosition.Offset(position);
                this.Location = newPosition;
            }
        }

        private void AutoBolck_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        private void 加载文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本功能尚在研发阶段!", "一键导入广告过滤规则", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 系统广告捕获ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本功能尚在研发阶段!", "抓取系统弹窗广告", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 视频网站广告拦截ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本程序目前已实现部分视频网站的广告过滤功能，详情请查阅帮助文档！", "视频广告拦截", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void vIP视频服务ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本功能将在第二阶段完善，感谢支持！", "VIP视频服务", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void autoBlock帮助文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"感谢使用本程序，本帮助文档将指引您如何使用本程序，在使用前，请确保您已经正常接入互联网，本程序核心组件为FiddlerCore，以MITM（即中间攻击）的形式对流量进行一系列的操作，程序的设计初衷仅为了学习与研究一系列广告过滤，无任何针对性的攻击等行为；阁下在初次使用时，需按要求安装一份系统证书，将其设为信任后，届时程序会在阁下的系统中生成一份专门用于缓存证书的文件，它位于阁下系统的：Users\AppData\Roaming\AdBolck 目录之中，证书有一定的时效性，如证书过期后，需阁下手工到该目录下删除证书，然后重新运行程序；另外，因本程序可能会对相关网站或厂商造成一定的损失（某些企业依赖广告为生），故请阁下在使用之前谨慎考虑，由本程序操作带来的一系列法律相关责任与本程序及所有参与本项目的开发者无关，请阁下知悉；目前我们的程序仅支持爱奇艺、优酷、腾讯视频、乐视TV、芒果TV等节目预播前广告的过滤，更多过滤规则还处于开发的阶段，如您有兴趣参与我们的开源计划欢迎前往：https://github.com/juedi998/AdBolck，本程序仅供学习与研究之用，请遵循网站的相关协议，一旦开启广告过滤功能即表明您已授权本程序进行对目标站点过滤，如您不确定请关闭本程序！最后祝您畅游愉快！", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
