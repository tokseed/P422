using System.Threading;

namespace БезопасныйБанковскийСчет;

internal class Program
{
    private static void Main(string[] args)
    {
        // Исходный массив данных
        int[] numbers = { 2, -3, 5, int.MaxValue, 8, 0, 10, -1, 3, 4, 7, 12 };

        Console.WriteLine("Исходный массив:");
        Console.WriteLine(string.Join(", ", numbers));

        // Навешиваем параллельную обработку через Thread
        SquarePositiveNumbersParallel(numbers);

        Console.WriteLine("\nМассив после возведения положительных чисел в квадрат:");
        Console.WriteLine(string.Join(", ", numbers));
    }

    private static void SquarePositiveNumbersParallel(int[] array)
    {
        // Определяем оптимальное количество потоков (например, по числу логических ядер)
        int threadCount = Environment.ProcessorCount;
        
        // На случай, если массив слишком маленький, нет смысла плодить лишние потоки
        if (array.Length < threadCount)
        {
            threadCount = array.Length;
        }

        if (threadCount == 0) return;

        Thread[] threads = new Thread[threadCount];
        
        // Вычисляем размер порции данных для каждого потока
        int chunkSize = array.Length / threadCount;

        for (int i = 0; i < threadCount; i++)
        {
            // Вычисляем границы отрезка массива для текущего потока
            int startIndex = i * chunkSize;
            
            // Последний поток забирает весь остаток (если массив не делится поровну)
            int endIndex = (i == threadCount - 1) ? array.Length : startIndex + chunkSize;

            // Передаем индексы в поток. Захватываем переменные локально, чтобы избежать race condition на индексах
            int localStart = startIndex;
            int localEnd = endIndex;

            threads[i] = new Thread(() => ProcessSegment(array, localStart, localEnd));
            threads[i].Start();
        }

        // Обязательно дожидаемся завершения работы всех потоков перед выходом из метода
        for (int i = 0; i < threadCount; i++)
        {
            threads[i].Join();
        }
    }

    // Метод, который выполняет каждый отдельный поток внутри своего сегмента
    private static void ProcessSegment(int[] array, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            // Проверяем условие ТЗ: только положительные числа (больше 0)
            if (array[i] > 0)
            {
                // Защита от переполнения (overflow), если число слишком большое для int
                long squared = (long)array[i] * array[i];
                
                if (squared > int.MaxValue)
                {
                    // Если результат выходит за границы int, можно либо бросить ошибку, либо записать MaxValue
                    array[i] = int.MaxValue; 
                }
                else
                {
                    array[i] = (int)squared;
                }
            }
        }
    }
}