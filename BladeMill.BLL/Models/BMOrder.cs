using BladeMill.BLL.Services;
using System.IO;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Podstawowe dane ordera BladeMilla
    /// </summary>
    public class BMOrder
    {
        public string OrderName { get; set; }
        public string OrderNameDir { get; set; }
        private string CurrentToolXml { get; set; }
        public BMOrder(string currentToolXml)
        {
            CurrentToolXml = currentToolXml;
            OrderName = GetOrderName();
            OrderNameDir = GetOrderNameDir();
        }
        private string GetOrderName()
        {
            var xmlService = new ToolXmlService();
            var orderName = xmlService.GetFromFileValue(CurrentToolXml, "PRGNUMBER");
            return orderName;
        }

        private string GetOrderNameDir()
        {
            var appGetDir = new AppXmlConfDirectories();
            var orderNameDir = appGetDir.MFG_ORDER_DIR;
            var orderFile = Path.Combine(orderNameDir, OrderName);
            return orderFile;
        }
    }
}
