namespace NFCZavrsniWeb.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        public long ID { get; set; }

        public long Employee { get; set; }

        public int Tag { get; set; }

        public DateTime DateTime { get; set; }

        [StringLength(120)]
        public string NFCContentRead { get; set; }

        [StringLength(120)]
        public string NFCContentUploaded { get; set; }

        public bool? Confirmed { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Tag Tag1 { get; set; }
    }
}
