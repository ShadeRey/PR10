namespace PR10.DataBase;

public static class DataBaseConnectionString
{
    private static string _connectionString = "server=10.10.1.24;user=user_01;password=user01pro;database=pro1_23;";

    public static string ConnectionString
    {
        get => _connectionString;
    }
}