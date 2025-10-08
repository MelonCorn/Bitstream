using System;
using System.Collections.Generic;

namespace Bitstream
{
    partial class UpgradeManager
    {
        // 재화를 사용할 플레이어
        private Player player;

        // 업그레이드 목록
        private List<UpgradeItem> upgradeItems = new List<UpgradeItem>();

        // 현재 선택된 업그레이드 (partial UI에서 조작)
        private int selectedItemNum = 0;

        public UpgradeManager(Player inputPlayer)
        {
            player = inputPlayer;
            // 업그레이드 생성
            upgradeItems.Add(new HpUpgrade("Hp++", "최대 체력 확장", 8, 25));
            upgradeItems.Add(new BitUpgrade("Bit++", "공격 비트 확장" , 10, 15));
            upgradeItems.Add(new CoreUpgrade("Core++", "전투당 스킬 사용 횟수 확장", 300, 3));
            upgradeItems.Add(new DefUpgrade("전위감소", "피격 데미지가 1 감소", 500, 3));
        }

        // 구매 시도
        void Purchase()
        {
            UpgradeItem item = upgradeItems[selectedItemNum];

            // 비용 확인
            if (PriceCheck(item) == false) return;

            // 레벨 확인 - 최대 레벨
            if (LevelCheck(item) == true) return;

            // 해금 상태 확인
            if (UnlockCheck(item) == false)  return;

            // 재화 감소
            player.Data.Memory -= item.Price;

            // 업그레이드
            item.Upgrade(player.Data);

            // UI 갱신
            PrintUpgrageListUI(selectedItemNum);

            UIManager.UpdateLog(new Log(LogType.Upgrade, $"{item.Name} 업그레이드. {item.CurrentLevel} / {item.MaxLevel}"));
        }

        // 비용 확인
        bool PriceCheck(UpgradeItem item)
        {
            // 플레이어의 재화가 업그레이드 비용보다 적다면
            if (item.Price > player.Data.Memory)
            {
                UIManager.UpdateLog(new Log(LogType.Warning ,"Memory 부족"));
                // 재화 부족하다고 알림
                return false;
            }

            return true;
        }

        // 레벨 확인
        bool LevelCheck(UpgradeItem item)
        {
            // 최대 레벨인가
            if (item.CurrentLevel >= item.MaxLevel)
            {
                UIManager.UpdateLog(new Log(LogType.Warning, "최대 레벨"));
                // 최대 레벨입니다.
                return true;
            }

            return false;

        }

        // 해금 상태 확인
        bool UnlockCheck(UpgradeItem item)
        {
            // 잠금 레벨 목록이 있는가
            // 잠금 레벨 목록에 현재 레벨이 포함되어있나
            if (item.LockLevels.Count > 0 && item.LockLevels.ContainsKey(item.CurrentLevel))
            {
                string key = item.LockLevels[item.CurrentLevel];

                // 해금 목록에 항목의 해금 조건이 포함 되어있나
                if (GameManager.Instance.Unlock.ContainsKey(key))
                {
                    // 해금이 되었나
                    if (GameManager.Instance.Unlock[key] == false)
                    {
                        UIManager.UpdateLog(new Log(LogType.Danger, $"해금 필요 / {key} 처치"));
                        return false;
                    }
                }
                // 포함 안 되어있어도 안전상
                else
                {
                    return false;
                }
            }

            // 모든 조건 통과

            return true;
        }

        // 업그레이드 이용 중
        public void UpgradeLoop()
        {
            selectedItemNum = 0;

            // UI 출력
            PrintUpgrageListUI(selectedItemNum);
            PrintInfoUI(selectedItemNum);
            UIManager.UpdateLog(new Log(LogType.Normal, "업그레이드 UI 로딩"));

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
                            UIManager.UpdateLog(new Log(LogType.Normal, "업그레이드 UI 종료"));
                            Console.Clear();
                            return;
                    }
                }

                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
