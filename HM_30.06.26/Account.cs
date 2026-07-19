using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp;

public class Account
{
    private decimal _balance;
    private readonly object _lock = new object();

    public Account(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative.");

        _balance = initialBalance;
    }

    public decimal Balance
    {
        get
        {
            lock (_lock)
            {
                return _balance;
            }
        }
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Credit amount must be positive.");

        lock (_lock)
        {
            _balance += amount;
        }
    }

    public bool Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Debit amount must be positive.");

        lock (_lock)
        {
            if (_balance < amount)
                return false;

            _balance -= amount;
            return true;
        }
    }
}
