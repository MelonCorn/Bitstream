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

        int def;        // 방어력 (전위감소)

        // 업그레이드용 재화
        private int memory;

        public int Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        public int Def
        {
            get { return def; }
        }

        public PlayerData()
        {
            // 기본 스탯
            maxHp = 5;
            maxCore = 0;
            maxBit = 1;

            memory = 0;

            currentHp = maxHp;
            currentCore = maxCore;
        }

        // 업그레이드 적용
        public void UpgradeApply(StatType statType, int amount)
        {
            switch (statType)
            {
                case StatType.Hp:
                    maxHp += amount;
                    currentHp += amount;
                    break;
                case StatType.Bit:
                    maxBit += amount;
                    PrintBitUI();
                    break;
                case StatType.Core:
                    maxCore += amount;
                    currentCore += amount;
                    break;
                case StatType.Def:
                    def += amount;
                    break;
            }
        }

        // 플레이어 스탯 정보 갱신 (전투용)
        public void UpdatePlayerStat(StatType type, int amount)
        {
            switch (type)
            {
                case StatType.Hp:
                    currentHp -= amount;
                    if (currentHp < 0)
                    {
                        currentHp = 0;
                    }
                    PrintStatUI();
                    UIManager.UpdateLog(new Log(LogType.MonsterDamage, amount + " 피해"));
                    break;
                case StatType.Core:
                    break;
            }
        }

        // 체력 최대 회복
        public void HPRecovery()
        {
            currentHp = maxHp;
        }

        // 체력 상태 확인
        public bool CheckDead()
        {
            return currentHp <= 0 ? true : false;
        }

        // 메모리 소실
        public int MemoryLeak()
        {
            return memory /= 2;
        }

        // 메모리 확보
        public void MemorySecure(int amount)
        {
            memory += amount;
        }
        
    }
}
