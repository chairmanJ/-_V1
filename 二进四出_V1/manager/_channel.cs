using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 二进四出_V1.channel;
using System.Drawing;
using 二进四出_V1.comm;
using 二进四出_V1.calc;
using 二进四出_V1.manager;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using 二进四出_v1.channel;
//using 二进四出_V1.channel;

namespace 二进四出_V1.manager
{
    /// <summary>
    /// 通道父类
    /// </summary>
    [Serializable]
    public class _channel
    {
        public static string start = "5A00A5FF";
        public static string end = "A5FF005A";

        //private const int FLAG_MUTE = 0X00;
        public const int FLAG_MUTE = 0X00;
        //private const int FLAG_CHASE = 0X01;
        public const int FLAG_CHASE = 0X01;
        //private const int FLAG_DELAY = 0X02;
        public const int FLAG_DELAY = 0X02;
        //private const int FLAG_GAIN = 0X04;
        public const int FLAG_GAIN = 0X04;
        //private const int FLAG_EQ = 0X08;
        public const int FLAG_EQ = 0X08;
        //private const int FLAG_HL = 0X10;
        public const int FLAG_HL = 0X10;
        //private const int FLAG_LIMIT_T = 0X20;
        public const int FLAG_LIMIT_T = 0X20;
        //private const int FLAG_LIMIT_S = 0X21;
        public const int FLAG_LIMIT_S = 0X21;
        //private const int FLAG_LIMIT_R = 0X22;
        public const int FLAG_LIMIT_R = 0X22;

        public const int FLAG_ISCHECKMUTE = 0X25;

        private _emute mute = new _emute();
        private _ephase phase = new _ephase();
        private _gain gain = new _gain();
        private _delay delay = new _delay();
        private _eq eq = new _eq();
        private _hl hl = new _hl();


        //private _muteState muteState=new _muteState();
        //private _limit limit = new _limit();

        //public  bool btnChMute = true;//指示cbxInOut是否可用


        //MUTE
        public _mute getMute()
        {
            return this.mute.getMute();
        }
        public void setMute(_mute mute)
        {
            int ret = 0;
            ret = this.mute.setMute(mute);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_MUTE, mute);
        }
        public void setMute(_mute mute, int nosend)
        {
            int ret = 0;
            ret = this.mute.setMute(mute);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_MUTE, mute);
        }
        //CHASE
        public _phase getPhase()
        {
            return this.phase.getPhase();
        }
        public void setPhase(_phase chase)
        {
            int ret = 0;
            ret = this.phase.setPhase(chase);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_CHASE, chase);
        }
        public void setPhase(_phase chase, int nosend)
        {
            int ret = 0;
            ret = this.phase.setPhase(chase);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_CHASE, chase);
        }
        //DELAY
        public double getDealy()
        {
            return this.delay.getDealy();
        }
        public void setDelay(double delay)
        {
            int ret = 0;
            ret = this.delay.setDelay(delay);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_DELAY, delay);
        }
        public void setDelay(double delay, int nosend)
        {
            int ret = 0;
            ret = this.delay.setDelay(delay);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_DELAY, delay);
        }
        //GAIN
        public double getGain()
        {
            return this.gain.getGain();
        }
        public void setGain(double gain)
        {
            int ret = 0;
            ret = this.gain.setGain(gain);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_GAIN, gain);
        }
        public void setGain(double gain, int nosend)
        {
            int ret = 0;
            ret = this.gain.setGain(gain);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_GAIN, gain);
        }
        //HL
        public void setHL_F(double freq, int num)//( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            ret = this.hl.setF(freq, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_HL, num);
        }
        public void setHL_F(double freq, int num, int nosend)//( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            ret = this.hl.setF(freq, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_HL, num);
        }
        public void setHL_X(float x, int num)//( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            ret = this.hl.setX(x, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_HL, num);
        }
        public void setHL_X(float x, int num, int nosend)//( 0 表示低通  ---  1 表示高通 )
        {
            int ret = 0;
            ret = this.hl.setX(x, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_HL, num);
        }
        public void setHL_S(byte setp, int num)
        {
            if (_dev.user[sgags.usernum].ch[sgags.channel].getHL_T(num) == 2 && setp == 1)
            {
                return;
            }
            int ret = 0;
            ret = this.hl.setS(setp, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_HL, num);
        }
        public void setHL_S(byte setp, int num, int nosend)
        {
            if (_dev.user[sgags.usernum].ch[sgags.channel].getHL_T(num) == 2 && setp == 1)
            {
                return;
            }
            int ret = 0;
            ret = this.hl.setS(setp, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_HL, num);
        }
        public void setHL_T(byte type, int num)
        {
            if (_dev.user[sgags.usernum].ch[sgags.channel].getHL_S(num) == 3 && type == 2)
            {
                return;
            }

            int ret = 0;
            ret = this.hl.setT(type, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_HL, num);
        }
        public void setHL_T(byte type, int num, int nosend)
        {
            if (_dev.user[sgags.usernum].ch[sgags.channel].getHL_S(num) == 3 && type == 2)
            {
                return;
            }

            int ret = 0;
            ret = this.hl.setT(type, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_HL, num);
        }
        public double getHL_F(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return this.hl.getF(num);
        }
        public float getHL_X(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return this.hl.getX(num);
        }
        public float getHL_Y(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return this.hl.getY(num);
        }
        public PointF getHL_PointF(int num)//( 0 表示低通  ---  1 表示高通 )
        {
            return this.hl.getPoint(num);
        }
        public byte getHL_S(int num)
        {
            return this.hl.getS(num);
        }
        public byte getHL_T(int num)
        {
            return this.hl.getT(num);
        }
        //EQ
        public void setEQ_F(double freq, int num)
        {
            int ret = 0;
            ret = this.eq.setF(freq, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_F(double freq, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setF(freq, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_G(double gain, int num)
        {
            int ret = 0;
            ret = this.eq.setG(gain, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_G(double gain, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setG(gain, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_X(float x, int num)
        {
            int ret = 0;
            ret = this.eq.setX(x, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_X(float x, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setX(x, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_Y(float y, int num)
        {
            int ret = 0;
            ret = this.eq.setY(y, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_Y(float y, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setY(y, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_FG(double freq, double gain, int num)
        {
            int ret = 0;
            ret = this.eq.setFG(freq, gain, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_FG(double freq, double gain, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setFG(freq, gain, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_XY(float x, float y, int num)
        {
            int ret = 0;
            ret = this.eq.setXY(x, y, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_XY(float x, float y, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setXY(x, y, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_Q(double q, int num)
        {
            int ret = 0;
            ret = this.eq.setQ(q, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_Q(double q, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setQ(q, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_T(byte type, int num)
        {
            int ret = 0;
            ret = this.eq.setT(type, num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void setEQ_T(byte type, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setT(type, num);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_EQ, num);
        }
        public double getEQ_F(int num)
        {
            return this.eq.getF(num);
        }
        public double getEQ_G(int num)
        {
            return this.eq.getG(num);
        }
        public double getEQ_Q(int num)
        {
            return this.eq.getQ(num);
        }
        public byte getEQ_T(int num)
        {
            return this.eq.getT(num);
        }
        public PointF getEQ_PointF(int num)
        {
            return this.eq.getPointF(num);
        }
        public int setEQ_PointF(PointF pt, int num)
        {
            int ret = 0;
            ret = this.eq.setPointF(pt, num);
            if (ret != 0)
            {
                return -1;
            }
            UpdateCurve(FLAG_EQ, num);
            return 0;
        }
        public int setEQ_PointF(PointF pt, int num, int nosend)
        {
            int ret = 0;
            ret = this.eq.setPointF(pt, num);
            if (ret != 0)
            {
                return -1;
            }
            //UpdateCurve(FLAG_EQ, num);
            return 0;
        }
        public void addX(int num)
        {
            int ret = 0;
            ret = this.eq.addX(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void subX(int num)
        {
            int ret = 0;
            ret = this.eq.subX(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void addY(int num)
        {
            int ret = 0;
            ret = this.eq.addY(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void subY(int num)
        {
            int ret = 0;
            ret = this.eq.subY(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void addQ(int num)
        {
            int ret = 0;
            ret = this.eq.addQ(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }
        public void subQ(int num)
        {
            int ret = 0;
            ret = this.eq.subQ(num);
            if (ret != 0)
            {
                return;
            }
            UpdateCurve(FLAG_EQ, num);
        }

        //LIMIT       
        //public int getLT_Re()
        //{
        //    return this.limit.getRe();
        //}
        //public int getLT_Th()
        //{
        //    return this.limit.getTh();
        //}
        //public int getLT_St()
        //{
        //    return this.limit.getSt();
        //}
        //public void setLT_Th(int threshold)
        //{
        //    int ret = 0;
        //    ret = this.limit.setTh(threshold);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    UpdateCurve(FLAG_LIMIT_T, threshold);
        //}
        //public void setLT_Th(int threshold, int nosend)
        //{
        //    int ret = 0;
        //    ret = this.limit.setTh(threshold);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    //UpdateCurve(FLAG_LIMIT_T, threshold);
        //}
        //public void setLT_St(int startTime)
        //{
        //    int ret = 0;
        //    ret = this.limit.setSt(startTime);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    UpdateCurve(FLAG_LIMIT_S, startTime);
        //}
        //public void setLT_St(int startTime, int nosend)
        //{
        //    int ret = 0;
        //    ret = this.limit.setSt(startTime);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    //UpdateCurve(FLAG_LIMIT_S, startTime);
        //}
        //public void setLT_Re(int releaseTime)
        //{
        //    int ret = 0;
        //    ret = this.limit.setRe(releaseTime);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    UpdateCurve(FLAG_LIMIT_R, releaseTime);
        //}
        //public void setLT_Re(int releaseTime, int nosend)
        //{
        //    int ret = 0;
        //    ret = this.limit.setRe(releaseTime);
        //    if (ret != 0)
        //    {
        //        return;
        //    }
        //    //UpdateCurve(FLAG_LIMIT_R, releaseTime);
        //}
        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        public  static void UpdateCurve(int FLAG, object obj)
        {
            switch (FLAG)
            {

                case FLAG_ISCHECKMUTE:
                    SendCheckMute(sgags.devcbx, sgags.usernum, sgags.switchChannel);
                    break;

                case FLAG_MUTE:

                    //if(Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].mute = DeepCopyByBin<_emute>(_dev.user[Form1.usernum].ch[2].mute);
                    //    SendMute_btl((_mute)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);/////////////////////////////////////
                    //}
                    //else
                    //{
                    //SendMute((_mute)obj, Form1.devcbx, Form1.usernum, Form1.channel);
                    SendMute_2((_mute)obj, sgags.devcbx, sgags.usernum, sgags.switchChannel);
                    //}
                    break;
                case FLAG_CHASE:
                   
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].phase = DeepCopyByBin<_ephase>(_dev.user[Form1.usernum].ch[2].phase);
                    //    SendPhase_btl((_phase)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        SendPhase((_phase)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}


                    break;
                case FLAG_DELAY:
                   
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].delay = DeepCopyByBin<_delay>(_dev.user[Form1.usernum].ch[2].delay);
                    //    SendDelay_btl((double)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        SendDelay((double)obj, sgags.devcbx, sgags.usernum, sgags.channel);

                    //}
                    break;
                case FLAG_GAIN:
                 
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //   // Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].gain = DeepCopyByBin<_gain>(_dev.user[Form1.usernum].ch[2].gain);
                    //    SendGain_btl((double)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        SendGain((double)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;
                case FLAG_EQ:
                   
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //   // Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].eq = DeepCopyByBin<_eq>(_dev.user[Form1.usernum].ch[2].eq);
                    //    SendEq_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        SendEq((int)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;
                case FLAG_HL:
                   
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].hl = DeepCopyByBin<_hl>(_dev.user[Form1.usernum].ch[2].hl);
                    //    SendHl_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        SendHl((int)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;
                case FLAG_LIMIT_T:
                   
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
                    //    SendLimit_T_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                        _user.SendLimit_T((int)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;
                case FLAG_LIMIT_S:
                  
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //    //Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
                    //    SendLimit_S_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                      _user.SendLimit_S((int)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;
                case FLAG_LIMIT_R:
                  
                    //if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
                    //{
                    //   // Thread.Sleep(Form1.delay_tbr);
                    //    _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
                    //    SendLimit_R_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
                    //}
                    //else
                    //{
                       _user.SendLimit_R((int)obj, sgags.devcbx, sgags.usernum, sgags.channel);
                    //}
                    break;

                default: break;
            }
        }

//#if false
//        private void UpdateCurve(int FLAG, object obj)
//        {
//            switch (FLAG)
//            {
//                case FLAG_MUTE:
//                    SendMute((_mute)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].mute = DeepCopyByBin<_emute>(_dev.user[Form1.usernum].ch[2].mute);
//                        SendMute_btl((_mute)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);/////////////////////////////////////
//                    }
//                    break;
//                case FLAG_CHASE:
//                    SendPhase((_phase)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].phase = DeepCopyByBin<_ephase>(_dev.user[Form1.usernum].ch[2].phase);
//                        SendPhase_btl((_phase)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }
//                    break;
//                case FLAG_DELAY:
//                    SendDelay((double)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].delay = DeepCopyByBin<_delay>(_dev.user[Form1.usernum].ch[2].delay);
//                        SendDelay_btl((double)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_GAIN:
//                    SendGain((double)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        // Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].gain = DeepCopyByBin<_gain>(_dev.user[Form1.usernum].ch[2].gain);
//                        SendGain_btl((double)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_EQ:
//                    SendEq((int)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        // Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].eq = DeepCopyByBin<_eq>(_dev.user[Form1.usernum].ch[2].eq);
//                        SendEq_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_HL:
//                    SendHl((int)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].hl = DeepCopyByBin<_hl>(_dev.user[Form1.usernum].ch[2].hl);
//                        SendHl_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_LIMIT_T:
//                    SendLimit_T((int)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
//                        SendLimit_T_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_LIMIT_S:
//                    SendLimit_S((int)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        //Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
//                        SendLimit_S_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;
//                case FLAG_LIMIT_R:
//                    SendLimit_R((int)obj, Form1.devcbx, Form1.usernum, Form1.channel);
//                    if (Form1.channel == 2 && _dev.user[Form1.usernum].getBtl() == true)
//                    {
//                        // Thread.Sleep(Form1.delay_tbr);
//                        _dev.user[Form1.usernum].ch[3].limit = DeepCopyByBin<_limit>(_dev.user[Form1.usernum].ch[2].limit);
//                        SendLimit_R_btl((int)obj, Form1.devcbx, Form1.usernum, 3/*low channel*/);
//                    }

//                    break;

//                default: break;
//            }
//        }
//#endif

        static void SendMute_2(_mute mute,int dev,int user,int switchChannel)
        {
            string sdata = getSwitchMuteData(dev, user, switchChannel);
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";
            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);

        }


        public static void SendCheckMute(int dev, int user, int switchChannel)
        {
            string sdata = getSwitchMuteData(dev, user, switchChannel);
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";
            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);

        }
        public static void SendCheckMuteData(int dev, int user, int switchChannel)
        {
            string sdata = getSwitchMuteData(dev, user, switchChannel);
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";
            string order = start + sdev + suser + scmd + slength + sdata + end;
        }

        //static void SendMute(_mute mute, int dev, int user, int ch)
        // {
        //     string sdata = getMuteData(dev, user, ch);
        //     //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //     string sdev = dev.ToString("X2");
        //     string suser = user.ToString("X2");
        //     string scmd = "01";
        //     //string slength = "000C";//12 -> HEX,8字节数据+4字节地址
        //     //string slength = "000C";//12->HEX,4字节数据+8字节地址
        //     string slength = "0006";//占用一个空间长度

        //     string order = start + sdev + suser + scmd + slength + sdata + end;
        //     _facty.send(order);
        // }

        //static void SendMute_2(_mute mute,int dev,int user,int switchChannel)
        //{
        //    string sdata = getSwitchMuteData(dev, user, switchChannel);
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    string slength = "0006";
        //    string order = start + sdev + suser + scmd + slength + sdata + end;
        //    _facty.send(order);
        //}
#if true
        //void SendMute_btl(_mute mute, int dev, int user, int ch)
        //{
        //    string sdata1 = getMuteData(dev, user, 2);
        //    string sdata2 = getMuteData(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    string h = ((byte)((sdata1.Length ) / 256)).ToString("X2");
        //    string l = ((byte)((sdata1.Length ) % 256)).ToString("X2");
        //    string slength = h + l; //"000C";//12 -> HEX

        //    string order = start + sdev + suser + scmd + slength + sdata1 + sdata2 + end;
        //    _facty.send(order);
        //}
#else
             
        void SendMute_btl(_mute mute, int dev, int user, int ch)
        {
            string sdata1 = getMuteData(dev, user, 2);
            string sdata2 = getMuteData(dev, user, 3);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string h = ((sdata1.Length * 2) / 256).ToString("X2");
            string l = ((sdata1.Length * 2) % 256).ToString("X2");
            string slength = h + l; //"000C";//12 -> HEX

            string order = start + sdev + suser + scmd + slength + sdata1 + sdata2 + end;
            _facty.send(order);
        }
#endif
        static void SendPhase(_phase phase, int dev, int user, int ch)
        {
            string sdata = getPhaseData_2(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";//6 -> HEX

            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);
        }
        //static void SendMuteState(_checkMuteState muteState,int dev,int user,int ch)
        //{
            
        //}

        //void SendPhase_btl(_phase phase, int dev, int user, int ch)
        //{
        //    string sdata1 = getPhaseData(dev, user, 2);
        //    string sdata2 = getPhaseData(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    //string slength = "0006";//6 -> HEX

        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;
        //    string order = start + sdev + suser + scmd + slength + sdata1 + sdata2 + end;

        //    _facty.send(order);
        //}
        static void SendDelay(double delay, int dev, int user, int ch)
        {
            string sdata = getDealyData(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";//6 -> HEX

            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);
        }
        //void SendDelay_btl(double delay, int dev, int user, int ch)
        //{
        //    string sdata1 = getDealyData(dev, user, 2);
        //    string sdata2 = getDealyData(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    //string slength = "0006";//6 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;

        //    string order = start + sdev + suser + scmd + slength + sdata1 + sdata2 + end;
        //    _facty.send(order);
        //}
        static void SendGain(double gain, int dev, int user, int ch)
        {
            string sdata = getGainData(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";//6 -> HEX

            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);
        }
        //void SendGain_btl(double gain, int dev, int user, int ch)
        //{
        //    string sdata1 = getGainData(dev, user, 2);
        //    string sdata2 = getGainData(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //   // string slength = "0006";//6 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;
        //    string order = start + sdev + suser + scmd + slength + sdata1+ sdata2 + end;
        //    _facty.send(order);
        //}


        //static void SendLimit_T(int thr, int dev, int user, int ch)
        //{
        //    string sdata = getLimit_T_Data(dev, user, ch);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    string slength = "0030";//6 -> HEX

        //    string order = start + sdev + suser + scmd + slength + sdata + end;
        //    _facty.send(order);
        //}


        //void SendLimit_T_btl(int thr, int dev, int user, int ch)
        //{
        //    string sdata1 = getLimit_T_Data(dev, user, 2);
        //    string sdata2 = getLimit_T_Data(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //   // string slength = "0030";//6 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;
        //    string order = start + sdev + suser + scmd + slength + sdata1+ sdata2 + end;
        //    _facty.send(order);
        //}

        //static void SendLimit_S(int str, int dev, int user, int ch)
        //{
        //    string sdata = getLimit_S_Data(dev, user, ch);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    string slength = "0006";//6 -> HEX

        //    string order = start + sdev + suser + scmd + slength + sdata + end;
        //    _facty.send(order);
        //}

        //void SendLimit_S_btl(int str, int dev, int user, int ch)
        //{
        //    string sdata1 = getLimit_S_Data(dev, user, 2);
        //    string sdata2 = getLimit_S_Data(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    //string slength = "0006";//6 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;

        //    string order = start + sdev + suser + scmd + slength + sdata1+ sdata2 + end;
        //    _facty.send(order);
        //}

        //static void SendLimit_R(int rel, int dev, int user, int ch)
        //{
        //    string sdata = getLimit_R_Data(dev, user, ch);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    string slength = "0006";//6 -> HEX

        //    string order = start + sdev + suser + scmd + slength + sdata + end;
        //    _facty.send(order);
        //}

        //void SendLimit_R_btl(int rel, int dev, int user, int ch)
        //{
        //    string sdata1 = getLimit_R_Data(dev, user, 2);
        //    string sdata2 = getLimit_R_Data(dev, user, 3);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    //string slength = "0006";//6 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;

        //    string order = start + sdev + suser + scmd + slength + sdata1+sdata2 + end;
        //    _facty.send(order);
        //}
        public static void SendEq(int pt, int dev, int user, int ch)
        {
            string sdata = getEquData(dev, user, ch, pt);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "001E";//30 -> HEX

            string order = start + sdev + suser + scmd + slength + sdata + end;
            _facty.send(order);
            if (sgags.odbflag == true) { Thread.Sleep(20); }
            //Console.WriteLine("single:\r\n"+order + "\r\n");
        }
        //public void SendEq_btl(int pt, int dev, int user, int ch)
        //{
        //    string sdata1 = getEquData(dev, user, 2, pt);
        //    string sdata2 = getEquData(dev, user, 3, pt);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = user.ToString("X2");
        //    string scmd = "01";
        //    //string slength = "001E";//30 -> HEX
        //    string h = ((sdata1.Length ) / 256).ToString("X2");
        //    string l = ((sdata1.Length ) % 256).ToString("X2");
        //    string slength = h + l;

        //    string order = start + sdev + suser + scmd + slength + sdata1+ sdata2 + end;
        //    _facty.send(order);
        //    if (Form1.odbflag == true) { Thread.Sleep(20); }
        //    //Console.WriteLine("single:\r\n"+order + "\r\n");
        //}
        static public void SendHl(int pt, int dev, int user, int ch)
        {
            //string[] sdata = getHlData(Form1.devcbx, Form1.usernum, Form1.channel, pt);
            string sdata1 = "";
            string sdata2 = "";

            getHlData(dev, user, ch, pt, out sdata1, out sdata2);

            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = sgags.usernum.ToString("X2");
            string scmd = "01";
            string slength = "003C";//30 -> HEX

            string order1 = start + sdev + suser + scmd + slength + sdata1+sdata2 + end;
            //string order2 = start + sdev + suser + scmd + slength + sdata2 + end;

            _facty.send(order1);
            if (sgags.odbflag == true) { Thread.Sleep(200); }
            //_facty.send(order2);
            //if (Form1.odbflag == true) { Thread.Sleep(200); }
        }
        //public void SendHl_btl(int pt, int dev, int user, int ch)
        //{
        //    //string[] sdata = getHlData(Form1.devcbx, Form1.usernum, Form1.channel, pt);
        //    string sdata1 = "";
        //    string sdata2 = "";

        //    string sdata3 = "";
        //    string sdata4 = "";
        //    getHlData(dev, user, 2, pt, out sdata1, out sdata2);
        //    getHlData(dev, user, 3, pt, out sdata3, out sdata4);
        //    //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
        //    string sdev = dev.ToString("X2");
        //    string suser = Form1.usernum.ToString("X2");
        //    string scmd = "01";
        /*
        //string slength = "001E";//30 -> HEX
        string h = (((sdata1 + sdata2).Length ) / 256).ToString("X2");
        string l = (((sdata1 + sdata2).Length ) % 256).ToString("X2");
        string slength = h + l;


        string order1 = start + sdev + suser + scmd + slength + sdata1 + end;
        string order2 = start + sdev + suser + scmd + slength + sdata2 + end;

        string order3 = start + sdev + suser + scmd + slength + sdata3 + end;
        string order4 = start + sdev + suser + scmd + slength + sdata4 + end;

        _facty.send_btl(order1);
        if (Form1.odbflag == true) { Thread.Sleep(200); }
        _facty.send_btl(order2);
        if (Form1.odbflag == true) { Thread.Sleep(200); }

        _facty.send_btl(order3);
        if (Form1.odbflag == true) { Thread.Sleep(200); }
        _facty.send_btl(order4);
        if (Form1.odbflag == true) { Thread.Sleep(200); }
*/

        //    string h = (((sdata1 + sdata2   ).Length) / 256).ToString("X2");
        //    string l = (((sdata1 + sdata2   ).Length) % 256).ToString("X2");
        //    string slength = h + l;
        //    string order1 = start + sdev + suser + scmd + slength + sdata1 + sdata2 + sdata3 + sdata4 + end;
        //    _facty.send(order1);
        //    if (Form1.odbflag == true) { Thread.Sleep(200); }
        //}

        //获取静音状态，1---静音，0----未静音
        //public byte[] getGuiMuteData()
        //{
        //    byte[] mute = new byte[1];
        //    //for(int i=0,j=0;i<8;i++,j++)
        //    //{
        //    //    mute[i] = (byte)((_dev.user[Form1.usernum].ch[j].getMute() == _mute.Mute) ? 1 : 0);

        //    //}
        //    //for(int j=0;j<6;j++)
        //    //{
        //    //    for(int i = 0; i < 8; i++)
        //    //    {

        //    //    }
        //    //}

        //   if()
        //    return mute;

        //    //mute[0]=(byte)((_dev.user[Form1.usernum].ch[]))
        //}


        //public byte[] getGuiMuteData()
        //{
        //    byte[] mute = new byte[8];
        //    if(_dev.user[Form1.usernum].ch[0].getMute()==_mute.Mute)
        //    {
        //        if(_dev.user[Form1.usernum].ch[2].getMute()==_mute.Mute)
        //        {
        //            mute[0] = 1;
        //        }
        //        else if(_dev.user[Form1.usernum].ch[3].getMute()==_mute.Mute)
        //        {
        //            mute[1] = 1;
        //        }
        //        else if(_dev.user[Form1.usernum].ch[4].getMute()==_mute.Mute)
        //        {
        //            mute[2] = 1;
        //        }
        //        else if (_dev.user[Form1.usernum].ch[5].getMute() == _mute.Mute)
        //        {
        //            mute[3] = 1;
        //        }
        //        else
        //        {
        //            mute[0] = 0;
        //            mute[1] = 0;
        //            mute[2] = 0;
        //            mute[3] = 0;
        //        }

        //    }
        //    else if(_dev.user[Form1.usernum].ch[1].getMute()==_mute.Mute)
        //    {
        //        if(_dev.user[Form1.usernum].ch[2].getMute()==_mute.Mute)
        //        {
        //            mute[4] = 1;
        //        }
        //        else if (_dev.user[Form1.usernum].ch[3].getMute() == _mute.Mute)
        //        {
        //            mute[5] = 1;
        //        }
        //        else if (_dev.user[Form1.usernum].ch[4].getMute() == _mute.Mute)
        //        {
        //            mute[6] = 1;
        //        }
        //        else if (_dev.user[Form1.usernum].ch[5].getMute() == _mute.Mute)
        //        {
        //            mute[7] = 1;
        //        }
        //        else
        //        {
        //            mute[4] = 0;
        //            mute[5] = 0;
        //            mute[6] = 0;
        //            mute[7] = 0;
        //        }
        //    }
        //    else
        //    {
        //    }
        //    return mute;
        //}



        public static int nor = 0xff;
        //下发
        /// <summary>
        /// getAllData()不包括EQ数据
        /// </summary>
        /// <returns></returns>
        public string getAllData()
        {
            int i = 0;

            string mute = "";

            string limit = "";
            string start = "";
            string relea = "";

            string gain = "";
            string delay = "";
            string phase = "";

            //下位机空间不够，要发两次数据，所以把eq数据提取出来，单独发送一次
            //string equ = "";

            //
            string hl = "";
            //string lowmend = "";


            string hl1 = "";
            string hl2 = "";

            //无lowmend低音增强
            //string lowmend = "";

            
            limit +=_user.getLimit_T_Data(sgags.devcbx, sgags.usernum, i);
            start +=_user.getLimit_S_Data(sgags.devcbx, sgags.usernum, i);
            relea +=_user.getLimit_R_Data(sgags.devcbx, sgags.usernum, i);

            //for (i = 0; i < 4; i++)
            //for(int j=0;j<8;j++)
            //{
            //    mute += getMuteData(Form1.devcbx, Form1.usernum, j);

            //}
            //for(int j=0;j<8;j++)
            //{
            //    mute += getSwitchMuteData(Form1.devcbx, Form1.usernum, j);
            //}

            for (i=0;i<6;i++)
            {
                //mute += getMuteData(Form1.devcbx, Form1.usernum, i);
               
                phase += getPhaseData_2(sgags.devcbx, sgags.usernum, i);

                //if(i != 0)//主通道不带压限
                //{
                //    limit += getLimit_T_Data(Form1.devcbx, Form1.usernum, i);
                //    start += getLimit_S_Data(Form1.devcbx, Form1.usernum, i);
                //    relea += getLimit_R_Data(Form1.devcbx, Form1.usernum, i);
                //}

                gain += getGainData(sgags.devcbx, sgags.usernum, i);
                delay += getDealyData(sgags.devcbx, sgags.usernum, i);

                //equ += getEquAllData(Form1.devcbx, Form1.usernum, i);

                getHlData(sgags.devcbx, sgags.usernum, i, 0, out hl1, out hl2);
                hl += hl1;
                hl += hl2;
                getHlData(sgags.devcbx, sgags.usernum, i, 1, out hl1, out hl2);
                hl += hl1;
                hl += hl2;

                //无
                //lowmend = _dev.user[Form1.usernum].getLowMendData(Form1.devcbx, Form1.usernum);
            }
            //for (int j = 0; j < 8; j++)
            //{
            //    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, j);
            //}

            //for (int j = 0; j < 8; j++)
            //{
            //Form1 f = new Form1();
            if (_dev.user[sgags.usernum].RecentMute_ch2 == "取消静音")
            {
                if (_dev.user[sgags.usernum].cbxIn1Out1_IsCheck == true)
                {
                   _dev.user[sgags.usernum].switchChannel[0].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 0);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[0].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 0);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out1_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[4].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 4);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[4].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 4);
                }
            }
            else
            {
                if (_dev.user[sgags.usernum].cbxIn1Out1_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[0].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 0);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[0].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 0);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out1_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[4].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 4);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[4].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 4);
                }

            }

            if (_dev.user[sgags.usernum].RecentMute_ch3 == "取消静音")
            {
                if (_dev.user[sgags.usernum].cbxIn1Out2_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[1].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 1);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[1].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 1);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out2_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[5].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 5);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[5].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 5);
                }
            }
            else
            {
                if (_dev.user[sgags.usernum].cbxIn1Out2_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[1].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 1);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[1].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 1);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out2_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[5].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 5);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[5].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 5);
                }

            }

            if (_dev.user[sgags.usernum].RecentMute_ch4 == "取消静音")
            {
                if (_dev.user[sgags.usernum].cbxIn1Out3_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[2].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 2);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[2].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 2);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out3_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[6].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 6);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[6].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 6);
                }

            }
            else
            {
                if (_dev.user[sgags.usernum].cbxIn1Out3_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[2].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 2);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[2].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 2);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out3_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[6].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 6);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[6].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 6);
                }

            }

            if (_dev.user[sgags.usernum].RecentMute_ch5 == "取消静音")
            {
                if (_dev.user[sgags.usernum].cbxIn1Out4_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[3].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 3);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[3].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 3);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out4_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[7].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 7);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[7].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 7);
                }
            }
            else
            {
                if (_dev.user[sgags.usernum].cbxIn1Out4_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[3].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 3);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[3].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 3);
                }
                if (_dev.user[sgags.usernum].cbxIn2Out4_IsCheck == true)
                {
                    _dev.user[sgags.usernum].switchChannel[7].setMute(_mute.UnMute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 7);
                }
                else
                {
                    _dev.user[sgags.usernum].switchChannel[7].setMute(_mute.Mute, nor);
                    mute += getSwitchMuteData(sgags.devcbx, sgags.usernum, 7);
                }

            }

            string all = mute+limit + start + relea + gain + phase + delay + hl;
            return all;
        }

        //因为下位机空间不够，要发两次，把EQ数据和HL数据提取出来，单独发送一次
        public string getEQUdata()
        {
            string equ = "";
            //string hl = "";
            //string hl1 = "";
            //string hl2 = "";
            for(int i = 0; i < 6; i++)
            {
                //equ+=getEquAllData(Form1.devcbx,Form1.usernum,)
                equ += getEquAllData(sgags.devcbx, sgags.usernum, i);

                //getHlData(Form1.devcbx, Form1.usernum, i, 0, out hl1, out hl2);
                //hl += hl1;
                //hl += hl2;
                //getHlData(Form1.devcbx, Form1.usernum, i, 0, out hl1, out hl2);
                //hl += hl1;
                //hl += hl2;
            }
            //string equhl = equ/* + hl*/;
            return equ /*+ hl*/;
        }


        static string getGainData(int dev, int user, int ch)
        {
            string gain = MyData.AddrGain[ch].ToString("X4");
            byte[] gainBuf = new byte[4];
            _calc.Ten2Two_523(Math.Pow(10, (_dev.user[user].ch[ch].getGain() / 200)), gainBuf);
            gain += gainBuf[0].ToString("X2") + gainBuf[1].ToString("X2") + gainBuf[2].ToString("X2") + gainBuf[3].ToString("X2");
            return gain;
        }

        static string getDealyData(int dev, int user, int ch)
        {
            string delay = MyData.AddrDelay[ch].ToString("X4");
            int delayInt = (int)Math.Round(_dev.user[user].ch[ch].getDealy()/100/*除100，精确两位小数*/ * 48/*48K采样频率 48000除1000 从s得到ms */);
            delayInt = (delayInt < 1) ? 1 : delayInt;
            delay += ((delayInt >> 24) & 0xFF).ToString("X2") + ((delayInt >> 16) & 0xFF).ToString("X2") + ((delayInt >> 8) & 0xFF).ToString("X2") + ((delayInt >> 0) & 0xFF).ToString("X2");
            return delay;
        }
        //static string getPhaseData(int dev, int user, int ch)
        //{
        //    string phase = MyData.AddrPhase[ch].ToString("X4");
        //    byte[] phaseBuf = new byte[4];
        //    _calc.Ten2Two_523((_dev.user[user].ch[ch].getPhase() == _phase.Phase) ? (-1) : (1), phaseBuf);//-1 反相，1 未反相
        //    phase += phaseBuf[0].ToString("X2") + phaseBuf[1].ToString("X2") + phaseBuf[2].ToString("X2") + phaseBuf[3].ToString("X2");
        //    return phase;
        //}
        
        static string getPhaseData_2(int dev,int user,int ch)
        {
            string phase = "";
            phase += MyData.AddrPhase[ch].ToString("X4");
            for(int md=0;md<4;md++)
            {
                if(_dev.user[user].ch[ch].getPhase()==_phase.UnPhase)//未反相
                {
                    phase += MyData.DataPhase[0, md].ToString("X2");
                }
                else//反相
                {
                    phase += MyData.DataPhase[1, md].ToString("X2");
                }
            }
            return phase;
        }

        //public enum switchCh
        //{
        //    in1out1,

        //}
        public static string getSwitchMuteData(int dev,int user,int switchChannel)
        {
            string mute = "";
            mute += MyData.AddrMute[switchChannel].ToString("X4");
            for(int md=0;md<4;md++)
            {
                if(_dev.user[user].switchChannel[switchChannel].getMute()==_mute.UnMute)//未静音
                {
                    mute += MyData.DataMute[0, md].ToString("X2");
                }
                else
                {
                    mute += MyData.DataMute[1, md].ToString("X2");
                }
            }
            return mute;

        }

        //public static string getMuteData(int dev, int user, int ch)
        //{
        //    string mute = "";
        //    //for (int ms = 0; ms < 2; ms++)//静音-两个存储空间，总共 ：数据量为：2 * （4 byte 地址 + 4 byte 数据） = 16 byte
        //    //for(int ms=0;ms<2;ms++)
        //    //{
        //    //    #region 地址
        //    //    mute += (MyData.AddrMute[ch] + ms).ToString("X4");//加地址，mute = 4 byte，
        //    //    #endregion
        //    //    for (int md = 0; md < 4; md++)//每个存储空间4 byte
        //    //    {
        //    //        if (_dev.user[user].ch[ch].getMute() == _mute.UnMute)//未静音
        //    //        {
        //    //            //mute += MyData.DataMute[0, ms, md].ToString("X2");
        //    //            mute += MyData.DataMute[0, md].ToString("X2");
        //    //        }
        //    //        else//静音
        //    //        {
        //    //            //mute += MyData.DataMute[1, ms, md].ToString("X2");
        //    //            mute += MyData.DataMute[1, md].ToString("X2");
        //    //        }
        //    //    }
        //    //}
            
        //    //1个空间长度
        //    mute += MyData.AddrMute[ch].ToString("X4");
        //    for(int md=0;md<4;md++)//4字节数据
        //    {
        //        if(_dev.user[user].ch[ch].getMute()==_mute.UnMute)//未静音
        //        {
        //            mute += MyData.DataMute[0, md].ToString("X2");//地址+数据
        //        }
        //        else//静音
        //        {
        //            mute += MyData.DataMute[1, md].ToString("X2");
        //        }
        //    }
        //    return mute;
        //}

        //0+2,0+3,0+4,0+5;
        //1+2,1+3,1+4,1+5
        //static string getMuteData_2(int dev,int user,int ch_2)
        //{
        //    if()
        //}
        //static string getLimit_S_Data(int dev, int user, int ch)
        //{
        //    string start = "";
        //    //start += MyData.AddrLmtSta[ch].ToString("X4");//启动：一个存储空间 总共 ：4+4 = 8 byte
        //    start += MyData.AddrLmtSta.ToString("X4");
        //    for (int sd = 0; sd < 4; sd++)
        //    {
        //        start += MyData.DataLmtStr[_dev.user[user].ch[ch].getLT_St(), sd].ToString("X2");
        //    }
        //    return start;
        //}
        //static string getLimit_R_Data(int dev, int user, int ch)
        //{
        //    string relea = "";
        //    //relea += MyData.AddrLmtRle[ch].ToString("X4");//释放：一个存储空间 总共 ：4+4 = 8 byte
        //    relea += MyData.AddrLmtRle.ToString("X4");
        //    for (int rd = 0; rd < 4; rd++)
        //    {
        //        relea += MyData.DataLmtRel[_dev.user[user].ch[ch].getLT_Re(), rd].ToString("X2");
        //    }
        //    return relea;
        //}
        //static string getLimit_T_Data(int dev, int user, int ch)
        //{
        //    string limit = "";

        //    //int address = MyData.AddrLmtThr[ch];
        //    int address = MyData.AddrLmtThr;
        //    int index = _dev.user[user].ch[ch].getLT_Th();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        limit += (address >> 8).ToString("X2") + (address & 0xFF).ToString("X2")
        //            + MyData.DataLmtThr[index, i, 0].ToString("X2") + MyData.DataLmtThr[index, i, 1].ToString("X2") + MyData.DataLmtThr[index, i, 2].ToString("X2") + MyData.DataLmtThr[index, i, 3].ToString("X2");
        //        address++;
        //    }
        //    return limit;
        //}

        string getEquAllData(int dev, int user, int ch)
        {
            string equall = "";
            for (int pt = 0; pt < 6; pt++)
            {
                equall += getEquData(dev, user, ch, pt);
            }
            return equall;
        }
        public static string getEquData(int dev, int user, int ch, int pt)
        {

            return getEqValue(_dev.user[user].ch[ch].getEQ_F(pt), _dev.user[user].ch[ch].getEQ_G(pt), _dev.user[user].ch[ch].getEQ_Q(pt), 48000, MyData.AddrEqu[ch, pt], _dev.user[user].ch[ch].getEQ_T(pt));
        }
        static string getEqValue(double f0, double gain, double Q, double fs, int addr, int type/*pt*/)
        {
            double[] eqcft = new double[5];
            //             buf[0] = b0;
            //             buf[1] = b1;
            //             buf[2] = b2;
            //             buf[3] = a1;
            //             buf[4] = a2;
            switch (type/*_dev.user[user].ch[ch].getEQ_T(pt)*/)
            //switch (Config.eqtype[MyData.PDownNum])
            {
                case 0:
                    _calc.getEqCft(eqcft, gain, f0, Q, fs);//获取5个系数
                    break;
                case 1:
                    _calc.getLowShelfCft(eqcft, gain, f0, Q, fs);//获取5个系数
                    break;
                case 2:
                    _calc.getHighShelfCft(eqcft, gain, f0, Q, fs);//获取5个系数
                    break;
                default:
                    _calc.getEqCft(eqcft, gain, f0, Q, fs);//获取5个系数
                    break;
            }

            byte[] byte_b0a0 = new byte[4];
            byte[] byte_b1a0 = new byte[4];
            byte[] byte_b2a0 = new byte[4];
            byte[] byte_a1a0 = new byte[4];
            byte[] byte_a2a0 = new byte[4];
            _calc.Ten2Two_523(eqcft[0]/*b0a0*/, byte_b0a0);
            _calc.Ten2Two_523(eqcft[1]/*b1a0*/, byte_b1a0);
            _calc.Ten2Two_523(eqcft[2]/*b2a0*/, byte_b2a0);
            _calc.Ten2Two_523(-eqcft[3]/*a1a0*/, byte_a1a0);//这两个系数取反
            _calc.Ten2Two_523(-eqcft[4]/*a2a0*/, byte_a2a0);//这两个系数取反
            byte[,] byteB0_A0 = new byte[5, 4];
            for (int i = 0; i < 4; i++) byteB0_A0[0, i] = byte_b0a0[i];
            for (int i = 0; i < 4; i++) byteB0_A0[1, i] = byte_b1a0[i];
            for (int i = 0; i < 4; i++) byteB0_A0[2, i] = byte_b2a0[i];
            for (int i = 0; i < 4; i++) byteB0_A0[3, i] = byte_a1a0[i];
            for (int i = 0; i < 4; i++) byteB0_A0[4, i] = byte_a2a0[i];
            string data = GetAddrValue(addr, 5, byteB0_A0);
            return data;
        }
        public static string GetAddrValue(int addr, int count, byte[,] B_Datas)
        {
            string data = "";
            int address = addr;
            for (int i = 0; i < count; i++)
            {
                data += (address >> 8).ToString("X2") + (address & 0xFF).ToString("X2") + B_Datas[i, 0].ToString("X2") + B_Datas[i, 1].ToString("X2") + B_Datas[i, 2].ToString("X2") + B_Datas[i, 3].ToString("X2");
                address++;
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="user"></param>
        /// <param name="ch"></param>   0->1   1->2
        /// <param name="hl"></param>   0->L   1->H
        /// <param name="type"></param>
        /// <param name="step"></param>
        static int getHlData(int dev, int user, int ch, int hl, out string sdata1, out string sdata2)//HIGH
        {
            byte type = _dev.user[user].ch[ch].getHL_T(hl);
            byte step = _dev.user[user].ch[ch].getHL_S(hl);
            double[] datahl = new double[5];
            string[] strHl = new string[2];
            switch (type)
            {
                case 0:
                    switch (step)//H-BEL  hl * 2+
                    {
                        case 0:
                            _calc.Getbel2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.GetAllPass2st(datahl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000, 1);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);//user.channel[channelSet].HLPFValue[1]
                            break;
                        case 1:
                            _calc.Getbel2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.GetCoefficientTwo1st(datahl, hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        case 2:
                            _calc.Getbel2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.Getbel2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        default: break;
                    }
                    break;

                case 1:
                    switch (step)//H-BWT
                    {
                        case 0:
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.GetAllPass2st(datahl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000, 1);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        case 1:
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.GetCoefficientTwo1st(datahl, hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        case 2:
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        default: break;
                    }
                    break;
                case 2:
                    switch (step)//H-LR
                    {
                        case 0:
                            _calc.GetCoefficientTwo1st(datahl, hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.GetCoefficientTwo1st(datahl, hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        //case 1:
                        //    //假的
                        //    GetCoefficientTwo1st(hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000)
                        //    strHl[0] = CalcHlData(AddrHL[ch, hl * 2 + 0], );
                        //    GetCoefficientTwo1st(hl, 2, _dev.user[user].ch[ch].getHL_F(hl), _dev.user[user].ch[ch].getHL_F(hl), 0, 48000)
                        //    strHl[1] = CalcHlData(AddrHL[ch, hl * 2 + 1], );
                        //    break;
                        case 2:
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[0] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 0], datahl);
                            _calc.Getbwt2st(datahl, hl, _dev.user[user].ch[ch].getHL_F(hl), 0, 48000);
                            strHl[1] = CalcHlData(MyData.AddrHL[ch, hl * 2 + 1], datahl);
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
            sdata1 = strHl[0];
            sdata2 = strHl[1];
            return 0;
        }

        static string CalcHlData(int addr, double[] data)
        {
            byte[] Hbyte_a1a0 = new byte[4];
            byte[] Hbyte_a2a0 = new byte[4];
            byte[] Hbyte_b0a0 = new byte[4];
            byte[] Hbyte_b1a0 = new byte[4];
            byte[] Hbyte_b2a0 = new byte[4];

            _calc.Ten2Two_523(data[0], Hbyte_a1a0);//TenToTwo(Ha1a0);
            _calc.Ten2Two_523(data[1], Hbyte_a2a0);//TenToTwo(Ha2a0);
            _calc.Ten2Two_523(data[2], Hbyte_b0a0);//TenToTwo(Hb0a0);
            _calc.Ten2Two_523(data[3], Hbyte_b1a0);//TenToTwo(Hb1a0);
            _calc.Ten2Two_523(data[4], Hbyte_b2a0);//TenToTwo(Hb2a0);

            byte[,] byteB0_A0 = new byte[5, 4];

            for (int i = 0; i < 4; i++) byteB0_A0[0, i] = Hbyte_b0a0[i];
            for (int i = 0; i < 4; i++) byteB0_A0[1, i] = Hbyte_b1a0[i];

            for (int i = 0; i < 4; i++) byteB0_A0[3, i] = Hbyte_a1a0[i];//line 4
            for (int i = 0; i < 4; i++) byteB0_A0[2, i] = Hbyte_b2a0[i];//line 3

            for (int i = 0; i < 4; i++) byteB0_A0[4, i] = Hbyte_a2a0[i];


            string hldata = GetAddrValue(addr, 5, byteB0_A0);
            return hldata;
        }


      
    }
}
