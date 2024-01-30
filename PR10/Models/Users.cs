using System;

namespace PR10.Models;

public class Users
{
    public int Id { get; set; }
    public int Role { get; set; }
    public string FullName { get; set; } = String.Empty;
    public string Login { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}