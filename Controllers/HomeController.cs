using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateKeyforPartners.Models;
using Newtonsoft.Json;

namespace GenerateKeyforPartners.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            UserLogin user = new UserLogin();
            return View(user);
        }
        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                Utility utility = new Utility();
                ClsUserDetails response = new ClsUserDetails();
               
                DataTable dt_res = utility.ValidateOTPS(user.EmpId.ToString(), user.Password);
                if (dt_res.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt_res.Rows[0]["IsSuccess"]) == true)
                    {
                        Session["Emp_ID"] = dt_res.Rows[0]["Emp_ID"].ToString();
                        
                        //string test = Session["Emp_ID"].ToString();
                        Session["Emp_Name"] = dt_res.Rows[0]["Emp_Name"].ToString();
                        Session["UserID"] = dt_res.Rows[0]["User_ID"].ToString();
                        Session["MobileNo"] = dt_res.Rows[0]["MobileNo"].ToString();
                        return Redirect("~/Home/DisplaySecretKey");
                    }
                    else
                    {
                        ModelState.AddModelError("", dt_res.Rows[0]["ReturnMessage"].ToString());
                        user.Password = "";
                        return View(user);
                    }                    
                }
                else if (response.IsSuccess == false)
                {
                    ModelState.AddModelError("", "Invalid OPT");
                    user.Password = "";
                    return View(user);
                }
                else
                {
                    ModelState.AddModelError("", "");
                    return View(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "User is Not Authorised!");
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult ValidateUser(UserLogin request)
        {
            SMSSvcRoot objSMSService = new SMSSvcRoot();
            Utility utility = new Utility();
            SMS objSMS = new SMS();
            OTP_Details response = new OTP_Details();
            CommonOperation _CommonOperation = new CommonOperation();
            DataTable dt_OTP = utility.ValidateUserAndGetOTP(request.EmpId.ToString());
            //string mobileNo = objSMSService.CustomerMobileNo;
            if (dt_OTP.Rows.Count > 0)
            {
                PartnerDetails objPartnerDetails = new PartnerDetails();
                PolicyDetails objPolicyDetails = new PolicyDetails();
                string Generated_OTP = dt_OTP.Rows[0]["Generated_OTP"].ToString();
                if (Generated_OTP != "0")
                {
                    string PartnerName = ConfigurationManager.AppSettings["PartnerName"];
                    string PartnerCode = ConfigurationManager.AppSettings["PartnerCode"];


                    DateTime ReqDateTime = DateTime.Now;
                    objPartnerDetails.Partner = PartnerName;
                    objPartnerDetails.PartnerCode = PartnerCode;
                    objSMSService.PartnerDetails = objPartnerDetails;

                    objPolicyDetails.PolicyNo = "";
                    objPolicyDetails.DOB = "";
                    objPolicyDetails.ApplicationNumber = "";
                    objPolicyDetails.ClientId = "";
                    objSMSService.PolicyDetails = objPolicyDetails;
                    request.Password = Generated_OTP;
                    Session["Password"] = Generated_OTP;
                    List<List> lstkeyvalue = new List<List>
                {
                new List { Key = "@applicationname", Value ="UBI" },
                new List { Key = "@otp", Value =Generated_OTP },
                new List { Key = "@datetime", Value =Convert.ToString(ReqDateTime)},
                };
                    objSMS.CustomerMobileNo = dt_OTP.Rows[0]["MobileNo"].ToString();
                    objSMS.List = lstkeyvalue;
                    objSMS.TemplateName = "LoginOTP";
                    objSMSService.SMS = objSMS;
                    objSMSService.SMS = objSMS;
                    var jsonSerializeSMSReq = JsonConvert.SerializeObject(objSMSService);
                    jsonSerializeSMSReq = jsonSerializeSMSReq.Remove(jsonSerializeSMSReq.Length - 123);
                    jsonSerializeSMSReq = jsonSerializeSMSReq + "}";
                    string HmacChecksum = _CommonOperation.ComputeHashFromJsonSMS(jsonSerializeSMSReq);
                    dynamic SendSMSSvcResponse = _CommonOperation.ConsumeSMSServiceAPI(jsonSerializeSMSReq, HmacChecksum);
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            else
            {
                response.IsSuccess = false;
            }
            return Json(response);
        }
        public ActionResult ExpireOTP(UserLogin user)
        {
            Utility utility = new Utility();
            ClsUserDetails response = new ClsUserDetails();
            DataTable dtExpire = utility.ExpiredOTP(user.EmpId,"");
            return Json(dtExpire);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult DisplaySecretKey(UserLogin user)
        {
            Utility utility = new Utility();
            List<SMSSvcRoot> list = new List<SMSSvcRoot>();
            string EmpID = Session["Emp_ID"].ToString();
            var result = utility.GetPartnerDetails(EmpID);
            foreach (DataRow item in result.Rows)
            {
                SMSSvcRoot objSMSSvcRoots = new SMSSvcRoot();
                objSMSSvcRoots.SourceID = Convert.ToString((item["PartnerId"]));
                objSMSSvcRoots.PartnerName = Convert.ToString((item["PartnerName"]));
                objSMSSvcRoots.IsActive = Convert.ToBoolean((item["IsActive"]));
                list.Add(objSMSSvcRoots);
            }
            ViewBag.Source = new SelectList(list.ToList(), "SourceID", "PartnerName");
            return View();
        }
        [HttpPost]
        public JsonResult DisplaySecretKeyDetails(SMSSvcRoot objSMSSvcRoot)
        {
            Utility utility = new Utility();
            Session["UserID"].ToString();
            string SecretKeyValue = "";
            string CreatedBy = "";
            int SourceName = utility.InsertPartnerDetails(Session["UserID"].ToString(), objSMSSvcRoot.PartnerName, SecretKeyValue, Session["MobileNo"].ToString(), CreatedBy);
            DataTable SecretKey = utility.GetPartnerDetailsByEmpID(Session["UserID"].ToString(), objSMSSvcRoot.PartnerName);
            objSMSSvcRoot.SecretKey = SecretKey.Rows[0]["SecretKey"].ToString();
            List<SMSSvcRoot> list = new List<SMSSvcRoot>();
            string EmpID = Session["Emp_ID"].ToString();
            var result = utility.GetPartnerDetails(EmpID);
            foreach (DataRow item in result.Rows)
            {
                SMSSvcRoot objSMSSvcRoots = new SMSSvcRoot();
                objSMSSvcRoots.SourceID = Convert.ToString((item["PartnerId"]));
                objSMSSvcRoots.PartnerName = Convert.ToString((item["PartnerName"]));
                objSMSSvcRoots.IsActive = Convert.ToBoolean((item["IsActive"]));
                list.Add(objSMSSvcRoots);
            }
            ViewBag.Source = new SelectList(list.ToList(), "SourceID", "PartnerName");
            return Json(objSMSSvcRoot.SecretKey);
        }
        public JsonResult DisplaySecretKeyValue(SMSSvcRoot objSMSSvcRoot)
        {
            Utility utility = new Utility();
            bool SourceName = false;
            string SecretKeyName = "";
            objSMSSvcRoot.IsActive = false;
            DataSet ds = new DataSet();
            if (objSMSSvcRoot.PartnerName != null)
            {
                 ds = utility.GetSecretKeyExist(objSMSSvcRoot.PartnerName);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                SecretKeyName = ds.Tables[0].Rows[0]["SecretKey"].ToString();
                objSMSSvcRoot.IsActive = true;
            }
            objSMSSvcRoot.SecretKey = SecretKeyName;
            
            return Json(objSMSSvcRoot);
        }
    }
}