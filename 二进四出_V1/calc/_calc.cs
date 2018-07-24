using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
 
namespace 二进四出_V1.calc
{
    [Serializable]
    public class _calc
    {

        //根据 gain freq q fs 获取 EQ 每个点调节的 5个系数
        [DllImport("calc.dll")]
        public static extern void getEqCft(double[] buf, double gain, double freq, double Q, double fs);
        //根据 gain freq q fs 获取 高价 每个点调节的 5个系数
        [DllImport("calc.dll")]
        public static extern void getHighShelfCft(double[] buf, double gain, double freq, double Q, double fs, double slope = 1.0);
        //根据 gain freq q fs 获取 低价 每个点调节的 5个系数
        [DllImport("calc.dll")]
        public static extern void getLowShelfCft(double[] buf, double gain, double freq, double Q, double fs, double slope = 1.0);


        ///**********************************************************************************************************/
        ///*****************************************    图像部分   **************************************************/
        ///**********************************************************************************************************/

        //均衡调节
        [DllImport("calc.dll")]
        public static extern void CalcPeak(double[] linegain, double[] linefreq, int linewidth, double gain, double freq, double Q, double fs);
        //高价调节
        [DllImport("calc.dll")]
        public static extern void CalcHighShelf(double[] linegain, double[] linefreq, int linewidth, double gain, double freq, double Q, double fs, double slope = 1.0);
        //低价调节
        [DllImport("calc.dll")]
        public static extern void CalcLowShelf(double[] linegain, double[] linefreq, int linewidth, double gain, double freq, double Q, double fs, double slope = 1.0);
        //贝塞尔 12dB调节
        [DllImport("calc.dll")]
        public static extern void Bessel_12dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //贝塞尔 18dB调节
        [DllImport("calc.dll")]
        public static extern void Bessel_18dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //贝塞尔 24dB调节
        [DllImport("calc.dll")]
        public static extern void Bessel_24dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //巴特沃斯 12dB调节
        [DllImport("calc.dll")]
        public static extern void Butterworth_12dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //巴特沃斯 18dB调节
        [DllImport("calc.dll")]
        public static extern void Butterworth_18dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //巴特沃斯 24dB调节
        [DllImport("calc.dll")]
        public static extern void Butterworth_24dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //林克维茨 12dB调节
        [DllImport("calc.dll")]
        public static extern void Linkwitz_Riley_12dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //林克维茨 24dB调节
        [DllImport("calc.dll")]
        public static extern void Linkwitz_Riley_24dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //林克维茨 36dB调节
        [DllImport("calc.dll")]
        public static extern void Linkwitz_Riley_36dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);
        //林克维茨 48dB调节
        [DllImport("calc.dll")]
        public static extern void Linkwitz_Riley_48dB(double[] linegain, double[] linefreq, int linewidth, int hl, double freq, double fs = 48000);



        /**********************************************************************************************************/
        /*****************************************    数据部分   **************************************************/
        /**********************************************************************************************************/
        //1阶 高低通
        [DllImport("calc.dll")]
        public static extern int bwt1st(double[] realData, int hl, double frequency, double g, double fs);

        [DllImport("calc.dll")]
        public static extern int GetBwt1st(double[] realData, int hl, double freq);
        //2阶 全通
        [DllImport("calc.dll")]
        public static extern int GetAllPass2st(double[] realData, double frequency, double g, double Fs, double Q);
        //2阶 贝塞尔
        [DllImport("calc.dll")]
        public static extern int Getbel2st(double[] realData, int LH, double frequency, double g, double Fs);
        //2阶 巴特沃斯
        [DllImport("calc.dll")]
        public static extern int Getbwt2st(double[] realData, int HL, double frequency, double g, double Fs);
        //2阶 巴特沃斯（高阶）
        [DllImport("calc.dll")]
        public static extern int GetbwtHigh2st(double[] realData, int HL, double frequency, double g, double Fs, double orderindex, double i);

        /// <summary>
        /// 两个1阶（2 * 3系数）组成1个2阶（5个系数）计算方法
        /// </summary>
        /// <param name="type1"></param> 第一个1阶类型（0:低通   1:高通  2:全通）
        /// <param name="type2"></param> 第二个1阶类型（0:低通   1:高通  2:全通）
        /// <param name="f1"></param>第一个截止频率
        /// <param name="f2"></param>第二个截止频率
        /// <param name="g"></param>截止增益（因为增益都为0，所以就没有区分两组）
        /// <param name="fs"></param>采样频率
        /// <returns></returns>返回合成的 5个系数数组
        [DllImport("calc.dll")]
        public static extern int GetCoefficientTwo1st(double[] realData, int type1, int type2, double f1, double f2, double g, double fs);


        /**********************************************************************************************************/
        /*****************************************    格式转换   **************************************************/
        /**********************************************************************************************************/

        //5.23格式 十进制转二进制
        [DllImport("calc.dll")]
        public static extern void Ten2Two_523(double n, byte[] realByte);
        //5.23格式 二进制转十进制
        [DllImport("calc.dll")]
        public static extern void Two2Ten_523(byte[] byt1, double[] real_num);



        public static void GetHLPassValue(double freq, int step, int type, byte HL)//0-L  1-H
        {
            double Cfree = freq;// user.channel[channelSet].HLPFValue[0];

            if (sgags.bypass == false)
            {
                if (HL == 1)//high
                {
                    switch (step)
                    {
                        case 0://12db
                            switch (type)
                            {
                                case 0: Bessel_12dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//bel
                                case 1: Butterworth_12dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw,  /*int hl*/1, freq, 48000); break;//bwt
                                case 2: Linkwitz_Riley_12dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw,  /*int hl*/1, freq, 48000); break;//lr
                                default: break;
                            }
                            break;
                        case 1://18db
                            switch (type)
                            {
                                case 0: Bessel_18dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//bel
                                case 1: Butterworth_18dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//bwt
                                default: break;
                            }
                            break;
                        case 2://24db
                            switch (type)
                            {
                                case 0: Bessel_24dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//bel
                                case 1: Butterworth_24dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//bwt
                                case 2: Linkwitz_Riley_24dB(sgags.HighDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/1, freq, 48000); break;//lr

                                default: break;
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    switch (step)
                    {
                        case 0://12db
                            switch (type)
                            {
                                case 0: Bessel_12dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//bel
                                case 1: Butterworth_12dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw,  /*int hl*/0, freq, 48000); break;//bwt
                                case 2: Linkwitz_Riley_12dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw,  /*int hl*/0, freq, 48000); break;//lr
                                default: break;
                            }
                            break;
                        case 1://18db
                            switch (type)
                            {
                                case 0: Bessel_18dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//bel
                                case 1: Butterworth_18dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//bwt
                                default: break;
                            }
                            break;
                        case 2://24db
                            switch (type)
                            {
                                case 0: Bessel_24dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//bel
                                case 1: Butterworth_24dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//bwt
                                case 2: Linkwitz_Riley_24dB(sgags.LowDbValue, sgags.wholeFreq, sgags.regondraw, /*int hl*/0, freq, 48000); break;//lr
                                default: break;
                            }
                            break;
                        default: break;
                    }
                }
            }
            else
            {
                if (HL == 0)
                {
                    for (int i = 0; i < sgags.calcWidth; i++)
                    {
                        sgags.LowDbValue[i] = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < sgags.calcWidth; i++)
                    {
                        sgags.HighDbValue[i] = 0;
                    }
                }
            }
        }




    }
}
