namespace NFCZavrsniService.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Attendance = new HashSet<Attendance>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(11)]
        public string OIB { get; set; }

        public int Client { get; set; }

        public bool Working { get; set; }

        [StringLength(50)]
        public string WorkPlace { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmploymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string PhoneID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendance { get; set; }

        public virtual Client Client1 { get; set; }

        public virtual Person Person { get; set; }
    }
}
