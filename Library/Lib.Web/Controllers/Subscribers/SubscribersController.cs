using Lib.Services.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers.Subscribers
{
    public class SubscribersController : BaseController
    {
        public SubscribersController() 
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
