using System;
using System.Collections.Generic;

namespace Bitstream
{
    class Program
    {

        // 필드 출력
        static void PrintField(Player player, Map map)
        {
            player.Move(map.PrintMap());        // 맵
            UIManager.UpdateLog();              // 로그 박스
            player.Data.PrintStatUI();          // 플레이어 정보 UI
            player.Data.PrintBitUI();           // 플레이어 비트 UI
        }

        // 클리어 출력
        static void ClearGame()
        {
            Console.Clear();
            bool success = UIManager.TryPrintUI(UIType.Clear, out int x, out int y, out int width, out int height);

            if (success == false)
            {
                Console.WriteLine("클리어");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(x + 6, y + 2);
            Console.Write("세계 최적화를 마쳤습니다");
            Console.SetCursorPosition(x + 5, y + 3);
            Console.Write("플레이해 주셔서 감사합니다");
            Console.ResetColor();

            System.Threading.Thread.Sleep(5000);

            Console.Clear();

        }


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


            bool isClear = false;
            // 클리어 전까지 루프
            while (isClear == false)
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
                        UpgradeManager.UpgradeLoop();       // 업그레이드 루프
                        PrintField(player, map);            // 끝나면 필드
                        break;

                    // 전투
                    case GameState.Battle:
                        battleManager.CombatLoop();         // 전투 루프
                        PrintField(player, map);            // 끝나면 필드
                        break;

                    // 클리어
                    case GameState.Clear:
                        isClear = true;
                        break;
                }

                System.Threading.Thread.Sleep(20);
            }

            // 마지막 콘솔 출력
            ClearGame();

        }
    }
}
