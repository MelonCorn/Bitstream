using System;
using System.Collections.Generic;

namespace Bitstream
{
    // 업그레이드 타입
    enum UpgradeType
    {
        None,       // 아무것도아님
        MaxHp,      // 최대 체력
        MaxBit,     // 최대 비트
        MaxCore,    // 최대 코어
    }
    class UpgradeItem
    {
        public string Name { get; private set; }    // 이름
        public string Info { get; set; }            // 설명
        public int Price { get; }                   // 비용
        public int CurrentLevel { get; set; }       // 현재 레벨
        public int MaxLevel { get; set; }           // 최대 레벨

        protected UpgradeType TargetStat { get; set; }   // 업그레이드 스탯
        protected int IncreaseAmount { get; set; }       // 업그레이드 수치


        // 잠긴 레벨의 해금 조건
        public readonly Dictionary<int, string> LockLevels = new Dictionary<int, string>();

        // 생성자 - 이름, 설명, 비용, 최대레벨, 해금여부
        public UpgradeItem(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
        {
            Name = inputName;
            Info = inputInfo;
            Price = inputPrice;
            CurrentLevel = 0;
            MaxLevel = inputMaxLvl;
        }

        // 업그레이드
        public void Upgrade(PlayerData playerData)
        {
            // 최대 업그레이드 시
            if (CurrentLevel == MaxLevel) return;

            // 레벨 증가
            CurrentLevel++;

            // 업그레이드 스탯 적용  (타겟, 수치)
            playerData.UpgradeApply(TargetStat, IncreaseAmount);
        }
    }

    // 체력 업그레이드
    class HpUpgrade : UpgradeItem
    {
        public HpUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base (inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = UpgradeType.MaxHp;
            IncreaseAmount = 1;

            //       byte   short        int           
            // ♥♥♥ ♥♥ /♥♥♥♥♥/ ♥♥♥♥♥ ♥♥♥♥♥/ ♥♥♥♥♥ ♥♥♥♥♥
            LockLevels.Add(2, "ByteBoss");
            LockLevels.Add(7, "ShortBoss");
            LockLevels.Add(17, "IntBoss");
        }
    }

    // 코어 업그레이드
    class CoreUpgrade : UpgradeItem
    {
        public CoreUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = UpgradeType.MaxCore;
            IncreaseAmount = 1;

            //      byte short int
            //       /★/ ★/ ★
            LockLevels.Add(0, "ByteBoss");
            LockLevels.Add(1, "ShortBoss");
            LockLevels.Add(2, "IntBoss");
        }
    }
    
    // 비트 업그레이드
    class BitUpgrade : UpgradeItem
    {
        public BitUpgrade(string inputName = "NULL", string inputInfo = "NULL", int inputPrice = 999999, int inputMaxLvl = 1)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = UpgradeType.MaxBit;
            IncreaseAmount = 1;
            //           byte        short                  int                                       
            // 0000 0000 / 0000 0000  / 0000 0000 0000 0000 / 0000 0000 0000 0000  0000 0000 0000 0000
            LockLevels.Add(1, "ByteBoss");
            LockLevels.Add(3, "ShortBoss");
            LockLevels.Add(7, "IntBoss");
        }
    }






}
