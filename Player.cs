using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    struct Vector2
    {
        public int x;
        public int y;
    }

    class Player
    {
        // 현재 위치
        Vector2 currentPos;

        public Vector2 CurrentPos
        {
            get { return currentPos; }
            set { currentPos = value; }
        }


        // 이동
        public Vector2 Move()
        {
            ConsoleKeyInfo inputKey = Console.ReadKey();

            switch (inputKey.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    currentPos.y--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    currentPos.y++;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    currentPos.x--;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    currentPos.x++;
                    break;
            }

            return currentPos;
        }
    }
}
