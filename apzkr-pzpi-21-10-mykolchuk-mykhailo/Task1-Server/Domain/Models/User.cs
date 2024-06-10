namespace APZ_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegDate { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public ICollection<Car>? Cars { get; set; }
        public ICollection<Subordinate>? Subordinates { get; set; }
    }
}
