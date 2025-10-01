using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    class Monster
    {
        // 몬스터 이름
        string Name { get; set; }

        // 현재 체력
        int CurrentHp { get; set; }

        // 심볼 Char 
        char Symbol {  get; set; }



        // 몬스터 도감에서 복사 생성
        public Monster(Monster template)
        {
            Name = template.Name;
            CurrentHp = template.CurrentHp;
        }

    }
}
