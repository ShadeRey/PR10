using Avalonia.Collections;
using PR10.Models;
using ReactiveUI;

namespace PR10.ViewModels;

public class AuthorizedCustomerViewModel: GoodsViewModel
{
    public AuthorizedCustomerViewModel() : base() { }
    private AvaloniaList<Goods> _goodsPreSearch;

    public AvaloniaList<Goods> GoodsPreSearch
    {
        get => _goodsPreSearch;
        set => this.RaiseAndSetIfChanged(ref _goodsPreSearch, value);
    }
}