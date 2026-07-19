// MathLib.h
// Заголовочный файл C++ библиотеки с математическими функциями.
// Функции экспортируются через MathLib.def (см. файл), поэтому
// используем чистый C-интерфейс (extern "C") без декорирования имён.

#ifdef __cplusplus
extern "C" {
#endif

    // Сложение двух чисел
    __declspec(dllexport) double Add(double a, double b);

    // Вычитание: a - b
    __declspec(dllexport) double Subtract(double a, double b);

    // Умножение двух чисел
    __declspec(dllexport) double Multiply(double a, double b);

    // Деление: a / b. При делении на ноль возвращает NaN.
    __declspec(dllexport) double Divide(double a, double b);

#ifdef __cplusplus
}
#endif
