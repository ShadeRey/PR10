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
}