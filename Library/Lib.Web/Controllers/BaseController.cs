using Lib.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers
{
    public class BaseController : Controller
    {
        public static AppSettings _appSettings;

        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
    }
}
