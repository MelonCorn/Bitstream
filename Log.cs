using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    enum LogType
    {
        Normal, Warning, Danger,        // 위험도 (White, Yellow, Red)
        Upgrade = 10,                   // 업그레이드 성공 (Cyan)
        PlayerDamage = 20, MonsterDamage, // 전투 데미지 (Green, Magenta)
    }

    class Log
    {
        public string Content { get; }
        public LogType Type { get; }

        public Log()
        {
            Content = "NULL";
            Type = LogType.Danger;
        }

        public Log(LogType inputType, string inputString)
        {
            Content = inputString;
            Type = inputType;
        }
    }
}
