// MathLib.cpp
// Реализация математических функций для DLL (Windows).
// Собирается только под Windows (MSVC).

#include "MathLib.h"
#include <cmath>   // для std::nan

extern "C" {

    __declspec(dllexport) double Add(double a, double b) {
        return a + b;
    }

    __declspec(dllexport) double Subtract(double a, double b) {
        return a - b;
    }

    __declspec(dllexport) double Multiply(double a, double b) {
        return a * b;
    }

    __declspec(dllexport) double Divide(double a, double b) {
        if (b == 0.0) {
            return std::nan("");   // деление на ноль -> NaN
        }
        return a / b;
    }

}
