using System;
using System.Collections.Generic;

namespace Bitstream
{
    class Monster
    {
       
        public readonly string Name;        // 몬스터 이름
        public ulong CurrentHp { get; set; } // 현재 체력
        public ulong MaxHp { get; set; }    // 최대 체력
        public int Bits { get; set; }       // 비트


        public Monster(string inputName, ulong inputHp, int bits)
        {
            Name = inputName;
            MaxHp = inputHp;
            Bits = bits;
        }

        // 몬스터 도감에서 복사 생성
        public Monster(Monster template)
        {
            Name = template.Name;
            MaxHp = template.MaxHp;
            CurrentHp = MaxHp;
            Bits = template.Bits;
        }

    }
}
