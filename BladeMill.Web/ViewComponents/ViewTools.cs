using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BladeMill.Web.ViewComponents
{
    public class ViewTools : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string ncProgram)
        {
            var toolService = new ToolNcService();
            var viewTools = toolService.GetAll();//.Where(m => m.NcProgram == ncProgram);
            return View(viewTools);
        }
    }
}
