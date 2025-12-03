using System;

namespace FastPayApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty; // identifier
        public string Email { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0m; // simple wallet balance
        public string Pin { get; set; } = string.Empty; // simple auth for demo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}