using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;

namespace Bitstream
{
    enum Tile
    {
        None, BaseLayer, ByteLayer, ShortLayer, IntLayer, LongLayer,    // 맵
        Player = 10,                                    // 플레이어
        Upgrade = 20,                                   // 업그레이드
        LongMob = 30, IntMob, ShortMob, ByteMob,        // 일반몬스터
        ByteBoss = 40, ShortBoss, IntBoss, LongBoss,    // 보스몬스터
    }
    struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int inputX, int inputY)
        {
            x = inputX;
            y = inputY;
        }
    }

    class Map
    {
        // 맵
        int[,] mapTemplate = new int[,] {
            { 5, 5, 5, 5, 5,43,43,43,43, 5, 5, 5, 5, 5,5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
            { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,42,42,42, 5, 5, 5, 5, 5 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 4, 4, 4, 4,41,41,41,41,41, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,40,40,40,40, 3, 3, 3, 3, 3, 3 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,20,20,20,20,20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,20,20,20,20,20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        };

        int[,] currentMap;

        Dictionary<Tile, char[]> bossSymbol = new Dictionary<Tile, char[]>()
        {
            { Tile.ByteBoss, new char[] { 'Β', 'Υ', 'Τ', 'Ε'} },
            { Tile.ShortBoss,new char[] { 'Σ', 'Η', 'Ο', 'Ρ', 'Τ' } },
            { Tile.IntBoss, new char[] { 'Ι', 'Ν', 'Τ' } },
            { Tile.LongBoss, new char[] { 'λ', 'Ο', 'Ν', 'Θ'} },
        };

        // 맵 복사
        public void GetMap()
        {
            currentMap = mapTemplate;
        }

        // 몬스터 타일 추가
        public void AddMonsterTile()
        {
            Random random = GameManager.Instance.rand;

            // 4층
            for (int i = 0; i < 4; i++)
            {
                // 10마리
                for (int j = 0; j < 10; j++)
                {
                    int randX = random.Next(2, mapTemplate.GetLength(1) - 2);
                    int randY = random.Next(1 + 6 * i, 6 * i + 1 + 5);
                    Console.SetCursorPosition((randX + 1) * 2, randY + 1);
                    // 위치 추가
                    currentMap[randY, randX] = 30 + i;
                }
            }
        }

        // 해금 체크
        bool CheckUnlock(Tile tile)
        {
            // 해금 확인
            return GameManager.Instance.Unlock[tile.ToString()];
        }

        // 보스 타일 추가
        Tile AddBossTile(Tile tile, ref int count)
        {
            // 해금 O
            if (CheckUnlock(tile) == true)
            {
                Console.Write("　");
                return Tile.None;
            }

            // 해금 X
            Console.Write(bossSymbol[tile][count]);
            count++;

            return tile;
        }

        // 맵 출력 (벽, 몬스터, 플레이어)
        public Vector2 PrintMap()
        {
            Vector2 startPos = new Vector2();

            int byteCount = 0;
            int shortCount = 0;
            int intCount = 0;
            int longCount = 0;
            Tile newTile = Tile.None;

            for (int y = 0; y < currentMap.GetLength(0); y++)
            {
                Console.Write("　");
                Console.SetCursorPosition(2, y + 1);
                for (int x = 0; x < currentMap.GetLength(1); x++)
                {
                    Tile tile = (Tile)currentMap[y, x];

                    switch (tile)
                    {
                        case Tile.None:
                            Console.Write("　");
                            break;
                        case Tile.BaseLayer:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("▣");
                            break;
                        case Tile.ByteLayer:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("▥");
                            break;
                        case Tile.ShortLayer:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("▤");
                            break;
                        case Tile.IntLayer:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("▦");
                            break;
                        case Tile.LongLayer:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("▩");
                            break;
                        case Tile.ByteMob:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("Β");
                            break;
                        case Tile.ShortMob:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("ξ");
                            break;
                        case Tile.IntMob:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("Ι");
                            break;
                        case Tile.LongMob:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("ι");
                            break;
                        case Tile.ByteBoss:
                            newTile = AddBossTile(tile, ref byteCount);
                            currentMap[y, x] = (int)newTile;
                            break;
                        case Tile.ShortBoss:
                            newTile =  AddBossTile(tile, ref shortCount);
                            currentMap[y, x] = (int)newTile;
                            break;
                        case Tile.IntBoss:
                            newTile =  AddBossTile(tile, ref intCount);
                            currentMap[y, x] = (int)newTile;
                            break;
                        case Tile.LongBoss:
                            newTile =  AddBossTile(tile, ref longCount);
                            currentMap[y, x] = (int)newTile;
                            break;
                        case Tile.Upgrade:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("★");
                            break;
                        case Tile.Player:
                            Console.Write("●");
                            startPos.x = x;
                            startPos.y = y;
                            break;
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor();

            return startPos;
        }

        // 플레이어 위치, 출력 갱신
        public Vector2 UpdatePlayerPos(Vector2 currentPos, Vector2 newPos)
        {
            // 다음 타일
            Tile nextTile = (Tile)mapTemplate[newPos.y, newPos.x];

            switch (nextTile)
            {
                // 벽
                case Tile.BaseLayer:
                case Tile.ByteLayer:
                case Tile.ShortLayer:
                case Tile.IntLayer:
                case Tile.LongLayer:
                    break;

                // 몬스터
                case Tile.ByteMob:
                case Tile.ShortMob:
                case Tile.IntMob:
                case Tile.LongMob:
                case Tile.ByteBoss:
                case Tile.ShortBoss:
                case Tile.IntBoss:
                case Tile.LongBoss:
                    BattleManager.combatType = nextTile;
                    GameManager.Instance.CurrentGameState = GameState.Battle;
                    break;

                // 업그레이드
                case Tile.Upgrade:
                    GameManager.Instance.CurrentGameState = GameState.Upgrade;
                    break;

                default:
                    Console.SetCursorPosition(currentPos.x * 2 + 2, currentPos.y + 1);
                    Console.Write("　");
                    Console.SetCursorPosition(newPos.x * 2 + 2, newPos.y + 1);
                    Console.Write("●");
                    return newPos;
            }

            return currentPos;
        }
    }
}
