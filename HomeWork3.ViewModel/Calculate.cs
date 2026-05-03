using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace HomeWork3.ViewModel;

public class Calculate : INotifyPropertyChanged
{
    private string _firstValue = "";
    private string _result = "";
    private string _display = "0";
    
    private double _storedValue; // Здесь храним первое число
    private string _activeOperation = ""; // Здесь храним знак
    private bool _isNewEntry = true; // Флаг: начинаем ли мы вводить новое число
    private string _print = "";

    public string Print
    {
        get => _print;
        set
        {
            _print = value; 
            OnPropertyChanged();
        }
    }

    public string FirstValue
    {
        get => _firstValue;

        set
        {
            _firstValue = value; 
            OnPropertyChanged();
        }
    }

    public string Result
    {
        get => _result;

        set
        {
            _result = value; 
            OnPropertyChanged();
        }
    }

    public string Display 
    {
        get => _display;
        set 
        {
            _display = value;
            OnPropertyChanged(); 
        }
    }
    
    public void OnClickButton(object parameter)
    {
        string input = parameter.ToString();
        
    
        if (input == "C")
        {
            Display = "0";
            Print = "";
            _storedValue = 0;
            _activeOperation = "";
            _isNewEntry = true;
        }
        
        else if (input == "+" || input == "-" || input == "*" || input == "/")
        {
            if (double.TryParse(Display, out double currentVal))
            {
                _storedValue = currentVal;
                _activeOperation = input;
                
                Print = $"{_storedValue} {input} ";
                
                _isNewEntry = true; 
            }
        }
        
        else if (input == "=")
        {
            if (double.TryParse(Display, out double secondVal) && _activeOperation != "")
            {
                Print += $"{secondVal}";
                double res = 0; 
                switch (_activeOperation)
                {
                    case "+": res =_storedValue += secondVal; break;
                    case "-": res = _storedValue - secondVal; break; 
                    case "*": res = _storedValue * secondVal; break;
                    case "/": res = secondVal != 0 ? _storedValue / secondVal : 0; break;
                }

                Display = res.ToString();
                _activeOperation = "";
                _isNewEntry = true; 
            }
        }
        else 
        {
            if (_isNewEntry)
            {
                Display = input;
                _isNewEntry = false;
            }
            else
            {
                Display += input;
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged; 
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
