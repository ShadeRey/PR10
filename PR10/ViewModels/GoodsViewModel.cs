using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using MySqlConnector;
using PR10.DataBase;
using PR10.Models;
using PR10.Views;
using ReactiveUI;
using SukiUI.Controls;
using Brushes = Avalonia.Media.Brushes;
using Image = System.Drawing.Image;

namespace PR10.ViewModels;

public class GoodsViewModel : ViewModelBase {
    public GoodsViewModel() {
        GoodsList = GetGoodsFromDataBase();
    }

    private AvaloniaList<Goods> _goodsList = new AvaloniaList<Goods>();

    public AvaloniaList<Goods> GoodsList {
        get => _goodsList;
        set => this.RaiseAndSetIfChanged(ref _goodsList, value);
    }

    private AvaloniaList<Goods> GetGoodsFromDataBase() {
        AvaloniaList<Goods> goodsData = new AvaloniaList<Goods>();
        MySqlConnection connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
        try {
            connection.Open();

            string query = "SELECT * FROM Goods JOIN pro1_23.Manufacturer M on M.Id = Goods.Manufacturer";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                Goods goodItem = new Goods {
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
                    Image = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Image"))
                };

                goodsData.Add(goodItem);
            }
        }
        catch (MySqlException ex) {
            Console.WriteLine("Ошибка подключения к БД: " + ex);
        }
        finally {
            connection.Close();
        }

        return goodsData;
    }

    private async Task<string?> OpenImageDialog(Visual visual) {
        TopLevel tl = TopLevel.GetTopLevel(visual);

        if (tl == null) {
            return null;
        }

        var f = await tl.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            AllowMultiple = false,
        });

        Stream str = await f[0].OpenReadAsync();
        return f.FirstOrDefault()?.Path?.LocalPath;
    }

    public void OnNew(Goods goods) {
        GoodsList.Add(goods);
    }

    public void AddGoodToDB() {
        var db = new DataBaseCommandAdd();

        var pg = new List<ProductCategory>();
        {
            using var connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("""
                                             select * from ProductCategory;
                                             """, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read() && reader.HasRows) {
                var item = new ProductCategory() {
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
            while (reader.Read() && reader.HasRows) {
                var item = new Unit() {
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
            while (reader.Read() && reader.HasRows) {
                var item = new Purveyor() {
                    Id = reader.GetInt32("Id"),
                    PurveyorName = reader.GetString("Purveyor_name")
                };
                purveyor.Add(item);
            }
        }

        var manufacturer = new List<Manufacturer>();
        {
            using var connection = new MySqlConnection(DataBaseConnectionString.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("""
                                             select * from Manufacturer;
                                             """, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read() && reader.HasRows) {
                var item = new Manufacturer() {
                    Id = reader.GetInt32("Id"),
                    ManufacturerName = reader.GetString("Manufacturer_name")
                };
                manufacturer.Add(item);
            }
        }

        var add = ReactiveCommand.Create<Goods>((i) => {
            var newId = db.InsertData(
                "Goods",
                new MySqlParameter("@Name", MySqlDbType.String) {
                    Value = i.Name
                },
                new MySqlParameter("Product_category", MySqlDbType.Int32) {
                    Value = i.ProductCategory
                },
                new MySqlParameter("Quantity_in_stock", MySqlDbType.Int32) {
                    Value = i.QuantityInStock
                },
                new MySqlParameter("Unit", MySqlDbType.Int32) {
                    Value = i.Unit
                },
                new MySqlParameter("Purveyor", MySqlDbType.Int32) {
                    Value = i.Purveyor
                },
                new MySqlParameter("Price", MySqlDbType.Double) {
                    Value = i.Price
                },
                new MySqlParameter("Description", MySqlDbType.String) {
                    Value = i.Description
                },
                new MySqlParameter("Image", MySqlDbType.String) {
                    Value = i.Image
                }
            );
            i.Id = newId;
            OnNew(i);
        });
        Visual vis = null;
        var dataContext = new Goods();
        InteractiveContainer.ShowDialog(new StackPanel() {
            DataContext = dataContext,
            Name = "AddDialog",
            Children = {
                new TextBox() {
                    Watermark = "Артикул",
                    [!TextBox.TextProperty] = new Binding("VendorCode")
                },
                new TextBox() {
                    Watermark = "Наименование",
                    [!TextBox.TextProperty] = new Binding("Name")
                },
                new ComboBox() {
                    PlaceholderText = "Единица измерения",
                    ItemsSource = units,
                    Name = "UnitsComboBox",
                    DisplayMemberBinding = new Binding("UnitName"),
                    [!ComboBox.SelectedValueProperty] = new Binding("Units"),
                    SelectedValueBinding = new Binding("Id")
                },
                new TextBox() {
                    Watermark = "Стоимость",
                    Text = "",
                    [!TextBox.TextProperty] = new Binding("Price")
                },
                new TextBox() {
                    Watermark = "Максимальная скидка",
                    Text = "",
                    [!TextBox.TextProperty] = new Binding("MaximumPossibleDiscountSize")
                },
                new ComboBox() {
                    PlaceholderText = "Производитель",
                    ItemsSource = manufacturer,
                    Name = "ManufacturerComboBox",
                    DisplayMemberBinding = new Binding("ManufacturerName"),
                    [!ComboBox.SelectedValueProperty] = new Binding("Manufacturer"),
                    SelectedValueBinding = new Binding("Id")
                },
                new ComboBox() {
                    PlaceholderText = "Поставщик",
                    ItemsSource = purveyor,
                    Name = "PurveyorComboBox",
                    DisplayMemberBinding = new Binding("PurveyorName"),
                    [!ComboBox.SelectedValueProperty] = new Binding("Purveyor"),
                    SelectedValueBinding = new Binding("Id")
                },
                new ComboBox() {
                    PlaceholderText = "Категория",
                    ItemsSource = pg,
                    Name = "CategoryComboBox",
                    DisplayMemberBinding = new Binding("ProductCategoryName"),
                    [!ComboBox.SelectedValueProperty] = new Binding("ProductCategory"),
                    SelectedValueBinding = new Binding("Id")
                },
                new TextBox() {
                    Watermark = "Текущая скидка",
                    Text = "",
                    [!TextBox.TextProperty] = new Binding("CurrentDiscount")
                },
                new TextBox() {
                    Watermark = "Кол-во на складе",
                    Text = "",
                    [!TextBox.TextProperty] = new Binding("QuantityInStock")
                },
                new TextBox() {
                    Watermark = "Описание",
                    [!TextBox.TextProperty] = new Binding("Description")
                },
                new Button() {
                    Command = ReactiveCommand.Create(async () => {
                        var path = await OpenImageDialog(vis);
                        if (path == null) {
                            return;
                        }

                        var bytes = await File.ReadAllBytesAsync(path);
                        dataContext.Image = bytes;
                    }),
                    Content = new Avalonia.Controls.Image() {
                        [!Avalonia.Controls.Image.SourceProperty] = new Binding("Image") {
                            Converter = new ImageConverter()
                        }
                    }
                },
                new Button() {
                    Content = "Добавить",
                    Classes = { "Primary" },
                    Command = add,
                    Foreground = Brushes.White,
                    [!Button.CommandParameterProperty] = new Binding(".")
                },
                new Button() {
                    Content = "Закрыть",
                    Command = ReactiveCommand.Create(InteractiveContainer.CloseDialog)
                }
            }
        });
    }
}

class ImageConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is not string path) {
            return null;
        }

        if (targetType != typeof(IImage)) {
            return null;
        }

        return new Avalonia.Media.Imaging.Bitmap(path);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}