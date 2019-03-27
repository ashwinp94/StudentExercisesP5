using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudentExercisesAPI.Models;
using Microsoft.AspNetCore.Http;

namespace StudentExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (q != null)
                    {
                        cmd.CommandText = $@"SELECT c.Id as CohortId, s.Id as sId, s.CohortId as sCohortId, c.CohortName, s.FirstName, s.LastName, i.FirstName as iFirstName, 
                                        i.LastName as iLastName, i.CohortId as iCohortId, i.Id as iId 
                                        FROM Cohort c JOIN Student s ON s.CohortId = c.Id JOIN Instructor i on i.CohortId = c.Id
                                        Where CohortName LIKE @b";

                        cmd.Parameters.Add(new SqlParameter("@b", $"%{q}%"));

                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                        while (reader.Read())
                        {
                            int cohortid = reader.GetInt32(reader.GetOrdinal("CohortId"));

                            if (!cohorts.ContainsKey(cohortid))
                            {
                                Cohort cohort = new Cohort
                                {
                                    Id = cohortid,
                                    CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                                    studentList = new List<Student>(),
                                    instructorList = new List<Instructor>()
                                };

                                cohorts.Add(cohortid, cohort);

                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("sCohortId")))
                            {
                                Cohort currentCohort = cohorts[cohortid];

                                if (!currentCohort.studentList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("sId"))))
                                {
                                    currentCohort.studentList.Add(
                                        new Student
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("sId")),
                                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                            CohortId = reader.GetInt32(reader.GetOrdinal("sCohortId")),
                                        }
                                    );
                                }

                                if (!currentCohort.instructorList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("iId"))))
                                {
                                    currentCohort.instructorList.Add(
                                        new Instructor
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("iId")),
                                            FirstName = reader.GetString(reader.GetOrdinal("iFirstName")),
                                            LastName = reader.GetString(reader.GetOrdinal("iLastName")),
                                            CohortId = reader.GetInt32(reader.GetOrdinal("iCohortId")),
                                        }
                                    );
                                }
                            }

                        }
                        reader.Close();

                        return Ok(cohorts);

                    }
                    else
                    {
                        cmd.CommandText = "SELECT c.Id as CohortId, s.Id as sId, s.CohortId as sCohortId, c.CohortName, s.FirstName, s.LastName, i.FirstName as iFirstName, i.LastName as iLastName, i.CohortId as iCohortId, i.Id as iId FROM Cohort c JOIN Student s ON s.CohortId = c.Id JOIN Instructor i on i.CohortId = c.Id";
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                        while (reader.Read())
                        {
                            int cohortid = reader.GetInt32(reader.GetOrdinal("CohortId"));

                            if (!cohorts.ContainsKey(cohortid))
                            {
                                Cohort cohort = new Cohort
                                {
                                    Id = cohortid,
                                    CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                                    studentList = new List<Student>(),
                                    instructorList = new List<Instructor>()
                                };

                                cohorts.Add(cohortid, cohort);

                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("sCohortId")))
                            {
                                Cohort currentCohort = cohorts[cohortid];

                                if (!currentCohort.studentList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("sId"))))
                                {
                                    currentCohort.studentList.Add(
                                        new Student
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("sId")),
                                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                            CohortId = reader.GetInt32(reader.GetOrdinal("sCohortId")),
                                        }
                                    );
                                }

                                if (!currentCohort.instructorList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("iId"))))
                                {
                                    currentCohort.instructorList.Add(
                                        new Instructor
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("iId")),
                                            FirstName = reader.GetString(reader.GetOrdinal("iFirstName")),
                                            LastName = reader.GetString(reader.GetOrdinal("iLastName")),
                                            CohortId = reader.GetInt32(reader.GetOrdinal("iCohortId")),
                                        }
                                    );
                                }
                            }

                        }
                        reader.Close();

                        return Ok(cohorts);
                    }
                }
            }
        }

        [HttpGet("{id}", Name = "GetCohort")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT c.Id as CohortId, s.Id as sId, s.CohortId as sCohortId, c.CohortName, s.FirstName, s.LastName, i.FirstName as iFirstName, i.LastName as iLastName, i.CohortId as iCohortId, i.Id as iId FROM Cohort c JOIN Student s ON s.CohortId = c.Id JOIN Instructor i on i.CohortId = c.Id WHERE c.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                    if (reader.Read())
                    {
                        int cohortid = reader.GetInt32(reader.GetOrdinal("CohortId"));

                        if (!cohorts.ContainsKey(cohortid))
                        {
                            Cohort cohort = new Cohort
                            {
                                Id = cohortid,
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                                studentList = new List<Student>(),
                                instructorList = new List<Instructor>()
                            };

                            cohorts.Add(cohortid, cohort);

                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("sCohortId")))
                        {
                            Cohort currentCohort = cohorts[cohortid];

                            if (!currentCohort.studentList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("sId"))))
                            {
                                currentCohort.studentList.Add(
                                    new Student
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("sId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        CohortId = reader.GetInt32(reader.GetOrdinal("sCohortId")),
                                    }
                                );
                            }

                            if (!currentCohort.instructorList.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("iId"))))
                            {
                                currentCohort.instructorList.Add(
                                    new Instructor
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("iId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("iFirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("iLastName")),
                                        CohortId = reader.GetInt32(reader.GetOrdinal("iCohortId")),
                                    }
                                );
                            }
                        }

                    }
                    reader.Close();

                    return Ok(cohorts);
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (CohortName)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.Add(new SqlParameter("@name", cohort.CohortName));
                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return CreatedAtRoute("GetCohort", new { id = newId }, cohort);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Cohort cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET CohortName = @name,
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", cohort.CohortName));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!CohortExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!CohortExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool CohortExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CohortName
                        FROM Cohort
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
// currentCohort.instructorList.Add(
//new Instructor
//                                 {
//                                      Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                                      FirstName = reader.GetString(reader.GetOrdinal("iFirstName")),
//                                      LastName = reader.GetString(reader.GetOrdinal("iLastName")),
//                                 }
//                           );