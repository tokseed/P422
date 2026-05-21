using Avalonia.Controls;
using HMSquareCalc.ViewModels;

namespace HMSquareCalc;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new QuadraticSolverViewModel();
    }
}
