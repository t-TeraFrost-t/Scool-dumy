using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace School_D.Models
{
    public partial class Class :IValidatableObject
    {
        public Class()
        {
            Student = new HashSet<Student>();
        }
        [Required]
        public int IdClass { get; set; }
        [Required]
        public string Letter { get; set; }
        [Required]
        public int IdGrade { get; set; }

        public Grade IdGradeNavigation { get; set; }
        public ICollection<Student> Student { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            students_databaseContext context = new students_databaseContext();
            if (context.Class.SingleOrDefault(s => s.Letter == Letter && s.IdGrade == IdGrade) != null)
            {
                yield return new ValidationResult(
                    $"{Letter} has already been taken",
                    new[] { "Letter" });
            }
        }
    }
}
