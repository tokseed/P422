using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PracticalTask.ViewModels;

namespace PracticalTask;

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
