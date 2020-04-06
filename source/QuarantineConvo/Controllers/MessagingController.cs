using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

namespace QuarantineConvo.Controllers {
    public class MessagingController : Controller {

        QuarantineConvoContext db;

        public MessagingController (QuarantineConvoContext _db) 
        {
            db = _db;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Connections()
        {
            Connection c = db.Connection.FirstOrDefault();
            string tempUser = "smeath";
            return View();
        }
    }
}