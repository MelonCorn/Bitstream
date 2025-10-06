using System;
using System.Collections.Generic;

namespace Bitstream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(91, 39);
            Console.CursorVisible = false;

            Map map = new Map();
            Player player = new Player();
            UpgradeManager UpgradeManager = new UpgradeManager(player);
            BattleManager battleManager = new BattleManager(player);


            // 기본 맵 불러오기
            map.GetMap();
            // 맵에 몬스터 추가
            map.AddMonsterTile();
            // 맵 출력, 플레이어 포지션 설정
            player.Move(map.PrintMap());

            // 플레이어 정보 UI 출력
            player.Data.PrintStatUI();
            player.Data.PrintBitUI();

            // 로그 UI 출력
            UIManager.UpdateLog();

            while (true)
            {
                // 현재 게임 상태
                switch (GameManager.Instance.CurrentGameState)
                {
                    // 필드
                    case GameState.Field:           
                        if (Console.KeyAvailable)
                        {
                            // 다음 위치 = 플레이어 위치 갱신 메서드(현재 위치, 입력)
                            Vector2 nextPos = map.UpdatePlayerPos(player.CurrentPos, player.MoveInput());
                            player.Move(nextPos); // 이동
                        }
                        break;

                    // 업그레이드
                    case GameState.Upgrade:                 
                        UpgradeManager.UpgradeLoop();       // 루프
                        player.Move(map.PrintMap());        // 나오면 다시 맵
                        UIManager.UpdateLog();              // 로그 박스
                        player.Data.PrintStatUI();          // 플레이어 정보 UI 출력
                        player.Data.PrintBitUI();
                        break;

                    // 전투
                    case GameState.Battle:
                        battleManager.CombatLoop();
                        player.Move(map.PrintMap());        // 나오면 다시 맵
                        UIManager.UpdateLog();              // 로그 박스
                        player.Data.PrintStatUI();          // 플레이어 정보 UI 출력
                        player.Data.PrintBitUI();
                        break;
                }

                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
