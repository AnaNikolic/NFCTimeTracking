using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace NFCZavrsniService.Models
{
    public class AttendanceModel
    {
        [StringLength(20)]
        public string SerialNumber { get; set; }
        
        [StringLength(120)]
        public string NFCContentRead { get; set; }
        
    }

    public class AttendanceResponseModel
    {
        [StringLength(6)]
        public string ConfirmationToken { get; set; }

        [StringLength(120)]
        public string NFCContentUploaded { get; set; }

        public long ID { get; set; }
    }
}