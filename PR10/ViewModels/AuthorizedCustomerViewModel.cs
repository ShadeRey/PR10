using Avalonia.Collections;
using HarfBuzzSharp;
using MySqlConnector;
using PR10.DataBase;
using PR10.Models;
using ReactiveUI;

namespace PR10.ViewModels;

public class AuthorizedCustomerViewModel: GoodsViewModel {
    public AuthorizedCustomerViewModel() : base() { }
    private AvaloniaList<Goods> _goodsPreSearch;

    public AvaloniaList<Goods> GoodsPreSearch
    {
        get => _goodsPreSearch;
        set => this.RaiseAndSetIfChanged(ref _goodsPreSearch, value);
    }
    
    private string ValidateUser(string login, string password)
    {
        string connectionString = DataBaseConnectionString.ConnectionString;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Full_name FROM Users WHERE Login = @Login AND Password = @Password";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) {
                        return reader["Full_name"].ToString();
                    }
                }
            }
        }

        return null;
    }

    private string _login;
    private string _password;
    public string Login {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    public string Password {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string _fullName;

    public string FullName {
        get => ValidateUser(Login, Password);
        set => this.RaiseAndSetIfChanged(ref _fullName, ValidateUser(Login, Password));
    }
}