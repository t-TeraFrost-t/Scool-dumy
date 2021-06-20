using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace School_D.Models
{
    public partial class School : IValidatableObject
    {
        public School()
        {
            Grade = new HashSet<Grade>();
        }
        [Required]
        public int IdSchool { get; set; }
        [Required]
        [StringLength(16)]
        public string Name { get; set; }

        public ICollection<Grade> Grade { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            students_databaseContext context = new students_databaseContext();
            if (context.School.SingleOrDefault(s => s.Name == Name) != null)
            {
                yield return new ValidationResult(
                    $"{Name} has already been taken",
                    new[] { "Name" });
            }
        }
    }
}

