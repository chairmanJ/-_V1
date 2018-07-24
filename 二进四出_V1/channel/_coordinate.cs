using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 二进四出_V1.channel
{
    [Serializable]
    public class _coordinate
    {
        public const Double MaxF = 3.69897000433602;

        public static float f2x(double f)
        {
            return (float)((Math.Log(f) / Math.Log(10) - 1) / MaxF * (sgags.calcWidth) + sgags.curveLeft);
        }
        public static float g2y(double g)//( 15 - g ) * 6 + 20
        {
            return (float)((15 - g) * 6 + 20);
        }
        public static double x2f(float x)
        {
            return (double)(Math.Pow(10, ((x - sgags.curveLeft) * MaxF / (sgags.calcWidth) + 1)));
        }
        public static double y2g(float y)
        {
            return (double)(15 - (y - 20) / 6);//15 - (y-20)/30 * 5
        }
    }
}
