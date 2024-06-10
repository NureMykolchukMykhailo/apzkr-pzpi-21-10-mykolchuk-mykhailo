namespace APZ_backend.Models
{
    public class Record
    {
        public int Id { get; set; }
        public DateTime TripStart { get; set; }
        public DateTime TripEnd { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int FastStart { get; set; }
        public int LeftTurns { get; set; }
        public int RightTurns { get; set; }
        public int DangerousLeftTurns { get; set; }
        public int DangerousRightTurns { get; set; }
        public ICollection<EngineSpeedMoment> EngineSpeeds { get; set; }
        public ICollection<BrakingMoment> SuddenBraking { get; set; }
    }
}
