using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdBolck
{
    class APPConfigurtiong
    {
        public static string Cert, Key = null;
        static string UserDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AdBolck\\";
        static string JsonConfigurationFile = UserDataPath + "AdBolckConfiguration.json";
        static Dictionary<string, string> dict = new Dictionary<string, string>();
        //初始化配置信息（用于缓存证书）
        public bool InitializeComponent()
        {
            //判断是否存在目录
            if (!Directory.Exists(UserDataPath))
            {
                Directory.CreateDirectory(UserDataPath);
            }
            //判断是否存在配置文件，不存在则创建后返回false
            if (!File.Exists(JsonConfigurationFile)) {
                var SaveFile = this.SaveData();
                WriteFile(SaveFile);
                return false;
            }
            //存在则加载证书信息
            ReadFile(JsonConfigurationFile);
            //如果证书信息为空则返回fales
            if (Cert == null) {
                return false;
            }
            return true;
            
        }

        private string SaveData() {
            dict.Add("Cert", Cert);
            dict.Add("Key", Key);
            var ConvertJson = JsonConvert.SerializeObject(dict);
            return ConvertJson;
        }

        private static void WriteFile(string JsonData)
        {

            if (!File.Exists(JsonConfigurationFile))
            {
                FileStream file = new FileStream(JsonConfigurationFile, FileMode.Create, FileAccess.ReadWrite);
                file.Close();
            }
            File.WriteAllText(JsonConfigurationFile, JsonData);

        }

        public void UpdateFile() {

            if (Cert!=null) {
                WriteFile(this.SaveData());
            }
        }

        private static void ReadFile(string FileName)
        {
            var DeJsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FileName));
            Cert = DeJsonData["Cert"];
            Key = DeJsonData["Key"];

        }


        public string GetCert() {
            return Cert;
        }
        public string GetKey() {
            return Key;
        }

        public void SetCert(string CertName) {

            Cert = CertName;
        }

        public void SetKey(string KeyName) {
            Key = KeyName;
        }



    }
}
