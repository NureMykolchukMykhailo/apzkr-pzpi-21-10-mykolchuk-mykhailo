namespace APZ_backend.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public int? CarId { get; set; }
        public Car? Car { get; set; }
    }
}
