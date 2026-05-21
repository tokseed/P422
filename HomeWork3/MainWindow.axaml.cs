using Avalonia.Controls;
using Avalonia.Interactivity;
using HomeWork3.ViewModel;

namespace HomeWork3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        
        InitializeComponent();
        DataContext = new Calculate();
    }
}