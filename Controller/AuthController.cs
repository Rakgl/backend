using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BACKEND.Entity;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email or password" });

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims = claims,
            DateTime.Now.AddHours(1));
           

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin(string redirectUri)
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUri ?? $"{Request.Scheme}://{Request.Host}/api/auth/google-response");
        return new ChallengeResult("Google", properties);
    }

    [HttpPost("google-response")]
    public async Task<IActionResult> GoogleResponse([FromBody] GoogleCodeModel model)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            Console.WriteLine("ExternalLoginInfo is null");
            return Unauthorized("Failed to retrieve Google login info.");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? email.Split('@')[0],
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "Unknown"
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
                return BadRequest(addLoginResult.Errors);
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (!signInResult.Succeeded)
            return Unauthorized("Google sign-in failed.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims = claims,
            DateTime.Now.AddHours(1));

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class GoogleCodeModel
{
    public string Code { get; set; }
}