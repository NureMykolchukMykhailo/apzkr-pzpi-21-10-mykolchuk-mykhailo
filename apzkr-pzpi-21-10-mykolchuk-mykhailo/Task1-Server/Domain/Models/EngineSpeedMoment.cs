namespace APZ_backend.Models
{
    public class EngineSpeedMoment
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public Record Record { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public double AvgEngineSpeed { get; set; }
    }
}
