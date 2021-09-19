using CMS_BackEnd.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT Id as id, FirstName as firstName, LastName as lastName, Email as email, Gender as gender, convert(varchar(10),BirthDate,120) as birthDate, ImgSrc as imgSrc
                            FROM
                            dbo.Employee
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                            INSERT INTO dbo.Employee
                            (FirstName,LastName,Email,Gender,BirthDate,ImgSrc)
                            VALUES (@FirstName,@LastName,@Email,@Gender,@BirthDate,@ImgSrc)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                    myCommand.Parameters.AddWithValue("@Email", employee.Email);
                    myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                    myCommand.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    myCommand.Parameters.AddWithValue("@ImgSrc", employee.ImgSrc);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, Employee employee)
        {
            string query = @"
                            UPDATE dbo.Employee
                            SET 
                            FirstName= @FirstName,
                            LastName=@LastName,
                            Email=@Email,
                            Gender=@Gender,
                            BirthDate=@BirthDate,
                            ImgSrc=@ImgSrc
                            where Id=@Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                    myCommand.Parameters.AddWithValue("@Email", employee.Email);
                    myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                    myCommand.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    myCommand.Parameters.AddWithValue("@ImgSrc", employee.ImgSrc);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            DELETE 
                            FROM dbo.Employee
                            WHERE Id=@Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
