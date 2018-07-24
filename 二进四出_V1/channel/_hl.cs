using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 二进四出_V1.channel
{
    [Serializable]
    public struct PValueHL
    {
        public byte VT;//type
        public byte VS;//step
        public double VF;//freq
        public PointF Pt;
    }
    [Serializable]
    public class _hl : _coordinate
    {
        //均衡器参数值
        PValueHL[] PveHL = new PValueHL[2];         //高低通滤波器类型 ( 0 表示低通  ---  1 表示高通 )

        private static double MAXF = 20252;
        private static double MINF = 15;
        private static float MAXX = 0;
        private static float MINX = 0;

        public static double getMaxF()
        {
            return MAXF;
        }
        public static double getMinF()
        {
            return MINF;
        }
        int MAXT = 2;
        int MINT = 0;
        int MAXS = 2;
        int MINS = 0;

        public _hl()
        {
            //public const int Bessel = 0;
            //public const int Butterworth = 1;
            //public const int Linkwitz_Riley = 2;
            for (int i = 0; i < PveHL.Length; i++)
            {
                PveHL[i].VT = 1;//高通滤波器类型:巴特沃斯( = ?  0 1 2  分别表示 Bessel,Butterworth ,Linkwitz_Riley) ([?]  0 表示低通  ---  1 表示高通 )
                PveHL[i].VS = 2;//高通滤波器阶数:12      ( = ?  0 1 2 3 4 5 6 7 分别表示 0  6 12 18 24 30 36 42 48 )   ([?] 0 表示低通  ---  1 表示高通 )
                PveHL[i].VF = i * 1000;//低通初始截止频率：22000    高通初始截止频率：20                                 ( 0 表示低通  ---  1 表示高通 )
                //PveHL[i].Pt.X = i * 100;
                //PveHL[i].Pt.Y = 0;
            }
            PveHL[0].VT = 1;//高通滤波器类型:巴特沃斯( = ?  0 1 2  分别表示 Bessel,Butterworth ,Linkwitz_Riley) ([?]  0 表示低通  ---  1 表示高通 )
            PveHL[0].VS = 2;//高通滤波器阶数:12      ( = ?  0 1 2 3 4 5 6 7 分别表示 0  6 12 18 24 30 36 42 48 )   ([?] 0 表示低通  ---  1 表示高通 )
            PveHL[0].VF = 20000;//低通初始截止频率：22000    高通初始截止频率：20         


            PveHL[1].VT = 1;//高通滤波器类型:巴特沃斯( = ?  0 1 2  分别表示 Bessel,Butterworth ,Linkwitz_Riley) ([?]  0 表示低通  ---  1 表示高通 )
            PveHL[1].VS = 2;//高通滤波器阶数:12      ( = ?  0 1 2 3 4 5 6 7 分别表示 0  6 12 18 24 30 36 42 48 )   ([?] 0 表示低通  ---  1 表示高通 )
            PveHL[1].VF = 15.5;//低通初始截止频率：22000    高通初始截止频率：20         

            MAXX = f2x(MAXF);
            MINX = f2x(MINF);
            PveHL[0].Pt.X = f2x(MAXF);
            PveHL[1].Pt.X = f2x(MINF);

            PveHL[0].Pt.Y = g2y(0);
            PveHL[1].Pt.Y = g2y(0);
        }
        public PointF getPoint(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].Pt;
        }
        public double getF(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].VF;
        }
        public float getX(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].Pt.X;
        }
        public float getY(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].Pt.Y;
        }
        public byte getT(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].VT;// 0 1 2  分别表示 Bessel,Butterworth ,Linkwitz_Riley)
        }
        public byte getS(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return PveHL[num].VS;// 0 1 2 3 4 5 6 7 -> 0 6 12 18 24 30 36 42 48
        }
        public int setX(float x, int num) //( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            if (x < MINX || x > MAXX)
            {
                ret = -1;
                return ret;
            }
            PveHL[num].Pt.X = x;
            PveHL[num].VF = x2f(x);
            return ret;
        }
         public int setF(double freq, int num) //( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            if (freq < MINF || freq > MAXF)
            {
                ret = -1;
                return ret;
            }
            PveHL[num].VF = freq;
            PveHL[num].Pt.X = f2x(freq);
            return ret;
        }
        public int setT(byte type, int num)//TYPE
        {
            int ret = 0;
            if (type < MINT || type > MAXT)
            {
                ret = -1;
                return ret;
            }
            PveHL[num].VT = type;// 0 1 2  ->  Bessel,Butterworth ,Linkwitz_Riley
            return ret;
        }
        public int setS(byte step, int num)//STEP
        {
            int ret = 0;
            if (step < MINS || step > MAXS)
            {
                ret = -1;
                return ret;
            }
            //PveHL[num].VS = (byte)(step + 2);// 0 1 2 3 4 5 6 7 -> 0 6 12 18 24 30 36 42 48
            PveHL[num].VS = (byte)(step);// 0 1 2 3 4 5 6 7 -> 0 6 12 18 24 30 36 42 48
            return ret;
        }
    }
}
