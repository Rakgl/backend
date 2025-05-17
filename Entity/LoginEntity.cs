using System.ComponentModel.DataAnnotations;

namespace BACKEND.Entity;

//Class for login request payload
public class LoginEntity
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string username { set; get; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string password { set; get; }
}