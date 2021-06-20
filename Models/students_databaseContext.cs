using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace School_D.Models
{
    public partial class students_databaseContext : DbContext
    {
        public students_databaseContext()
        {
        }

        public students_databaseContext(DbContextOptions<students_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentSubject> StudentSubject { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=students_database;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.IdClass);

                entity.ToTable("class");

                entity.Property(e => e.IdClass).HasColumnName("id_class");

                entity.Property(e => e.IdGrade).HasColumnName("id_grade");

                entity.Property(e => e.Letter)
                    .IsRequired()
                    .HasColumnName("letter")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGradeNavigation)
                    .WithMany(p => p.Class)
                    .HasForeignKey(d => d.IdGrade)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__class__id_grade__18EBB532");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.IdGrade);

                entity.ToTable("grade");

                entity.Property(e => e.IdGrade).HasColumnName("id_grade");

                entity.Property(e => e.IdSchool).HasColumnName("id_school");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.IdSchoolNavigation)
                    .WithMany(p => p.Grade)
                    .HasForeignKey(d => d.IdSchool)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__grade__id_school__160F4887");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasKey(e => e.IdSchool);

                entity.ToTable("school");

                entity.Property(e => e.IdSchool).HasColumnName("id_school");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent);

                entity.ToTable("student");

                entity.Property(e => e.IdStudent).HasColumnName("id_student");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.IdClass).HasColumnName("id_class");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClassNavigation)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.IdClass)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__student__id_clas__1BC821DD");
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.ToTable("student_subject");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdStudent).HasColumnName("id_student");

                entity.Property(e => e.IdSubject).HasColumnName("id_subject");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__student_s__id_st__1EA48E88");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.IdSubject)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__student_s__id_su__1F98B2C1");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.IdSubject);

                entity.ToTable("subject");

                entity.Property(e => e.IdSubject).HasColumnName("id_subject");

                entity.Property(e => e.IdGrade).HasColumnName("id_grade");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                 entity.HasOne(d => d.IdGradeNavigation)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.IdGrade)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__student_s__id_su__1F98B2C1");
            });
        }
    }
}
