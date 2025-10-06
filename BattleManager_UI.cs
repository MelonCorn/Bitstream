using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Bitstream
{
    partial class BattleManager
    {
        // 몬스터 UI 출력
        void PrintMonsterUI(Tile monsterType)
        {
            bool success = UIManager.TryPrintUI(UIType.BattleInfo, out int x, out int y, out int width, out int height);

            if (success == false) return;

            UIManager.UpdateLog(new Log(LogType.Warning, $"'{combatMonster.Name}' 와 전투"));

            // 체력 
            PrintMonsterHp(x, y);

            // 파티션
            Console.SetCursorPosition(x + 1, y + 4);
            PrintPartition(width);

            // 비트
            PrintMonsterBit(x + 4, y + 6);

            // 파티션
            Console.SetCursorPosition(x + 1, y + 10);
            PrintPartition(width);

            // 몬스터 아스키
            PrintMonsterASCII(monsterType , x, y);
        }

        // 체력
        void PrintMonsterHp(int x, int y)
        {
            Console.SetCursorPosition(x + 2, y + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("HP : |");
            Console.BackgroundColor = ConsoleColor.Red;

            // 체력 퍼센트
            float hpRatio = (float)combatMonster.CurrentHp / combatMonster.MaxHp;
            // 칸 수 계산
            int fill = (int)Math.Ceiling(hpRatio * 19);

            // 체력 칸 출력
            for (int i = 0; i < fill; i++)
            {
                Console.Write("　");
            }

            // 끝 지점
            int barLength = 46;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(x + barLength, y + 1);
            Console.Write("|");

            // 현재 체력 , 오른쪽 정렬
            string currentHp = combatMonster.CurrentHp.ToString("N0");
            Console.SetCursorPosition(x + barLength - currentHp.Length, y + 2);
            Console.Write(currentHp);

            // 최대 체력 , 오른쪽 정렬
            string maxHp = "/ " + combatMonster.MaxHp.ToString("N0");
            Console.SetCursorPosition(x + barLength - maxHp.Length, y + 3);
            Console.Write(maxHp);
            Console.ResetColor();
        }

        // 비트
        void PrintMonsterBit(int x, int y)
        {
            Console.SetCursorPosition(x, y + 2);

            for (int i = 8; i > 0; i--)
            {
                Console.Write(" ");
                Console.ForegroundColor = i > combatMonster.Bits ? ConsoleColor.DarkGray : ConsoleColor.Green;
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(0);
                }
            }

            Console.SetCursorPosition(x, y);
            for (int i = 8; i > 0; i--)
            {
                Console.Write(" ");
                Console.ForegroundColor = i > combatMonster.Bits - 8 ? ConsoleColor.DarkGray : ConsoleColor.Green;
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(0);
                }
            }
            Console.ResetColor();
        }

        // 아스키
        void PrintMonsterASCII(Tile monsterType ,int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for(int i = 0; i < mobASCII[monsterType].Length; i++)
            {
                Console.SetCursorPosition(x + 2, y + 12 + i);
                Console.Write(mobASCII[monsterType][i]);
            }
            Console.ResetColor();
        }

        // 파티션
        void PrintPartition(int width)
        {
            // 파티션
            for (int i = 0; i < width - 1; i++)
            {
                Console.Write("─");
            }
        }


        //---------------------------------------------


        // 플레이어 공격 UI
        void PlayerAttackUI()
        {
            player.PrintAttackUI();
        }
    }
}
