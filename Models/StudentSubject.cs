using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace School_D.Models
{
    public partial class StudentSubject : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdStudent { get; set; }
        [Required]
        public int IdSubject { get; set; }

        public Student IdStudentNavigation { get; set; }
        public Subject IdSubjectNavigation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            students_databaseContext context = new students_databaseContext();
            if (context.StudentSubject.SingleOrDefault(s => s.IdStudent == IdStudent && s.IdSubject == IdSubject) != null)
            {
                yield return new ValidationResult(
                    $"{IdStudentNavigation.Name} -> {IdSubjectNavigation.Name} has already been made",
                    new[] { "IdStudent" , "IdSubject"});
            }
        }
    }
}
