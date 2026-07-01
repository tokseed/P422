using System;
using System.Threading.Tasks;

namespace СинхронизацияПотоков
{
    public class Account
    {
        private readonly object _lockObject = new object();
        private decimal _balance;

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

        public Account(decimal initialBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Error");
            _balance = initialBalance;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0) return;
            lock (_lockObject)
            {
                _balance += amount;
            }
        }

        public bool Debit(decimal amount)
        {
            if (amount <= 0) return false;
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

    class Program
    {
        static async Task Main()
        {
            Account account = new Account(1000);
            Console.WriteLine(account.Balance);

            Task[] tasks = new Task[100];

            for (int i = 0; i < 50; i++)
            {
                tasks[i] = Task.Run(() => account.Credit(100));          
                tasks[i + 50] = Task.Run(() => account.Debit(100));      
            }

            await Task.WhenAll(tasks);

            Console.WriteLine(account.Balance);
        }
    }
}