using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BACKEND.Entity;
using BACKEND.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BACKEND.Controller;

[ApiController]
[Route(template:"api/login")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult DoLogin([FromBody] LoginEntity request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("Username and password are required");
            }

            var conStr = _configuration.GetConnectionString(name: "Default");
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                var sql = "SELECT id, first_name, last_name, email, username, password FROM BACKEND.dbo.users WHERE username = @username";
                using (SqlCommand cd = new SqlCommand(sql, conn))
                {
                    cd.Parameters.AddWithValue("@username", request.username);
                    using (SqlDataReader reader = cd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new UserEntity
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                first_name = reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.GetString(reader.GetOrdinal("last_name")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                username = reader.GetString(reader.GetOrdinal("username")),
                                password = reader.GetString(reader.GetOrdinal("password"))
                            };

                            var helper = new GeneralHelper();
                            if (helper.VerifyPassword(request.password, user.password))
                            {
                                // Generate JWT token
                                var token = GenerateJwtToken(user.id.ToString(), user.username);
                                var response = new
                                {
                                    firstName = user.first_name,
                                    lastName = user.last_name,
                                    email = user.email,
                                    userName = user.username,
                                    token = token
                                };
                                return Ok(response);
                            }
                        }
                    }
                }
            }
            return Unauthorized("Invalid username or password");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during login: {ex.Message}");
        }
    }

    // Helper method to generate JWT token
    private string GenerateJwtToken(string userId, string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8), // Token expires in 8 hour
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}