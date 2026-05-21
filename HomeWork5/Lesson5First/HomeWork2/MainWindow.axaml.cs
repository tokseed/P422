using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Utilities;

namespace SquareEquation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SquareRoot_Calculate(object sender, RoutedEventArgs e)
    {

        bool isAParsed = double.TryParse(TextBoxA.Text ?? "", out double a);
        bool isBParsed = double.TryParse(TextBoxB.Text ?? "", out double b);
        bool isCParsed = double.TryParse(TextBoxС.Text ?? "", out double c);

        string resultMessage = "";

        if (!isAParsed || !isBParsed || !isCParsed)
        {
            resultMessage = "Ошибка: введите корректные числовые значения.";
            
            var errorWin = new SecondWindow(resultMessage);
            errorWin.Show();
            return; 
        }

        // 2. Логика вычислений
        double d = b * b - 4 * a * c;

        if (Math.Abs(a) < 1e-10)
        {
            if (Math.Abs(b) > 1e-10)
            {
                double x = -c / b;
                resultMessage = $"Линейное уравнение. Корень x = {x:F2}";
            }
            else
            {
                resultMessage = (Math.Abs(c) < 1e-10) ? "Бесконечное множество решений" : "Решений нет";
            }
        }
        else if (d > 1e-10)
        {
            double sqrtD = Math.Sqrt(d);
            double x1 = (-b + sqrtD) / (2 * a);
            double x2 = (-b - sqrtD) / (2 * a);
            resultMessage = $"Два корня: x1 = {x1:F2}, x2 = {x2:F2}";
        }
        else if (Math.Abs(d) < 1e-10)
        {
            double x = -b / (2 * a);
            resultMessage = $"Один корень: x = {x:F2}";
        }
        else
        {
            resultMessage = "Действительных корней нет (D < 0)";
        }

        // 3. Открываем новое окно и передаем результат
        var win = new SecondWindow(resultMessage);
    

        win.Show();
    
        this.Close();
    }
}