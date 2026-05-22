using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lesson5First.Models;

namespace Lesson5First.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Product> Products { get; set; } = new();
    
    private string _newName = "";
    public string NewName
    {
        get => _newName;
        set { _newName = value; OnPropertyChanged(); }
    }
    
    private string _newCategory = "";
    public string NewCategory
    {
        get => _newCategory;
        set { _newCategory = value; OnPropertyChanged(); }
    }
    
    private string _newPrice = "";
    public string NewPrice
    {
        get => _newPrice;
        set { _newPrice = value; OnPropertyChanged(); }
    }
    
    private Product? _selectedProduct;
    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set { _selectedProduct = value; OnPropertyChanged(); }
    }
    
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }
    
    public MainWindowViewModel()
    {
        Products.Add(new Product { Id = 1, Name = "Ноутбук", Category = "Электроника", Price = 50000 });
        Products.Add(new Product { Id = 2, Name = "Мышь", Category = "Электроника", Price = 1500 });
        
        AddCommand = new RelayCommand(_ => AddProduct());
        DeleteCommand = new RelayCommand(p => DeleteProduct(p as Product));
        SaveCommand = new RelayCommand(_ => SaveProduct());
    }
    
    private void AddProduct()
    {
        if (string.IsNullOrWhiteSpace(NewName) || 
            string.IsNullOrWhiteSpace(NewCategory) || 
            !decimal.TryParse(NewPrice, out var price))
            return;
        
        int newId = Products.Count + 1;
        Products.Add(new Product { Id = newId, Name = NewName, Category = NewCategory, Price = price });
        
        NewName = "";
        NewCategory = "";
        NewPrice = "";
    }
    
    private void DeleteProduct(Product? product)
    {
        if (product != null) Products.Remove(product);
    }
    
    private void SaveProduct()
    {
        if (SelectedProduct != null)
        {
            // Находим индекс выбранного продукта
            var index = Products.IndexOf(SelectedProduct);
            
            // Создаём КОПИЮ с обновлёнными данными
            var updated = new Product
            {
                Id = SelectedProduct.Id,
                Name = SelectedProduct.Name,
                Category = SelectedProduct.Category,
                Price = SelectedProduct.Price
            };
            
            // Заменяем старый продукт новым
            Products[index] = updated;
            
            // Обновляем SelectedProduct на новый объект
            SelectedProduct = updated;
            
            // Принудительно обновляем список
            OnPropertyChanged(nameof(Products));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
