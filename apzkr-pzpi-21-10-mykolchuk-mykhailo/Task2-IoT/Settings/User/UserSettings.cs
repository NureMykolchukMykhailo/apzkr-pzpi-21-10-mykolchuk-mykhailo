using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APZ_IoT.Settings.User
{
    internal class UserSettings
    {
        public string DeviceId { get; set; }
        public string MaxEngineSpeed { get; set; }
        public string MinEngineSpeed { get; set; }
        public string MaxAcceleration { get; set; }
        public string MinAcceleration { get; set; }
        public string MaxSpeed { get; set; }
    }
}
