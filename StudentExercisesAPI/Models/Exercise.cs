using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Exercise
    {
        [Required]
        public string ExerciseName { get; set; }

        [Required]
        public string Language { get; set; }

        public int Id { get; set; }

        public List<Student> assignedStudents {get; set;}

    }
}
