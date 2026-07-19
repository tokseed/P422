using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelAndPLINQ;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("=== Tasks on Parallel ===");
        ParallelFactorial();
        ParallelFibonacci();

        Console.WriteLine();
        Console.WriteLine("=== Tasks on PLINQ ===");
        CountEvenNumbers();
        FindPrimeNumbers();
        SumOfSquares();
    }

    private static void ParallelFactorial()
    {
        Console.WriteLine("Parallel Factorial:");
        int[] numbers = { 5, 10, 15, 20 };

        long[] results = new long[numbers.Length];

        Parallel.For(0, numbers.Length, i =>
        {
            results[i] = checked(Factorial(numbers[i]));
        });

        for (int i = 0; i < numbers.Length; i++)
        {
            Console.WriteLine($"{numbers[i]}! = {results[i]}");
        }
    }

    private static long Factorial(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n > 20) throw new OverflowException($"Factorial of {n} exceeds long.");
        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            checked { result *= i; }
        }
        return result;
    }

    private static void ParallelFibonacci()
    {
        Console.WriteLine("Parallel Fibonacci:");
        int[] indices = { 10, 20, 30, 40, 45, 50 };

        long[] results = new long[indices.Length];

        Parallel.For(0, indices.Length, i =>
        {
            results[i] = Fibonacci(indices[i]);
        });

        for (int i = 0; i < indices.Length; i++)
        {
            Console.WriteLine($"F({indices[i]}) = {results[i]}");
        }
    }

    private static long Fibonacci(int n)
    {
        if (n <= 1) return n;
        long a = 0, b = 1;
        for (int i = 2; i <= n; i++)
        {
            long temp = a + b;
            a = b;
            b = temp;
        }
        return b;
    }

    private static void CountEvenNumbers()
    {
        Console.WriteLine("Count Even Numbers (PLINQ):");
        int[] numbers = Enumerable.Range(1, 1_000_000).ToArray();

        int count = numbers.AsParallel().Count(n => n % 2 == 0);

        Console.WriteLine($"Even numbers count: {count}");
    }

    private static void FindPrimeNumbers()
    {
        Console.WriteLine("Find Prime Numbers (PLINQ):");
        int[] numbers = Enumerable.Range(2, 1_000_000).ToArray();

        var primes = numbers
            .AsParallel()
            .Where(IsPrime)
            .Take(20)
            .ToList();

        Console.WriteLine($"First 20 primes: {string.Join(", ", primes)}");
    }

    private static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0 || n % 3 == 0) return false;

        for (int i = 5; i * i <= n; i += 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
                return false;
        }

        return true;
    }

    private static void SumOfSquares()
    {
        Console.WriteLine("Sum of Squares (PLINQ):");
        int[] numbers = Enumerable.Range(1, 100_000).ToArray();

        long sum = numbers
            .AsParallel()
            .Sum(n =>
            {
                long x = n;
                checked { return x * x; }
            });

        Console.WriteLine($"Sum of squares: {sum}");
    }
}
