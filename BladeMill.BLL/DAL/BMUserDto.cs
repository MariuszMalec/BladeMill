using System;
using System.ComponentModel.DataAnnotations;

namespace BladeMill.BLL.DAL
{
    public class BMUserDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Please provide first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        public string LastName { get; set; }
        public int Sso { get; set; }
        public string? FullName { get; set; }
    }
}
