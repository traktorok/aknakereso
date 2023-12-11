using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace aknakereso
{
    /// <summary>
    /// Egy ketdimenzios koordinata
    /// x es y ertekkel.
    /// </summary>
    struct Coordinate
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// Egy adatstruktura, amely tartalmazza
    /// a mezo erteket, es hogy fel van-e fedve,
    /// vagy nem.
    /// </summary>
    struct Mezo {
        public int value;
        public bool revealed;
    }

    class AknakeresoGame
    {
        // A kurzor jelenlegi koordinatai.
        private Coordinate curPos;

        // A jatektabla merete
        private Coordinate tablaSize;

        // A jatektabla
        private Mezo[,] tabla;

        // Ez az elso felfedes?
        private bool elsoReveal = true;

        // Az AKNA ertek a 10
        private const int AKNA = 10;

        /// <summary>
        /// Megnezi, hogy az x es y koordinata a 
        /// jatektablan belul tartozkodik-e.
        /// </summary>
        /// <param name="x">X koordinata, amit ellenoriznunk kell</param>
        /// <param name="y">Y koordinata, amit ellenoriznunk kell</param>
        /// <returns>Benne vannak-e a koordinatak a jatekterben, vagy nem</returns>
        private bool CheckCoordInBounds(int x, int y)
        {
            if (0 > x || tablaSize.x <= x)
            {
                return false;
            }

            if (0 > y || tablaSize.y <= y)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Kiirja az X, Y koordinatan elhelyezkedo mezo erteket, az ertektol,
        /// es attol fuggoen, hogy fel van-e fedve.
        /// </summary>
        /// <param name="x">Az X koordinata</param>
        /// <param name="y">Az Y koordinata</param>
        private void WriteMezo(int x, int y)
        {
            if (!CheckCoordInBounds(x, y))
                return;

            if (tabla[y, x].revealed == false)
            {
                Console.Write('#');
            }
            else
            {
                if (tabla[y, x].value == AKNA)
                {
                    Console.Write('*');
                }
                else if (tabla[y, x].value == 0)
                {
                    Console.Write(' ');
                }
                else if (tabla[y, x].value < AKNA)
                {
                    Console.Write(tabla[y, x].value.ToString());
                }
            }
        }

        /// <summary>
        /// Letorli a konzolt, majd ujrarajzolja a teljes tablat.
        /// </summary>
        private void RenderTabla()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < tablaSize.y; y++)
            {
                for (int x = 0; x < tablaSize.x; x++)
                {
                    WriteMezo(x, y);
                }
                Console.Write('\n');
            }

            Console.SetCursorPosition(curPos.x, curPos.y);
        }

        /// <summary>
        /// Az AknakeresoGame osztaly konstruktore, inicializalja
        /// a ketdimenzios tabla tombot es a curPos adatstrukturat,
        /// beallitja a konzol tulajdonsagokat.
        /// </summary>
        /// <param name="tablaWidth">A tabla szelessege</param>
        /// <param name="tablaHeight">A tabla magassaga</param>
        /// <param name="difficulty">A nehezseg (az aknak szamat befolyasolja)</param>
        public AknakeresoGame(int tablaWidth, int tablaHeight, int difficulty)
        {
            curPos = new Coordinate();
            tablaSize = new Coordinate();
            tabla = new Mezo[tablaHeight, tablaWidth];

            curPos.x = 0;
            curPos.y = 0;
            tablaSize.x = tablaWidth;
            tablaSize.y = tablaHeight;
            
            Console.CursorVisible = false;

            RenderTabla();
        }
        
        /// <summary>
        /// Kirajzolja (vagy felulirja onmagaval) az Y sorszamu sort.
        /// </summary>
        /// <param name="y">A sor szama, amit ki kell rajzolni</param>
        private void RenderSor(int y)
        {
            for (int i = 0; i < tablaSize.x; i++)
            {
                Console.SetCursorPosition(i, y);
                WriteMezo(i, y);
            }
            
            Console.SetCursorPosition(curPos.x, curPos.y);

            Console.BackgroundColor = ConsoleColor.Blue;
            WriteMezo(curPos.x, curPos.y);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Ha elmozdulas tortenik, elmozditja a kurzort es ujrarajzolja
        /// azt a sort, amelybol elmozdult, es azt is amelybe elmozdult.
        /// Ha nem tortenik elmozdulas, csak ujrarajzolja a jelenlegi sort.
        /// Az negativ elmozdulas az X tengelyen balra, az Y-on felfele mozditja
        /// el a kurzort.
        /// A pozitiv elmozdulas pedig az X tengelyen jobbra, az Y-on lefele mozgat.
        /// </summary>
        /// <param name="xChange">Az elvaltozas merteke az X tengelyen</param>
        /// <param name="yChange">Az elvaltozas merteke az Y tengelyen</param>
        public void MoveCursor(int xChange, int yChange)
        {
            RenderSor(curPos.y);

            // Csak akkor valtoztatjuk a kurzort, ha az
            // a tabla teruleten belul van
            if (CheckCoordInBounds(xChange + curPos.x, yChange + curPos.y))
            {
                Console.SetCursorPosition(curPos.x, curPos.y);
                WriteMezo(curPos.x, curPos.y);

                curPos.x += xChange;
                curPos.y += yChange;

                Console.SetCursorPosition(curPos.x, curPos.y);

                Console.BackgroundColor = ConsoleColor.Blue;
                WriteMezo(curPos.x, curPos.y);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        /// <summary>
        /// Vegigmegy az egesz tablan, es ellenorzi, hogy a lefedett mezok kozott
        /// mar csak aknak vannak-e?
        /// </summary>
        /// <returns>
        /// Igazat ad vissza, ha a jatekos minden aknat ki tudott kerulni,
        /// es felfedett minden olyan mezot, amelyen nincs akna.
        /// Hamisat ad vissza, ha van meg olyan lefedett mezo, amin nem akna
        /// van.
        /// </returns>
        private bool CheckWin()
        {
            bool didWin = true;
            for (int x = 0; x < tablaSize.x; x++)
            {
                for (int y = 0; y < tablaSize.y; y++)
                {
                    if (tabla[y, x].value != AKNA && tabla[y, x].revealed == false)
                    {
                        didWin = false;
                    }
                }
            }

            return didWin;
        }

        /// <summary>
        /// Egy rekurziv eljaras, ami az egymas mellett levo
        /// ures mezoket (0 erteku) felfedi, amennyiben az egyiket kozuluk felfedjuk.
        /// </summary>
        /// <param name="x">A felfedendo ures mezo koordinataja az X tengelyen. (hanyadik oszlop?)</param>
        /// <param name="y">A felfedendo ures mezo koordinataja az Y tengelyen. (hanyadik sor?)</param>
        private void RevealEmpty(int x, int y)
        {
            if (CheckCoordInBounds(x, y))
            {
                if (tabla[y, x].value != AKNA && tabla[y, x].revealed == false)
                {
                    tabla[y, x].revealed = true;

                    if (tabla[y, x].value == 0)
                    {
                        for (int i = y - 1; i <= y + 1; i++)
                        {
                            for (int j = x - 1; j <= x + 1; j++)
                            {
                                if (!CheckCoordInBounds(j, i))
                                {
                                    continue;
                                }

                                // Sajat koordinatain ne hivja ujra,
                                // hiszen az egy orok loop.
                                if (!(i == y && j == x))
                                {
                                    RevealEmpty(j, i);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Elhelyez egy aknat az X, Y koordinatan levo mezon,
        /// es a korulotte levo mezok ertekeit noveli 1-gyel.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void PlaceAkna(int x, int y)
        {
            tabla[y, x].value = AKNA;
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (!CheckCoordInBounds(j, i))
                    {
                        continue;
                    }

                    if (tabla[i, j].value != AKNA && !(i == y && j == x))
                    {
                        tabla[i, j].value++;
                    }
                }
            }
        }

        /// <summary>
        /// Elhelyez megadott szamu aknat random koordinataju mezokre, amig a
        /// random mezo nem a start mezo (azaz a jatekos altal
        /// valasztott elso mezo).
        /// </summary>
        /// <param name="aknakSzama">A lehelyezendo aknak szama</param>
        /// <param name="startPosX">A start mezo X koordinataja (oszlop)</param>
        /// <param name="startPosY">A start mezo Y koordinataja (sor)</param>
        private void GenerateAknak(int aknakSzama, int startPosX, int startPosY)
        {
            Random rnd = new Random();

            int x, y;
            int i = 0;
            while (i < aknakSzama)
            {
                x = rnd.Next(0, tablaSize.x);
                y = rnd.Next(0, tablaSize.y);

                if (startPosX == x && startPosY == y)
                {
                    i++;
                    continue;
                }

                // Ketszer nem rakunk ugyanoda aknat
                if (tabla[y, x].value != AKNA)
                {
                    PlaceAkna(x, y);
                    i++;
                }
            }
        }

        /// <summary>
        /// Felfedi azt a mezot, amelyen jelenleg a kurzor all.
        /// Amennyiben ez az elso felfedes, legeneralja az aknak poziciojat.
        /// Amennyiben a felfedendo mezo egy akna, a jatekos veszit.
        /// Amennyiben az az utolso nem akna felfedheto mezo, a jatekos nyer.
        /// Egyebkent pedig ujrarajzolja a jelenlegi sort.
        /// </summary>
        /// <returns>
        /// -1 ha a jatekos vesztett, 1 ha nyert, 0 ha folytatodik a jatek.
        /// </returns>
        public int RevealCurPos()
        {
            // Az aknageneralas az elso mezo kivalasztasakor tortenik,
            // hogy biztosak legyunk abban, hogy azon nincs akna.
            if (elsoReveal == true)
            {
                GenerateAknak(8, curPos.x, curPos.y);
                elsoReveal = false;
            }

            Mezo currMezo;
            currMezo = tabla[curPos.y, curPos.x];

            if (currMezo.value == 0 && currMezo.revealed == false)
            {
                RevealEmpty(curPos.x, curPos.y);
                RenderTabla();
            }

            if (currMezo.value == AKNA && currMezo.revealed == false)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.WriteLine("Rossz");
                Console.ReadKey();
                return -1;
            }

            tabla[curPos.y, curPos.x].revealed = true;
            // Azert az egesz sort rendereljuk ujra, mivel
            // a ReadKey felulirhat karaktereket a sorban.
            RenderSor(curPos.y);

            if (CheckWin())
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Clear();
                Console.WriteLine("Nyertel");
                Console.ReadKey();
                return -1;
            }

            return 0;
        }
    }
}
