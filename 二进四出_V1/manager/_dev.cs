using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using 二进四出_V1.comm;

namespace 二进四出_V1.manager
{
    [Serializable]
    public enum _board
    {
        Board,      //广播模式
        UnBoard,    //非广播模式
    }
    [Serializable]
    public static class _dev
    {
        static public _user[] user = new _user[10] { new _user(), new _user(), new _user(), new _user(), new _user(), new _user(), new _user(), new _user(), new _user(), new _user() };       
    }
}
