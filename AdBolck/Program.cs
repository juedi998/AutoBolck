using AdBolck.FormManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdBolck
{
    public class Program
    {
        public static AutoBolck autoBolck = new AutoBolck();
        static void Main(string[] args) {
            autoBolck.ShowDialog();
        }


    }
}
