using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aknakereso
{
    class Program
    {
        /// <summary>
        /// Keszit egy AknakeresoGame peldanyt, majd a konzolrol bemeneteket kezel,
        /// es ez alapjan iranyitja az AknakeresoGame-et. A jatek kimenetelet figyelembe
        /// veve elinditja a Nyert, vagy a Vesztett eljarasokat.
        /// </summary>
        /// <param name="tablaSzelesseg">A jatektabla szelessege (karakterekben)</param>
        /// <param name="tablaMagassag">A jatektabla magassaga (karakterekben)</param>
        /// <param name="bombakSzama">A jatektablan elhelyezkedo bombak maximalis szama.</param>
        static void StartAknakereso(int tablaSzelesseg, int tablaMagassag, int bombakSzama)
        {
            AknakeresoGame game = new AknakeresoGame(tablaSzelesseg, tablaMagassag, bombakSzama);
            bool shouldQuit = false;
            int exitCode = 0;

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
                        exitCode = game.RevealCurPos();
                        if (exitCode != 0)
                        {
                            shouldQuit = true;
                        }
                        break;
                };
            }

            switch (exitCode)
            {
                case -1:
                    Vesztett();
                    break;
                case 1:
                    Nyert();
                    break;
            }
        }

        /// <summary>
        /// Kirajzolja a konzolra a menut, bekeri a jatekmezo magassagat, szelesseget,
        /// es az aknak szamat amivel szeretnenk jatszani
        /// </summary>
        static void AknaMenu()
        {
            Console.CursorVisible = true;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            int szelesseg, magassag, aknak;

            Console.WriteLine();
            string a = "Aknakereső\n";
            Console.SetCursorPosition((Console.WindowWidth - a.Length) / 2, Console.CursorTop);
            Console.WriteLine(a);

            Console.WriteLine("  A szélesség maximum 100 lehet, a magasság maximum 25 lehet.");
            Console.WriteLine("  A maximum aknák száma a szélesség * magsság.\n");

            Console.Write("  Szélesség: ");
            while (!int.TryParse(Console.ReadLine(), out szelesseg) || szelesseg > 100 || szelesseg <= 0) {
                Console.Write("  Szélesség: ");
            }

            Console.Write("  Magasság: ");
            while (!int.TryParse(Console.ReadLine(), out magassag) || magassag > 25 || magassag <= 0)
            {
                Console.Write("  Magasság: ");
            }

            Console.Write("  Aknák: ");
            while (!int.TryParse(Console.ReadLine(), out aknak) || aknak < 1 || aknak > magassag * szelesseg)
            {
                Console.Write("  Aknák: ");
            }

            Console.WriteLine("  A nyilakkal lehet irányítani, a Space-el vagy Enterrel lehet felfedni egy mezőt.");
            Console.WriteLine("\n  Nyomj meg egy billentyűt a kezdéshez");

            Console.ReadKey();

            StartAknakereso(szelesseg, magassag, aknak);
        }

        /// <summary>
        /// Megvaltoztatja a konzol hatterszinet pirosra, es
        /// kiirja, hogy "Vesztettel", majd egy gombnyomasra
        /// var a jatek bezarasahoz.
        /// </summary>
        static void Vesztett()
        {
            Console.SetWindowSize(100, 25);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            string a = "Vesztettél :( Nyomd meg az R-t az újraindításhoz, vagy mást a kilépéshez\n";
            Console.SetCursorPosition((Console.WindowWidth - a.Length) / 2, Console.WindowHeight / 2);
            Console.WriteLine(a);
            Console.BackgroundColor = ConsoleColor.Black;
            if (Console.ReadKey().Key == ConsoleKey.R)
            {
                AknaMenu();
            }
        }

        /// <summary>
        /// Megvaltoztatja a konzol hatterszinet zoldre, a betuszint
        /// feketere, es kiirja, hogy "Nyertel", majd egy gombnyomasra
        /// var a jatek bezarasahoz.
        /// </summary>
        static void Nyert()
        {
            Console.SetWindowSize(100, 25);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            string a = "Nyertél! :) Nyomd meg az R-t az újraindításhoz, vagy mást a kilépéshez\n";
            Console.SetCursorPosition((Console.WindowWidth - a.Length) / 2, Console.WindowHeight / 2);
            Console.WriteLine(a);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            if (Console.ReadKey().Key == ConsoleKey.R)
            {
                AknaMenu();
            }
        }

        static void Main(string[] args)
        {
            AknaMenu();
        }
    }
}
