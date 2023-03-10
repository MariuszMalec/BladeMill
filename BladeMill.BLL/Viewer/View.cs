using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BladeMill.BLL.Viewer
{
    public class View
    {
        public void ShowUsers(IEnumerable<User> users)
        {
            if (users.Count() > 0)
            {
                var textPaddingWidth = 15;
                var paddingChar = ' ';
                var numberOfCollumn = 4;
                Console.WriteLine(("").PadRight(textPaddingWidth * numberOfCollumn, '='));
                Console.WriteLine($"|{"FirstName".PadRight(textPaddingWidth, paddingChar)} " +
                                  $"|{"LastName".PadRight(textPaddingWidth, paddingChar)} " +
                                  $"|{"SSO".PadRight(textPaddingWidth, paddingChar)}");
                Console.WriteLine(("").PadRight(textPaddingWidth * numberOfCollumn, '='));
                if (users != null)
                {
                    foreach (var item in users)
                    {
                        Console.WriteLine($"|{item.FirstName.ToString().PadRight(textPaddingWidth, paddingChar)} " +
                                          $"|{item.LastName.ToString().PadRight(textPaddingWidth, paddingChar)} " +
                                          $"|{item.Sso.ToString().PadRight(textPaddingWidth, paddingChar)}");

                    }
                    Console.WriteLine(("").PadRight(textPaddingWidth * numberOfCollumn, '='));
                    Console.WriteLine($"Press any key to continue");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"No users, Please add new user");
                Console.WriteLine($"Press any key to continue");
                Console.ReadKey();
            }
        }
    }
}
