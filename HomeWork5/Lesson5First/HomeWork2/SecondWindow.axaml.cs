using Avalonia.Controls;

namespace SquareEquation;

public partial class SecondWindow : Window
{
    // Добавляем : this(), чтобы вызвать пустой конструктор и InitializeComponent()
    public SecondWindow(string resultMessage) : this()
    {
        // Ищем TextBox, который ты объявил в AXAML под именем ResultTextBox
        var tb = this.FindControl<TextBox>("ResultTextBox");
        
        if (tb != null)
        {
            tb.Text = resultMessage;
        }
    }
    
    public SecondWindow()
    {
        InitializeComponent();
    }
}