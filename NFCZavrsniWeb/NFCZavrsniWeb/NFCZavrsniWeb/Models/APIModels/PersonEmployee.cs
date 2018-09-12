using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NFCZavrsniWeb.Models.APIModels
{
    public class PersonEmployee
    {
        public long ID { get; set; }

        [Required]
        [StringLength(11)]
        public string OIB { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(12)]
        public string MACAdress { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

    }
}