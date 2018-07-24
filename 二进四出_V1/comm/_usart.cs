using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 二进四出_V1.comm
{
    [Serializable]
    class _usart: _ic
    {
        #region DllImport
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(ref long count);
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(ref long count);
        #endregion

        private SerialPort sp = new SerialPort();
        private static _usart serial = new _usart();
        public string SerialNames
        {
            get { return sp.PortName; }
            set { sp.PortName = value; }
        }
        public static _ic getSerial()
        {
            if (serial == null)
            {
                serial = new _usart();
            }
            return serial;
        }
        public int Init()
        {
            //sp.PortName = "COM5";
            sp.BaudRate = 9600;
            sp.DataBits = 8;
            sp.StopBits = System.IO.Ports.StopBits.One;
            sp.Parity = System.IO.Ports.Parity.None;
            sp.ReadTimeout = 3000;
            sp.WriteTimeout = -1;
            //sp.Open();
            //List<string> comList = CheckAvailablePorts();
            //return comList;
            tsendswitch.Start();
            return 0;
        }
        public _comState getState()
        {
            if (sp.IsOpen)
            {
                return _comState.Link;
            }
            else
            {
                return _comState.Break;
            }
        }
        public void setState(_comState state)
        {
            if (state == _comState.Link)
            {
                try
                {
                    sp.Open();
                }
                catch
                {
                    MessageBox.Show("打开串口失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                sp.Close();
            }
        }

        public void setPort(string comPort)
        {
            sp.PortName = comPort;
        }

        public List<string> GetCom()
        {
            List<string> list = new List<string>();
            list.Clear();
            string[] allAvailablePorts = SerialPort.GetPortNames();
            Array.Sort(allAvailablePorts);
            if (allAvailablePorts.Length > 0)
            {
                for (int i = 0; i < allAvailablePorts.Length; i++)
                {
                    sp.PortName = allAvailablePorts[i];
                    try
                    {
                        sp.Open();
                        if (sp.IsOpen)
                        {
                            list.Add(allAvailablePorts[i]);
                            sp.Close();
                        }
                    }
                    catch
                    {

                    }
                }
                //if (list.Count > 0)
                //{
                //    list.Add("检测设备");
                //}
                //else
                if (list.Count == 0)
                {
                    return list;
                    //MessageBox.Show("没有可用的设备", "提示");
                }
            }
            else
            {
                return list;
                //MessageBox.Show("没有可用的设备", "提示");
            }
            return list;
        }
       static bool sendflag = false;
       static void  SendDelay()
        {
            while(true)
            {
                
                if (sendflag == false)
                {
                    Thread.Sleep(80);
                    sendflag = true;
                }
                else
                {
                    Thread.Sleep(10);
                }                
            }
        }

        public Thread tsendswitch = new Thread(SendDelay);
        public static bool alwaysSend = false;
        public int send(string str)
        {
            int ret = 0;

            if(alwaysSend == false)
            {
                if (sendflag == false)
                {
                    return -1;
                }
                else
                {
                    sendflag = false;
                }

            }


            if (sp.IsOpen)
            {
                sp.DiscardInBuffer();//每次发生命令前清空接收缓存
                byte[] buffer;
                buffer = new byte[(str.Length % 2 == 0) ? (str.Length / 2) : (str.Length / 2 + 1)];
                String2Hex(str, ref buffer); //转为16进制输出   
                sp.Write(buffer, 0, buffer.Length);
                //waitRes();
            }
            else
            {
                ret = -1;
                //MessageBox.Show("设备未连接", "提示");
            }
            return ret;
        }
        public int send_btl(string str)
        {
            int ret = 0;    
            if (sp.IsOpen)
            {
                sp.DiscardInBuffer();//每次发生命令前清空接收缓存
                byte[] buffer;
                buffer = new byte[(str.Length % 2 == 0) ? (str.Length / 2) : (str.Length / 2 + 1)];
                String2Hex(str, ref buffer); //转为16进制输出   
                sp.Write(buffer, 0, buffer.Length);
                //waitRes();
            }
            else
            {
                ret = -1;
                //MessageBox.Show("设备未连接", "提示");
            }
            return ret;
        }
        public int send(byte[] buf)
        {
            if (buf == null)
            {
                return -1;
            }
            if (sp.IsOpen)
            {
                sp.DiscardInBuffer();//每次发生命令前清空接收缓存
                sp.Write(buf, 0, buf.Length);
            }
            return 0;
        }

        public int sendnp(string str)
        {
            int ret = 0;
            if (sp.IsOpen)
            {
                sp.DiscardInBuffer();//每次发生命令前清空接收缓存
                byte[] buffer;
                buffer = new byte[(str.Length % 2 == 0) ? (str.Length / 2) : (str.Length / 2 + 1)];
                String2Hex(str, ref buffer); //转为16进制输出   
                sp.Write(buffer, 0, buffer.Length);

            }
            else
            {
                ret = -1;
                //MessageBox.Show("设备未连接", "提示");
            }
            return ret;
        }
        private void waitRes()
        {
            // 临时关闭
            int readCount = 0;

            long count = 0;
            long count1 = 0;
            long freq = 0;
            double result = 0;
            QueryPerformanceFrequency(ref freq);
            QueryPerformanceCounter(ref count);
            try
            {
                readCount = sp.ReadByte();
            }
            catch
            {
                QueryPerformanceCounter(ref count1);
                count = count1 - count;
                result = (double)(count) / (double)freq;

                MessageBox.Show("设备连接失败：" + Math.Round(result, 3) + " s\r\n");
            }

        }

        public byte[] read()
        {
            byte[] buffer = null;
            // 临时关闭
            int count = sp.BytesToRead;
            buffer = new byte[count];
            sp.Read(buffer, 0, count);
            //
            return buffer;


        }
        public void String2Hex(String str, ref Byte[] senddata)
        {
            int hexdata, lowhexdata;
            int hexdatalen = 0;
            int len = str.Length;
            for (int i = 0; i < len;)
            {
                byte lstr, hstr = (byte)str[i];
                if (hstr == ' ')
                {
                    i++;
                    continue;
                }
                i++;
                if (i >= len)
                    break;
                lstr = (byte)str[i];
                hexdata = ConvertHexChar(hstr);
                lowhexdata = ConvertHexChar(lstr);
                if ((hexdata == 16) || (lowhexdata == 16))
                    break;
                else
                    hexdata = hexdata * 16 + lowhexdata;
                i++;
                senddata[hexdatalen] = (byte)hexdata;
                hexdatalen++;
            }
        }
        public byte ConvertHexChar(byte ch)
        {
            if ((ch >= '0') && (ch <= '9'))
                return (byte)(ch - 0x30);
            else if ((ch >= 'A') && (ch <= 'F'))
                return (byte)(ch - 'A' + 10);
            else if ((ch >= 'a') && (ch <= 'f'))
                return (byte)(ch - 'a' + 10);
            else return 0;
        }
        public int search()
        {
            Console.WriteLine("Rs232 search");
            return 0;
        }
        public int code()
        {
            Console.WriteLine("Rs232 code");
            return 0;
        }
        public int save()
        {
            Console.WriteLine("Rs232 save");
            return 0;
        }
        public int load()
        {
            Console.WriteLine("Rs232 load");
            return 0;
        }

    }
}
