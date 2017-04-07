using ECClaimSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ECClaimSystem.Models.Elements.ConstantsKey;

namespace ECClaimSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IECClaimRepository _ecClaimRepository;
        public UserController()
        {
            this._ecClaimRepository = new ECClaimRepository();
        }

        // GET: User
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {

            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login(string message)
        {
            if (message != null || message != "")
            {
                ViewBag.Error = message;
            }
            return View("Login");
        }
        [HttpPost]
        public ActionResult LoginAccount(string username, string password)
        {
            string errormessage = null;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
            {
                User user = _ecClaimRepository.Login(username.ToLower(), password);
                if (user != null)
                {
                    string controllerName = null;
                    switch ((Models.Elements.ConstantsKey.Role)user.RoleId)
                    {
                        case Models.Elements.ConstantsKey.Role.Student:
                            controllerName = "Student";
                            break;
                        case Models.Elements.ConstantsKey.Role.ECManager:
                            controllerName = "ECManager";

                            break;
                        case Models.Elements.ConstantsKey.Role.ECCoordinator:
                            controllerName = "ECCoordinator";

                            break;
                        case Models.Elements.ConstantsKey.Role.Administrator:
                            controllerName = "Administrator";

                            break;
                        default:
                            break;

                    }
                    Session["User"] = user;
                    return RedirectToAction("Index", controllerName);
                }
                errormessage = "This account is incorrect";
            }
            errormessage = "Check data again!";
            return View("Login", new { message = errormessage });
        }

        [HttpGet]
        public ActionResult ViewListEC()
        {
            if (Session["User"] != null)
            {
                User user = Session["User"] as User;
                string controllerName = GetControllerNameFromUser();
                List<ECClaim> lsClaim = _ecClaimRepository.GetListECClaimWithSpecificUser(user);
                Session["ListClaim"] = lsClaim;
                return RedirectToAction("ShowListECClaim", controllerName);
            }
            return View("Login");
        }

        public ActionResult SortECClaim(int type)
        {
            List<ECClaim> lsClaim = null;
            string controllerName = GetControllerNameFromUser();
            if (Session["ListClaim"] != null)
            {
                lsClaim = Session["ListClaim"] as List<ECClaim>;
                switch (type)
                {
                    case (int)SortType.ClosureDate:
                        lsClaim = lsClaim.OrderByDescending(x => x.FinalClosureDate).ToList();
                        break;
                   
                    default: break;
                }
                Session["ListClaim"] = lsClaim;
            }
            return RedirectToAction("ShowListECClaim", controllerName);

        }

        public ActionResult DeleteECClaim(int id)
        {
            List<ECClaim> lsClaim = null;
            string controllerName = GetControllerNameFromUser();
            if (Session["ListClaim"] != null)
            {
                _ecClaimRepository.DeleteECClaim(id);
                lsClaim = Session["ListClaim"] as List<ECClaim>;
                lsClaim.Remove(lsClaim.Where(x => x.ClaimId == id).SingleOrDefault());
                Session["ListClaim"] = lsClaim;
            }
            return RedirectToAction("ShowListECClaim", controllerName);
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        private string GetControllerNameFromUser()
        {
            string controllerName = null;
            if (Session["User"] != null)
            {
                User user = Session["User"] as User;
                switch ((Models.Elements.ConstantsKey.Role)user.RoleId)
                {
                    case Models.Elements.ConstantsKey.Role.Student:
                        controllerName = "Student";
                        break;
                    case Models.Elements.ConstantsKey.Role.ECManager:
                        controllerName = "ECManager";

                        break;
                    case Models.Elements.ConstantsKey.Role.ECCoordinator:
                        controllerName = "ECCoordinator";

                        break;
                    case Models.Elements.ConstantsKey.Role.Administrator:
                        controllerName = "Administrator";

                        break;
                    default:
                        break;

                }
            }
            return controllerName;
        }
    }
}