using System;
using System.Collections.Generic;

namespace Bitstream
{
    class UpgradeItem
    {
        public string Name { get; private set; }    // 이름
        public string Info { get; set; }            // 설명
        public int Price { get; }                   // 비용
        public int CurrentLevel { get; set; }       // 현재 레벨
        public int MaxLevel { get; set; }           // 최대 레벨

        // 잠긴 레벨의 해금 조건
        public Dictionary<int, string> LockLevels { get; protected set; }

        // 생성자 - 이름, 설명, 비용, 최대레벨, 해금여부
        public UpgradeItem(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
        {
            Name = inputName;
            Info = inputInfo;
            Price = inputPrice;
            CurrentLevel = 0;
            MaxLevel = inputMaxLvl;
            LockLevels = new Dictionary<int, string>();
        }

        // 업그레이드
        public virtual void Upgrade()
        {
            // 최대 업그레이드 시
            if (CurrentLevel == MaxLevel)
                return;

            CurrentLevel++;
        }
    }

    // 업그레이드 잠금 인터페이스
    interface IUpgradeLock
    {
        void UpgradeLock();

    }

    // 체력 업그레이드
    class HpUpgrade : UpgradeItem, IUpgradeLock
    {
        public HpUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base (inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            //       byte   short        int           
            // ♥♥♥ ♥♥ /♥♥♥♥♥/ ♥♥♥♥♥ ♥♥♥♥♥/ ♥♥♥♥♥ ♥♥♥♥♥
            LockLevels.Add(2, "Byte");
            LockLevels.Add(7, "Short");
            LockLevels.Add(17, "Int");
        }

        public void UpgradeLock()
        {

        }
    }

    // 코어 업그레이드
    class CoreUpgrade : UpgradeItem, IUpgradeLock
    {
        public CoreUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            //      byte short int
            //       /★/ ★/ ★
            LockLevels.Add(0, "Byte");
            LockLevels.Add(1, "Short");
            LockLevels.Add(2, "Int");
        }
        public void UpgradeLock()
        {
        }
    }
    
    // 비트 업그레이드
    class BitUpgrade : UpgradeItem, IUpgradeLock
    {
        public BitUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            //           byte        short                  int                                       
            // 0000 0000 / 0000 0000  / 0000 0000 0000 0000 / 0000 0000 0000 0000  0000 0000 0000 0000
            LockLevels.Add(1, "Byte");
            LockLevels.Add(3, "Short");
            LockLevels.Add(7, "Int");
        }
        public void UpgradeLock()
        {
        }
    }






}
