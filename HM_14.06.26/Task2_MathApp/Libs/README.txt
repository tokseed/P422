# Положите сюда собранную библиотеку MathLib.dll
#
# 1. Откройте Task2_MathLib/MathLib.vcxproj в Visual Studio (Release / x64)
#    или соберите через MSBuild:
#      msbuild MathLib.vcxproj /p:Configuration=Release /p:Platform=x64
# 2. Скопируйте полученный x64/Release/MathLib.dll в эту папку.
# 3. Запустите C# приложение: dotnet run -c Release
#
# DLL и процесс C# должны иметь одинаковую разрядность (оба x64).
