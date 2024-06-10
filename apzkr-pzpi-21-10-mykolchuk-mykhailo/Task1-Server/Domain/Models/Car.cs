namespace APZ_backend.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Type { get; set; }
        public DateTime Added { get; set; }
        public int? SensorId { get; set; }
        public Sensor? Sensor { get; set; }
        public ICollection<Subordinate>? Drivers { get; set; }
        public ICollection<Record>? Records { get; set; }
    }
}
