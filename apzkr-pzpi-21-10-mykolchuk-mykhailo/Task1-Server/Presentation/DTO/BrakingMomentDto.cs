namespace APZ_backend.DTO
{
    public class BrakingMomentDto
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public DateTime Time { get; set; }
        public double InitialSpeed { get; set; }
        public double SubsequentSpeed { get; set; }
    }
}
