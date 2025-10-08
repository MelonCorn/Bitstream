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
        private Dictionary<Tile, string[]> mobASCII = new Dictionary<Tile, string[]>();

        // 전투 몬스터 타입
        public static Tile combatType;

        // 현재 전투 몬스터
        private Monster combatMonster;

        // 몬스터 총 공격력
        private int totalMonsterDmg = 0;

        // 턴 상태
        private Turn turnState;

        // 승패
        private bool isWin = false;
        private bool isLose = false;

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
            mobTemplate.Add(Tile.ByteMob, new Monster("Byte Jr", 20, 1, 1));
            mobTemplate.Add(Tile.ShortMob, new Monster("Short Jr", 1500, 2, 3));
            mobTemplate.Add(Tile.IntMob, new Monster("Int Jr", 10000000, 4, 6));
            mobTemplate.Add(Tile.LongMob, new Monster("Long Jr", 1000000000000000000, 5, 14));

            // 보스
            mobTemplate.Add(Tile.ByteBoss, new Monster("Byte", byte.MaxValue, 2, 2));
            mobTemplate.Add(Tile.ShortBoss, new Monster("Short", ushort.MaxValue, 3, 4));
            mobTemplate.Add(Tile.IntBoss, new Monster("Int", uint.MaxValue, 5, 8));
            mobTemplate.Add(Tile.LongBoss, new Monster("Long", ulong.MaxValue, 6, 16));
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
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:

                        // 전투 UI 새로고침
                        PrintCombatUI(combatType);

                        // 플레이어 데미지 계산
                        ulong playerDmg = player.Data.AttackBitSpin();

                        UIManager.UpdateLog(new Log(LogType.PlayerDamage, playerDmg.ToString("N0") + " 피해"));

                        // 몬스터에 데미지
                        combatMonster.TakeDamage(playerDmg);

                        // 몬스터 상태 확인
                        isWin = combatMonster.CheckDead();

                        PrintCombatUI(combatType);

                        // 잠시 대기
                        System.Threading.Thread.Sleep(1000);

                        // 비트 UI 초기화
                        player.Data.PrintBitUI();

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
                    case ConsoleKey.Spacebar:

                        // 주사위 대결
                        DefenseBattle();

                        UIManager.UpdateLog(new Log(LogType.Warning, "공격 턴"));
                        turnState = Turn.PlayerAttack;
                        player.PrintAttackUI();

                        break;
                }
            }
        }

        // 몬스터 공격 턴
        void MonsterAttackTurn()
        {
            PrintMonsterAttack();

            player.PrintDefenseUI();

            turnState = Turn.PlayerDefense;

            UIManager.UpdateLog(new Log(LogType.Warning, "방어 턴"));
        }

        // 전투 시작, 초기화
        void CombatStart()
        {
            isWin = false;
            isLose = false;
            turnState = Turn.PlayerAttack;

            // 전투 몬스터 생성
            combatMonster = new Monster(mobTemplate[combatType]);

            // 전투 UI
            PrintCombatUI(combatType);

            // 공격 행동 UI
            player.PrintAttackUI();

            UIManager.UpdateLog(new Log(LogType.Warning, $"'{combatMonster.Name}' 와 전투"));
        }

        // 전투 루프
        public void CombatLoop()
        {
            // 전투 시작, 초기화
            CombatStart();

            // 턴 루프
            // 승리와 패배 될 때까지
            while (!isWin && !isLose)
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

            // 패배
            if (isLose == true)
            {
                // 메모리 절반 삭제
                int lostMemory = player.Data.MemoryLeak();
                UIManager.UpdateLog(new Log(LogType.Danger, $"패배, Memory {lostMemory} 누수"));
            }
            // 승리
            else
            {
                // 메모리 추가
                int gainMemory = combatMonster.Memory;
                player.Data.MemorySecure(gainMemory);
                UIManager.UpdateLog(new Log(LogType.PlayerDamage, $"승리, Memory {gainMemory} 확보"));

                // 보스면 해금
                if (GameManager.Instance.Unlock.ContainsKey(combatType.ToString()))
                {
                    GameManager.Instance.Unlock[combatType.ToString()] = true;
                    UIManager.UpdateLog(new Log(LogType.Upgrade, $"해금, {combatMonster.Name} 처치"));
                }

                // 최종 보스라면
                if (combatType == Tile.LongBoss)
                {
                    GameManager.Instance.CurrentGameState = GameState.Clear;
                    return;
                }
            }

            // 체력 모두 회복
            player.Data.HPRecovery();

            // 필드로
            GameManager.Instance.CurrentGameState = GameState.Field;
        }

    }
}
