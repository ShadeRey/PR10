using Avalonia.Collections;
using HarfBuzzSharp;
using MySqlConnector;
using PR10.DataBase;
using PR10.Models;
using ReactiveUI;

namespace PR10.ViewModels;

public class AuthorizedCustomerViewModel: GoodsViewModel{
    public AuthorizedCustomerViewModel() : base() { }
    private AvaloniaList<Goods> _goodsPreSearch;

    public AvaloniaList<Goods> GoodsPreSearch
    {
        get => _goodsPreSearch;
        set => this.RaiseAndSetIfChanged(ref _goodsPreSearch, value);
    }

    private string _fullName;

    public string FullName 
    {
        get => _fullName;
        set => this.RaiseAndSetIfChanged(ref _fullName, value);
    }
}