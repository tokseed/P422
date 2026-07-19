using System;
using System.Threading;
using System.Threading.Tasks;
using BankApp;

namespace BankApp;

class Program
{
    static async Task Main(string[] args)
    {
        var account = new Account(1000m);

        var tasks = new Task[100];

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 100; j++)
                {
                    account.Credit(10m);
                }
            });
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"Expected balance: {1000m + 100 * 100 * 10m}");
        Console.WriteLine($"Actual balance:   {account.Balance}");

        bool debited = account.Debit(500m);
        Console.WriteLine($"Debit 500: {debited}");

        bool overdraw = account.Debit(account.Balance + 1m);
        Console.WriteLine($"Overdraw attempt: {overdraw}");

        Console.WriteLine($"Final balance: {account.Balance}");
    }
}
