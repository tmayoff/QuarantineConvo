using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

namespace QuarantineConvo.Controllers
{
    public class MessagingController : Controller
    {

        private readonly QuarantineConvoContext db;

        public MessagingController(QuarantineConvoContext context)
        {
            db = context;
        }

        public IActionResult Index() {
            Connection c = db.Connection.FirstOrDefault();
            return View(c);
        }

        public IActionResult FindConnection()
        {
            var interests = db.Interest.ToList();

            return View(interests);
        }

        public IActionResult Search()
        {
            return Index();
        }

    }
}