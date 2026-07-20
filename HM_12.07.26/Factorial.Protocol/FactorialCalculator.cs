namespace Factorial.Protocol;

public static class FactorialCalculator
{
    public static string Compute(int n)
    {
        if (n < 0)
        {
            throw new ArgumentException("Факториал определён только для неотрицательных чисел.");
        }

        if (n > 1000)
        {
            throw new ArgumentException("Число слишком большое (максимум 1000).");
        }

        var result = System.Numerics.BigInteger.One;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
