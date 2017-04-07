using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECClaimSystem.Controllers
{
    public class ECCoordinatorController : Controller
    {
        // GET: ECCoordinator
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {

            }
            return RedirectToAction("Login");
        }
    }
}