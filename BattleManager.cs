using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace Bitstream
{
    // 현재 턴
    enum Turn
    {
        PlayerAttack,PlayerDefense,     // 플레이어 턴
        MonsterAttack,                   // 몬스터 턴
    }
    partial class BattleManager
    {
        // 플레이어
        private Player player;

        // 몬스터 도감
        private Dictionary<Tile, Monster> mobTemplate = new Dictionary<Tile, Monster>();

        // 몬스터 아스키
        Dictionary<Tile, string[]> mobASCII = new Dictionary<Tile, string[]>();

        // 현재 전투 몬스터
        private Monster combatMonster;

        // 전투 몬스터 타입
        public static Tile combatType;

        // 턴 상태
        Turn turnState;

        public BattleManager(Player inputPlayer)
        {
            player = inputPlayer;

            SetMonsterTemplate();
            SetMonsterASCII();
        }


        // 몬스터 템플릿 세팅
        void SetMonsterTemplate()
        {
            // 템플릿 생성
            // 졸개
            mobTemplate.Add(Tile.ByteMob, new Monster("Byte Jr", byte.MaxValue / 5, 1));
            mobTemplate.Add(Tile.ShortMob, new Monster("Short Jr", ushort.MaxValue / 5, 3));
            mobTemplate.Add(Tile.IntMob, new Monster("Int Jr", uint.MaxValue / 5, 6));
            mobTemplate.Add(Tile.LongMob, new Monster("Long Jr", ulong.MaxValue / 5, 14));

            // 보스
            mobTemplate.Add(Tile.ByteBoss, new Monster("Byte", byte.MaxValue, 2));
            mobTemplate.Add(Tile.ShortBoss, new Monster("Short", ushort.MaxValue, 4));
            mobTemplate.Add(Tile.IntBoss, new Monster("Int", uint.MaxValue, 8));
            mobTemplate.Add(Tile.LongBoss, new Monster("Long", ulong.MaxValue, 16));
        }

        // 몬스터 아스키 세팅
        void SetMonsterASCII()
        {
            mobASCII.Add(Tile.ByteMob, new string[] { "　　　　　　　　　　　 __", "　　　　　　　　　　　|__)", "　　　　　　　　　　　|__)" });
            mobASCII.Add(Tile.ShortMob, new string[] { "　　　　　　　　　　　 __", "　　　　　　　　　　　/__`", "　　　　　　　　　　　.__/" });
            mobASCII.Add(Tile.IntMob, new string[] { "　　　　　　　　　　  `", "　　　　　　　　　　　|", "　　　　　　　　　　　|" });
            mobASCII.Add(Tile.LongMob, new string[] { "　　　　　　　　　　  |", "　　　　　　　　　　　|", "　　　　　　　　　　　|___" });

            mobASCII.Add(Tile.ByteBoss, new string[] { "　　　　　　　 __      ___  ___", "　　　　　　　|__) ＼/  |  |___", "　　　　　　　|__)  |   |  |___" });
            mobASCII.Add(Tile.ShortBoss, new string[] { "　　　　　　 __        __　　__  ___", "　　　　　　/__` |__| /  ＼ |__)  | ", "　　　　　　.__/ |  | ＼__/ |  ＼ | " });
            mobASCII.Add(Tile.IntBoss, new string[] { "　　　　　　　　　　　　　___", "　　　　　        | |＼ |  | ", "　　　　　        | | ＼|  | " });
            mobASCII.Add(Tile.LongBoss, new string[] { "　　　　　  |     __　　　　　__ ", "　　　　　　|    /  ＼ |＼ | / _`", "　　　　　　|___ ＼__/ | ＼| ＼__>" });
        }


        // 플레이어 공격 턴
        void PlayerAttackTurn()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                switch (inputKey.Key)
                {
                    case ConsoleKey.Spacebar:
                        PrintMonsterUI(combatType);
                        player.Attack();
                        UIManager.UpdateLog(new Log(LogType.Warning, "플레이어 공격 턴 종료"));
                        turnState = Turn.MonsterAttack;
                        break;
                }
            }
        }
        // 플레이어 방어 턴
        void PlayerDefenseTurn()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                switch (inputKey.Key)
                {
                    case ConsoleKey.Enter:
                        UIManager.UpdateLog(new Log(LogType.Warning, "플레이어 방어 턴 종료"));
                        turnState = Turn.PlayerAttack;
                        break;
                }
            }
        }
        // 몬스터 공격 턴
        void MonsterAttackTurn()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                switch (inputKey.Key)
                {
                    case ConsoleKey.V:
                        UIManager.UpdateLog(new Log(LogType.Warning, "몬스터 공격 턴 종료"));
                        turnState = Turn.PlayerDefense;
                        break;
                }
            }
        }

        // 전투 루프
        public void CombatLoop()
        {
            UIManager.UpdateLog(new Log(LogType.Normal, "전투 UI 로딩"));
            // 전투 몬스터 생성
            combatMonster = new Monster(mobTemplate[combatType]);

            // 전투 UI
            PrintMonsterUI(combatType);

            // 플레이어 공격 턴
            PlayerAttackUI();

            // 턴 루프
            while (true)
            {
                switch (turnState)
                {
                    // 플레이어 공격
                    case Turn.PlayerAttack:
                        PlayerAttackTurn();
                        break;
                    // 플레이어 방어
                    case Turn.PlayerDefense:
                        PlayerDefenseTurn();
                        break;
                    // 몬스터 공격
                    case Turn.MonsterAttack:
                        MonsterAttackTurn();
                        break;
                }
                System.Threading.Thread.Sleep(20);
            }
        }


    }
}
