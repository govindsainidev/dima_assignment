using Lib.Services.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        public DashboardController() 
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
