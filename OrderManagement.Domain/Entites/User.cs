namespace OrderManagement.Domain.Entites
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }

}
