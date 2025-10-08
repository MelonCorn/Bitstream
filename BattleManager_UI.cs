using System;
using System.Xml.Serialization;

namespace Bitstream
{
    partial class BattleManager
    {
        // 전투 UI 출력
        void PrintCombatUI(Tile monsterType)
        {
            bool success = UIManager.TryPrintUI(UIType.BattleInfo, out int x, out int y, out int width, out int height);

            if (success == false) return;

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

            // 데미지
            PrintMonsterDamage(0);

            // 몬스터 아스키
            PrintMonsterASCII(monsterType , x, y);

            // 플레이어 
            PrintPlayer(x + 1 + width / 2, y + height - 4);
        }

        // 몬스터 체력
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

        // 몬스터 비트
        void PrintMonsterBit(int x, int y)
        {
            Console.SetCursorPosition(x, y + 2);

            // 아래
            for (int i = 8; i > 0; i--)
            {
                Console.ForegroundColor = i > combatMonster.Bits ? ConsoleColor.DarkGray : ConsoleColor.Green;
                Console.Write(" 0000");
            }

            // 위
            Console.SetCursorPosition(x, y);
            for (int i = 8; i > 0; i--)
            {
                Console.ForegroundColor = i > combatMonster.Bits - 8 ? ConsoleColor.DarkGray : ConsoleColor.Green;
                Console.Write(" 0000");
            }

            Console.ResetColor();
        }

        // 몬스터 아스키
        void PrintMonsterASCII(Tile monsterType ,int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for(int i = 0; i < mobASCII[monsterType].Length; i++)
            {
                Console.SetCursorPosition(x + 2, y + 14 + i);
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

        // 플레이어
        void PrintPlayer(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("●");
        }






        // 몬스터 공격 출력
        void PrintMonsterAttack()
        {
            int x = 2;
            int y = 1;

            int xOffset = 8 * 5;    // 끝 길이
            int bonusDmg = 0;       // 보너스 누적 대미지

            // 비트 없는 만큼 비활성화 처리 
            Console.ForegroundColor = ConsoleColor.DarkGray;

            //---------------------아래------------------------

            int bottomCount = combatMonster.Bits > 8 ? 8 : combatMonster.Bits;

            int spinX = x + xOffset - 1;

            for (int i = 0; i < bottomCount; i++)
            {
                MonsterBitSpin(ref bonusDmg, ref spinX, y + 8);

                System.Threading.Thread.Sleep(200);
            }


            //---------------------위------------------------

            spinX = x + xOffset - 1;

            for (int i = 0; i < combatMonster.Bits - 8; i++)
            {
                MonsterBitSpin(ref bonusDmg, ref spinX, y + 6);

                System.Threading.Thread.Sleep(200);
            }

            // 최종 데미지 : 기본 데미지 : 추가 데미지
            totalMonsterDmg = combatMonster.Dmg + bonusDmg;

            // 잠시 대기
            System.Threading.Thread.Sleep(1000);

        }

        // 몬스터 비트 스핀
        int MonsterBitSpin(ref int monsterDmg, ref int x, int y)
        {
            Random random = GameManager.Instance.rand;

            int blockAttackCount = 0;
            int[] randNums = new int[4];

            for (int j = 0; j < 4; j++)
            {
                randNums[j] = random.Next(0, 2);
                if (randNums[j] == 1)
                    blockAttackCount++;
            }

            if (blockAttackCount == 4)
            {
                monsterDmg++;
                PrintMonsterDamage(monsterDmg);
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            for (int j = 0; j < 4; j++)
            {
                Console.Write(randNums[j]);
            }

            x -= 5;

            Console.ResetColor();

            return monsterDmg;
        }

        // 몬스터 데미지 출력
        void PrintMonsterDamage(int plusDmg)
        {
            Console.SetCursorPosition(4, 12);
            Console.Write($"총 데미지 : {combatMonster.Dmg} ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"+ {plusDmg} ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"- {player.Data.Def}");
        }

        // 방어 주사위
        void DefenseBattle()
        {
            bool success = UIManager.TryPrintUI(UIType.PlayerDefense, out int x, out int y, out int width, out int height);

            if (success == false) return;

            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("-- [ 방어 주사위 ] --");

            int monsterNumber = 0;
            int playerNumber = 0;

            do
            {
                // 몬스터
                for (int i = 0; i < 10; i++)
                {
                    Console.SetCursorPosition(x + 8, y + 3);
                    monsterNumber = DefenceRandomNumber();
                    Console.Write($"상대 : {monsterNumber}");
                }
                System.Threading.Thread.Sleep(300);

                // 플레이어
                for (int i = 0; i < 10; i++)
                {
                    Console.SetCursorPosition(x + 8, y + 5);
                    playerNumber = DefenceRandomNumber();
                    Console.Write($"당신 : {playerNumber}");
                }
                System.Threading.Thread.Sleep(300);
            }
            // 둘 중 숫자가 큰 쪽이 나올 때 까지
            while (monsterNumber == playerNumber);

            System.Threading.Thread.Sleep(200);
            Console.SetCursorPosition(x + 11, y + 7);

            // 플레이어 크다면
            if (playerNumber > monsterNumber)
            {
                totalMonsterDmg = 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"승리");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"패배");
            }
            Console.ResetColor();

            System.Threading.Thread.Sleep(700);

            // 최종 데미지 = 몬스터 데미지 - 데미지 감소
            int totalDmg = totalMonsterDmg - player.Data.Def;

            if (totalDmg < 0)
            {
                totalDmg = 0;
            }

            // 플레이어 체력 감소
            player.TakeDamage(totalMonsterDmg - player.Data.Def);

            // 사망,패배 상태 확인
            isLose = player.Data.CheckDead();
        }

        // 방어 주사위 스핀
        int DefenceRandomNumber()
        {
            Random rand = GameManager.Instance.rand;
            int randNum = rand.Next(0, 100);
            System.Threading.Thread.Sleep(100);
            return randNum;
        }
    }
}
