using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.Interfaces
{
    public interface IXmlService
    {
        string GetFromFileValue(string xmlFile, string findtext);
    }
}
