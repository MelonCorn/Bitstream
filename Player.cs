using System;
using System.Collections.Generic;

namespace Bitstream
{
    class Player
    {
        public readonly PlayerData Data = new PlayerData();     // 데이터(스탯, 재화)
        private Vector2 currentPos;             // 현재 위치

        public Vector2 CurrentPos
        {
            get { return currentPos; }
        }


        // 선택된 공격 행동
        int selectAttackNum = 0;
        // 선택된 방어 행동
        int selectDefenseNum = 0;

        // 이동 입력
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

        // 이동
        public void Move(Vector2 nextPos)
        {
            currentPos.x = nextPos.x;
            currentPos.y = nextPos.y;
        }



        // 플레이어 공격 UI 출력
        public void PrintAttackUI()
        {
            bool success = UIManager.TryPrintUI(UIType.PlayerAttack, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 출력
            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("--- [ 공격 행동 ] ---");
            Console.SetCursorPosition(x + 2, y + 3);
            Console.WriteLine("1번 공격");
        }


        // 플레이어 공격
        public void Attack()
        {
            Data.PrintBitSpin();
        }
    }
}
