using System;
using System.Collections.Generic;

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
        public Vector2 MoveInput()
        {
            ConsoleKeyInfo inputKey = Console.ReadKey(true);

            Vector2 nextPos = currentPos;

            switch (inputKey.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    nextPos.y--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    nextPos.y++;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    nextPos.x--;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    nextPos.x++;
                    break;
            }

            return nextPos;
        }

        public void Move(Vector2 nextPos)
        {
            currentPos.x = nextPos.x;
            currentPos.y = nextPos.y;
        }
    }
}
