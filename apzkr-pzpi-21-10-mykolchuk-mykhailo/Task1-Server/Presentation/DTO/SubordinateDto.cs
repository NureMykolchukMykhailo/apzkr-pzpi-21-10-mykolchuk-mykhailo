namespace APZ_backend.DTO
{
    public class SubordinateDto
    {
        public int Id { get; set; }
        public int ChiefId { get; set; }
        public CarForSubordinateDto? Car { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegDate { get; set; }
        public string Language { get; set; }
    }

    public class CarForSubordinateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Added { get; set; }
        public SensorForCarDto? Sensor { get; set; }
    }
}
