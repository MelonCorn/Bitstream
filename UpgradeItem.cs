using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bitstream
{
    // 업그레이드 타입
    enum StatType
    {
        Hp,      // 체력
        Core,    // 코어
        Bit,     // 비트
        Def, // 전위감소
    }
    class UpgradeItem
    {
        public string Name { get; }    // 이름
        public string Info { get; }            // 설명
        public int Price { get; set; }                   // 비용
        public int CurrentLevel { get; set; }       // 현재 레벨
        public int MaxLevel { get; }           // 최대 레벨

        protected StatType TargetStat { get; set; }   // 업그레이드 스탯
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
        public virtual void Upgrade(PlayerData playerData)
        {
            // 최대 업그레이드 시
            if (CurrentLevel == MaxLevel) return;

            // 레벨 증가
            CurrentLevel++;

            // 비용 증가
            Price += Price / 3;

            // 업그레이드 스탯 적용  (타겟, 수치)
            playerData.UpgradeApply(TargetStat, IncreaseAmount);
        }
    }

    // 체력 업그레이드
    class HpUpgrade : UpgradeItem
    {
        public HpUpgrade(string inputName, string inputInfo, int inputPrice, int inputMaxLvl)
            : base (inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = StatType.Hp;
            IncreaseAmount = 1;

            //             byte   short    int           
            // ♥♥♥♥♥ / ♥♥♥♥♥/ ♥♥♥♥♥ / ♥♥♥♥♥/ ♥♥♥♥♥ ♥♥♥♥♥
            LockLevels.Add(5, "ByteBoss");
            LockLevels.Add(10, "ShortBoss");
            LockLevels.Add(15, "IntBoss");
        }
    }

    // 코어 업그레이드
    class CoreUpgrade : UpgradeItem
    {
        public CoreUpgrade(string inputName, string inputInfo, int inputPrice, int inputMaxLvl)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = StatType.Core;
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
        public BitUpgrade(string inputName, string inputInfo, int inputPrice, int inputMaxLvl)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = StatType.Bit;
            IncreaseAmount = 1;
            //           byte        short                  int                                       
            // 0000 0000 / 0000 0000  / 0000 0000 0000 0000 / 0000 0000 0000 0000  0000 0000 0000 0000
            LockLevels.Add(1, "ByteBoss");
            LockLevels.Add(3, "ShortBoss");
            LockLevels.Add(7, "IntBoss");
        }
    }

    // 전위감소 업그레이드
    class DefUpgrade : UpgradeItem
    {
        public DefUpgrade(string inputName, string inputInfo, int inputPrice, int inputMaxLvl)
            : base(inputName, inputInfo, inputPrice, inputMaxLvl)
        {
            TargetStat = StatType.Def;
            IncreaseAmount = 1;

            LockLevels.Add(0, "ByteBoss");
            LockLevels.Add(1, "ShortBoss");
            LockLevels.Add(2, "IntBoss");
        }
    }






}
