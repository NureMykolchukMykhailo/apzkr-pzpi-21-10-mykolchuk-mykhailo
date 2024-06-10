namespace APZ_backend.Models
{
    public class BrakingMoment
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public Record Record { get; set; }
        public DateTime Time { get; set; }
        public double InitialSpeed { get; set; }
        public double SubsequentSpeed { get; set; }
    }
}
