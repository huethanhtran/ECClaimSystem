using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                        lsClaim = _db.ECClaims.Where(x => x.UserId == user.UserId && x.Active == true).OrderByDescending(x => x.SubmittedDate).ToList();
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

        public void DeleteECClaim(long ClaimId)
        {
            ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == ClaimId).SingleOrDefault();
            if (ec != null)
            {
                ec.Active = false;
                _db.SubmitChanges();
            }
        }

        public ECClaim GetECClaimById(long ClaimId)
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

        public List<ECEvidence> GetEvidencesByECClaim(long ClaimId)
        {
            return _db.ECEvidences.Where(x => x.ClaimId == ClaimId && x.Active == true).ToList();
        }

        public void AcceptOrDenyECClaim(ECClaim ec)
        {
            ECClaim claim = _db.ECClaims.Where(x => x.ClaimId == ec.ClaimId).SingleOrDefault();
            if (claim != null)
            {
                claim.ClaimStatus = ec.ClaimStatus;
                claim.ProcessedUser = ec.ProcessedUser;
                _db.SubmitChanges();
            }
        }

        public void UpdateDecision(int decisionStatus, long claimId)
        {
            ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == claimId).SingleOrDefault();
            if (ec != null)
            {
                ec.ClaimStatus = (int)ClaimStatus.Processed;
                ec.DecisionStatus = decisionStatus;
                ec.DecisionDate = DateTime.Now;
                _db.SubmitChanges();
            }
        }

        public string SendMail(User u, long claimId)
        {
            ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == claimId).SingleOrDefault();
            if (ec != null)
            {
                try
                {
                    MailMessage mailmsg = new MailMessage();
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("be.justkidding@gmail.com", "mjaxtina");
                    mailmsg.From = new MailAddress("be.justkidding@gmail.com");
                    mailmsg.To.Add(ec.User.Email);
                    mailmsg.Subject = "Result Of Your ECClaim";
                    mailmsg.Body = "Through all information you submitted, you were" + (ec.DecisionStatus == (int)DecisionStatus.Success ? "successful" : "fail") + "to solve your problem";
                    client.Send(mailmsg);
                    return "Email was sent successfully";
                }
                catch (Exception)
                {
                    return "You cannot do this action";

                }

            }
            return "You cannot do this action";
        }

        public string ChangeActiveOfECClaim(long claimId, bool value)
        {
            ECClaim ec = _db.ECClaims.Where(x => x.ClaimId == claimId).SingleOrDefault();
            if (ec != null)
            {
                ec.Active = value;
                _db.SubmitChanges();
                return "You updated successfully";
            }
            return "You updated fail";
        }

        public List<User> GetAllUser()
        {
            return _db.Users.OrderByDescending(x => x.UserId).ToList();
        }

        public string ChangeActiveStatusUser(long userId, bool value)
        {
            User u = _db.Users.Where(x => x.UserId == userId).SingleOrDefault();
            if (u != null)
            {
                u.Active = value;
                _db.SubmitChanges();
                return "You updated successfully";
            }
            return "You cannot do this action";
        }

        public User GetUserById(long userId)
        {
            return _db.Users.Where(x => x.UserId == userId).SingleOrDefault();
        }

        public int UpdateUser(Models.User user)
        {
            User u = _db.Users.Where(x => x.UserId == user.UserId).SingleOrDefault();
            if (u != null)
            {
                u.UserFullName = user.UserFullName;
                u.Phone = user.Phone;
                u.Address = user.Address;
                u.BirthDay = user.BirthDay;
                u.Email = user.Email;
                u.FacultyId = user.FacultyId;
                u.Gender = user.Gender;
                u.RoleId = user.RoleId;
                if (_db.Users.Any(x=>x.UserId != user.UserId && x.Email.Trim().ToLower() == user.Email.Trim().ToLower()))
                {
                    return 0;
                }
                else
                {
                    _db.SubmitChanges();
                    return 1;
                }
            }
            return 0;
        }

        public int InsertUser(Models.User user)
        {
            if (_db.Users.Any(x => x.Email.Trim().ToLower() == user.Email.Trim().ToLower()) || DateTime.Now < user.BirthDay || (DateTime.Now.Year - user.BirthDay.Year) > 60)
            {
                return 0;
            }
            if (_db.Users.Any(x => x.AccountId.Trim().ToLower() == user.AccountId.Trim().ToLower()))
            {
                user.AccountId = string.Format("{0}{1}", user.AccountId, 1);
            }
            User u = new User
            {
                AccountId = user.AccountId,
                Active = true,
                Address = user.Address,
                BirthDay = user.BirthDay,
                Email = user.Email,
                FacultyId = user.FacultyId == 0 ? (int?)null : user.FacultyId,
                Gender = user.Gender,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                UserFullName = user.UserFullName

            };
            _db.Users.InsertOnSubmit(u);
            _db.SubmitChanges();
            return 1;
        }

        public List<Setting> GetAllSetting()
        {
            return _db.Settings.ToList();
        }

        public void UpdateOrInsertSetting(int? id, string key, string value)
        {
            if (id.HasValue && id.Value > 0)
            {
                Setting s = _db.Settings.Where(x => x.Id == id.Value).SingleOrDefault();
                if (s != null)
                {
                    s.Key = key;
                    s.Value = value;
                    _db.SubmitChanges();
                }
            }
            else
            {
                Setting s = new Setting
                {
                    Active = true,
                    Key = key,
                    Value = value
                };
                _db.Settings.InsertOnSubmit(s);
                _db.SubmitChanges();
            }
        }

        public Setting GetSettingById(int id)
        {
            return _db.Settings.Where(x => x.Id == id).SingleOrDefault();
        }
        public string ChangeActiveSetting(int id, bool value)
        {
            Setting s = _db.Settings.Where(x => x.Id == id).SingleOrDefault();
            if (s != null)
            {
                s.Active = value;
                _db.SubmitChanges();
                return "You updated successfully";
            }
            return "You cannot do this action";
        }
        public List<Setting> SearchSetting(string searchkey)
        {
            return _db.Settings.Where(x => x.Key.ToLower().Trim().Contains(searchkey.ToLower().Trim())).ToList();
        }
    }
}
