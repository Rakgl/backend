using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BACKEND.Entity;
using BACKEND.Helper;
using Microsoft.AspNetCore.Authorization; 


namespace BACKEND.Controller;

[ApiController]
[Route(template:"api/users")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public UserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    //------------------------------Create User
    [HttpPost(template: "create")]
    public string CreateUser([FromBody] UserEntity request)
    {
        try
        {
            var conString = _configuration.GetConnectionString("Default");
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                var sql = "INSERT INTO " +
                          "BACKEND.dbo.users(profile,first_name, last_name, email, employee, position, department, phone, hire_date, status, password) " +
                          "VALUES (@profile,@first_name, @last_name, @email, @employee, @position, @department, @phone, @hire_date, @status, @password)";
                using (SqlCommand cd = new SqlCommand(sql, conn))
                {
                    cd.Parameters.AddWithValue("@profile", request.profile);
                    cd.Parameters.AddWithValue("@first_name", request.first_name);
                    cd.Parameters.AddWithValue("@last_name", request.last_name);
                    cd.Parameters.AddWithValue("@email", request.email);
                    cd.Parameters.AddWithValue("@employee", request.employee);
                    cd.Parameters.AddWithValue("@position", request.position);
                    cd.Parameters.AddWithValue("@department", request.department);
                    cd.Parameters.AddWithValue("@phone", request.phone);
                    cd.Parameters.AddWithValue("@hire_date", request.hire_date);
                    cd.Parameters.AddWithValue("@status", request.status);
                    
                    var helper = new GeneralHelper();
                    var hashedPassword = helper.HashPassword(request.password);
                    cd.Parameters.AddWithValue("@password", hashedPassword);

                    var affectedRow = cd.ExecuteNonQuery();
                    if (affectedRow > 0)
                    {
                        return "Succes to create user";
                    }
                    return "Fail to create user";
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error creating user: {ex.Message}";
        }
    }
    
    // ------------------------------Get User
    [Authorize]
    [HttpGet]
    public IActionResult GetUsers()
    {
        try
        {
            var users = new List<UserEntity>();
            var conStr = _configuration.GetConnectionString("Default");
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                var sql = "SELECT id,profile, first_name, last_name, email, employee, position, department, phone, hire_date, status FROM BACKEND.dbo.users";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserEntity
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                profile = reader.GetString(reader.GetOrdinal("profile")),
                                first_name = reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.GetString(reader.GetOrdinal("last_name")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                employee = reader.GetString(reader.GetOrdinal("employee")),
                                position = reader.GetString(reader.GetOrdinal("position")),
                                department = reader.GetString(reader.GetOrdinal("department")),
                                phone = reader.GetString(reader.GetOrdinal("phone")),
                                hire_date = reader.GetDateTime(reader.GetOrdinal("hire_date")),
                                status = reader.GetString(reader.GetOrdinal("status"))
                            });
                        }
                    }
                }
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving users: {ex.Message}");
        }
    }
    
    //------------------------------Get user by Id
    [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var conStr = _configuration.GetConnectionString("Default");
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    var sql = "SELECT id, profile, first_name, last_name, email, employee, position, department, phone, hire_date, status FROM BACKEND.dbo.users WHERE id = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return NotFound("User not found");
                            }

                            var user = new UserEntity
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                profile = reader.GetString(reader.GetOrdinal("profile")),
                                first_name = reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.GetString(reader.GetOrdinal("last_name")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                employee = reader.GetString(reader.GetOrdinal("employee")),
                                position = reader.GetString(reader.GetOrdinal("position")),
                                department = reader.GetString(reader.GetOrdinal("department")),
                                phone = reader.GetString(reader.GetOrdinal("phone")),
                                hire_date = reader.GetDateTime(reader.GetOrdinal("hire_date")),
                                status = reader.GetString(reader.GetOrdinal("status"))
                            };
                            return Ok(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }

    //------------------------------update
    [Authorize]
    [HttpPut("update")]
    public string UpdateUser([FromBody] UserEntity request)
    {
        try
        {
            if (request == null || request.id <= 0)
            {
                return ("Invalid request");
            }
            var connectionString = _configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE BACKEND.dbo.users SET first_name=@profile,@first_name,  last_name=@last_name,   email=@email,  password=@password, employee=@employee, position=@position, department=@department, phone=@phone, hire_date=@hire_date, status=@status WHERE id=@id;";
                SqlCommand cd = new SqlCommand(sql, connection);
                cd.Parameters.AddWithValue("@id", request.id);
                cd.Parameters.AddWithValue("@profile", request.profile);
                cd.Parameters.AddWithValue("@first_name", request.first_name);
                cd.Parameters.AddWithValue("@last_name", request.last_name);
                cd.Parameters.AddWithValue("@email", request.email);
                cd.Parameters.AddWithValue("@employee", request.employee);
                cd.Parameters.AddWithValue("@position", request.position);
                cd.Parameters.AddWithValue("@department", request.department);
                cd.Parameters.AddWithValue("@phone", request.phone);
                cd.Parameters.AddWithValue("@hire_date", request.hire_date);
                cd.Parameters.AddWithValue("@status", request.status);
                var helper = new GeneralHelper();
                var hashedPassword = helper.HashPassword(request.password);
                cd.Parameters.AddWithValue("@password", hashedPassword);
                
                var rowaffected = cd.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    return "User Updated successfully";
                }
                else
                {
                    return "Can not update User";
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error Update User {ex.Message} ";
        }
    }

    //------------------------------Delete users
    [Authorize]
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            var conStr = _configuration.GetConnectionString("Default");
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                var sql = "DELETE FROM BACKEND.dbo.users WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound("User not found");
                    }
                }
            }
            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting user: {ex.Message}");
        }
    }
}