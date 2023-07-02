using Lib.Services;
using Lib.Services.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers
{
    public class BaseController : Controller
    {
        public static AppSettings _appSettings;
        public static AdminSettings _adminSettings;
        public static IGenericMapper _mapper;
        protected internal string _connectionString;
        public BaseController()
        {

            _mapper = ServiceActivator.GetScope().ServiceProvider.GetService<IGenericMapper>();
            _adminSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AdminSettings>>().Value;
            _appSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AppSettings>>().Value;
            _connectionString = _appSettings.ConnectionString;
        }
    }
}
