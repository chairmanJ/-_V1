using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace 二进四出_V1.channel
{
    /// <summary>
    /// 增益
    /// </summary>
    [Serializable]
    public class _gain
    {
        double gain = 0;

        static double MAXGAIN = 50;
        static double MINGAIN = -600;
        public static double getMaxGain()
        {
            return MAXGAIN;
        }
        public static double getMinGain()
        {
            return MINGAIN;
        }
        public double getGain()
        {
            return this.gain;
        }
        public int setGain(double gain)
        {
            if (gain < MINGAIN || gain > MAXGAIN)
            {
                return -1;
            }
            this.gain = gain;
            return 0;
        }
        public _gain()
        {
            gain = 0.0;
        }
    }
}
