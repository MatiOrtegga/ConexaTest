namespace ConexaTest.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual Role Role { get; set; } = null!;
    }
}
