using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PR10.ViewModels;

namespace PR10.Views;

public partial class AuthorizedCustomerView : ReactiveUserControl<AuthorizedCustomerViewModel>
{
    public AuthorizedCustomerView()
    {
        InitializeComponent();
        ViewModel = new AuthorizedCustomerViewModel();
    }
}