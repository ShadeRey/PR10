using System;
using Avalonia.Collections;
using MySqlConnector;
using PR10.DataBase;
using PR10.Models;
using ReactiveUI;

namespace PR10.ViewModels;

public class GoodsViewModel: ViewModelBase
{
    public GoodsViewModel()
    {
        GoodsList = GetGoodsFromDataBase();
    }
    private AvaloniaList<Goods> _goodsList = new AvaloniaList<Goods>();

    public AvaloniaList<Goods> GoodsList
    {
        get => _goodsList;
        set => this.RaiseAndSetIfChanged(ref _goodsList, value);
    }

    private AvaloniaList<Goods> GetGoodsFromDataBase()
    {
        AvaloniaList<Goods> goodsData = new AvaloniaList<Goods>();
        MySqlConnection connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
        try
        {
            connection.Open();

            string query = "SELECT * FROM Goods JOIN pro1_23.Manufacturer M on M.Id = Goods.Manufacturer";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Goods goodItem = new Goods
                {
                    VendorCode = reader.GetString("Vendor_code"),
                    Name = reader.GetString("Name"),
                    Unit = reader.GetInt32("Unit"),
                    Price = reader.GetDouble("Price"),
                    MaximumPossibleDiscountSize = reader.GetDouble("Maximum_possible_discount_size"),
                    Manufacturer_name = reader.GetString("Manufacturer_name"),
                    Purveyor = reader.GetInt32("Purveyor"),
                    ProductCategory = reader.GetInt32("Product_category"),
                    CurrentDiscount = reader.GetDouble("Current_discount"),
                    QuantityInStock = reader.GetInt32("Quantity_in_stock"),
                    Description = reader.GetString("Description"),
                    Image = reader.GetString("Image")
                };

                goodsData.Add(goodItem);
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Ошибка подключения к БД: " + ex);
        }
        finally
        {
            connection.Close();
        }

        return goodsData;
    }
}