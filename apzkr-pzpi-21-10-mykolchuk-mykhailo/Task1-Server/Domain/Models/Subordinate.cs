namespace APZ_backend.Models
{
    public class Subordinate
    {
        public int Id { get; set; }
        public int ChiefId { get; set; }
        public User Chief { get; set; }
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegDate { get; set; }
        public string Language { get; set; }
    }
}
