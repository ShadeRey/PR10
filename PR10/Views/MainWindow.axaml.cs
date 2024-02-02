using Avalonia.Controls;
using PR10.ViewModels;

namespace PR10.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; set; }
    public MainWindow()
    {
        ViewModel = new MainWindowViewModel();
        InitializeComponent();
        CaptchaControl.Generate();
    }
}