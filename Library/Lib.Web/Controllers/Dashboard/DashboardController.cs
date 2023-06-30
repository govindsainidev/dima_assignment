using Lib.Services.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        public DashboardController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
