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

        [StringLength(120)]
        public string NFCContentUploaded { get; set; }

    }

    public class AttendanceResponseModel
    {
        public long ID { get; set; }

        public string TagInfo { get; set; }

        public string EmployeeInfo { get; set; }
    }
}