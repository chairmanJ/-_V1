using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 


namespace 二进四出_V1.channel
{
    [Serializable]
    public enum _mute
    {
        Mute,
        UnMute
    }

    /// <summary>
    /// 静音类
    /// </summary>
    [Serializable]
    public class _emute
    {
        _mute mute = _mute.UnMute;

        public _mute getMute()
        {
            return this.mute;
        }
        public int setMute(_mute mute)
        {
            int ret = 0;
            this.mute = mute;
            return ret;
        }
        public _emute()
        {
            mute = _mute.UnMute;
        }

    }
}
