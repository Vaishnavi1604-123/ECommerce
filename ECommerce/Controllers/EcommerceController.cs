//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using System.Data.SqlClient;

//namespace ECommerce.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]/[action]")]
//    public class EcommerceController : Controller
//    {
//        [HttpGet]
//        public IActionResult Login()
//        {
//            var details = GetLoginDetails();
//            return Ok(details);
//        }
//        private IEnumerable<Login> GetLoginDetails()
//        {
//            var logindetails = new List<Login>();

//            var connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=ECommerce;User ID=SA;Password=Welcome2evoke@1234";
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                using (var command = new SqlCommand("Getlogindetails", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;

//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var details=new Login
//                            {
//                                Id = (int)reader["Id"],
//                                Name = reader["Name"].ToString(),
//                                Email = reader["Email"].ToString(),
//                                Password = reader["Password"].ToString(),
//                                Role = reader["Role"].ToString()
//                            };
//                            logindetails.Add(details);


//                        }
//                    }
//                }
//            }
//            return logindetails;
//        }
//    }
//}
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;

//namespace ECommerce.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]/[action]")]
//    public class EcommerceController : Controller
//    {
//        [HttpPost]
//        public IActionResult Login(Login model)
//        {
//            // Validate the user credentials against the database
//            var isValid = ValidateUser(model);

//            if (isValid)
//            {
//                // Perform any additional logic for a successful login
//                return Ok("Login successful!");
//            }
//            else
//            {
//                return BadRequest("Invalid login attempt.");
//            }
//        }

//        private bool ValidateUser(Login model)
//        {
//            // Implement logic to validate user credentials against the database
//            var connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=ECommerce;User ID=SA;Password=Welcome2evoke@1234";
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                using (var command = new SqlCommand("Getlogindetails", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;

//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            // Compare the user input with the database values
//                            if (model.Email == reader["Email"].ToString() && model.Password == reader["Password"].ToString())
//                            {
//                                return true;
//                            }
//                        }
//                    }
//                }
//            }

//            return false;
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EcommerceController : Controller
    {
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (IsValidUser(model))
            {
                string role = GetRoleFromDatabase(model.Email);
                return Json(new LoginResponse { Success = true, Role = role });
            }

            return Json(new LoginResponse { Success = false });
        }

        private bool IsValidUser(Login model)
        {
            var connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=ECommerce;User ID=SA;Password=Welcome2evoke@1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Login WHERE Email = @Email AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private string GetRoleFromDatabase(string email)
        {
            var connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=ECommerce;User ID=SA;Password=Welcome2evoke@1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Role FROM Login WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var result = command.ExecuteScalar();

                    // Check if the result is null
                    if (result != null)
                    {
                        return result.ToString();
                    }

                    // Handle the case where the email does not exist in the database
                    return null;
                }
            }
        }
    }
}
