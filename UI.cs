using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    static class UI
    {
        // 로그 시작점
        private const int log_UiStartX = 62;
        private const int log_UiStartY = 16;

        // 로그 UI 크기
        private const int log_UiWidth = 31;
        private const int log_UiHeight = 18;

        // 로그 길이
        private const int logSize = 9;

        // 로그
        private static LinkedList<Log> gameLog = new LinkedList<Log>();



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


        // 로그 박스 출력, 로그 출력
        public static void UpdateLog()
        {
            DrawBox(log_UiStartX, log_UiStartY, log_UiWidth, log_UiHeight);

            PrintLog();
        }

        // 로그 박스 출력 + 로그 추가, 로그 출력
        public static void UpdateLog(Log message)
        {
            // 박스 그리기
            DrawBox(log_UiStartX, log_UiStartY, log_UiWidth, log_UiHeight);

            // 제한 넘어가면 맨 위 삭제
            if (gameLog.Count == logSize)
            {
                gameLog.RemoveLast();
            }
            // 앞에 추가
            gameLog.AddFirst(message);

            PrintLog();

        }

        // 로그 출력
        static void PrintLog()
        {
            int count = 0;

            // 출력
            foreach (var log in gameLog)
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

                Console.SetCursorPosition(log_UiStartX + 2, log_UiStartY + log_UiHeight - 1 - (count * 2));
                Console.Write(log.Content);

                Console.ResetColor();
                count++;
            }
        }
    }
}
