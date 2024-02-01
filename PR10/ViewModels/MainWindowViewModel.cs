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

        if (role != null)
        {
            switch (role)
            {
                case "1":
                    AdministratorView administratorView = new AdministratorView();
                    administratorView.Show();
                    MainWindow mainWindow1 = new MainWindow();
                    mainWindow1.Close();
                    break;
                case "2":
                    AuthorizedСustomerView authorizedСustomerView1 = new AuthorizedСustomerView();
                    authorizedСustomerView1.Show();
                    MainWindow mainWindow2 = new MainWindow();
                    mainWindow2.Close();
                    break;
                case "3":
                    AuthorizedСustomerView authorizedСustomerView2 = new AuthorizedСustomerView();
                    authorizedСustomerView2.Show();
                    MainWindow mainWindow3 = new MainWindow();
                    mainWindow3.Close();
                    break;
            }
        }
        else
        {
            InvalidVisible = true;
        }
    }
}