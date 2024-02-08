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

public class AdministratorViewModel: GoodsViewModel
{
    public AdministratorViewModel() : base() { }
    private AvaloniaList<Goods> _goodsPreSearch;

    public AvaloniaList<Goods> GoodsPreSearch
    {
        get => _goodsPreSearch;
        set => this.RaiseAndSetIfChanged(ref _goodsPreSearch, value);
    }
    
    public void OnNew(Goods goods) {
        Goods.Add(goods);
    }
    
    public void AddHousingTypeToDB()
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
    
        var add = ReactiveCommand.Create<Goods>((i) =>
        {
            var newVendorCode = db.InsertData(
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
            i.VendorCode = newVendorCode;
            OnNew(i);
        });
    
        InteractiveContainer.ShowDialog(new StackPanel()
        {
            DataContext = new Goods(),
            Children =
            {
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
                    [!ComboBox.SelectedValueProperty] = new Binding("Unit")
                },
                new TextBox()
                {
                    Watermark = "Кол-во на складе",
                    [!TextBox.TextProperty] = new Binding("Quantity_in_stock")
                },
                new TextBox()
                {
                    Watermark = "Единица измерения",
                    [!TextBox.TextProperty] = new Binding("Units")
                },
                new TextBox()
                {
                    Watermark = "Поставщик",
                    [!TextBox.TextProperty] = new Binding("Purveyor")
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