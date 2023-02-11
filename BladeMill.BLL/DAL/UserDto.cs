using BladeMill.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BladeMill.BLL.DAL
{
    public class UserDto : Entity//Data Transfer Object
    {
        [Required(ErrorMessage = "Please provide first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        public string LastName { get; set; }
        public int Sso { get; set; }
        public string? FullName { get; set; }
    }
}
