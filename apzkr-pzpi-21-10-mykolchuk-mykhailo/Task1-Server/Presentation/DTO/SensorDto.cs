namespace APZ_backend.DTO
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CarForSensorDto? Car { get; set; }
    }

    public class CarForSensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Added { get; set; }
        public ICollection<SubordinateForCarDto>? Drivers { get; set; }
    }
}
