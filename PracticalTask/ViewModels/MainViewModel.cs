using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PracticalTask.Helpers;
using PracticalTask.Models;
using PracticalTask.Services;

namespace PracticalTask.ViewModels;

public class MainViewModel : BaseViewModel
{
    private DatabaseService? _databaseService;

    private Book? _selectedBook;
    private string _title = string.Empty;
    private string _author = string.Empty;
    private string _publishYearText = string.Empty;
    private string _genre = string.Empty;
    private string _statusMessage = string.Empty;
    private string _searchText = string.Empty;

    public MainViewModel()
    {
        AddCommand = new RelayCommand(_ => AddBook());
        UpdateCommand = new RelayCommand(_ => UpdateBook(), _ => SelectedBook != null);
        DeleteCommand = new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
        ClearCommand = new RelayCommand(_ => ClearFields());
        ClearSearchCommand = new RelayCommand(_ => ClearSearch());

        TryInitializeDatabase();
    }

    // Оригинальная коллекция (только для внутреннего использования)
    private ObservableCollection<Book> _allBooks = [];
    
    // Отфильтрованная коллекция для отображения в UI
    public ObservableCollection<Book> FilteredBooks { get; } = [];

    // Свойство для поиска
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                UpdateFilteredBooks();
                OnPropertyChanged(nameof(IsSearchActive));
                OnPropertyChanged(nameof(SearchResultText));
            }
        }
    }

    // Активен ли поиск (для отображения кнопки очистки и статистики)
    public bool IsSearchActive => !string.IsNullOrWhiteSpace(SearchText);

    // Текст статистики поиска
    public string SearchResultText => IsSearchActive ? 
        $"Найдено: {FilteredBooks.Count} из {_allBooks.Count}" : "";

    // Команда очистки поиска
    public ICommand ClearSearchCommand { get; }

    public ICommand AddCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ClearCommand { get; }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (!SetProperty(ref _selectedBook, value))
            {
                return;
            }

            if (value != null)
            {
                Title = value.Title;
                Author = value.Author;
                PublishYearText = value.PublishYear.ToString();
                Genre = value.Genre;
            }

            RaiseCommandStates();
        }
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Author
    {
        get => _author;
        set => SetProperty(ref _author, value);
    }

    public string PublishYearText
    {
        get => _publishYearText;
        set => SetProperty(ref _publishYearText, value);
    }

    public string Genre
    {
        get => _genre;
        set => SetProperty(ref _genre, value);
    }

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

    private void LoadBooks()
    {
        _allBooks.Clear();
        FilteredBooks.Clear();

        foreach (var book in _databaseService.GetBooks())
        {
            _allBooks.Add(book);
        }
        
        UpdateFilteredBooks();
    }

    // Метод фильтрации книг
    private void UpdateFilteredBooks()
    {
        FilteredBooks.Clear();
        
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // Если поиск пуст - показываем все книги
            foreach (var book in _allBooks)
                FilteredBooks.Add(book);
        }
        else
        {
            // Ищем по всем полям (название, автор, жанр, год)
            var searchLower = SearchText.ToLower();
            var filtered = _allBooks.Where(book => 
                book.Title.ToLower().Contains(searchLower) ||
                book.Author.ToLower().Contains(searchLower) ||
                book.Genre.ToLower().Contains(searchLower) ||
                book.PublishYear.ToString().Contains(SearchText) // поиск по году
            ).ToList();
            
            foreach (var book in filtered)
                FilteredBooks.Add(book);
        }
        
        OnPropertyChanged(nameof(SearchResultText));
        
        // Если выбранная книга не входит в результаты поиска - снимаем выделение
        if (SelectedBook != null && !FilteredBooks.Contains(SelectedBook))
        {
            SelectedBook = null;
        }
    }

    // Очистка поиска
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    private void AddBook()
    {
        if (!TryBuildBook(out var book))
        {
            return;
        }

        try
        {
            if (_databaseService == null)
            {
                StatusMessage = "Нет подключения к базе данных.";
                return;
            }

            _databaseService.AddBook(book);
            LoadBooks(); // Теперь LoadBooks вызывает UpdateFilteredBooks
            ClearFields();
            StatusMessage = "Книга успешно добавлена.";
        }
        catch (Exception ex)
        {
            ShowDatabaseError(ex);
        }
    }

    private void UpdateBook()
    {
        if (SelectedBook == null)
        {
            StatusMessage = "Выберите книгу для редактирования.";
            return;
        }

        if (!TryBuildBook(out var book))
        {
            return;
        }

        try
        {
            if (_databaseService == null)
            {
                StatusMessage = "Нет подключения к базе данных.";
                return;
            }

            book.Id = SelectedBook.Id;
            _databaseService.UpdateBook(book);
            LoadBooks(); // Теперь LoadBooks вызывает UpdateFilteredBooks
            ClearFields();
            StatusMessage = "Книга успешно обновлена.";
        }
        catch (Exception ex)
        {
            ShowDatabaseError(ex);
        }
    }

    private void DeleteBook()
    {
        if (SelectedBook == null)
        {
            StatusMessage = "Выберите книгу для удаления.";
            return;
        }

        try
        {
            if (_databaseService == null)
            {
                StatusMessage = "Нет подключения к базе данных.";
                return;
            }

            _databaseService.DeleteBook(SelectedBook.Id);
            LoadBooks(); // Теперь LoadBooks вызывает UpdateFilteredBooks
            ClearFields();
            StatusMessage = "Книга успешно удалена.";
        }
        catch (Exception ex)
        {
            ShowDatabaseError(ex);
        }
    }

    private bool TryBuildBook(out Book book)
    {
        book = new Book();

        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Author) ||
            string.IsNullOrWhiteSpace(PublishYearText) ||
            string.IsNullOrWhiteSpace(Genre))
        {
            StatusMessage = "Все поля должны быть заполнены.";
            return false;
        }

        if (!int.TryParse(PublishYearText, out var publishYear) ||
            publishYear < 0 ||
            publishYear > DateTime.Now.Year)
        {
            StatusMessage = "Введите корректный год издания.";
            return false;
        }

        book.Title = Title.Trim();
        book.Author = Author.Trim();
        book.PublishYear = publishYear;
        book.Genre = Genre.Trim();
        StatusMessage = string.Empty;
        return true;
    }

    private void ClearFields()
    {
        SelectedBook = null;
        Title = string.Empty;
        Author = string.Empty;
        PublishYearText = string.Empty;
        Genre = string.Empty;
        StatusMessage = string.Empty;
        RaiseCommandStates();
    }

    private void RaiseCommandStates()
    {
        ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
    }

    private void TryLoadBooks()
    {
        try
        {
            if (_databaseService == null)
            {
                StatusMessage = "Нет подключения к базе данных.";
                return;
            }

            LoadBooks();
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
            TryLoadBooks();
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