namespace BACKEND.Entity;

// Class for Crud user
public class UserEntity
{
    public int id { set; get; }
    public string first_name { set; get; }
    public string last_name { set; get; }
    public string email { set; get; }
    public string username { set; get; }
    public string password { set; get; }
}