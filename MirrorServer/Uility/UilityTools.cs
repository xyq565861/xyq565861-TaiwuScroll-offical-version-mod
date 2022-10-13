using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace MirrorNet
{
     public class UilityTools
    {
        public static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();

        }
    }
}
