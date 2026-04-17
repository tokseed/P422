using System;
using System.IO;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace P422HMandCL; // Исправлена опечатка

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            if (!File.Exists("person.json"))
                return;

            var personJson = File.ReadAllText("person.json");
            var person = JsonSerializer.Deserialize<Person>(personJson);

            if (person != null)
            {
                FirstNameTextBox.Text = person.FirstName;
                LastNameTextBox.Text = person.LastName;
                PatronymicTextBox.Text = person.Patronymic;
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибок чтения/десериализации
            Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
        }
    }

    private void Button_Click_SaveData(object? sender, RoutedEventArgs e)
    {
        // Очистка сообщений об ошибках
        ErrorResultFirstName.Text = string.Empty;
        ErrorResultLastName.Text = string.Empty;
        ErrorResultPatronymic.Text = string.Empty;

        // Проверка имени
        if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
        {
            ErrorResultFirstName.Text = "Некорректное имя";
            return;
        }

        // Проверка фамилии
        if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
        {
            ErrorResultLastName.Text = "Некорректная фамилия";
            return;
        }

        // Проверка отчества (исправлена логика)
        var patronymic = PatronymicTextBox.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(patronymic))
        {
            ErrorResultPatronymic.Text = "Некорректное отчество";
            return;
        }

        // Создание объекта и сохранение
        var person = new Person
        {
            FirstName = FirstNameTextBox.Text,
            LastName = LastNameTextBox.Text,
            Patronymic = patronymic
        };

        try
        {
            var json = JsonSerializer.Serialize(person);
            File.WriteAllText("person.json", json);
        }
        catch (Exception ex)
        {
            // Обработка ошибок сохранения
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox checkBox)
        {
            return;
        }

        if (CheckBoxR.IsChecked == true)
        {
            RedacttextboBox.IsReadOnly = true;
        }

        if (!CheckBoxR.IsChecked == true)
        {
            RedacttextboBox.IsReadOnly = false;
        }
    }

// Определение класса Person (если ещё не создан)
    public class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;

    }
}