namespace APZ_backend.Models
{
    public class State
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

        public override string ToString()
        {
            return SteeringWheelAngle.ToString() + Environment.NewLine +
                SteeringWheelCount.ToString() + Environment.NewLine +
                ReturnWheelToBase.ToString() + Environment.NewLine +
                Speed.ToString() + Environment.NewLine +
                Acceleration.ToString() + Environment.NewLine +
                EngineSpeed.ToString() + Environment.NewLine +
                ConsiderFastStart.ToString() + Environment.NewLine + 
                Time.ToString() + Environment.NewLine + 
                EngineStarted.ToString();
        }
    }
}
