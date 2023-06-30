using Lib.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers.Books
{
    public class BooksController : BaseController
    {
        public BooksController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
