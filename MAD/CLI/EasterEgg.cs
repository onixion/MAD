using System;
using System.Threading;

namespace MAD.CLICore
{
    public class EGG
    {
        public static void EasterEgg()
        {
            Random _rand = new Random();

            int _x;
            int _y;

            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(10);

                _x = _rand.Next(Console.BufferWidth);
                _y = _rand.Next(20);

                Console.SetCursorPosition(_x, _y);
                Console.ForegroundColor = GetRandomColor();
                Console.BackgroundColor = GetRandomColor();
                Console.Write("       ");

                if (i > 500)
                {
                    string _header = "BUFFER-OVERFLOW-EXCEPTION!";
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, 9);
                    Console.Write(" ".PadLeft(Console.BufferWidth));
                    Console.SetCursorPosition(0, 10);
                    Console.Write(" ".PadLeft(Console.BufferWidth));
                    Console.SetCursorPosition(0, 11);
                    Console.Write(" ".PadLeft(Console.BufferWidth));
                    Console.SetCursorPosition(Console.BufferWidth / 2 - _header.Length / 2, 10);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(_header);
                }
            }

            Thread.Sleep(250);
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ".PadLeft(Console.BufferWidth));
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(".NET injection ");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Console.Write(" Success.");
            Thread.Sleep(1500);
            Console.SetCursorPosition(0, 0);
            Console.Write(" ".PadLeft(Console.BufferWidth));
            Console.Write(@"C:\Windows\system32>");
            Thread.Sleep(1200);
            Console.Write("exploit.exe ");
            Thread.Sleep(500);
            Console.Write("-n ");
            Thread.Sleep(500);
            Console.Write("-a ");
            Thread.Sleep(500);
            Console.Write("-build nt2013204 ");
            Thread.Sleep(500);
            Console.Write("-release 032ad2234d ");
            Thread.Sleep(500);
            Console.Write("-brute-force ");
            Thread.Sleep(1500);
            Console.Clear();
            Thread.Sleep(2000);
            Console.SetCursorPosition(0, 0);
            Console.Write("Hi, I am Jack .. ");
            Thread.Sleep(2000);
            Console.Write("i mean this is windows-mediacenter updater.\n");
            Thread.Sleep(3000);
            Console.WriteLine("Sorry to interupt your work.");
            Thread.Sleep(3000);
            Console.WriteLine("All updates has been installed ... i guess.");
            Thread.Sleep(3000);
            Console.WriteLine("So please continue working ...");
            Thread.Sleep(3000);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        public static ConsoleColor GetRandomColor()
        {
            Random _rand = new Random();

            switch (_rand.Next(1, 6))
            {
                case 1:
                    return ConsoleColor.Blue;
                case 2:
                    return ConsoleColor.Red;
                case 3:
                    return ConsoleColor.Green;
                case 4:
                    return ConsoleColor.Yellow;
                case 5:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.Gray;
            }
        }
    }
}
