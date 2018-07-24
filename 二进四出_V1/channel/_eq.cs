using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 二进四出_V1.channel
{
    /// <summary>
    /// 均衡点
    /// </summary>
    [Serializable]
    public struct PValue
    {
        public double VF;//频率
        public double VG;//增益
        public double VQ;//Q值
        public byte VT;//点的类型（0：PEQ --- 1：LS --- 2：HS）
        public PointF Pt;
        //public PointType TypeP;//点类型
    }
    /// <summary>
    /// 均衡器+高低通
    /// </summary>
    [Serializable]
    public class _eq : _coordinate
    {
        private PValue[] Pve = new PValue[6];         //6个点的参数数值数组  point-value
        //public PointF[] Pce = new PointF[15];         //6个点的坐标值        point-coordinate 
        static double MAXF = 0;
        static double MINF = 0;
        static double MAXG = 15;
        static double MING = -30;
        static double MAXQ = 50;
        static double MINQ = 1;
        static float MAXX = 0;
        static float MINX = 0;
        static float MAXY = 0;
        static float MINY = 0;
        static int MINT = 0;
        static int MAXT = 2;//只有三项

        public static double getMaxF()
        {
            return MAXF;
        }
        public static double getMinF()
        {
            return MINF;
        }
        public static double getMaxG()
        {
            return MAXG;
        }
        public static double getMinG()
        {
            return MING;
        }
        public static double getMaxQ()
        {
            return MAXQ;
        }
        public static double getMinQ()
        {
            return MINQ;
        }

        public static float getMaxX()
        {
            return MAXX;
        }
        public static float getMinX()
        {
            return MINX;
        }
        public static float getMaxY()
        {
            return MAXY;
        }
        public static float getMinY()
        {
            return MINY;
        }
        double[] freqFirst = new double[] { 50, 100, 500, 1000, 2000, 5000 };

        public _eq()
        {
            for (int i = 0; i < Pve.Length; i++)
            {
                Pve[i].VF = freqFirst[i];
                Pve[i].VQ = 1;
                Pve[i].VG = 0;
                Pve[i].VT = 0;//（TypeP   0：PEQ-- - 1：LS-- - 2：HS）
                Pve[i].Pt.X = f2x(Pve[i].VF);
                Pve[i].Pt.Y= g2y(0);
            }
            MAXF = 20000;
            MINF = 15;
            MAXG = 15;
            MING = -30;

            MAXX = f2x(MAXF);
            MINX = f2x(MINF);
            MAXY = g2y(MING);
            MINY = g2y(MAXG);
        }

        /*********************** get ************************/
        public double getF(int num)
        {
            return Pve[num].VF;
        }
        public double getG(int num)
        {
            return Pve[num].VG;
        }
        public double getQ(int num)
        {
            return Pve[num].VQ;
        }
        public byte getT(int num)
        {
            return Pve[num].VT;
        }
        public PointF getPointF(int num)
        {
            return new PointF(Pve[num].Pt.X, Pve[num].Pt.Y);
        }
        public int setPointF(PointF pt, int num)
        {
            int ret1 = 0, ret2 = 0;

            ret1 = setX(pt.X, num);
            ret2 = setY(pt.Y, num);

            if (ret1 != 0 || ret2 != 0)
            {
                return -1;
            }
            return 0;
        }
        /*********************** add - sub ************************/
        public int addX(int num)
        {
            int ret = 0;
            float x_coor = Pve[num].Pt.X;
            x_coor += 1;
            ret = setX(x_coor, num);
            return ret;
        }
        public int subX(int num)
        {
            int ret = 0;
            float x_coor = Pve[num].Pt.X;
            x_coor -= 1;
            ret = setX(x_coor, num);
            return ret;
        }
        public int addY(int num)
        {
            int ret = 0;
            float y_coor = Pve[num].Pt.Y;
            y_coor -= 1;
            ret = setY(y_coor, num);
            return ret;
        }
        public int subY(int num)
        {
            int ret = 0;
            float y_coor = Pve[num].Pt.Y;
            y_coor += 1;
            ret = setY(y_coor, num);
            return ret;
        }

        public int addQ(int num)
        {
            int ret = 0;
            double qValue = Pve[num].VQ;
            qValue += 1;
            ret = setQ(qValue, num);
            return ret;
        }
        public int subQ(int num)
        {
            int ret = 0;
            double qValue = Pve[num].VQ;
            qValue -= 1;
            ret = setQ(qValue, num);
            return ret;
        }

        /*********************** set ************************/
        public int setF(double freq, int num)
        {
            int ret = 0;
            if (freq < MINF || freq > MAXF)
            {
                ret = -1;
                return ret;
            }
            Pve[num].VF = freq;
            Pve[num].Pt.X = f2x(freq);
            return ret;
        }
        public int setG(double gain, int num)
        {
            int ret = 0;
            if (gain < MING || gain > MAXG)
            {
                ret = -1;
                return ret;
            }
            Pve[num].VG = gain;
            Pve[num].Pt.Y = g2y(gain);
            return ret;
        }
        public int setX(float x, int num)
        {
            int ret = 0;
            if (x < MINX || x > MAXX)
            {
                ret = -1;
                return ret;
            }
            Pve[num].Pt.X = x;
            Pve[num].VF = x2f(x);
            return ret;
        }
        public int setY(float y, int num)
        {
            int ret = 0;
            if (y < MINY || y > MAXY)
            {
                ret = -1;
                return ret;
            }
            Pve[num].Pt.Y = y;
            Pve[num].VG = y2g(y);
            return ret;
        }
        public int setFG(double freq, double gain, int num)
        {
            int ret1 = 0, ret2 = 0;
            ret1 = setF(freq, num);
            ret2 = setG(gain, num);

            if (ret1 != 0 || ret2 != 0)
            {
                return -1;
            }
            return 0;
        }
        public int setXY(float x, float y, int num)
        {
            int ret1 = 0, ret2 = 0;

            ret1 = setX(x, num);
            ret2 = setY(y, num);

            if (ret1 != 0 || ret2 != 0)
            {
                return -1;
            }
            return 0;
        }
        public int setQ(double q, int num)
        {
            int ret = 0;
            if (q < MINQ || q > MAXQ)
            {
                ret = -1;
                return ret;
            }
            Pve[num].VQ = q;
            return ret;
        }

        public int setT(byte type, int num)
        {
            int ret = 0;
            if (type < MINT || type > MAXT)
            {
                ret = -1;
                return ret;
            }
            Pve[num].VT = type;
            return ret;
        }
    }
}
