using BladeMill.BLL.Models;
using System;
using System.IO;
using System.IO.Compression;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Pakowanie katalogu przy uzycia Zip
    /// </summary>
    public class ZipService
    {
        public void PackFile(string currenttoolxmlfile)
        {
            var order = new BMOrder(currenttoolxmlfile);
            Console.WriteLine("OrderName:{0} OrderNameDir:{1}", order.OrderName, order.OrderNameDir);
            var zipOrder = order.OrderNameDir + ".zip";
            Console.WriteLine($"zipOrder = {zipOrder}");
            if (File.Exists(zipOrder))
            {
                Console.WriteLine($"Usuwanie ordera zipa {zipOrder}");
                File.Delete(zipOrder);
            }
            if (Directory.Exists(order.OrderNameDir))
            {
                Console.WriteLine($"Pakowanie ordera {order.OrderNameDir} na {zipOrder}");
                ZipFile.CreateFromDirectory(order.OrderNameDir, zipOrder);
            }
        }
    }
}
