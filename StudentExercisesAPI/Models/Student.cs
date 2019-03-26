using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StudentExercisesAPI.Models
{
            
    public class Student
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [StringLength(12, MinimumLength = 3)]
        public string SlackHandle { get; set; }

        public int CohortId { get; set; }

        public Cohort Cohort { get; set; }


        public List<Exercise> studentExercises { get; set; }

        // public static bool IsEmpty<T>(this IEnumerable<T> list)
        // {
        //     if (studentExercises is ICollection<T>) return ((ICollection<T>)studentExercises).Count == 0;

        //     return !studentExercises.Any();
        // }
   
    }
}
