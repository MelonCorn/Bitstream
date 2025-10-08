using System;

namespace Bitstream
{
    partial class PlayerData
    {
        // 스탯 UI 출력
        public void PrintStatUI()
        {
            bool success = UIManager.TryPrintUI(UIType.PlayerStat, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 제목 출력
            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("------ [ 플레이어 정보 ] ------");

            #region Hp
            Console.SetCursorPosition(x + 2, y + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"  HP : ");

            // 최대 업그레이드 만큼
            for (int i = 1; i <= 30; i++)
            {
                int lineIndex = (i - 1) % 10;
                int lineNumber = (i - 1) / 10;
                int cursorX = x + 9 + lineIndex * 2;
                int cursorY = y + 3 + lineNumber;

                Console.SetCursorPosition(cursorX, cursorY);

                Console.ForegroundColor = i <= maxHp ? ConsoleColor.Red : ConsoleColor.DarkGray;
                Console.Write(i <= currentHp ? "♥" : "♡");
                Console.ResetColor();
            }
            #endregion

            #region Core
            Console.SetCursorPosition(x + 2, y + 7);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"  Core : ");
            // 최대 업그레이드 만큼
            for (int i = 1; i <= 3; i++)
            {
                Console.ForegroundColor = i <= maxCore ? ConsoleColor.Blue : ConsoleColor.DarkGray;
                Console.Write(i <= currentCore ? "★" : "☆");
                Console.ResetColor();
            }
            #endregion

            #region Memory
            Console.SetCursorPosition(x + 2, y + 9);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"  Memory : {memory}");
            Console.ResetColor();
            #endregion
        }

        // 비트 UI 출력
        public void PrintBitUI()
        {
            bool success = UIManager.TryPrintUI(UIType.PlayerBit, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 비트 출력
            Console.SetCursorPosition(x + 3, y + 2);

            for (int i = 16; i > 0; i--)
            {
                Console.ForegroundColor = i > maxBit ? ConsoleColor.DarkGray : ConsoleColor.Yellow;
                Console.Write(" 0000");
            }
            Console.ResetColor();
        }

        // 공격 비트 랜덤 출력
        public ulong AttackBitSpin()
        {
            bool success = UIManager.TryPrintUI(UIType.PlayerBit, out int x, out int y, out int width, out int height);

            if (success == false) return 0;

            Random random = GameManager.Instance.rand;

            Console.SetCursorPosition(x + 3, y + 2);

            // 비활성화 길이
            int xOffset = 0;
            // 비트 없는 만큼 비활성화 처리 
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 16; i > maxBit; i--)
            {
                Console.Write(" 0000");
                xOffset += 5;
            }

            int setCount = 0;   // 정해진 수
            ulong playerDmg = 0; // 누적 대미지
            
            // 정해진 숫자가 비트보다 적으면
            // 루프마다 뒤에서부터 숫자 하나씩 정해짐
            while(setCount < maxBit * 4)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                // 비활성화 비트 뒤
                Console.SetCursorPosition(x + 4 + xOffset, y + 2);

                // 비트업글 * 4 - 정해진 수 만큼
                for (int i = 0; i < maxBit * 4 - setCount; i++)
                {
                    // 0, 1 중 랜덤 출력
                    int bitNum = random.Next(0, 2) == 0 ? 0 : 1;

                    // 확정자리 숫자
                    if (i == maxBit * 4 - setCount - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        // 이진수 대미지 더하기
                        if(bitNum == 1)
                        {
                            playerDmg += (1UL << setCount);
                        }
                    }

                    Console.Write((i + 1) % 4 == 0 ? $"{bitNum} " : $"{bitNum}");
                }

                setCount++;

                // 잠시 대기
                System.Threading.Thread.Sleep(40);
            }

            Console.ResetColor();

            return playerDmg;
        }
    }
}
