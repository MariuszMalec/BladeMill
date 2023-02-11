using System;

namespace BladeMill.BLL.Models
{
    public class UserFromJson
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
