using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Task2MathApp
{
    class Program
    {
        // Имя подключаемой неуправляемой библиотеки (DLL, собранной в C++).
        // Для x86 укажите "MathLib.dll" той же разрядности, что и процесс.
        private const string DllName = "MathLib.dll";

        // P/Invoke объявления четырёх математических функций.
        // Соглашение вызова по умолчанию для C++ dll = Cdecl,
        // поэтому явно задаём CallingConvention.Cdecl.
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double Add(double a, double b);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double Subtract(double a, double b);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double Multiply(double a, double b);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double Divide(double a, double b);

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            double x = 12.5;
            double y = 4.0;

            Console.WriteLine("=== Демонстрация P/Invoke к C++ библиотеке MathLib.dll ===");
            Console.WriteLine($"Операнды: a = {x}, b = {y}");
            Console.WriteLine();

            Console.WriteLine($"Add(a, b)       = {Add(x, y)}");
            Console.WriteLine($"Subtract(a, b)  = {Subtract(x, y)}");
            Console.WriteLine($"Multiply(a, b)  = {Multiply(x, y)}");
            Console.WriteLine($"Divide(a, b)    = {Divide(x, y)}");
            Console.WriteLine($"Divide(a, 0)    = {Divide(x, 0)}  (деление на ноль -> NaN)");

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
