using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesAPI.Models
{
    public class Exercise
    {

        public string ExerciseName { get; set; }

        public string Language { get; set; }

        public int Id { get; set; }

        public List<Student> assignedStudents {get; set;}

    }
}
