using BladeMill.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Lista uzytkownikow BladeMilla
    /// </summary>
    public class User : Entity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "Please provide first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please provide SSO")]
        //[Range(8, 9, ErrorMessage = "Please provide value from range 12")]
        //[RegularExpression("([0-9])")]
        //[MaxLength(9), MinLength(9)]//dla stringu tylko!!!jak dla inta
        public int Sso { get; set; }
        public User() { }
        public User(int id, string firstName, string lastname, int sso)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastname;
            Sso = sso;
        }
        public override string ToString()
        {
            return "|" + Sso.ToString().PadRight(10, ' ') + "|" + LastName.ToString().PadRight(15, ' ') + "|";
        }
    }
}
