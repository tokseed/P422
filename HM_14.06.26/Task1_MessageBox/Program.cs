using System;
using System.Runtime.InteropServices;

namespace Task1MessageBox
{
    class Program
    {
        // Импорт неуправляемой функции MessageBoxW из user32.dll
        // Возвращает int (результат нажатия кнопки), принимает:
        //  hWnd      - дескриптор родительского окна (0 = null)
        //  text      - текст сообщения
        //  caption   - заголовок окна
        //  type      - стиль окна (кнопки/иконки)
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBoxW(
            IntPtr hWnd,
            string text,
            string caption,
            uint type);

        static void Main(string[] args)
        {
            // Заголовок окна = "ФИО", тело окна = ваше ФИО
            string caption = "ФИО";
            string text = "Пасаворонков Павел Андреевич";

            // 0 = MB_OK (обычная кнопка OK)
            int result = MessageBoxW(IntPtr.Zero, text, caption, 0);

            Console.WriteLine($"MessageBox вернул: {result}");
        }
    }
}
