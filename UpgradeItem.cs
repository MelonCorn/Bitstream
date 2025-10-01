using System;
using System.Collections.Generic;

namespace Bitstream
{
    class UpgradeItem
    {
        public string Name { get; private set; }

        public string Info { get; set; }

        public int Price { get; set; }

        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }


        // 이름, 설명, 비용, 최대레벨, 해금여부
        public UpgradeItem(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1, bool requireUnlock = false)
        {
            Name = inputName;
            Info = inputInfo;
            Price = inputPrice;
            CurrentLevel = 1;
            MaxLevel = inputMaxLvl;
            if (requireUnlock == true)
            {
                GameManager.Instance.UnLock.Add(Name, false);
            }
        }

        // 업그레이드
        public void Upgrade()
        {
            // 최대 업그레이드 시
            if (CurrentLevel == MaxLevel)
                return;

            CurrentLevel++;
        }
    }

    // 업그레이드 잠금 인터페이스
    interface ITierLock
    {
        void UpgradeLock();
    }

    // 체력 업그레이드
    class HpUpgradeItem : UpgradeItem, ITierLock
    {
        public void UpgradeLock()
        {
        }
    }

    // 코어 업그레이드
    class CoreUpgradeItem : UpgradeItem, ITierLock
    {
        public void UpgradeLock()
        {
        }
    }
    
    // 비트 업그레이드
    class BitUpgradeItem : UpgradeItem, ITierLock
    {
        public void UpgradeLock()
        {
        }
    }






}
