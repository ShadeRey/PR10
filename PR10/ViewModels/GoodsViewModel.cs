using System;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using MySqlConnector;
using PR10.DataBase;
using PR10.Models;
using ReactiveUI;
using SukiUI.Controls;

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
    
    public void OnNew(Goods goods) {
        GoodsList.Add(goods);
    }
    
    public void AddGoodToDB()
    {
        var db = new DataBaseCommandAdd();
        
        var pg = new List<ProductCategory>();
        {
            using var connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("""
                                             select * from ProductCategory;
                                             """, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var item = new ProductCategory()
                {
                    Id = reader.GetInt32("Id"),
                    ProductCategoryName = reader.GetString("Product_category_name")
                };
                pg.Add(item);
            }
        }
        
        var units = new List<Unit>();
        {
            using var connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("""
                                             select * from Units;
                                             """, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var item = new Unit()
                {
                    Id = reader.GetInt32("Id"),
                    UnitName = reader.GetString("Unit_name")
                };
                units.Add(item);
            }
        }
        
        var purveyor = new List<Purveyor>();
        {
            using var connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("""
                                             select * from Purveyor;
                                             """, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var item = new Purveyor()
                {
                    Id = reader.GetInt32("Id"),
                    PurveyorName = reader.GetString("Purveyor_name")
                };
                purveyor.Add(item);
            }
        }
    
        var add = ReactiveCommand.Create<Goods>((i) =>
        {
            var newId = db.InsertData(
                "Goods",
                new MySqlParameter("@Name", MySqlDbType.String)
                {
                    Value = i.Name
                },
                new MySqlParameter("Product_category", MySqlDbType.Int32)
                {
                    Value = i.ProductCategory
                },
                new MySqlParameter("Quantity_in_stock", MySqlDbType.Int32)
                {
                    Value = i.QuantityInStock
                },
                new MySqlParameter("Units", MySqlDbType.Int32)
                {
                    Value = i.Unit
                },
                new MySqlParameter("Purveyor", MySqlDbType.Int32)
                {
                    Value = i.Purveyor
                },
                new MySqlParameter("Price", MySqlDbType.Double)
                {
                    Value = i.Price
                },
                new MySqlParameter("Description", MySqlDbType.String)
                {
                    Value = i.Description
                },
                new MySqlParameter("Image", MySqlDbType.String)
                {
                    Value = i.Image
                }
            );
            i.Id = newId;
            OnNew(i);
        });
    
        InteractiveContainer.ShowDialog(new StackPanel()
        {
            DataContext = new Goods(),
            Children =
            {
                new TextBox()
                {
                    Watermark = "Артикул",
                    [!TextBox.TextProperty] = new Binding("VendorCode")
                },
                new TextBox()
                {
                    Watermark = "Наименование",
                    [!TextBox.TextProperty] = new Binding("Name")
                },
                new ComboBox()
                {
                    PlaceholderText = "Категория",
                    ItemsSource = pg,
                    Name = "CategoryComboBox",
                    [!ComboBox.SelectedValueProperty] = new Binding("ProductCategory")
                },
                new TextBox()
                {
                    Watermark = "Кол-во на складе",
                    [!TextBox.TextProperty] = new Binding("QuantityInStock")
                },
                new ComboBox()
                {
                    PlaceholderText = "Единица измерения",
                    ItemsSource = units,
                    Name = "UnitsComboBox",
                    [!ComboBox.SelectedValueProperty] = new Binding("Units")
                },
                new ComboBox()
                {
                    PlaceholderText = "Поставщик",
                    ItemsSource = purveyor,
                    Name = "PurveyorComboBox",
                    [!ComboBox.SelectedValueProperty] = new Binding("Purveyor")
                },
                new TextBox()
                {
                    Watermark = "Стоимость",
                    [!TextBox.TextProperty] = new Binding("Price")
                },
                new TextBox()
                {
                    Watermark = "Описание",
                    [!TextBox.TextProperty] = new Binding("Description")
                },
                new TextBox()
                {
                    Watermark = "Изображение",
                    [!TextBox.TextProperty] = new Binding("Image")
                },
                new Button()
                {
                    Content = "Добавить",
                    Classes = { "Primary" },
                    Command = add,
                    Foreground = Brushes.White,
                    [!Button.CommandParameterProperty] = new Binding(".")
                },
                new Button()
                {
                    Content = "Закрыть",
                    Command = ReactiveCommand.Create(InteractiveContainer.CloseDialog)
                }
            }
        });
    }
}