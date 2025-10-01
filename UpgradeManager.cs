using System;
using System.Collections.Generic;

namespace Bitstream
{
    class UpgradeManager
    {
        // 업그레이드 UI 시작점
        private const int upgrade_UiStartX = 5;
        private const int upgrade_UiStartY = 2;

        // 업그레이드 UI 크기
        private const int upgrade_UiWidth = 45;
        private const int upgrade_UiHeight = 30;

        // 정보 UI 시작점
        private const int info_UiStartX = 62;
        private const int info_UiStartY = 6;

        // 정보 UI 크기
        private const int info_UiWidth = 31;
        private const int info_UiHeight = 24;


        List<UpgradeItem> upgradeItems;

        int selectedItemNum = 0;


        public UpgradeManager()
        {
            // 아이템 미리 생성
            upgradeItems = new List<UpgradeItem>();

            upgradeItems.Add(new UpgradeItem("체력 증가", "최대 체력이 증가", 10));
            upgradeItems.Add(new UpgradeItem("코어 확장", "전투마다 스킬을 사용 가능 횟수를 확장", 10));
            upgradeItems.Add(new UpgradeItem("비트 확장", "공격에 사용할 수 있는 비트 수를 확장" , 10));
            upgradeItems.Add(new UpgradeItem("Queue", "최근 적에게 준 데미지를 3번 저장. 가장 최근 준 데미지를 현재 턴에 합산", 10));
            upgradeItems.Add(new UpgradeItem("Stack", "", 10));
        }

        // 항목 UI 출력
        void ShowUpgrageUI(int newSelectedNum)
        {
            // 영역 삭제, 박스 출력
            GameManager.Instance.DrawBox(upgrade_UiStartX, upgrade_UiStartY, upgrade_UiWidth, upgrade_UiHeight);

            // 업그레이드 UI
            // 제목 출력
            Console.SetCursorPosition(upgrade_UiStartX + 2, upgrade_UiStartY + 1);
            Console.WriteLine("----------- [ 업그레이드 목록 ] -----------");
            Console.SetCursorPosition(upgrade_UiStartX + 2, upgrade_UiStartY + 3);
            Console.WriteLine("    [이름]         [비용]         [레벨]");

            // 각 항목 출력
            for (int i = 0; i < upgradeItems.Count; i++)
            {
                var item = upgradeItems[i];

                // 언락이 필요한 항목인지 확인
                bool unlockItem = GameManager.Instance.UnLock.ContainsKey(item.Name);

                if (unlockItem == true)
                {

                }
                else
                {
                    int yOffset = 5 + i * 2;
                    
                    // 글자 색
                    ConsoleColor fontColor = ConsoleColor.White;
                    // 여백
                    string space = "";

                    // 선택된 항목이라면
                    if (i == newSelectedNum)
                    {
                        fontColor = ConsoleColor.Yellow;
                        space = "   ";
                    }

                    Console.ForegroundColor = fontColor;

                    Console.SetCursorPosition(upgrade_UiStartX + 2, upgrade_UiStartY + yOffset);
                    Console.Write($"  {space}{item.Name}");
                    Console.SetCursorPosition(upgrade_UiStartX + 22, upgrade_UiStartY + yOffset);
                    Console.Write($"{item.Price}");
                    Console.SetCursorPosition(upgrade_UiStartX + 36, upgrade_UiStartY + yOffset);
                    Console.Write($"{item.CurrentLevel} / {item.MaxLevel}");
                    Console.ResetColor();
                }
            }
            Console.SetCursorPosition(upgrade_UiStartX + 2, upgrade_UiStartY + upgrade_UiHeight - 1);
            Console.WriteLine("[Q]를 눌러 퇴장");
        }

        // 정보 UI 출력
        void ShowInfoUI(int itemNum)
        {
            // 정보 UI
            GameManager.Instance.DrawBox(info_UiStartX, info_UiStartY, info_UiWidth, info_UiHeight);

            // 업그레이드 UI
            // 제목 출력
            Console.SetCursorPosition(info_UiStartX + 2, info_UiStartY + 1);
            Console.WriteLine("---- [ 업그레이드 정보 ] ----");
            var item = upgradeItems[itemNum];

            // 언락이 필요한 항목인지 확인
            bool unlockItem = GameManager.Instance.UnLock.ContainsKey(item.Name);

            if (unlockItem == true)
            {

            }
            else
            {
                // 항목 설명 분할해서 담을 큐
                Queue<string> info = new Queue<string>();
                // 항목 설명 분할 기준 : 설명 너비 / 2(한글) - 2(여백)
                int cutLength = (info_UiWidth / 2) - 2;

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
                Console.SetCursorPosition(info_UiStartX + 2, info_UiStartY + 3);
                Console.Write($"  {item.Name}");

                // 항목 분할 설명 큐 출력
                for (int i = 0; i < info.Count; i++)
                {
                    Console.SetCursorPosition(info_UiStartX + 2, info_UiStartY + 5 + i);
                    Console.Write($"  {info.Dequeue().TrimStart()}");
                }
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

            ShowUpgrageUI(selectedItemNum);
            ShowInfoUI(selectedItemNum);
        }


        // 구매
        void Purchase()
        {

        }

        // 이용 중
        public void ShopLoop()
        {
            // UI 출력
            ShowUpgrageUI(0);
            ShowInfoUI(selectedItemNum);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo inputKey = Console.ReadKey(true);

                    switch (inputKey.Key)
                    {
                        // 위 (항목 선택)
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            ItemNavigation(-1);
                            break;
                        // 아래 (항목 선택)
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            ItemNavigation(1);
                            break;
                        // 결정 (구매)
                        case ConsoleKey.Spacebar:
                        case ConsoleKey.Enter:
                            Purchase();
                            break;
                        // 퇴장
                        case ConsoleKey.Q:
                            GameManager.Instance.CurrentGameState = GameState.Field;
                            return;
                    }
                }
            }
        }
    }
}
