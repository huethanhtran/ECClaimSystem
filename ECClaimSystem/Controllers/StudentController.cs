using ECClaimSystem.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ECClaimSystem.Models.Elements.ConstantsKey;

namespace ECClaimSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        private readonly IECClaimRepository _ecClaimRepository;
        public StudentController()
        {
            this._ecClaimRepository = new ECClaimRepository();
        }
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View("ECClaim");
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitECClaim(Models.ECClaim model, HttpPostedFileBase upEvidence)
        {
            if (ModelState.IsValid)
            {
                Setting keyOutCome = _ecClaimRepository.GetSettingByKey("EC_System_Time_OutCome");
                Setting keyClosureDate = _ecClaimRepository.GetSettingByKey("EC_System_Time_ClosureDate");
                User u = Session["User"] as User;
                ECClaim ec = new ECClaim
                {
                    UserId = u.UserId,
                    Summary = model.Summary,
                    Situation = model.Situation,
                    EffectSituation = model.EffectSituation,
                    CircumstanceEndDate = model.CircumstanceEndDate.Value,
                    CircumstanceStartDate = model.CircumstanceStartDate.Value,
                    OutCome = keyOutCome != null ? int.Parse(keyOutCome.Value) : 10,
                    SubmittedDate = DateTime.Now,
                    ClaimStatus = (int)ClaimStatus.Pending,
                    DecisionStatus = (int)DecisionStatus.Pending,
                    Active = true,
                    HasEvidence = false
                };
                if (keyClosureDate != null)
                {
                    ec.FinalClosureDate = ec.SubmittedDate.Value.AddDays((int.Parse(keyClosureDate.Value)));
                }
                string filename = "";
                if (upEvidence != null)
                {
                    ec.HasEvidence = true;
                    FileInfo file = new FileInfo(upEvidence.FileName);
                    filename = Guid.NewGuid().ToString("N") + file.Extension;
                    upEvidence.SaveAs(Server.MapPath("~/File/Evidence/" + filename));
                }
                long ClaimId = _ecClaimRepository.InsertECClaim(ec);
                if (upEvidence != null && ClaimId > 0)
                {
                    ECEvidence evidence = new ECEvidence
                    {
                        ClaimId = ClaimId,
                        CreatedDate = DateTime.Now,
                        EvidenceName = filename,
                        Type = filename.ToLower().Trim().Contains(".pdf") ? (int)EvidenceType.Pdf : (int)EvidenceType.Image,
                        Active = true
                    };
                    int evidenceId = _ecClaimRepository.InsertECClaimEvidence(evidence);
                }
                if (ClaimId > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("ECClaim", model);
        }

        [HttpGet]
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
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
                lsECClaim = lsClaim.Select(x => new Models.ECClaim {
                    ClaimId = x.ClaimId,
                    User = new Models.User { UserId = x.UserId.Value},
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
                    DecisionDate = x.DecisionDate.HasValue ?  x.DecisionDate.Value : (DateTime?)null,
                    ProcessedUser = x.ProcessedUser,
                    HasEvidence = x.HasEvidence.Value,
                    Active = x.Active.Value
                }).ToList();
            }
            return View(lsECClaim);
        }

        public ActionResult Edit(long id)
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
            return View("Edit", claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpdateECClaim(Models.ECClaim model, HttpPostedFileBase upEvidence)
        {
            if (ModelState.IsValid)
            {
                ECClaim ec = new ECClaim
                {
                    Summary = model.Summary,
                    Situation = model.Situation,
                    EffectSituation = model.EffectSituation,
                    CircumstanceEndDate = model.CircumstanceEndDate.Value,
                    CircumstanceStartDate = model.CircumstanceStartDate.Value,
                    ClaimId = model.ClaimId
                };
               
                string filename = "";
                if (upEvidence != null)
                {
                    ec.HasEvidence = true;
                    FileInfo file = new FileInfo(upEvidence.FileName);
                    filename = Guid.NewGuid().ToString("N") + file.Extension;
                    upEvidence.SaveAs(Server.MapPath("~/File/Evidence/" + filename));
                }
                _ecClaimRepository.UpdateECClaim(ec);
                if (upEvidence != null && model.ClaimId > 0)
                {
                    ECEvidence evidence = new ECEvidence
                    {
                        ClaimId = model.ClaimId,
                        CreatedDate = DateTime.Now,
                        EvidenceName = filename,
                        Type = filename.ToLower().Trim().Contains(".pdf") ? (int)EvidenceType.Pdf : (int)EvidenceType.Image,
                        Active = true
                    };
                    int evidenceId = _ecClaimRepository.InsertECClaimEvidence(evidence);
                }
                if (model.ClaimId > 0)
                {
                    return RedirectToAction("Edit", new { id = model.ClaimId});
                }
            }
            return View("Edit", model);
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
    }
}