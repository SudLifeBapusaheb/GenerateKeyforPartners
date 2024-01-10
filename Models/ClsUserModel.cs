using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenerateKeyforPartners.Models
{
    public class ClsUserModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public DateTime LastLoginDateTime { get; set; }
    }
    public class UserLogin
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmpId { get; set; }
        public string Password { get; set; }
    }

    public class OTP_Details
    {
        public bool IsSuccess { get; set; }
        public string EmpId { get; set; }
        public string Generated_OTP { get; set; }
        public string ReturnMessage { get; set; }
        public string EmpName { get; set; }
        public string MobileNo { get; set; }
    }
}