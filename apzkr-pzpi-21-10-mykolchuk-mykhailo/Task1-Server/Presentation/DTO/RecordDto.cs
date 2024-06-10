namespace APZ_backend.DTO
{
    public class RecordDto
    {
        public int Id { get; set; }
        public DateTime TripStart { get; set; }
        public DateTime TripEnd { get; set; }
        public int CarId { get; set; }
        public int FastStart { get; set; }
        public int LeftTurns { get; set; }
        public int RightTurns { get; set; }
        public int DangerousLeftTurns { get; set; }
        public int DangerousRightTurns { get; set; }
        public ICollection<EngineSpeedMomentDto> EngineSpeeds { get; set; }
        public ICollection<BrakingMomentDto> SuddenBraking { get; set; }
    }
}
