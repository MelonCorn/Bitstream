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
            UpgradeManager shopManager = new UpgradeManager();
            Player player = new Player();

            // 맵 출력, 플레이어 포지션 설정
            player.CurrentPos = map.PrintMap();

            while(true)
            {
                switch (GameManager.Instance.CurrentGameState)
                {
                        // 필드
                    case GameState.Field:
                        if (Console.KeyAvailable)
                        {
                            Vector2 nextPos = map.UpdatePlayerPos(player.CurrentPos, player.MoveInput());
                            player.Move(nextPos);
                        }
                        break;

                        // 업그레이드
                    case GameState.Upgrade:
                        shopManager.ShopLoop();
                        Console.Clear();
                        player.CurrentPos = map.PrintMap();
                        break;

                        // 전투
                    case GameState.Battle:
                        break;
                }
            }
        }
    }
}
