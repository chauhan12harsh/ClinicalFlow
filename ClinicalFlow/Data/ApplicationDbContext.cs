using ClinicalFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients => Set<Patient>();

        public DbSet<Doctor> Doctors => Set<Doctor>();

        public DbSet<Encounter> Encounters => Set<Encounter>();

        public DbSet<Prescription> Prescriptions => Set<Prescription>();

        public DbSet<PrescriptionMedication> PrescriptionMedications => Set<PrescriptionMedication>();

        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Application User configuration
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(u => u.ApplicationUserId);

                entity.Property(u => u.Email)
                   .HasMaxLength(200)
                   .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.PasswordHash)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);

                entity.HasIndex(p => p.MedicalRecordNumber)
                    .IsUnique();

                entity.Property(p => p.MedicalRecordNumber)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(p => p.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(d => d.ApplicationUserId)
                    .IsUnique();

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne()
                    .HasForeignKey<Patient>(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Doctor configuration
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId);

                entity.Property(d => d.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(d => d.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasIndex(d => d.Email)
                    .IsUnique();

                entity.Property(d => d.PhoneNumber)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasIndex(d => d.ApplicationUserId)
                    .IsUnique();

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne()
                    .HasForeignKey<Doctor>(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Encounter configuration
            modelBuilder.Entity<Encounter>(entity =>
            {
                entity.HasKey(e => e.EncounterId);

                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(e => e.ChiefComplaint)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(1000);

                // Patient 1 -> Many Encounters
                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Doctor 1 -> Many Encounters
                entity.HasOne(e => e.Doctor)
                    .WithMany(d => d.Encounters)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.PatientId);

                entity.HasIndex(e => e.DoctorId);

                entity.HasIndex(e => e.StartedAt);
            });

            // Prescription configuration
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.PrescriptionId);

                entity.HasOne(p => p.Encounter)
                    .WithMany(e => e.Prescriptions)
                    .HasForeignKey(p => p.EncounterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(p => p.EncounterId);
            });

            // Prescription medication configuration
            modelBuilder.Entity<PrescriptionMedication>(entity =>
            {
                entity.HasKey(pm => pm.PrescriptionMedicationId);

                entity.Property(pm => pm.MedicationName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(pm => pm.Dosage)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(pm => pm.Frequency)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(pm => pm.Prescription)
                    .WithMany(p => p.Medications)
                    .HasForeignKey(pm => pm.PrescriptionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(pm => pm.PrescriptionId);
            });

        }
    }
}
