using MySqlConnector;
using PR10.DataBase;
using PR10.Views;
using ReactiveUI;

namespace PR10.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _login;
    private string _password;
    private bool _invalidVisible;
    private bool _administratorViewVisible;
    private bool _authorizedCustomerViewVisible;
    private bool _loginDialogOpen = true;
    private bool _captchaVisible;
    private int _loginDialogHeight = 200;
    private string _captchaText;
    private string _captchaTextBoxText;

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public bool InvalidVisible
    {
        get => _invalidVisible;
        set => this.RaiseAndSetIfChanged(ref _invalidVisible, value);
    }

    public bool AdministratorViewVisible
    {
        get => _administratorViewVisible;
        set => this.RaiseAndSetIfChanged(ref _administratorViewVisible, value);
    }

    public bool AuthorizedCustomerViewVisible
    {
        get => _authorizedCustomerViewVisible;
        set => this.RaiseAndSetIfChanged(ref _authorizedCustomerViewVisible, value);
    }

    public bool LoginDialogOpen
    {
        get => _loginDialogOpen;
        set => this.RaiseAndSetIfChanged(ref _loginDialogOpen, value);
    }

    public bool CaptchaVisible
    {
        get => _captchaVisible;
        set => this.RaiseAndSetIfChanged(ref _captchaVisible, value);
    }

    public int LoginDialogHeight
    {
        get => _loginDialogHeight;
        set => this.RaiseAndSetIfChanged(ref _loginDialogHeight, value);
    }

    public string CaptchaText
    {
        get => _captchaText;
        set => this.RaiseAndSetIfChanged(ref _captchaText, value);
    }

    public string CaptchaTextBoxText
    {
        get => _captchaTextBoxText;
        set => this.RaiseAndSetIfChanged(ref _captchaTextBoxText, value);
    }

    private string ValidateUser(string login, string password)
    {
        string connectionString = DataBaseConnectionString.ConnectionString;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Role FROM Users WHERE Login = @Login AND Password = @Password";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["Role"].ToString();
                    }
                }
            }
        }

        return null;
    }

    public void LoginButton()
    {
        string login = Login;
        string password = Password;
        string role = ValidateUser(login, password);
        
        if (CaptchaTextBoxText == CaptchaText)
        {
            
        }
        else
        {
            
        }

        if (role != null)
        {
            switch (role)
            {
                case "1":
                    AdministratorViewVisible = true;
                    LoginDialogOpen = false;
                    break;
                case "2":
                    AuthorizedCustomerViewVisible = true;
                    LoginDialogOpen = false;
                    break;
                case "3":
                    AuthorizedCustomerViewVisible = true;
                    LoginDialogOpen = false;
                    break;
            }
        }
        else
        {
            InvalidVisible = true;
            CaptchaVisible = true;
            LoginDialogHeight = 350;
        }
    }
}