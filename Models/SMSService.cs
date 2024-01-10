using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateKeyforPartners.Models
{
    public class SMSSvcRoot
    {
        public PartnerDetails PartnerDetails { get; set; }
        public PolicyDetails PolicyDetails { get; set; }
        public SMS SMS { get; set; }

        public string CustomerMobileNo { get; set; }

        public string UserOTP { get; set; }

        public string SecretKey { get; set; }

        public string Source { get; set; }
        public string SourceID { get; set; }

        public string PartnerName { get; set; }
        public bool IsActive { get; set; }
    } 
    public class List
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class PartnerDetails
    {
        public string Partner { get; set; }
        public string PartnerCode { get; set; }
    }
    public class PolicyDetails
    {
        public string PolicyNo { get; set; }
        public string DOB { get; set; }
        public string ClientId { get; set; }
        public string ApplicationNumber { get; set; }
    }
    public class SMS
    {
        public string TemplateName { get; set; }
        public string CustomerMobileNo { get; set; }
        public List<List> List { get; set; }
    }
}