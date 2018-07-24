using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 二进四出_V1.comm
{
    [Serializable]
    public enum _comState
    {
        Link,
        Break,
    }
    interface _ic
    {
        int Init();
        List<string> GetCom();
        int send(string str);
        int send_btl(string str);
        int send(byte[] buf);
        byte[] read();
        int search();
        int code();
        int save();
        int load();
        _comState getState();
        void setState(_comState state);

        void setPort(string comPort);
    }
}
