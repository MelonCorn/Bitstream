using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Bitstream
{
    class Monster
    {
       
        public readonly string Name;         // 몬스터 이름
        public ulong CurrentHp { get; set; } // 현재 체력
        public ulong MaxHp { get; set; }     // 최대 체력
        public int Dmg { get; set; }         // 기본 데미지
        public int Bits { get; set; }        // 비트

        public int Memory { get; set; }      // 처치 시 획득 메모리


        public Monster(string inputName, ulong inputHp, int inputDmg, int bits)
        {
            Name = inputName;
            MaxHp = inputHp;
            Dmg = inputDmg;
            Bits = bits;
        }

        // 몬스터 도감에서 복사 생성
        public Monster(Monster template)
        {
            Name = template.Name;
            MaxHp = template.MaxHp;
            CurrentHp = MaxHp;
            Dmg = template.Dmg;
            Bits = template.Bits;
            Memory = GameManager.Instance.rand.Next(10 * Dmg, 20 * Dmg);
        }

        // 데미지로 인한 체력 감소
        public void TakeDamage(ulong dmg)
        {
            if (CurrentHp < dmg)
            {
                CurrentHp = 0;
            }
            else
            {
                CurrentHp -= dmg;
            }
        }

        // 체력 상태 확인
        public bool CheckDead()
        {
            return CurrentHp <= 0 ? true : false;
        }

    }
}
