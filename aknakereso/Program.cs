using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aknakereso
{
    class Program
    {
        static void Main(string[] args)
        {
            AknakeresoGame game = new AknakeresoGame(10, 10, 1);
            bool shouldQuit = false;

            while (shouldQuit == false)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        game.MoveCursor(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        game.MoveCursor(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        game.MoveCursor(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        game.MoveCursor(1, 0);
                        break;
                    case ConsoleKey.Escape:
                        shouldQuit = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        if (game.RevealCurPos() == -1)
                        {
                            shouldQuit = true;
                        }
                        break;
                };
            }
        }
    }
}
