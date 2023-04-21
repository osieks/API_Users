using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using APIKrolowie.Models;
using System.Net;

namespace APIKrolowie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (!trimmedEmail.Contains("@") | !trimmedEmail.Contains(".") | trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("addUser")]
        public JsonResult AddUser(Users user)
        {
            try
            {
                //Not empty Check
                if (user.FirstName == "" | user.LastName == "" | user.Password == "" | user.Pesel == "" | user.EMail == "" | user.PhoneNumber == "")
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("One or more validation errors occurred.");
                }
                //EMail Check
                if (!IsValidEmail(user.EMail))
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("E-mail not valid.");
                }
                //Pesel Check
                if (!Int64.TryParse(user.Pesel, out _))
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Pesel is not numeric.");
                }
                if (user.Pesel.Length != 11)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Incorrect Pesel length.");
                }
                //PhoneNumber Check
                if (!Int64.TryParse(user.PhoneNumber, out _))
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Phone number is not numeric.");
                }
                if (user.PhoneNumber.Length < 9 | user.PhoneNumber.Length > 12)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Incorrect Phone number length.");
                }
                //Password Check
                if (user.Password.Length < 8)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Incorrect Password length.");
                }
                if (user.Password.Where(char.IsUpper).Count() < 1)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Password needs at least one uppercase character.");
                }
                if (user.Password.Where(char.IsDigit).Count() < 1)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new JsonResult("Password needs at least one number.");
                }


                string query = "insert into [dbo].[Users] values('"
                    + user.FirstName.Trim() + "','"
                    + user.LastName.Trim() + "',"
                    + "HASHBYTES('SHA2_512', '" + user.Password + "'),'"
                    + user.Pesel.Trim() + "','"
                    + user.EMail.Trim() + "','"
                    + user.PhoneNumber.Trim() + "','"
                    + user.Age + "',"
                    + "CAST(REPLACE('" + user.ElectricityUsage + "',',','.') AS DECIMAL(10,3)))";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("UsersDBConnection");

                using (SqlConnection sqlConnection = new(sqlDataSource))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new(query, sqlConnection))
                    {
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        table.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(e);
            }
            return new JsonResult("User added successfully");
            
        }
        [HttpGet("GetUsers")]
        public JsonResult GetUsers()
        {
            string query = @"select FirstName,LastName from [dbo].[Users]";
            DataTable table = new DataTable();

            try
            {
                string sqlDataSource = _configuration.GetConnectionString("UsersDBConnection");
                SqlDataReader sqlDataReader;
                using (SqlConnection sqlConnection = new(sqlDataSource))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new(query, sqlConnection))
                    {
                        sqlDataReader = sqlCommand.ExecuteReader();
                        table.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(e);
            }
            return new JsonResult(table);
        }
        [HttpGet("GetUser/{EMail}/{Password}")]
        public JsonResult GetUser(string EMail,string Password)
        {
            string query =  @"select FirstName,LastName,Pesel,Email,PhoneNumber,Age,ElectricityUsage 
                            from [dbo].[Users] where EMail = '" + EMail +@"' 
                            and Password = HASHBYTES('SHA2_512', '" + Password + @"')"; 
            DataTable table = new DataTable();

            try
            {
                string sqlDataSource = _configuration.GetConnectionString("UsersDBConnection");
                SqlDataReader sqlDataReader;
                using (SqlConnection sqlConnection = new(sqlDataSource))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new(query, sqlConnection))
                    {
                        sqlDataReader = sqlCommand.ExecuteReader();
                        table.Load(sqlDataReader);
                        if (table.Rows.Count <= 0)
                        {
                            Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return new JsonResult("User not Found");
                        }
                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }
                }
            }catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(e);
            }
            return new JsonResult(table);
        }
    }
}
