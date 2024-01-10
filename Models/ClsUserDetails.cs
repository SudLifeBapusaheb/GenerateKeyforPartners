using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateKeyforPartners.Models
{
    public class ClsUserDetails
    {
        public string MobileNo { get; set; }
        public string Emp_ID { get; set; }
        public string Is_Active { get; set; }
        public string Emp_Name { get; set; }
        public string ReturnMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}