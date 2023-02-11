using BladeMill.BLL.Interfaces;

namespace BladeMill.BLL.Services
{
    public class XmlService
    {
        private readonly IXmlService _xmlService;

        public XmlService(IXmlService xmlService)
        {
            _xmlService = xmlService;
        }

        public string GetFromFileValue(string xmlFile, string findtext)
        {
            return _xmlService.GetFromFileValue(xmlFile, findtext);
        }
    }
}
