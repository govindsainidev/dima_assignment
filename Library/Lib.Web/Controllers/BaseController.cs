using Lib.Services.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers
{
    public class BaseController : Controller
    {
        public static AppSettings _appSettings;
        protected internal string _connectionString => _appSettings.ConnectionString;
        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public BaseController()
        {
            
        }
    }
}
