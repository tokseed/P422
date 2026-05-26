using System.Collections.ObjectModel;
using System.Windows.Input;
using HomeWork8.Helpers;
using HomeWork8.Models;
using HomeWork8.Services;

namespace HomeWork8.ViewModels;

public class MainViewModel : BaseViewModel
{
    private DatabaseService? _databaseService;
    private string _statusMessage = string.Empty;

    public MainViewModel()
    {
        RefreshCommand = new RelayCommand(_ => LoadUsers());
        TryInitializeDatabase();
    }

    public ObservableCollection<User> Users { get; } = [];

    public ICommand RefreshCommand { get; }

    public string QueryText => "SELECT Id, FirstName, LastName, Email, City, CreatedAt FROM Users ORDER BY Id";

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (SetProperty(ref _statusMessage, value))
            {
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }
    }

    public bool HasStatusMessage => !string.IsNullOrWhiteSpace(StatusMessage);

    private void LoadUsers()
    {
        try
        {
            if (_databaseService == null)
            {
                StatusMessage = "Нет подключения к базе данных.";
                return;
            }

            Users.Clear();

            foreach (var user in _databaseService.GetUsers())
            {
                Users.Add(user);
            }

            StatusMessage = $"Загружено записей: {Users.Count}.";
        }
        catch (Exception ex)
        {
            ShowDatabaseError(ex);
        }
    }

    private void TryInitializeDatabase()
    {
        try
        {
            _databaseService = new DatabaseService();
            LoadUsers();
        }
        catch (Exception ex)
        {
            ShowDatabaseError(ex);
        }
    }

    private void ShowDatabaseError(Exception ex)
    {
        StatusMessage = $"Ошибка при работе с базой данных: {ex.Message}";
    }
}
