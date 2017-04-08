using ECClaimSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ECClaimSystem.Models.Elements.ConstantsKey;

namespace ECClaimSystem.Controllers
{
    public class AdministratorController : Controller
    {
        IECClaimRepository _ecClaimRepository;
        public AdministratorController()
        {
            _ecClaimRepository = new ECClaimRepository();
        }
        // GET: Administrator
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                List<Faculty> lsFaculty = _ecClaimRepository.GetAllFaculty();
                Session["Faculties"] = lsFaculty;
                return RedirectToAction("ViewListEC", "User");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult ShowListECClaim()
        {
            List<ECClaim> lsClaim = Session["ListClaim"] as List<ECClaim>;
            List<Models.ECClaim> lsECClaim = new List<Models.ECClaim>();
            if (lsClaim == null || lsClaim.Count == 0)
            {
                ViewBag.Message = "There is no Ec claim";
            }
            else
            {
                lsECClaim = lsClaim.Select(x => new Models.ECClaim
                {
                    ClaimId = x.ClaimId,
                    User = new Models.User { UserId = x.UserId.Value },
                    Summary = x.Summary,
                    Situation = x.Situation,
                    EffectSituation = x.EffectSituation,
                    CircumstanceStartDate = x.CircumstanceStartDate,
                    CircumstanceEndDate = x.CircumstanceEndDate,
                    OutCome = x.OutCome.Value,
                    FinalClosureDate = x.FinalClosureDate,
                    SubmittedDate = x.SubmittedDate,
                    ClaimStatus = x.ClaimStatus,
                    DecisionStatus = x.DecisionStatus,
                    DecisionDate = x.DecisionDate.HasValue ? x.DecisionDate.Value : (DateTime?)null,
                    ProcessedUser = x.ProcessedUser,
                    HasEvidence = x.HasEvidence.Value,
                    Active = x.Active.Value
                }).ToList();

            }
            return View(lsECClaim);
        }
        public ActionResult Details(long id)
        {
            ECClaim ec = _ecClaimRepository.GetECClaimById(id);
            Models.ECClaim claim = new Models.ECClaim();
            if (ec != null)
            {
                claim.ClaimId = ec.ClaimId;
                claim.User = new Models.User { UserId = ec.UserId.Value };
                claim.Summary = ec.Summary;
                claim.Situation = ec.Situation;
                claim.EffectSituation = ec.EffectSituation;
                claim.CircumstanceStartDate = ec.CircumstanceStartDate;
                claim.CircumstanceEndDate = ec.CircumstanceEndDate;
                claim.OutCome = ec.OutCome.Value;
                claim.FinalClosureDate = ec.FinalClosureDate;
                claim.SubmittedDate = ec.SubmittedDate;
                claim.ClaimStatus = ec.ClaimStatus;
                claim.DecisionStatus = ec.DecisionStatus;
                claim.DecisionDate = ec.DecisionDate.HasValue ? ec.DecisionDate.Value : (DateTime?)null;
                claim.ProcessedUser = ec.ProcessedUser;
                claim.HasEvidence = ec.HasEvidence.Value;
                claim.Active = ec.Active.Value;
            }
            return View("Details", claim);
        }
        [HttpPost]
        public ActionResult ChangeActiveStatusOfECClaim(bool value, long claimId)
        {
            string response = _ecClaimRepository.ChangeActiveOfECClaim(claimId, value);
            return Json(response);
        }

        public ActionResult GetAllUser()
        {
            if (Session["User"] != null)
            {
                List<User> lsUser = _ecClaimRepository.GetAllUser();
                List<Models.User> ls = new List<Models.User>();
                if (lsUser != null)
                {
                    ls = lsUser.Select(x => new Models.User
                    {
                        AccountId = x.AccountId,
                        Active = x.Active.Value,
                        Address = x.Address,
                        BirthDay = x.BirthDay,
                        Email = x.Email,
                        Gender = x.Gender,
                        Phone = x.Phone,
                        UserFullName = x.UserFullName,
                        UserId = x.UserId,
                        Role = new Models.Role { RoleName = x.Role.RoleName, RoleId = x.Role.RoleId, Active = x.Role.Active.Value }

                    }).ToList();
                }
                else
                {
                    ViewBag.Message = "There is no user";
                }
                return View(ls);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult ChangeActiveStatusUser(long userId, bool value)
        {
            string response = _ecClaimRepository.ChangeActiveStatusUser(userId, value);
            return Json(response);
        }

        public ActionResult Edit(long id)
        {
            User u = _ecClaimRepository.GetUserById(id);
            Models.User user = new Models.User();
            if (u != null)
            {
                user.Address = u.Address;
                user.BirthDay = u.BirthDay;
                user.Email = u.Email;
                user.FacultyId = u.FacultyId.HasValue ? u.FacultyId.Value : 0;
                user.Gender = u.Gender;
                user.Phone = u.Phone;
                user.RoleId = u.RoleId;
                user.UserFullName = u.UserFullName;
                user.UserId = u.UserId;
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpdateUser(Models.User model)
        {
            if (ModelState.IsValid)
            {
                int result = _ecClaimRepository.UpdateUser(model);
                if (result == 0)
                {
                    ViewBag.Message = "Check information again, at least your email";
                }
                else
                {
                    return RedirectToAction("Edit", new { id = model.UserId });
                }
            }
            return View("Edit", model);
        }

        public ActionResult CancelUpdateUser()
        {
            return RedirectToAction("GetAllUser");
        }

        public ActionResult CreateUser()
        {
            if (Session["User"] != null)
            {
                return View("CreateUser", new Models.User());
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateNewUser(Models.User model)
        {
            if (ModelState.IsValid)
            {
                string[] SplitStrings = model.Email.Split('@');
                model.AccountId = SplitStrings[0];
                model.Password = model.AccountId;
                int result = _ecClaimRepository.InsertUser(model);
                if (result == 0)
                {
                    ViewBag.Message = "Check information again";
                }
                else
                {
                    return RedirectToAction("GetAllUser");
                }
            }
            return View("CreateUser", model);
        }

        public ActionResult GetAllSetting()
        {
            if (Session["User"] != null)
            {
                List<Setting> lsSetting = _ecClaimRepository.GetAllSetting();
                List<Models.Setting> ls = new List<Models.Setting>();
                if (lsSetting != null)
                {
                    ls = lsSetting.Select(x => new Models.Setting
                    {
                        Active = x.Active.Value,
                        Id = x.Id,
                        Key = x.Key,
                        Value = x.Value
                    }).ToList();
                }
                else
                {
                    ViewBag.Message = "There is no setting";
                }
                return View("Setting", ls);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult SubmitSetting(int? id, string key, string value)
        {
            if (key == null || key == "")
            {
                ViewBag.ErrorKey = "This field cannot be empty";
                return RedirectToAction("GetAllSetting");
            }
            if (_ecClaimRepository.GetSettingByKey(key) != null && !id.HasValue)
            {
                ViewBag.ErrorKey = "This key existed";
                return RedirectToAction("GetAllSetting");
            }
            _ecClaimRepository.UpdateOrInsertSetting(id, key, value);
            Session.Remove("Setting");
            return RedirectToAction("GetAllSetting");
        }

        public ActionResult EditSetting(int id)
        {
            Setting setting = _ecClaimRepository.GetSettingById(id);
            Session["Setting"] = setting;
            return RedirectToAction("GetAllSetting");
        }

        public ActionResult CancelCreateSetting()
        {
            Session.Remove("Setting");
            return RedirectToAction("GetAllSetting");
        }
        [HttpPost]
        public ActionResult ChangeActiveStatusSetting(bool value, int id)
        {
            string response = _ecClaimRepository.ChangeActiveSetting(id, value);
            return Json(response);
        }

        public ActionResult CancelCreateUser()
        {
            return RedirectToAction("CreateUser");
        }
        [HttpPost]
        public ActionResult SearchSetting(string searchkey)
        {
            List<Setting> lsSetting = _ecClaimRepository.SearchSetting(searchkey);
            List<Models.Setting> ls = new List<Models.Setting>();
            if (lsSetting != null)
            {
                ls = lsSetting.Select(x => new Models.Setting
                {
                    Active = x.Active.Value,
                    Id = x.Id,
                    Key = x.Key,
                    Value = x.Value
                }).ToList();
            }
            else
            {
                ViewBag.Message = "There is no setting";
            }
            return View("Setting", ls);
        }

        public ActionResult RedirectStatisticPage()
        {
            if (Session["User"] != null)
            {
                List<Faculty> lsFaculty = _ecClaimRepository.GetAllFaculty();
                Session["Faculties"] = lsFaculty;
                List<ECClaim> lsClaim = Session["ListClaim"] as List<ECClaim>;
                List<Models.ECClaim> lsECClaim = new List<Models.ECClaim>();
                if (lsClaim == null || lsClaim.Count == 0)
                {
                    ViewBag.Message = "There is no Ec claim";
                }
                else
                {
                    lsECClaim = lsClaim.Select(x => new Models.ECClaim
                    {
                        ClaimId = x.ClaimId,
                        User = new Models.User { UserId = x.UserId.Value },
                        Summary = x.Summary,
                        Situation = x.Situation,
                        EffectSituation = x.EffectSituation,
                        CircumstanceStartDate = x.CircumstanceStartDate,
                        CircumstanceEndDate = x.CircumstanceEndDate,
                        OutCome = x.OutCome.Value,
                        FinalClosureDate = x.FinalClosureDate,
                        SubmittedDate = x.SubmittedDate,
                        ClaimStatus = x.ClaimStatus,
                        DecisionStatus = x.DecisionStatus,
                        DecisionDate = x.DecisionDate.HasValue ? x.DecisionDate.Value : (DateTime?)null,
                        ProcessedUser = x.ProcessedUser,
                        HasEvidence = x.HasEvidence.Value,
                        Active = x.Active.Value
                    }).ToList();


                }
                return View("RedirectStatisticPage", lsECClaim);
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public ActionResult FilterECClaimByFaculty(int facultyId)
        {
            List<Models.ECClaim> lsECClaim = new List<Models.ECClaim>();
            List<ECClaim> lsClaim = new List<ECClaim>();
            if (Session["ListClaim"] != null)
            {
                List<ECClaim> ls = Session["ListClaim"] as List<ECClaim>;
                lsClaim = _ecClaimRepository.GetECClaimsByFaculty(facultyId);
                int totalUser = lsClaim.GroupBy(x => x.UserId).Select(x => x.First()).ToList().Count;
                decimal percent = (lsClaim.Count * 100) / ls.Count;
                ViewBag.TotalUser = string.Format("Total Student : {0}", totalUser);
                ViewBag.Percent = string.Format("Percent : {0}%", percent);

                if (lsClaim == null || lsClaim.Count == 0)
                {
                    ViewBag.Message = "There is no Ec claim";
                }
                else
                {
                    lsECClaim = lsClaim.Select(x => new Models.ECClaim
                    {
                        ClaimId = x.ClaimId,
                        User = new Models.User { UserId = x.UserId.Value },
                        Summary = x.Summary,
                        Situation = x.Situation,
                        EffectSituation = x.EffectSituation,
                        CircumstanceStartDate = x.CircumstanceStartDate,
                        CircumstanceEndDate = x.CircumstanceEndDate,
                        OutCome = x.OutCome.Value,
                        FinalClosureDate = x.FinalClosureDate,
                        SubmittedDate = x.SubmittedDate,
                        ClaimStatus = x.ClaimStatus,
                        DecisionStatus = x.DecisionStatus,
                        DecisionDate = x.DecisionDate.HasValue ? x.DecisionDate.Value : (DateTime?)null,
                        ProcessedUser = x.ProcessedUser,
                        HasEvidence = x.HasEvidence.Value,
                        Active = x.Active.Value
                    }).ToList();


                }
            }
            return View("RedirectStatisticPage", lsECClaim);
        }

        public ActionResult RedirectReportPage()
        {
            if (Session["User"] != null)
            {
                List<ECClaim> lsClaim = Session["ListClaim"] as List<ECClaim>;
                List<Models.ECClaim> lsECClaim = new List<Models.ECClaim>();
                if (lsClaim == null || lsClaim.Count == 0)
                {
                    ViewBag.Message = "There is no Ec claim";
                }
                else
                {
                    lsECClaim = lsClaim.Select(x => new Models.ECClaim
                    {
                        ClaimId = x.ClaimId,
                        User = new Models.User { UserId = x.UserId.Value },
                        Summary = x.Summary,
                        Situation = x.Situation,
                        EffectSituation = x.EffectSituation,
                        CircumstanceStartDate = x.CircumstanceStartDate,
                        CircumstanceEndDate = x.CircumstanceEndDate,
                        OutCome = x.OutCome.Value,
                        FinalClosureDate = x.FinalClosureDate,
                        SubmittedDate = x.SubmittedDate,
                        ClaimStatus = x.ClaimStatus,
                        DecisionStatus = x.DecisionStatus,
                        DecisionDate = x.DecisionDate.HasValue ? x.DecisionDate.Value : (DateTime?)null,
                        ProcessedUser = x.ProcessedUser,
                        HasEvidence = x.HasEvidence.Value,
                        Active = x.Active.Value
                    }).ToList();


                }
                return View("RedirectReportPage", lsECClaim);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult ReportClaims(int type)
        {
            List<Models.ECClaim> lsECClaim = new List<Models.ECClaim>();
            if (Session["ListClaim"] != null)
            {
                List<ECClaim> lsClaim = Session["ListClaim"] as List<ECClaim>;
                switch ((ReportType)type)
                {
                    case ReportType.DecisionLate:
                        lsClaim = lsClaim.Where(x => x.DecisionDate > (x.FinalClosureDate.Value.AddDays(x.OutCome.Value))).ToList();
                        break;
                    case ReportType.NotEvidence:
                        lsClaim = lsClaim.Where(x => x.HasEvidence == false).ToList();
                        break;
                    default:
                        break;
                }
                if (lsClaim == null || lsClaim.Count == 0)
                {
                    ViewBag.Message = "There is no Ec claim";
                }
                else
                {
                    lsECClaim = lsClaim.Select(x => new Models.ECClaim
                    {
                        ClaimId = x.ClaimId,
                        User = new Models.User { UserId = x.UserId.Value },
                        Summary = x.Summary,
                        Situation = x.Situation,
                        EffectSituation = x.EffectSituation,
                        CircumstanceStartDate = x.CircumstanceStartDate,
                        CircumstanceEndDate = x.CircumstanceEndDate,
                        OutCome = x.OutCome.Value,
                        FinalClosureDate = x.FinalClosureDate,
                        SubmittedDate = x.SubmittedDate,
                        ClaimStatus = x.ClaimStatus,
                        DecisionStatus = x.DecisionStatus,
                        DecisionDate = x.DecisionDate.HasValue ? x.DecisionDate.Value : (DateTime?)null,
                        ProcessedUser = x.ProcessedUser,
                        HasEvidence = x.HasEvidence.Value,
                        Active = x.Active.Value
                    }).ToList();
                }

            }
            return View("RedirectReportPage", lsECClaim);
        }
    }
}