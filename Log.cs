using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    enum LogType
    {
        Normal, Warning, Danger,        // 위험도 흰 노 빨
        Upgrade = 10,                   // 업그레이드 성공 하늘
        PlayerDamage = 20, MonsterDamage, // 전투 데미지 초 주

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
