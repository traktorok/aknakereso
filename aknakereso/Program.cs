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
        /// Megvaltoztatja a konzol hatterszinet pirosra, es
        /// kiirja, hogy "Vesztettel", majd egy gombnyomasra
        /// var a jatek bezarasahoz.
        /// </summary>
        static void Vesztett()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Vesztettel");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
        }

        /// <summary>
        /// Megvaltoztatja a konzol hatterszinet zoldre, a betuszint
        /// feketere, es kiirja, hogy "Nyertel", majd egy gombnyomasra
        /// var a jatek bezarasahoz.
        /// </summary>
        static void Nyert()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Nyertel");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
        }

        /// <summary>
        /// Keszit egy AknakeresoGame peldanyt, majd a konzolrol bemeneteket kezel,
        /// es ez alapjan iranyitja az AknakeresoGame-et. A jatek kimenetelet figyelembe
        /// veve elinditja a Nyertel, vagy a Vesztettel eljarasokat.
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

        static void Main(string[] args)
        {
            StartAknakereso(25, 25, 8);
        }
    }
}
