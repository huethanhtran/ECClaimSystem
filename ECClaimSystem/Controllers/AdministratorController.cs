using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECClaimSystem.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {

            }
            return RedirectToAction("Login");
        }
    }
}