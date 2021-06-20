using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace School_D.Models
{
    public partial class Grade : IValidatableObject
    {
        public Grade()
        {
            Class = new HashSet<Class>();
        }
        [Required]
        public int IdGrade { get; set; }
        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }
        [Required]
        public int IdSchool { get; set; }

        public School IdSchoolNavigation { get; set; }
        public ICollection<Class> Class { get; set; }
        public ICollection<Subject> Subject { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            students_databaseContext context = new students_databaseContext();
            if (context.Grade.SingleOrDefault(s => s.Year == Year && s.IdSchool == IdSchool) != null)
            {
                yield return new ValidationResult(
                    $"{Year} has already been taken",
                    new[] { "Year" });
            }
        }
    }
}
