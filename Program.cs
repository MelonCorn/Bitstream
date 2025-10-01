using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<string, Monster> mobList = new Dictionary<string, Monster>();
            Console.SetWindowSize(100, 40);
            Console.CursorVisible = false;

            Map map = new Map();
            Player player = new Player();

            player.CurrentPos = map.PrintMap();

            while(true)
            {
                map.UpdatePlayerPos(player.CurrentPos, player.Move());
            }
        }
    }
}
