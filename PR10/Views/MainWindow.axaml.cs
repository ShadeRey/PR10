using System;
using System.Reactive.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using PR10.ViewModels;
using ReactiveUI;

namespace PR10.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        ViewModel = new MainWindowViewModel();
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        ViewModel.WhenAnyValue(x => x.CaptchaVisible)
            .DistinctUntilChanged()
            .Subscribe(ViewModelOnCaptchaVisibilityChanged);
    }

    private void ViewModelOnCaptchaVisibilityChanged(bool obj)
    {
        CaptchaControl.Generate();
        if (ViewModel != null)
        {
            ViewModel.CaptchaText = CaptchaControl.CaptchaText;
        }
    }
}