using BladeMill.BLL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BladeMill.ConsoleApp.HowToReadConfFromJson
{
    public class ReadJson
    {
        public static List<UserFromJson> ReadUserFromJsonFile()
        {
            string fileName = @"users.json";
            string jsonString = File.ReadAllText(fileName);
            List<UserFromJson> userData = JsonConvert.DeserializeObject<List<UserFromJson>>(jsonString);
            return userData;
        }
    }
}
