using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECClaimSystem.Models.Elements.ConstantsKey;

namespace ECClaimSystem.Repository
{
   public class ECClaimRepository : IECClaimRepository
    {
        dbECClaimSystemDataContext _db;
        public ECClaimRepository()
        {
            _db = new dbECClaimSystemDataContext();
        }

        public User Login(string account, string password)
        {
            User u = _db.Users.Where(x => x.AccountId == account && x.Password == password).SingleOrDefault();
            return u;
        }

        public long InsertECClaim(ECClaim ec)
        {
            _db.ECClaims.InsertOnSubmit(ec);
            _db.SubmitChanges();
            return ec.ClaimId;
        }

        public Setting GetSettingByKey(string key)
        {
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrWhiteSpace(key))
            {
                Setting setting = _db.Settings.Where(x => x.Key.ToLower().Trim() == key.ToLower().Trim()).SingleOrDefault();
                return setting;
            }
            return null;
        }

        public int InsertECClaimEvidence(ECEvidence evidence)
        {
            _db.ECEvidences.InsertOnSubmit(evidence);
            _db.SubmitChanges();
            return evidence.EvidenceId;
        }

        public List<ECClaim> GetListECClaimWithSpecificUser(User user)
        {
            if (user != null)
            {
                List<ECClaim> lsClaim = new List<ECClaim>();
                switch ((Models.Elements.ConstantsKey.Role)user.RoleId)
                {
                    case Models.Elements.ConstantsKey.Role.Student:
                        lsClaim = _db.ECClaims.Where(x => x.UserId == user.UserId && x.Active == true).OrderByDescending(x=>x.SubmittedDate).ToList();
                        break;
                    case Models.Elements.ConstantsKey.Role.ECManager:
                        lsClaim = _db.ECClaims.Where(x => x.Active == true).OrderByDescending(x => x.SubmittedDate).ToList();

                        break;
                    case Models.Elements.ConstantsKey.Role.ECCoordinator:
                        lsClaim = _db.sp_ECClaim_GetAllECClaimsOfFaculty(user.FacultyId).Select(x => new ECClaim
                        {
                            ClaimId = x.ClaimId,
                            UserId = x.UserId,
                            Summary = x.Summary,
                            Situation = x.Situation,
                            EffectSituation = x.EffectSituation,
                            CircumstanceStartDate = x.CircumstanceStartDate,
                            CircumstanceEndDate = x.CircumstanceEndDate,
                            OutCome = x.OutCome,
                            FinalClosureDate = x.FinalClosureDate,
                            SubmittedDate = x.SubmittedDate,
                            ClaimStatus = x.ClaimStatus,
                            DecisionStatus = x.DecisionStatus,
                            DecisionDate = x.DecisionDate,
                            ProcessedUser = x.ProcessedUser,
                            HasEvidence = x.HasEvidence,
                            Active = x.Active
                        }).OrderByDescending(x => x.SubmittedDate).ToList();
                        break;
                    case Models.Elements.ConstantsKey.Role.Administrator:
                        lsClaim = _db.ECClaims.OrderByDescending(x => x.SubmittedDate).ToList();
                        break;
                    default:
                        break;
                }
                return lsClaim;
            }
            return null;
        }

        public void DeleteECClaim(int ClaimId)
        {
            ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == ClaimId).SingleOrDefault();
            if (ec != null)
            {
                ec.Active = false;
                _db.SubmitChanges();
            }
        }

        public ECClaim GetECClaimById(int ClaimId)
        {
           ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == ClaimId).SingleOrDefault();
            return ec;
        }

        public void UpdateECClaim(ECClaim ec)
        {
            ECClaim claim = _db.ECClaims.Where(x => x.ClaimId == ec.ClaimId).SingleOrDefault();
            if (claim != null)
            {
                claim.Situation = ec.Situation;
                claim.EffectSituation = ec.EffectSituation;
                claim.Summary = ec.Summary;
                claim.CircumstanceEndDate = ec.CircumstanceEndDate;
                claim.CircumstanceStartDate = ec.CircumstanceStartDate;
                claim.HasEvidence = ec.HasEvidence;
                _db.SubmitChanges();
            }
        }
        public List<Faculty> GetAllFaculty()
        {
            return _db.Faculties.Where(x => x.Active == true).ToList();
        }
        public List<ECClaim> GetECClaimsByFaculty(int facultyId)
        {
            List<ECClaim> lsClaim = _db.sp_GetECClaimOfFaculty(facultyId).Select(x => new ECClaim
            {
                ClaimId = x.ClaimId,
                UserId = x.UserId,
                Summary = x.Summary,
                Situation = x.Situation,
                EffectSituation = x.EffectSituation,
                CircumstanceStartDate = x.CircumstanceStartDate,
                CircumstanceEndDate = x.CircumstanceEndDate,
                OutCome = x.OutCome,
                FinalClosureDate = x.FinalClosureDate,
                SubmittedDate = x.SubmittedDate,
                ClaimStatus = x.ClaimStatus,
                DecisionStatus = x.DecisionStatus,
                DecisionDate = x.DecisionDate,
                ProcessedUser = x.ProcessedUser,
                HasEvidence = x.HasEvidence,
                Active = x.Active
            }).OrderByDescending(x => x.SubmittedDate).ToList();
            return lsClaim;
        }
    }
}
