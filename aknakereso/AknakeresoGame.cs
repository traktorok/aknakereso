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

        public AknakeresoGame(int boardWidth, int boardHeight)
        {
            curPos.x = 0;
            curPos.y = 0;
            boardSize.x = boardWidth;
            boardSize.y = boardHeight;
            board = new Square[boardHeight, boardWidth];
            board[0, 1].value = 10;
            Console.CursorVisible = false;
        }

        private bool CheckCoordInBounds(Coordinate coord) {
            if (0 > coord.x || boardSize.x < coord.x)
            {
                return false;
            }

            if (0 > coord.y || boardSize.y < coord.y)
            {
                return false;
            }

            return true;
        }

        public void PlaceAkna(Coordinate coord)
        {
            board[coord.y, coord.x].value = AKNA;
            for (int i = coord.y - 1; i < coord.y + 1; i++)
            {
                for (int j = coord.x - 1; j < coord.x + 1; j++)
                {
                    if (!CheckCoordInBounds(new Coordinate(j, i)))
                    {
                        continue;
                    }

                    if (board[i, j].value != AKNA && !(i == coord.y && j == coord.x))
                    {
                        board[i, j].value++;
                    }
                }
            }
        }

        public int MoveCursor(int x, int y)
        {
            if (0 > (curPos.x + x) || boardSize.x < (curPos.x + x))
            {
                return -1;
            }

            if (0 > (curPos.y + y) || boardSize.y < (curPos.y + y))
            {
                return -1;
            }

            Console.SetCursorPosition(curPos.x, curPos.y);
            Console.Write(' ');

            curPos.x += x;
            curPos.y += y;

            Console.SetCursorPosition(curPos.x, curPos.y);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(' ');
            Console.BackgroundColor = ConsoleColor.Black;

            return 0;
        }

        public void addAkna() {
            PlaceAkna(curPos);
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

            return 0;
        }
    }
}
