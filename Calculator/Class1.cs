using System;

namespace Calculator;

public class Calculator
{
    private double _currentValue = 0;
    private string _currentOperation = null;
    private string _displayValue = "0";
    
    public string DisplayValue
    {
        get => _displayValue;
        private set => _displayValue = value;
    }
    
    public void PressNumber(string digit)
    {
        if (_displayValue == "0")
            _displayValue = digit;
        else
            _displayValue += digit;
    }
    
    public void PressOperation(string operation)
    {
        _currentValue = double.Parse(_displayValue);
        _currentOperation = operation;
        _displayValue = "0";
    }
    
    public void PressEqual()
    {
        if (_currentOperation == null) return;
        
        double secondValue = double.Parse(_displayValue);
        double result = _currentOperation switch
        {
            "+" => _currentValue + secondValue,
            "-" => _currentValue - secondValue,
            "*" => _currentValue * secondValue,
            "/" => _currentValue / secondValue,
            _ => secondValue
        };
        
        _displayValue = result.ToString();
        _currentOperation = null;
    }
    
    public void PressDecimal()
    {
        if (!_displayValue.Contains("."))
            _displayValue += ".";
    }
    
    public void PressClear()
    {
        _displayValue = "0";
        _currentValue = 0;
        _currentOperation = null;
    }
    
    public void PressSign()
    {
        double value = double.Parse(_displayValue);
        _displayValue = (-value).ToString();
    }
    
    public void PressPercent()
    {
        double value = double.Parse(_displayValue);
        _displayValue = (value / 100).ToString();
    }
}