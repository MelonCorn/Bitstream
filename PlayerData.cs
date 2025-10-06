using System;
using System.Collections.Generic;

namespace Bitstream
{
    partial class PlayerData
    {
        // 최대
        int maxBit;       // 비트
        int maxHp;         // 체력
        int maxCore;       // 코어

        // 현재
        int currentHp;
        int currentCore;

        // 업그레이드용 재화
        private int memory;
        public int Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        public PlayerData()
        {
            // 기본 스탯
            maxHp = 3;
            maxCore = 0;
            maxBit = 16;

            memory = 1000;

            currentHp = maxHp;
            currentCore = maxCore;
        }

        // 업그레이드 적용
        public void UpgradeApply(UpgradeType statType, int amount)
        {
            switch (statType)
            {
                case UpgradeType.MaxHp:
                    maxHp += amount;
                    currentHp += amount;
                    break;
                case UpgradeType.MaxBit:
                    maxBit += amount;
                    PrintBitUI();
                    break;
                case UpgradeType.MaxCore:
                    maxCore += amount;
                    currentCore += amount;
                    break;
            }
        }
    }
}
