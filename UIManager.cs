using System;
using System.Collections.Generic;

namespace Bitstream
{
    enum UIType
    {
        UpgradeList, UpgradeInfo,
        PlayerStat, PlayerBit,
        PlayerAttack, PlayerDefense,
        BattleInfo,
        Log,
    }
    class UIManager
    {
        private static UIManager instance;

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }


        // UI 묶음
        public readonly Dictionary<UIType, UIComponent> UI = new Dictionary<UIType, UIComponent>();

        // 로그 길이
        private const int logSize = 7;

        // 로그 
        private LinkedList<Log> logs = new LinkedList<Log>();

        // UI 목록 생성
        public UIManager()
        {
            // 업그레이드
            UI.Add(UIType.UpgradeList, new UIComponent(4, 2, 44, 27));
            UI.Add(UIType.UpgradeInfo, new UIComponent(54, 19, 33, 12));

            // 플레이어
            UI.Add(UIType.PlayerStat, new UIComponent(54, 19, 33, 12));
            UI.Add(UIType.PlayerBit, new UIComponent(2, 33, 85, 4));
            UI.Add(UIType.PlayerAttack, new UIComponent(15, 24, 23, 6));
            UI.Add(UIType.PlayerDefense, new UIComponent(15, 24, 23, 6));

            // 전투
            UI.Add(UIType.BattleInfo, new UIComponent(2, 1, 48, 30));

            // 로그
            UI.Add(UIType.Log, new UIComponent(54, 1, 33, 16));
        }


        // UI 타입에 맞는 위치, 사이즈 반환
        public static bool TryPrintUI(UIType type, out int x, out int y, out int width, out int height)
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;

            UIComponent ui = Instance.UI[type];

            switch (type)
            {
                case UIType.UpgradeList:
                case UIType.UpgradeInfo:
                case UIType.PlayerStat:
                case UIType.PlayerBit:
                case UIType.PlayerAttack:
                case UIType.PlayerDefense:
                case UIType.BattleInfo:
                case UIType.Log:
                    x = ui.StartX;
                    y = ui.StartY;
                    width = ui.Width;
                    height = ui.Height;
                    DrawBox(x, y, width, height);
                    return true;
            }

            return false;
        }

        //-----------------------------------------------------

        // 영역 삭제
        static void ClearArea(int x, int y, int width, int height)
        {
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(new string(' ', width + 1));
            }
        }

        // 박스 출력
        public static void DrawBox(int x, int y, int width, int height)
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

        //-----------------------------------------------------

        // 로그 박스 출력, 로그 출력
        public static void UpdateLog()
        {
            bool success = TryPrintUI(UIType.Log, out int x, out int y, out int width, out int height);

            if (success == false) return;

            DrawBox(x, y, width, height);

            Instance.PrintLog(x, y, width, height);
        }

        // 로그 박스 출력 + 로그 추가, 로그 출력
        public static void UpdateLog(Log message)
        {
            bool success = TryPrintUI(UIType.Log, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 박스 그리기
            DrawBox(x, y, width, height);

            // 제한 넘어가면 맨 위 삭제
            if (Instance.logs.Count == logSize)
            {
                Instance.logs.RemoveLast();
            }
            // 앞에 추가
            Instance.logs.AddFirst(message);

            Instance.PrintLog(x, y, width, height);
        }

        // 로그 출력
        private void PrintLog(int x, int y, int width, int height)
        {
            int count = 0;

            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("------- [ 시스템 로그 ] -------");

            // 출력
            foreach (var log in logs)
            {
                ConsoleColor color = ConsoleColor.White;

                // 메세지 타입에 맞게 색
                switch (log.Type)
                {
                    case LogType.Warning:
                        color = ConsoleColor.Yellow;
                        break;
                    case LogType.Danger:
                        color = ConsoleColor.Red;
                        break;
                    case LogType.Upgrade:
                        color = ConsoleColor.Cyan;
                        break;
                    case LogType.PlayerDamage:
                        color = ConsoleColor.Green;
                        break;
                    case LogType.MonsterDamage:
                        color = ConsoleColor.Magenta;
                        break;
                    default:
                        break;
                }

                Console.ForegroundColor = color;

                Console.SetCursorPosition(x + 2, y + height - 1 - (count * 2));
                Console.Write(log.Content);

                Console.ResetColor();
                count++;
            }
        }
    }
}
