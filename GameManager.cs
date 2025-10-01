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
        Dictionary<string, bool> unlock;
        public Dictionary<string, bool> UnLock
        {
            get { return unlock; }
        }


        // 월드맵 리셋
        bool isReset;
        public bool IsReset
        {
            get { return isReset; }
            set { isReset = value; }
        }


        // 게임 상태
        GameState currentState;
        public GameState CurrentGameState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public GameManager()
        {
            unlock = new Dictionary<string, bool>();
            IsReset = false;
            currentState = GameState.Field;
        }


        // 영역 삭제
        void ClearArea(int x, int y, int width, int height)
        {
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(new string(' ', width + 1));
            }
        }

        // 박스 출력
        public void DrawBox(int x, int y, int width, int height)
        {
            // 영역 삭제
            ClearArea(x, y, width, height);

            // 코너
            Console.SetCursorPosition(x, y); Console.Write('┌');
            Console.SetCursorPosition(x + width, y); Console.Write('┐');
            Console.SetCursorPosition(x, y + height); Console.Write('└');
            Console.SetCursorPosition(x + width, y + height); Console.Write('┘');

            // 가로선
            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(x + i, y);
                Console.Write('─');
                Console.SetCursorPosition(x + i, y + height);
                Console.Write('─');
            }

            // 세로선
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write('│');
                Console.SetCursorPosition(x + width, y + i);
                Console.Write('│');
            }
        }

    }
}
