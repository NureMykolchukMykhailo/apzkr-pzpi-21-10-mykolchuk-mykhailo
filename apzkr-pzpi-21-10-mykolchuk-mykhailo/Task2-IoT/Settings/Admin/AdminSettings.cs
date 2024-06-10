using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APZ_IoT.Settings.Admin
{
    internal class AdminSettings
    {
        public string BootstrapServers { get; set; }
        public string KafkaTopic { get; set; }
        public string AuthorizationAdress { get; set; }
    }
}
