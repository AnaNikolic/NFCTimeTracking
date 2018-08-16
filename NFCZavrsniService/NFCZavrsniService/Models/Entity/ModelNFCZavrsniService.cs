namespace NFCZavrsniService.Models.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelNFCZavrsniService : DbContext
    {
        public ModelNFCZavrsniService()
            : base("name=ModelNFCZavrsniService")
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TypeOfAttendance> TypeOfAttendance { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Employee)
                .WithRequired(e => e.Client1)
                .HasForeignKey(e => e.Client);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Tag)
                .WithRequired(e => e.Client1)
                .HasForeignKey(e => e.Client);

            modelBuilder.Entity<Employee>()
                .Property(e => e.OIB)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneID)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendance)
                .WithRequired(e => e.Employee1)
                .HasForeignKey(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Tag)
                .WithRequired(e => e.Location1)
                .HasForeignKey(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.OIB)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tag>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Tag>()
                .HasMany(e => e.Attendance)
                .WithRequired(e => e.Tag1)
                .HasForeignKey(e => e.Tag)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfAttendance>()
                .HasMany(e => e.Tag)
                .WithRequired(e => e.TypeOfAttendance1)
                .HasForeignKey(e => e.TypeOfAttendance)
                .WillCascadeOnDelete(false);
        }
    }
}
