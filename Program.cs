using System;
using System.Collections.Generic;

namespace Bitstream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<string, Monster> mobList = new Dictionary<string, Monster>();
            Console.SetWindowSize(100, 40);
            Console.CursorVisible = false;

            Map map = new Map();
            Player player = new Player();
            UpgradeManager shopManager = new UpgradeManager(player);

            // 맵 출력, 플레이어 포지션 설정
            player.Move(map.PrintMap());
            // 로그 박스
            UI.UpdateLog();

            while (true)
            {
                // 현재 게임 상태
                switch (GameManager.Instance.CurrentGameState)
                {
                    case GameState.Field:           // 필드
                        if (Console.KeyAvailable)
                        {
                            // 다음 위치 = 플레이어 위치 갱신 메서드(현재 위치, 입력)
                            Vector2 nextPos = map.UpdatePlayerPos(player.CurrentPos, player.MoveInput());
                            player.Move(nextPos); // 이동
                        }
                        break;

                    case GameState.Upgrade:                 // 업그레이드
                        shopManager.UpgradeLoop();          // 루프
                        player.Move(map.PrintMap());        // 나오면 맵 다시
                        UI.UpdateLog(); // 로그 박스
                        break;

                    case GameState.Battle:      // 전투
                        break;
                }
            }
        }
    }
}
