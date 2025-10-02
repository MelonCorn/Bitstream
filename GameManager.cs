using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Bitstream
{
    public enum GameState
    {
        Field, Upgrade, Battle,
        PlayerTurn = 10, EnemyTurn,
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

        // 해금 목록
        private Dictionary<string, bool> unlock = new Dictionary<string, bool>();

        // 게임 상태
        private GameState currentState = GameState.Field;

        public Dictionary<string, bool> UnLock
        {
            get { return unlock; }
        }
        public GameState CurrentGameState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public GameManager()
        {
            unlock.Add("Byte", false);
            unlock.Add("Short", false);
            unlock.Add("int", false);
        }

    }
}
