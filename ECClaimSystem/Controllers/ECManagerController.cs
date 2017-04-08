using ECClaimSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECClaimSystem.Controllers
{
    public class ECManagerController : Controller
    {
        IECClaimRepository _ecClaimRepository;
        public ECManagerController()
        {
            _ecClaimRepository = new ECClaimRepository();
        }
        // GET: ECManager
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
                ViewBag.Message = string.Format("Total: {0}", lsClaim.Count);
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
        public ActionResult FilterECClaimByFaculty(int facultyId)
        {
            List<ECClaim> lsClaim = new List<ECClaim>();
            if (Session["ListClaim"] != null)
            {
                lsClaim = _ecClaimRepository.GetECClaimsByFaculty(facultyId);
                Session["ListClaim"] = lsClaim;
            }
            return RedirectToAction("ShowListECClaim");
        }
    }
}