using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Cohort
    {

        [Required]
        [StringLength(11, MinimumLength = 5)]
        public string CohortName { get; set; }

        public int Id { get; set; }

        public List<Student> studentList { get; set; }

        public List<Instructor> instructorList { get; set; }

        //public void ListCohort()
        //{

        //    foreach (Student student in studentList)
        //    {
        //        Console.WriteLine($"{student.FirstName} {student.LastName} is in {CohortName}.");
        //    }

        //    foreach (Instructor instructor in intructorList)
        //    {
        //        Console.WriteLine($"{instructor.FirstName} {instructor.LastName} is the teacher for {CohortName}.");
        //    }
        //}
    }
}

