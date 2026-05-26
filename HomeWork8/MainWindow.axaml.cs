using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HomeWork8.ViewModels;

namespace HomeWork8;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
