namespace OrderManagement.Domain.Entites
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string TokenHash { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }
    }
}
