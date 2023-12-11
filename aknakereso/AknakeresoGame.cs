using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace aknakereso
{
    struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int param_x, int param_y)
        {
            x = param_x;
            y = param_y;
        }
    }

    struct Square {
        public int value;
        public bool revealed;
    }

    class AknakeresoGame
    {
        private Coordinate curPos;
        private Coordinate boardSize;
        private Square[,] board;

        private const int AKNA = 10;

        /// <summary>
        /// Megnezi, hogy az x es y koordinata a 
        /// jatektablan belul tartozkodik-e.
        /// </summary>
        /// <param name="x">X koordinata, amit meg kell neznunk</param>
        /// <param name="y">Y koordinata, amit meg kell neznunk</param>
        /// <returns></returns>
        private bool CheckCoordInBounds(int x, int y)
        {
            if (0 > x || boardSize.x <= x)
            {
                return false;
            }

            if (0 > y || boardSize.y <= y)
            {
                return false;
            }

            return true;
        }

        public void PlaceAkna(int x, int y)
        {
            board[y, x].value = AKNA;
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (!CheckCoordInBounds(j, i))
                    {
                        continue;
                    }

                    if (board[i, j].value != AKNA && !(i == y && j == x))
                    {
                        board[i, j].value++;
                    }
                }
            }
        }

        private void GenerateAknak(int aknakSzama)
        {
            Random rnd = new Random();

            int x, y;
            int i = 0;
            while (i < aknakSzama)
            {
                x = rnd.Next(0, boardSize.x);
                y = rnd.Next(0, boardSize.y);

                if (board[y, x].value != 10)
                {
                    PlaceAkna(x, y);
                    i++;
                }
            }
        }

        public void WriteSquare(int x, int y)
        {
            if (!CheckCoordInBounds(x, y))
                return;

            if (board[y, x].revealed == false)
            {
                Console.Write('#');
            }
            else
            {
                if (board[y, x].value == 10)
                {
                    Console.Write('*');
                }
                else if (board[y, x].value == 0)
                {
                    Console.Write(' ');
                }
                else if (board[y, x].value < 10)
                {
                    Console.Write(board[y, x].value.ToString());
                }
            }
        }

        public void RenderBoard()
        {
            Console.SetCursorPosition(0, 0);

            curPos.x = 0;
            curPos.y = 0;

            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    WriteSquare(x, y);
                }
                Console.Write('\n');
            }
        }

        public AknakeresoGame(int boardWidth, int boardHeight, int difficulty)
        {
            curPos.x = 0;
            curPos.y = 0;
            boardSize.x = boardWidth;
            boardSize.y = boardHeight;
            board = new Square[boardHeight, boardWidth];
            Console.CursorVisible = false;

            GenerateAknak(2);

            RenderBoard();
        }
        
        private void RenderCurrSor()
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                Console.SetCursorPosition(i, curPos.y);
                WriteSquare(i, curPos.y);
            }
            
            Console.SetCursorPosition(curPos.x, curPos.y);

            Console.BackgroundColor = ConsoleColor.Blue;
            WriteSquare(curPos.x, curPos.y);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public int MoveCursor(int x, int y)
        {
            RenderCurrSor();

            // Csak akkor valtoztatjuk a kurzort, ha az
            // a tabla teruleten belul van
            if (CheckCoordInBounds(x + curPos.x, y + curPos.y))
            {
                Console.SetCursorPosition(curPos.x, curPos.y);
                WriteSquare(curPos.x, curPos.y);

                curPos.x += x;
                curPos.y += y;

                Console.SetCursorPosition(curPos.x, curPos.y);

                Console.BackgroundColor = ConsoleColor.Blue;
                WriteSquare(curPos.x, curPos.y);
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return 0;
        }

        public bool CheckWin()
        {
            bool didWin = true;
            for (int x = 0; x < boardSize.x; x++)
            {
                for (int y = 0; y < boardSize.x; y++)
                {
                    if (board[y, x].value != AKNA && board[y, x].revealed == false)
                    {
                        didWin = false;
                    }
                }
            }

            return didWin;
        }

        public int RevealCurPos()
        {

            Square currSquare;
            currSquare = board[curPos.y, curPos.x];

            if (currSquare.value == 10 && currSquare.revealed == false)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.WriteLine("Rossz");
                Console.ReadKey();
                return -1;
            }

            board[curPos.y, curPos.x].revealed = true;
            RenderCurrSor();

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
