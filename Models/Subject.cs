using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace School_D.Models
{
    public partial class Subject : IValidatableObject
    {
        public Subject()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }
        [Required]
        public int IdSubject { get; set; }
        [Required]
        [StringLength(16)]
        public string Name { get; set; }
        [Required]
        public int IdGrade { get; set; }

        public Grade IdGradeNavigation { get; set; }
        public ICollection<StudentSubject> StudentSubject { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            students_databaseContext context = new students_databaseContext();
            
            if (context.Subject.SingleOrDefault(s => s.Name == Name && s.IdGrade == IdGrade) != null)
            {
                yield return new ValidationResult(
                    $"{Name} has already been taken for {context.Grade.First(g => g.IdGrade==IdGrade).Year} year",
                    new[] { "Name" });
            }
        }
    }
}
