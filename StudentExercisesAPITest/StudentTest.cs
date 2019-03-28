using System;
using System.Threading.Tasks;
using Xunit;
using StudentExercisesAPI;
using Newtonsoft.Json;
using System.Net;
using StudentExercisesAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StudentExercisesAPITest
{
    public class StudentTest
    {
        [Fact]
        public async Task Test_Get_All_Students()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    GET section
                */
                var response = await client.GetAsync("/api/student");


                string responseBody = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>> (responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(studentList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Modify_Student()
        {

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                string newSlackHandle = "ashp94";   

                Student modifiedAsh = new Student
                {
                    FirstName = "Ashwin",
                    LastName = "Prakash",
                    SlackHandle = newSlackHandle,
                    CohortId = 1,
                };

                var modifiedAshAsJSON = JsonConvert.SerializeObject(modifiedAsh);

                var response = await client.PutAsync(
                    "/api/student/15",
                    new StringContent(modifiedAshAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                var getAsh = await client.GetAsync("/api/student/15");
                getAsh.EnsureSuccessStatusCode();

                string getAshBody = await getAsh.Content.ReadAsStringAsync();
                Student newAsh = JsonConvert.DeserializeObject<Student>(getAshBody);

                Assert.Equal(HttpStatusCode.OK, getAsh.StatusCode);
                Assert.Equal(newSlackHandle, newAsh.SlackHandle);
            }
        }

        [Fact]
        public async Task Test_Delete_Student()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/student/1");

                /*
                    DELETE section
                */
                var response1 = await client.GetAsync("/api/student/1");


                string responseBody = await response1.Content.ReadAsStringAsync();

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);
                
            }
        }

        [Fact]
        public async Task Test_Create_Student()
        {

            using (var client = new APIClientProvider().Client)
            {
                /*
                    CREATE section
                */
                var getOldList = await client.GetAsync("/api/student");
                getOldList.EnsureSuccessStatusCode();

                string getOldListBody = await getOldList.Content.ReadAsStringAsync();
                var oldList = JsonConvert.DeserializeObject<List<Student>>(getOldListBody);



                Student newStudent = new Student
                {
                    FirstName = "Brittany",
                    LastName = "Janeway",
                    SlackHandle = "@brittany",
                    CohortId = 1,
                };

                var modifiedStudentAsJSON = JsonConvert.SerializeObject(newStudent);

                var response = await client.PostAsync(
                    "/api/student",
                    new StringContent(modifiedStudentAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();
                
                /*
                    GET section
                    Verify that the Post operation was successful
                */
                var getStudents = await client.GetAsync("/api/student");
                getStudents.EnsureSuccessStatusCode();

                string getStudentBody = await getStudents.Content.ReadAsStringAsync();
                var newList = JsonConvert.DeserializeObject<List<Student>>(getStudentBody);

                Assert.Equal(HttpStatusCode.OK, getStudents.StatusCode);
                Assert.True(newList.Count > oldList.Count);
            }
        }

    }
}

