using System.Linq;
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

    private void GoodsSearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (ViewModel.GoodsPreSearch is null)
        {
            ViewModel.GoodsPreSearch = ViewModel.GoodsList;
        }

        if (GoodsSearchTextBox.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(GoodsSearchTextBox.Text))
        {
            GoodsListBox.ItemsSource = ViewModel.GoodsPreSearch;
            return;
        }
        
        Filter();
    }

    private void Filter()
    {
        if (GoodsSearchTextBox.Text is null)
        {
            return;
        }
        else
        {
            if (GoodsFilter.SelectedIndex == 0)
            {
                var filtered = ViewModel.GoodsPreSearch.Where(
                    it => it.Name.ToLower().Contains(GoodsSearchTextBox.Text)
                          || it.Description.ToLower().Contains(GoodsSearchTextBox.Text)
                          || it.Manufacturer_name.ToLower().Contains(GoodsSearchTextBox.Text)
                          || it.Price.ToString().Contains(GoodsSearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                GoodsListBox.ItemsSource = filtered;
            }
            else if (GoodsFilter.SelectedIndex == 1)
            {
                var filtered = ViewModel.GoodsPreSearch
                    .Where(it => it.Name.ToLower().Contains(GoodsSearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                GoodsListBox.ItemsSource = filtered;
            }
            else if (GoodsFilter.SelectedIndex == 2)
            {
                var filtered = ViewModel.GoodsPreSearch
                    .Where(it => it.Description.ToLower().Contains(GoodsSearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(description => description.Description).ToList();
                GoodsListBox.ItemsSource = filtered;
            }
            else if (GoodsFilter.SelectedIndex == 3)
            {
                var filtered = ViewModel.GoodsPreSearch
                    .Where(it => it.Manufacturer_name.ToLower().Contains(GoodsSearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(manufacturerName => manufacturerName.Manufacturer_name).ToList();
                GoodsListBox.ItemsSource = filtered;
            }
            else if (GoodsFilter.SelectedIndex == 4)
            {
                var filtered = ViewModel.GoodsPreSearch
                    .Where(it => it.Price.ToString().Contains(GoodsSearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(price => price.Price).ToList();
                GoodsListBox.ItemsSource = filtered;
            }
        }
    }

    private void GoodsFilter_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => Filter();
}