namespace NFCZavrsniService.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tag")]
    public partial class Tag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tag()
        {
            Attendance = new HashSet<Attendance>();
        }

        public int ID { get; set; }

        public long Location { get; set; }

        public byte TypeOfAttendance { get; set; }

        public int Client { get; set; }

        [Required]
        [StringLength(120)]
        public string TagContent { get; set; }

        [StringLength(20)]
        public string SerialNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendance { get; set; }

        public virtual Client Client1 { get; set; }

        public virtual Location Location1 { get; set; }

        public virtual TypeOfAttendance TypeOfAttendance1 { get; set; }
    }
}
