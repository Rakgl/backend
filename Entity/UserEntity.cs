using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace BACKEND.Entity;

// Class for Crud user
public class UserEntity
{
    public int id { set; get; }
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string first_name { set; get; }
    
    [Required(ErrorMessage = "Profile is required")]
    [StringLength(500,ErrorMessage = "Invalid Profile format")]
    public string profile { get; set; } = string.Empty;
    
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string last_name { set; get; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Employee is required")]
    [StringLength(50,ErrorMessage = "Invalid Employee format")]
    public string employee { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Position is required")]
    [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
    public string position { get; set; } = string.Empty;
    
    
    [Required(ErrorMessage = "Department is required")]
    [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters")]
    public string department { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone is required")]
    [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
    public string phone { get; set; } = string.Empty;
    
    public DateTime hire_date { get; set; }
    
    [Required(ErrorMessage = "status is required")]
    [StringLength(50, ErrorMessage = "status cannot null")]
    public string status { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string password { get; set; } = string.Empty;
    
}