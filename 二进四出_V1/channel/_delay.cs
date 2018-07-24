using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 二进四出_V1.channel
{
    [Serializable]
    public class _delay
    {
        double delay = 0;
        static double MAXDELAY = 800;  
        static double MINDELAY = 0; 
        public static double getMaxDelay()
        {
            return MAXDELAY;
        }
        public static double getMinDelay()
        {
            return MINDELAY;
        }
        public double getDealy()
        {
            return this.delay;
        }
        public int setDelay(double delay)
        {
            if (delay < MINDELAY || delay > MAXDELAY)
            {
                return -1;
            }
            this.delay = delay;
            return 0;
        }
        public _delay()
        {
            this.delay = 0.00;
        }

    }
}
