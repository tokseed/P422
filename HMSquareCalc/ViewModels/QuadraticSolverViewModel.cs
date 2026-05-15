using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HMSquareCalc.Infrastructure;

namespace HMSquareCalc.ViewModels;

public class QuadraticSolverViewModel : INotifyPropertyChanged
{
    private const double Epsilon = 1e-10;

    private string _coefficientA = string.Empty;
    private string _coefficientB = string.Empty;
    private string _coefficientC = string.Empty;
    private string _result = string.Empty;

    public QuadraticSolverViewModel()
    {
        SolveCommand = new RelayCommand(Solve);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string CoefficientA
    {
        get => _coefficientA;
        set => SetField(ref _coefficientA, value);
    }

    public string CoefficientB
    {
        get => _coefficientB;
        set => SetField(ref _coefficientB, value);
    }

    public string CoefficientC
    {
        get => _coefficientC;
        set => SetField(ref _coefficientC, value);
    }

    public string Result
    {
        get => _result;
        set => SetField(ref _result, value);
    }

    public ICommand SolveCommand { get; }

    private void Solve()
    {
        if (!TryParseCoefficient(CoefficientA, out var a) ||
            !TryParseCoefficient(CoefficientB, out var b) ||
            !TryParseCoefficient(CoefficientC, out var c))
        {
            Result = "Ошибка: введите корректные числовые коэффициенты.";
            return;
        }

        if (Math.Abs(a) < Epsilon)
        {
            SolveLinearEquation(b, c);
            return;
        }

        var discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            Result = "Действительных корней нет.";
            return;
        }

        if (Math.Abs(discriminant) < Epsilon)
        {
            var x = -b / (2 * a);
            Result = $"Один корень: x = {x:F2}";
            return;
        }

        var sqrtDiscriminant = Math.Sqrt(discriminant);
        var x1 = (-b + sqrtDiscriminant) / (2 * a);
        var x2 = (-b - sqrtDiscriminant) / (2 * a);
        Result = $"Два корня: x1 = {x1:F2}; x2 = {x2:F2}";
    }

    private void SolveLinearEquation(double b, double c)
    {
        if (Math.Abs(b) < Epsilon)
        {
            Result = Math.Abs(c) < Epsilon
                ? "Бесконечно много решений."
                : "Решений нет.";
            return;
        }

        var x = -c / b;
        Result = $"Линейное уравнение: x = {x:F2}";
    }

    private static bool TryParseCoefficient(string value, out double result)
    {
        return double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out result) ||
               double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }

    private void SetField(ref string field, string value, [CallerMemberName] string? propertyName = null)
    {
        if (field == value)
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
