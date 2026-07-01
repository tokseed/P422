using System.Threading;

namespace БезопасныйБанковскийСчет;

// Точка входа в программу, которую ищет компилятор
internal class Program
{
    private static void Main(string[] args)
    {
        // Простой тестовый запуск, чтобы программа успешно собралась и выполнилась
        Account testAccount = new Account(1000);
        Console.WriteLine($"Проект успешно собран! Начальный баланс: {testAccount.Balance}");
    }
}

// Класс банковского счета, защищенный от race condition
public class Account
{
    // Новый эффективный тип Lock из .NET 10 для синхронизации
    private readonly Lock _lockObject = new();
    private decimal _balance;

    // Конструктор, принимающий начальный баланс
    public Account(decimal initialBalance)
    {
        if (initialBalance < 0)
        {
            throw new ArgumentException("Начальный баланс не может быть отрицательным.", nameof(initialBalance));
        }
        _balance = initialBalance;
    }

    // Свойство Balance возвращает текущий баланс
    public decimal Balance
    {
        get
        {
            lock (_lockObject)
            {
                return _balance;
            }
        }
    }

    // Метод Credit - пополняет счет на указанную сумму
    public void Credit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма пополнения должна быть больше нуля.", nameof(amount));
        }

        lock (_lockObject)
        {
            _balance += amount;
        }
    }

    // Метод Debit - списывает указанную сумму со счета
    public bool Debit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма списания должна быть больше нуля.", nameof(amount));
        }

        lock (_lockObject)
        {
            if (_balance >= amount)
            {
                _balance -= amount;
                return true;
            }
            
            return false;
        }
    }
}