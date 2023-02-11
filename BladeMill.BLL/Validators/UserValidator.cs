using BladeMill.BLL.DatatAcess;
using BladeMill.BLL.DatatBaseAcess;
using System;
using System.Linq;

namespace BladeMill.BLL.Validators
{
    public class UserValidator
    {
        public string ValidateSSO(int Sso, int minValue, ApplicationDbContext _db)
        {
            string inputString = Convert.ToString(Sso);
            if (inputString.Length != minValue)
                return $"Invalid SSO number! Have to be {minValue} numbers. Retry!";

            var check = _db.Uzytkownicy.ToArray().Select(u => u.Sso == Sso);
            if (check.Contains(true))
            {
                return $"Invalid SSO number! Exist yet!";
            }
            return string.Empty;
        }
    }
}
