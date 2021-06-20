using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School_D.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }
        [Required]
        public int IdStudent { get; set; }
        [Required]
        [StringLength(16)]
        public string Name { get; set; }
        [Required]
        [StringLength(16)]
        public string Surname { get; set; }
        [Required]
        [Range(1, 20)]
        public int Age { get; set; }
        [Required]
        public int IdClass { get; set; }

        public Class IdClassNavigation { get; set; }
        public ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
