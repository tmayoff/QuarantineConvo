using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QuarantineConvo.Controllers
{
    public class MessagingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}