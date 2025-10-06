using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    partial class UpgradeManager
    {
        // 항목 UI 출력
        void PrintUpgrageListUI(int newSelectedNum)
        {
            bool success = UIManager.TryPrintUI(UIType.UpgradeList, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 업그레이드 UI
            // 제목 출력
            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("----------- [ 업그레이드 목록 ] ----------");
            Console.SetCursorPosition(x + 2, y + 3);
            Console.WriteLine("    [이름]         [비용]         [레벨]");

            // 각 항목 출력
            for (int i = 0; i < upgradeItems.Count; i++)
            {
                var item = upgradeItems[i];
               
                int yOffset = 5 + i * 2;                        // 글자 세로 간격
                ConsoleColor fontColor = ConsoleColor.White;    // 글자 색
                string space = "";                              // 여백   

                // 선택된 항목이라면
                if (i == newSelectedNum)
                {
                    // 강조 표시
                    fontColor = ConsoleColor.Yellow;
                    space = "   ";
                }

                Console.ForegroundColor = fontColor;

                Console.SetCursorPosition(x + 2, y + yOffset);
                Console.Write($"  {space}{item.Name}");
                Console.SetCursorPosition(x + 22, y + yOffset);
                Console.Write($"{item.Price}");
                Console.SetCursorPosition(x + 36, y + yOffset);
                Console.Write($"{item.CurrentLevel} / {item.MaxLevel}");
                Console.ResetColor();
            }

            Console.SetCursorPosition(x + 4, y + height - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Memory : {player.Data.Memory}");
            Console.ResetColor();

            Console.SetCursorPosition(x + 26, y + height - 1);
            Console.Write($"[Q]를 눌러 종료");
        }

        // 정보 UI 출력
        void PrintInfoUI(int itemNum)
        {
            bool success = UIManager.TryPrintUI(UIType.UpgradeInfo, out int x, out int y, out int width, out int height);

            if (success == false) return;

            // 업그레이드 UI
            // 제목 출력
            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("---- [ 업그레이드 정보 ] ----");
            var item = upgradeItems[itemNum];

            // 항목 설명 분할해서 담을 큐
            Queue<string> info = new Queue<string>();

            // 항목 설명 분할 기준 : 설명 너비 / 2(한글) - 1(여백)
            int cutLength = (width / 2) - 1;

            // 설명이 분할길이 이상이면. 분할.
            if (item.Info.Length >= cutLength)
            {
                for (int i = 0; i < item.Info.Length / cutLength; i++)
                {
                    info.Enqueue(item.Info.Substring(cutLength * i, cutLength));
                }
            }
            // 아니면 그냥 담기
            else
            {
                info.Enqueue(item.Info);
            }

            // 항목 이름
            Console.SetCursorPosition(x + 2, y + 3);
            Console.Write($"  {item.Name}");

            // 항목 분할 설명 큐 출력
            for (int i = 0; i < info.Count; i++)
            {
                Console.SetCursorPosition(x + 2, y + 5 + i);
                Console.Write($"  {info.Dequeue().TrimStart()}");
            }

        }

        // 항목 선택 
        void ItemNavigation(int dir)
        {
            selectedItemNum += dir;

            // 0 에서 한 번 더 위로
            if (selectedItemNum < 0)
            {
                selectedItemNum = upgradeItems.Count - 1;
            }
            // 최대 에서 한 번 더 아래로
            else if (selectedItemNum > upgradeItems.Count - 1)
            {
                selectedItemNum = 0;
            }

            PrintUpgrageListUI(selectedItemNum);
            PrintInfoUI(selectedItemNum);
        }
    }
}
