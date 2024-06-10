using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APZ_IoT.Models
{
    internal class State
    {
        public int DeviceId { get; set; }
        public int SteeringWheelAngle { get; set; }
        public int SteeringWheelCount { get; set; }
        public bool ReturnWheelToBase { get; set; }
        public double Speed { get; set; }
        public int Acceleration { get; set; }
        public int EngineSpeed { get; set; }
        public int ConsiderFastStart { get; set; }
        public DateTime Time { get; set; }
        public DateTime EngineStarted { get; set; }

    }
}
