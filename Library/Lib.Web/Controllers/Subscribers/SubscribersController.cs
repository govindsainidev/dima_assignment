﻿using Lib.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lib.Web.Controllers.Subscribers
{
    public class SubscribersController : BaseController
    {
        public SubscribersController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
