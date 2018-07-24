using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
namespace 二进四出_V1.channel
{
    /// <summary>
    /// 压限值
    /// </summary>
    [Serializable]
    public struct VLimit//其中3个成员都是有各自的数据库，只需要设定序号 - 对比不同数据库即可
    {
        public int lth;//阀值从0~14 对应阀值  -17 ~ 0 一共15个  18
        public int lst;//启动时间
        public int lre;//释放时间
    }
    /// <summary>
    /// 压限类
    /// </summary>
    /// 
    [Serializable]
    public class _limit
    {
        public VLimit limit = new VLimit();//压限 启动时间 释放时间
        public _limit()
        {
            limit.lth = 17;//默认序号对应第一个
            limit.lst = 0;//默认序号对应第一个
            limit.lre = 0;//默认序号对应第一个
        }

        public int getTh()
        {
            return this.limit.lth;
        }
        public int getRe()
        {
            return this.limit.lre;
        }
        public int getSt()
        {
            return this.limit.lst;
        }
        public int setRe(int releaseTime)
        {
            this.limit.lre = releaseTime;
            return 0;
        }
        public int setSt(int startTime)
        {
            this.limit.lst = startTime;
            return 0;
        }
        public int setTh(int threshold)
        {
            this.limit.lth = threshold;
            return 0;
        }
    }
}
