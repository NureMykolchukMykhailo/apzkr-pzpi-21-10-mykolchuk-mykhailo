namespace APZ_backend.DTO
{
    public class EngineSpeedMomentDto
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public double AvgEngineSpeed { get; set; }
    }
}
