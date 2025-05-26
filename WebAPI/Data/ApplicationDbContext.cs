using ClassLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using Entity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Tables

        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<PatientEntity> Patients { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<NotificationEntity> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DepartmentEntity>(entity =>
            {
                entity.HasKey(d => d.id); // Set the primary key

                entity.Property(d => d.name)
                      .IsRequired()       // Make it NOT NULL
                      .HasMaxLength(100); // Limit to 100 characters
            });

            modelBuilder.Entity<LogEntity>(entity =>
            {
                entity.HasKey(l => l.logId);

                entity.Property(l => l.actionType)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnType("nvarchar(50)");

                entity.Property(l => l.timestamp)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(l => l.user)
                      .WithOne()
                      .HasForeignKey<LogEntity>(l => l.userId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<PatientEntity>(entity =>
            {
                entity.HasKey(p => p.userId); // UserId acts as both PK and FK for 1:1

                entity.Property(p => p.bloodType)
                      .IsRequired()
                      .HasMaxLength(4);

                entity.Property(p => p.emergencyContact)
                      .IsRequired()
                      .HasMaxLength(10)
                      .IsFixedLength(true); // Enforces exactly 10 characters

                entity.Property(p => p.allergies)
                      .HasMaxLength(255);

                entity.Property(p => p.weight)
                      .IsRequired();

                entity.Property(p => p.height)
                      .IsRequired();

                entity.HasOne(p => p.user)
                      .WithOne()
                      .HasForeignKey<PatientEntity>(p => p.userId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DoctorEntity>(entity =>
            {
                entity.HasKey(d => d.userId); // UserId is both PK and FK for 1:1 relation with User

                entity.Property(d => d.doctorRating)
                      .IsRequired()
                      .HasDefaultValue(0.0f);

                entity.Property(d => d.licenseNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(d => d.user)
                      .WithOne()
                      .HasForeignKey<DoctorEntity>(d => d.userId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.department)
                      .WithMany()
                      .HasForeignKey(d => d.departmentId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascading deletions
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(u => u.userId);

                entity.Property(u => u.username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(u => u.username)
                      .IsUnique();

                entity.Property(u => u.password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(u => u.mail)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(u => u.mail)
                      .IsUnique();

                entity.Property(u => u.role)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("User");

                entity.Property(u => u.name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.birthDate)
                      .IsRequired();

                entity.Property(u => u.cnp)
                      .IsRequired()
                      .HasMaxLength(13)
                      .IsFixedLength();

                entity.HasIndex(u => u.cnp)
                      .IsUnique();

                entity.Property(u => u.address)
                      .HasMaxLength(255);

                entity.Property(u => u.phoneNumber)
                      .HasMaxLength(10)
                      .IsFixedLength();

                entity.Property(u => u.registrationDate)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<NotificationEntity>(entity =>
            {
                entity.HasKey(n => n.notificationId);

                entity.Property(n => n.message)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(n => n.deliveryDateTime)
                      .IsRequired();

                entity.HasOne(n => n.user)
                      .WithMany() 
                      .HasForeignKey(n => n.userId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}
