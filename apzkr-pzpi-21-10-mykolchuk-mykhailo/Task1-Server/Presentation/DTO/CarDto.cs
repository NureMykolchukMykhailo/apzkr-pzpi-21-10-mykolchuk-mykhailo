namespace APZ_backend.DTO
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string Type { get; set; }
        public DateTime Added { get; set; }
        public SensorForCarDto? Sensor { get; set; }
        public ICollection<SubordinateForCarDto>? Drivers { get; set; }
    }

    public class SensorForCarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SubordinateForCarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegDate { get; set; }
    }
}
