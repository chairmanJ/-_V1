using System;
using System.Windows.Forms;
using 二进四出_V1.calc;
using 二进四出_V1.channel;
using 二进四出_V1.comm;

namespace 二进四出_V1.manager
{
    public enum _lowmend
    {
        Mend,
        UnMend
    }

    [Serializable]
    public class _user
    {
        //低音增强 二进四出无低音增强，lowmend
        //public const int Addr_LMend = 0x0008 + 1;//0xxxxx;//OK
        //public readonly byte[,] NewEnhance = new byte[4, 4] { /*Enhance*/  { 0x00, 0x3F, 0x90, 0x56 },/*UnEnhance*/{ 0x00, 0x80, 0x00, 0x00 },/*Enhance*/  { 0x00, 0x3F, 0x90, 0x56 },/*UnEnhance*/{ 0x00, 0x80, 0x00, 0x00 } };

        //public _lowmend getLowMendState()
        //{
        //    return this.lowmend;
        //}
        //public void setLowMendState(_lowmend lowmend)
        //{
        //    this.lowmend = lowmend;


        //    //string start = "5A00A5FF";
        //    //string device = Form1.devcbx.ToString("X2");
        //    //string usergroup = Form1.usernum.ToString("X2");//命令-1
        //    //string cmds = "01";
        //    //string num = "0006";
        //    //string end = "A5FF005A";
        //    //string strlowmend = getLowMendData(Form1.devcbx, Form1.usernum);

        //    //string order = start + device + usergroup + cmds + num + strlowmend + end;
        //    //_facty.send(order);
        //}
        //public void SendLowMend()
        //{
        //    string start = "5A00A5FF";
        //    string device = Form1.devcbx.ToString("X2");
        //    string usergroup = Form1.usernum.ToString("X2");//命令-1
        //    string cmds = "01";
        //    string num = "0006";
        //    string end = "A5FF005A";
        //    string strlowmend = getLowMendData(Form1.devcbx, Form1.usernum);

        //    string order = start + device + usergroup + cmds + num + strlowmend + end;
        //    _facty.send(order);
        //}
        //public string getLowMendData(int dev, int user)
        //{
        //    string lowmendData = "";
        //    //int lowmendEn = (_dev.user[user].getLowMendState() == _lowmend.Mend) ? 0 : 1;
        //    lowmendData += Addr_LMend.ToString("X4");
        //    if (_dev.user[user].getLowMendState() == _lowmend.Mend)
        //    {
        //        lowmendData += "00278E48";
        //    }
        //    else
        //    {
        //        lowmendData += "00800000";
        //    }
        //    return lowmendData;
        //}


       
        #region Limit压限
        //limit
        public int getLT_Re()
        {
            return this.limit.getRe();
        }
        public int getLT_Th()
        {
            return this.limit.getTh();
        }
        public int getLT_St()
        {
            return this.limit.getSt();
        }
        public void setLT_Th(int threshold)
        {
            int ret = 0;
            ret = this.limit.setTh(threshold);
            if (ret != 0)
            {
                return;
            }
            //_channel.UpdateCurve(FLAG_LIMIT_T, threshold);
            _channel.UpdateCurve(_channel.FLAG_LIMIT_T,threshold);
  
        }
        public void setLT_Th(int threshold, int nosend)
        {
            int ret = 0;
            ret = this.limit.setTh(threshold);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_LIMIT_T, threshold);
        }
        public void setLT_St(int startTime)
        {
            int ret = 0;
            ret = this.limit.setSt(startTime);
            if (ret != 0)
            {
                return;
            }
           
            _channel.UpdateCurve(_channel.FLAG_LIMIT_S,startTime);
        }
        public void setLT_St(int startTime, int nosend)
        {
            int ret = 0;
            ret = this.limit.setSt(startTime);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_LIMIT_S, startTime);
        }
        public void setLT_Re(int releaseTime)
        {
            int ret = 0;
            ret = this.limit.setRe(releaseTime);
            if (ret != 0)
            {
                return;
            }
            _channel.UpdateCurve(_channel.FLAG_LIMIT_R, releaseTime);
        }
        public void setLT_Re(int releaseTime, int nosend)
        {
            int ret = 0;
            ret = this.limit.setRe(releaseTime);
            if (ret != 0)
            {
                return;
            }
            //UpdateCurve(FLAG_LIMIT_R, releaseTime);
        }



        public static string getLimit_S_Data(int dev, int user, int ch)
        {
            string start = "";
            //start += MyData.AddrLmtSta[ch].ToString("X4");//启动：一个存储空间 总共 ：4+4 = 8 byte
            start += MyData.AddrLmtSta.ToString("X4");
            for (int sd = 0; sd < 4; sd++)
            {
                //start += MyData.DataLmtStr[_dev.user[user].ch[ch].getLT_St(), sd].ToString("X2");
                start += MyData.DataLmtStr[_dev.user[user].getLT_St(), sd].ToString("X2");
            }
            return start;
        }
        public static string getLimit_R_Data(int dev, int user, int ch)
        {
            string relea = "";
            //relea += MyData.AddrLmtRle[ch].ToString("X4");//释放：一个存储空间 总共 ：4+4 = 8 byte
            relea += MyData.AddrLmtRle.ToString("X4");
            for (int rd = 0; rd < 4; rd++)
            {
                //relea += MyData.DataLmtRel[_dev.user[user].ch[ch].getLT_Re(), rd].ToString("X2");
                relea += MyData.DataLmtRel[_dev.user[user].getLT_Re(), rd].ToString("X2");
            }
            return relea;
        }
        public static string getLimit_T_Data(int dev, int user, int ch)
        {
            string limit = "";

            //int address = MyData.AddrLmtThr[ch];
            int address = MyData.AddrLmtThr;
            //int index = _dev.user[user].ch[ch].getLT_Th();
            int index = _dev.user[user].getLT_Th();
            for (int i = 0; i < 8; i++)
            {
                limit += (address >> 8).ToString("X2") + (address & 0xFF).ToString("X2")
                    + MyData.DataLmtThr[index, i, 0].ToString("X2") + MyData.DataLmtThr[index, i, 1].ToString("X2") + MyData.DataLmtThr[index, i, 2].ToString("X2") + MyData.DataLmtThr[index, i, 3].ToString("X2");
                address++;
            }
            return limit;
        }

        public static void SendLimit_T(int thr, int dev, int user, int ch)
        {
            string sdata = getLimit_T_Data(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            //string slength = "0030";//6 -> HEX
            string slength = "0030";//8个空间长度，48字节，对的

            string order = _channel.start + sdev + suser + scmd + slength + sdata + _channel.end;
            _facty.send(order);
        }

        public static void SendLimit_S(int str, int dev, int user, int ch)
        {
            string sdata = getLimit_S_Data(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";//6 -> HEX

            string order =_channel.start + sdev + suser + scmd + slength + sdata +_channel.end;
            _facty.send(order);
        }
        public static void SendLimit_R(int rel, int dev, int user, int ch)
        {
            string sdata = getLimit_R_Data(dev, user, ch);
            //string sdev = Mger.dev[Form1.devcbx].getDevNum().ToString("X2");
            string sdev = dev.ToString("X2");
            string suser = user.ToString("X2");
            string scmd = "01";
            string slength = "0006";//6 -> HEX

            string order =_channel.start + sdev + suser + scmd + slength + sdata +_channel.end;
            _facty.send(order);
        }
        #endregion
        
        //通道
        public _channel[] ch = new _channel[6];

        private _limit limit = new _limit();
        //public int[] switchChannel = new int[8];
        public _channel[] switchChannel=new _channel[8];

        public string RecentMute_ch2 = "静音";
        public string RecentMute_ch3 = "静音";
        public string RecentMute_ch4 = "静音";
        public string RecentMute_ch5 = "静音";

        public bool cbxIn1Out1_IsCheck =true;
        public bool cbxIn1Out2_IsCheck =false;
        public bool cbxIn1Out3_IsCheck = true;
        public bool cbxIn1Out4_IsCheck =false;
        public bool cbxIn2Out1_IsCheck =false;
        public bool cbxIn2Out2_IsCheck=true;
        public bool cbxIn2Out3_IsCheck =false;
        public bool cbxIn2Out4_IsCheck =true;

        public _user()
        {
            //2进4出6通道
            ch[0] = new _channel();
            ch[1] = new _channel();
            ch[2] = new _channel();
            ch[3] = new _channel();
            ch[4] = new _channel();
            ch[5] = new _channel();



            switchChannel[0] = new _channel();
            switchChannel[1] = new _channel();
            switchChannel[2] = new _channel();
            switchChannel[3] = new _channel();
            switchChannel[4] = new _channel();
            switchChannel[5] = new _channel();
            switchChannel[6] = new _channel();
            switchChannel[7] = new _channel();
          
            


    }








    }

}
