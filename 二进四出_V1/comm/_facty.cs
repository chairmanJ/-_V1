using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 二进四出_V1.comm
{
    [Serializable]
    class _facty
    {
        private static _ic com = _usart.getSerial();
        static public void setCom(_ic commu)
        {
            com = commu;
        }
        public static _comState getState()
        {
            return com.getState();
        }
        public static void setState(_comState state)
        {
            com.setState(state);
        }
        public static List<string> GetCom()
        {
            return com.GetCom();
        }
        public static void setPort(string port)
        {
            com.setPort(port);
        }

        static public int send(string str)
        {
            int ret = 0;
            ret = com.send(str);
            return ret;
        }
        static public int send_btl(string str)
        {
            int ret = 0;
            ret = com.send_btl(str);
            return ret;
        }
        static public int send(byte[] buf)
        {
            int ret = 0;
            ret = com.send(buf);
            return ret;
        }
        static public int sendnp(string str)
        {
            int ret = 0;
            ret = ((_usart)com).sendnp(str);
            return ret;
        }
        public static byte[] read()
        {
            byte[] buffer;
            buffer = com.read();
            return buffer;
        }
        static public int Init()
        {
            int ret = 0;
            ret = com.Init();
            return ret;
        }
        static public int search()
        {
            int ret = 0;
            ret = com.search();
            return ret;
        }
        static public int code()
        {
            int ret = 0;
            ret = com.code();
            return ret;
        }
        static public int save()
        {
            int ret = 0;
            ret = com.save();
            return ret;
        }
        static public int load()
        {
            int ret = 0;
            ret = com.load();
            return ret;
        }

    }
}
