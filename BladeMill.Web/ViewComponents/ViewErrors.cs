using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.Web.ViewComponents
{
    public class ViewErrors : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string message)
        {
            var toolXmlFile = new ToolsXmlFile();
            var ncCodeCheckService = new NcCodeCheckService();
            var errors = await ncCodeCheckService.FindErrorsInNcCode(toolXmlFile.GetMainProgramFileFromCurrentToolsXml());
            errors.Where(m => m.Message == message);
            return View(errors);
        }
    }
}
