using System;
using System.Collections.Generic;

namespace Bitstream
{
    public enum GameState
    {
        Field, Upgrade, Battle,
    }
    public class GameManager
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public Random rand = new Random();

        // 해금 목록
        public readonly Dictionary<string, bool> Unlock = new Dictionary<string, bool>();

        // 게임 상태
        private GameState currentState = GameState.Field;

        public GameState CurrentGameState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public GameManager()
        {
            Unlock.Add("ByteBoss", false);
            Unlock.Add("ShortBoss", false);
            Unlock.Add("IntBoss", false);
        }

    }
}
