using System;
using System.Collections.Generic;
using System.Threading;
using Fiddler;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;
using AdBolck.FormManage;

namespace AdBolck
{
    class FiddlerClass
    {
        private static bool Flag = true;
        private static APPConfigurtiong AppConfing = new APPConfigurtiong();
        static System.Windows.Forms.Label Label;
        static System.Windows.Forms.Timer Timer;
        static string Config = File.ReadAllText(@"Source\AutoBolckConfiguration.txt");

        
        public void FiddlerProxy() {
            //初始化配置信息
            var isTrue = AppConfing.InitializeComponent();
            if (!isTrue)
            {
                InstallCertificate();
            }
            else {
                FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.cert", AppConfing.GetCert());
                FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.key", AppConfing.GetKey());
            }
            Dictionary<string, string> AdBlockConfig = new Dictionary<string, string>();
            AdBlockConfig.Add("qiyi", "t7z.cupid.iqiyi.com");
            AdBlockConfig.Add("youku", "valf.atm.youku.com/vf");
            AdBlockConfig.Add("qq", "variety.tc.qq.com");
            AdBlockConfig.Add("youtube", "googleads.g.doubleclick.net");
            AdBlockConfig.Add("letv", "ark.letv.com");
            AdBlockConfig.Add("mgtv", "da.mgtv.com/pc");
            #region Resquest事件
            FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oSession) {
                oSession.bBufferResponse = true;
                if (oSession.uriContains("variety.tc.qq.com")
                            || oSession.uriContains("vd.l.qq.com/proxyhttp")
                            || oSession.uriContains("vlive.qqvideo.tc.qq.com")

                            )
                {
                    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                    EveQurey($"检测到腾讯视频的广告，程序自动过滤中，您无需作任何操作！");
                }
                if (oSession.uriContains("t7z.cupid.iqiyi.com"))
                {
                    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                    EveQurey($"检测到爱奇艺的广告，程序自动过滤中，您无需作任何操作！");
                }
                if (oSession.uriContains("vali.cp31.ott.cibntv.net")&& !oSession.uriContains("ccode"))
                {
                    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                    EveQurey($"检测到优酷的广告，程序自动过滤中，您无需作任何操作！");
                }
                if (oSession.uriContains("ark.letv.com")
                            || oSession.uriContains("fz.letv.com")
                            )
                {
                    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                    EveQurey($"检测到乐视TV的广告，程序自动过滤中，您无需作任何操作！");
                }
                if (oSession.uriContains("da.mgtv.com/pc"))
                {
                    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                    EveQurey($"检测到芒果TV的广告，程序自动过滤中，您无需作任何操作！");
                }
                
                
            };
            #endregion

            #region Response事件
            FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oSession)
            {
                //var Html = oSession.GetResponseBodyAsString();
                //foreach (var obj in Config.Split(';'))
                //{
                //    if (Html.Contains(obj) && obj != "")
                //    {

                //        oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                //        EveQurey($"检测到规则广告匹配项，程序将自动过滤处理，您无需作任何操作！");
                //    }
                //}
                //var Html = oSession.GetRequestBodyAsString();
                //if (Html.Contains("cps")) {
                //    oSession.oRequest.FailSession(404, "Blocked", "Fiddler blocked request");
                //}

            };
            #endregion

            //Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            Console.WriteLine("过滤器启动中......");
            Fiddler.FiddlerApplication.Startup(8887,FiddlerCoreStartupFlags.DecryptSSL);
            //Console.WriteLine("按Ctrl+C退出程序.");
            //while (Flag) {
            //    Thread.Sleep(1000);
            //}

        }

        private void EveQurey(string text) {
            Label = Program.autoBolck.label2;
            Timer = Program.autoBolck.timer1;
            Program.autoBolck.BeginInvoke(new Action(() => {
                Label.Text = text;
                Timer.Enabled = true;
                Timer.Interval = 120;
            }));
        }

        public string ResedStr(string str) {

            switch (str) {

                case "youku":
                    str = "优酷";
                    break;
                case "qiyi":
                    str = "爱奇艺";
                    break;
                case "qq":
                    str = "腾讯视频";
                    break;
                case "qqlive":
                    str = "腾讯视频";
                    break;
                case "youtube":
                    str = "Youtube";
                    break;
                case "letv":
                    str = "乐视TV";
                    break;
                case "letvLive":
                    str = "乐视TV";
                    break;
                case "mgtv":
                    str = "芒果TV";
                    break;
                default:
                    str = "未知站点";
                    break;
            }
            return str;
        }

        public static bool InstallCertificate()
        {
            if (!CertMaker.rootCertExists())
            {
                if (!CertMaker.createRootCert())
                    return false;

                if (!CertMaker.trustRootCert())
                    return false;
                AppConfing.SetCert(FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.cert", null));
                AppConfing.SetKey(FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.key", null));
                AppConfing.UpdateFile();
            }

            return true;
        }

        public static bool UninstallCertificate()
        {
            if (CertMaker.rootCertExists())
            {
                if (!CertMaker.removeFiddlerGeneratedCerts(true))
                    return false;
            }
            return true;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("程序即将退出...");
            e.Cancel = true;
            FiddlerApplication.Shutdown();
            //UninstallCertificate();
            Thread.Sleep(750);
            Flag = false;
        }











    }
}
