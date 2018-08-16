using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NFCZavrsniService.Models
{
    public class LoginDataModel
    {
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string PhoneID { get; set; }

        [StringLength(6)]
        public string Token { get; set; }
    }
}