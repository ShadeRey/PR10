using System;
using ReactiveUI;

namespace PR10.Models;

public class Users: ReactiveObject
{
    public int Id { get; set; }
    private string _role;
    private string _fullName;
    public string Login { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;

    public string Role {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }

    public string FullName {
        get => _fullName;
        set => this.RaiseAndSetIfChanged(ref _fullName, value);
    }
}