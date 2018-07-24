using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 二进四出_V1.calc;

using 二进四出_V1.comm;
using 二进四出_V1.manager;
using 二进四出_V1.channel;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace 二进四出_V1
{
    public partial class sgags : Form
    {



        /// <summary>
        /// ///
        /// </summary>

        public static int devcbx = 0;
        public static int usernum = 0;
        public static int usernumsave = 0;
        public static int usernumload = 0;
        public static int channel = 0;

        public static int switchChannel = 0;
        public static double[,] freq_y = new double[6, 790];//画曲线所用 - 每个像素点对应的频率  
        //public static double[,] freq_y = new double[6, 829];
        public static double[,] phase_y = new double[6, 790];//画曲线所用 - 每个像素点对应的频率  
        //public static double[,] phase_y = new double[6, 829];
        public static double[] LowDbValue = new double[790];//画曲线所用 - 每个像素点对应的频率  
        //public static double[] LowDbValue = new double[829];
        public static double[] HighDbValue = new double[790];//画曲线所用 - 每个像素点对应的频率  
        //public static double[] HighDbValue = new double[829];

        public static bool bypass = false;
        ////////////////////////////////////////////////////

        public static readonly int regondraw = 746;
        //public static readonly int regondraw = 810;

        //30 746 regondraw

        public static readonly int Interval_H = 30;                           //纵坐标间距
        public static readonly int calcWidth = 790;                          //计算宽度，并不是pic的实际宽度
        //public static readonly int calcWidth = 829;
        //public static readonly int calcWidth = 824;
        public static readonly int curveLeft = 40;                           //左边距像素点
        public static readonly int curveUp = 20;                             //上边距像素点
        public static readonly int curveDown = 20 + 10 * Interval_H;         //下边距像素点 
        public static readonly int ExtendR = 0/*44*/;                        //20K   ，往右延伸像素点，为右边距   "calcWidth = 780; 时候为 736" , "calcWidth = 790; 时候为 745"  ,  "790 - 745    =  44"

        public static Bitmap bmp;
        public static System.Drawing.Graphics g;                      //绘图
        public static SolidBrush[] sbr = new SolidBrush[15];   //点的颜色（拖动6个点）  
        public static SolidBrush[] hlsbr = new SolidBrush[2];   //点的颜色（拖动6个点）  
        public static PointF[] DrawPointBB = new PointF[/*calcWidth*/regondraw - curveLeft + 2];
        public static PointF[] DrawPoint = new PointF[/*calcWidth*/regondraw - curveLeft];
        public static Color[] pointColor = new Color[15] { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };
        public static Color[] hlColor = new Color[2] { Color.LightSkyBlue, Color.LightSkyBlue };
        public static float[] FreqLine = new float[] { 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 20000 };
        public static string[] FreqUnit = new string[] { "20Hz", "30Hz", "40Hz", "50Hz", "60Hz", "70Hz", "80Hz", "90Hz", "100Hz", "200Hz", "300Hz", "400Hz", "500Hz", "600Hz", "700Hz", "800Hz", "900Hz", "1KHz", "2KHz", "3KHz", "4KHz", "5KHz", "6KHz", "7KHz", "8KHz", "9KHz", "10KHz", "20KHz" };
        public static int[] loca = new int[10] { 0, 3, 8, 9, 12, 17, 18, 22, 26, 27 };//提取上面的有用横坐标
        public static int[] coordPoint = new int[28];                                 //纵坐标线的具体坐标值                   //右边线 745
        public static double[] wholeFreq = new double[calcWidth];                     //790个像素点，所对应的所有频率值
        public static int curveRight = 0;// DrawPoint[DrawPoint.Length -1 ] + ExtendR 745+44=790

        public static Pen pp = new Pen(Color.Yellow, (float)1.5);
        public static Brush bb = new SolidBrush(Color.FromArgb(38, Color.White));//15, 185, 185

        [DllImport("dlltest.dll")]
        public static extern void mydouble(double[] dd, int n);
        public sgags()
        {

            


            InitializeComponent();
            // skinEngine1.SkinFile = "./Skins/SteelBlack.ssk";
            //Vista2_color2.ssk
            //SteelBlack.ssk
            //SportsOrange.ssk
            //RealOne.ssk
            // skinEngine1.SkinFile = "./Skins/RealOne.ssk";
           // skinEngine1.SkinFile = "./Skins/Silver/Silver.ssk";
            InitDelay();
            InitControl();//控件初始化
            InitSerial();//串口初始化

            this.FormBorderStyle = FormBorderStyle.None;
            //this.Width = 786;// 816;//833
            timer1.Enabled = true;
            //cbxVT.SelectedIndex = 17;
            cbxVT.SelectedIndex = 0;
            InitPic();      //画坐标网格

           


            int nores = 0xff;
            

            _dev.user[usernum].switchChannel[0].setMute(_mute.UnMute, nores);
            _dev.user[usernum].switchChannel[1].setMute(_mute.Mute, nores);
            _dev.user[usernum].switchChannel[2].setMute(_mute.UnMute, nores);
            _dev.user[usernum].switchChannel[3].setMute(_mute.Mute, nores);
            _dev.user[usernum].switchChannel[4].setMute(_mute.Mute, nores);
            _dev.user[usernum].switchChannel[5].setMute(_mute.UnMute, nores);
            _dev.user[usernum].switchChannel[6].setMute(_mute.Mute, nores);
            _dev.user[usernum].switchChannel[7].setMute(_mute.UnMute, nores);

            UpdatePic();
        }
        System.Drawing.Bitmap[] bitmapdelay = new Bitmap[3];
        static int delayi = 0;

        //3张图片旋转
        void InitDelay()
        {
            
        }


        void delay_100ms(int x)
        {
            //picDelay.Visible = true;
            //this.picDelay.Invalidate();


            // picDelay.Refresh();
           
            for (int i = 0; i < x; i++)
            {
                delayi++;

                Thread.Sleep(100);
            }

            //picDelay.Visible = false;
            // this.picDelay.Invalidate();
            // picDelay.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void InitSerial()
        {
            _facty.setCom(_usart.getSerial());
            _facty.Init();
            RefreshCom();
        }
        double[] freqFirst = new double[] { 50, 100, 500, 1000, 2000, 5000 };
        #region 初始化控件
        void InitControl()
        {
            //skinEngine1.SkinFile = "./Skins/RealOne.ssk";

            cbxUserSave.SelectedIndex = 0;
            cbxUserLoad.SelectedIndex = 0;

            label20.Visible = false;

            cbxDev.Items.Clear();
            for (int i = 1; i <= 255; i++)
            {
                cbxDev.Items.Add(i);
            }
            cbxDev.SelectedIndex = 0;
            //cbxVT.SelectedIndex = 17;

            // cbxVT.SelectedIndex=

            btnClose.BackColor = Color.Transparent;
            //btnClose.BackgroundImage = global::二进四出_V1.Properties.Resources.btnCloseU;
            btnClose.BackgroundImage = global::二进四出_V1.Properties.Resources.close;
            btnClose.BackgroundImageLayout = ImageLayout.Stretch;

            btnMin.BackColor = Color.Transparent;
            //btnMin.BackgroundImage = global::二进四出_V1.Properties.Resources.btnMinU;
            btnMin.BackgroundImage = global::二进四出_V1.Properties.Resources.min;
            btnMin.BackgroundImageLayout = ImageLayout.Stretch;
            //rbtnIn2.Select();//输入通道2 激活
            rbtnIn1.Select();
            btnMute.Enabled = false;
            //btnMute.Enabled = false;
            //cbxIn1Out1.Checked = true;
            //cbxIn1Out3.Checked = true;
            //cbxIn2Out2.Checked = true;
            //cbxIn2Out4.Checked = true;
            //均衡调节 - 按键初始化
            {
                tbxF0.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxF1.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxF2.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxF3.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxF4.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxF5.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);

                tbxQ0.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxQ1.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxQ2.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxQ3.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxQ4.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxQ5.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);

                tbxG0.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxG1.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxG2.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxG3.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxG4.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);
                tbxG5.KeyDown += new KeyEventHandler(TbxKeyDown_FGQ);

                CbxT0.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT1.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT2.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT3.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT4.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT5.SelectedValueChanged += new EventHandler(cbxTypeChanged);
                CbxT0.MouseDown += new MouseEventHandler(cbxTypeDown);
                CbxT1.MouseDown += new MouseEventHandler(cbxTypeDown);
                CbxT2.MouseDown += new MouseEventHandler(cbxTypeDown);
                CbxT3.MouseDown += new MouseEventHandler(cbxTypeDown);
                CbxT4.MouseDown += new MouseEventHandler(cbxTypeDown);
                CbxT5.MouseDown += new MouseEventHandler(cbxTypeDown);

                tbxLF.KeyDown += new KeyEventHandler(TbxKeyDown_HL);
                tbxHF.KeyDown += new KeyEventHandler(TbxKeyDown_HL);
            }
            
            rbtnIn1.CheckedChanged += new EventHandler(RadioChangedEve);
            rbtnIn2.CheckedChanged += new EventHandler(RadioChangedEve);
            rbtnOut1.CheckedChanged += new EventHandler(RadioChangedEve);
            rbtnOut2.CheckedChanged += new EventHandler(RadioChangedEve);
            rbtnOut3.CheckedChanged += new EventHandler(RadioChangedEve);
            rbtnOut4.CheckedChanged += new EventHandler(RadioChangedEve);

            cbxLtype.SelectedIndexChanged += new EventHandler(cbxHLTypeStep);
            cbxHtype.SelectedIndexChanged += new EventHandler(cbxHLTypeStep);
            cbxLstep.SelectedIndexChanged += new EventHandler(cbxHLTypeStep);
            cbxHstep.SelectedIndexChanged += new EventHandler(cbxHLTypeStep);
            cbxLtype.MouseDown += new MouseEventHandler(cbxHLTS_MDowm);
            cbxHtype.MouseDown += new MouseEventHandler(cbxHLTS_MDowm);
            cbxLstep.MouseDown += new MouseEventHandler(cbxHLTS_MDowm);
            cbxHstep.MouseDown += new MouseEventHandler(cbxHLTS_MDowm);


            //cbxVS.MouseDown += new MouseEventHandler(cbxLimit_MDowm);
            //cbxVR.MouseDown += new MouseEventHandler(cbxLimit_MDowm);
            //cbxVT.MouseDown += new MouseEventHandler(cbxLimit_MDowm);
            //cbxVS.SelectedIndexChanged += new EventHandler(cbxLimitChanged);
            //cbxVR.SelectedIndexChanged += new EventHandler(cbxLimitChanged);
            //cbxVT.SelectedIndexChanged += new EventHandler(cbxLimitChanged);

        }
        #endregion

        #region 通道选择
        public void RadioChangedEve(object o, EventArgs e)
        {
            //SendOrderFlag = false;
            RadioButton r = (RadioButton)o;
            tbxQ0.Enabled = true;
            tbxQ1.Enabled = true;
            tbxQ2.Enabled = true;
            tbxQ3.Enabled = true;
            tbxQ4.Enabled = true;
            tbxQ5.Enabled = true;
            if (r.Checked == true)
            {
                if (r.Name == "rbtnIn1")
                {
                    channel = 0;
                    btnMute.Text = "静音";
                    btnMute.Enabled = false;


                }
                else if (r.Name == "rbtnIn2")
                {
                    channel = 1;
                    btnMute.Text = "静音";
                    btnMute.Enabled = false;
                }
                else if (r.Name == "rbtnOut1")
                {
                    channel = 2;
                    FLAG_SELECTCHANNEL = 1;
                    btnMute.Enabled = true;
                    // btnMute.Text = "静音";
                    //btn_Click()
                   
                    btnMute.Text = _dev.user[usernum].RecentMute_ch2;
                    //if (btnMute.Text == "取消静音")
                    //    btnMute.Text = "静音";

                }
                else if (r.Name == "rbtnOut2")
                {
                    channel = 3;
                    FLAG_SELECTCHANNEL = 2;
                    btnMute.Enabled = true;
                    //if (btnMute.Text == "取消静音")
                    //    btnMute.Text = "静音";
                    //btnMute.Text = "静音";
                   
                    btnMute.Text = _dev.user[usernum].RecentMute_ch3;
                }
                else if (r.Name == "rbtnOut3")
                {
                    channel = 4;
                    FLAG_SELECTCHANNEL = 3;
                    btnMute.Enabled = true;
                    //if (btnMute.Text == "取消静音")
                    //    btnMute.Text = "静音";
                    // btnMute.Text = "静音";
                    
                    btnMute.Text = _dev.user[usernum].RecentMute_ch4;
                }
                else
                {
                    channel = 5;
                    FLAG_SELECTCHANNEL = 4;
                    btnMute.Enabled = true;
                    //if (btnMute.Text == "取消静音")
                    //    btnMute.Text = "静音";
                    // btnMute.Text = "静音";
                    
                    btnMute.Text = _dev.user[usernum].RecentMute_ch5;
                }
                #region 一个用户组一个压限，所以这里要改
                //一个用户组一个压限，所以这里要改
                //if (channel == 0)
                //{
                //gbxLimit.Visible = false;
                gbxLimit.Visible = true;
                //pic_logo.Visible = true;
                //}
                //else
                //{
                // gbxLimit.Visible = true;
                //pic_logo.Visible = false;

                //cbxVR.SelectedIndex = _dev.user[usernum].ch[channel].getLT_Re();//user.channel[channelSet].vlimit.Time_R;
                cbxVR.SelectedIndex = _dev.user[usernum].getLT_Re();
                //cbxVS.SelectedIndex = _dev.user[usernum].ch[channel].getLT_St();//user.channel[channelSet].vlimit.Time_S;
                cbxVS.SelectedIndex = _dev.user[usernum].getLT_St();
                //cbxVT.SelectedIndex = _dev.user[usernum].ch[channel].getLT_Th();//user.channel[channelSet].vlimit.T_Hlod;
                cbxVT.SelectedIndex = _dev.user[usernum].getLT_Th();
                //}
                #endregion
                //UpdateEquHl();
                UpdatePic();

            }
            else
            {

            }

            //SendOrderFlag = true;
        }
        #endregion
        #region 高低通选择
        bool hlTsMdown = false;//用于判断是否是手动改变选项，还是程序设置改变选项
        public void cbxHLTypeStep(object sender, EventArgs e)
        {
            if (hlTsMdown == true)
            {
                ComboBox cbx = (ComboBox)sender;

                string hl = cbx.Name.Substring(3, 1);   // L  --  H
                string st = cbx.Name.Substring(4, 1);   // step  type
                int num = (hl == "L") ? 0 : 1;          // L:0  ,  H:1

                string str = cbx.Name.Substring(3, 5);

                switch (st)
                {
                    case "s":
                        _dev.user[usernum].ch[channel].setHL_S((byte)(cbx.SelectedIndex), num);
                        break;
                    case "t":
                        _dev.user[usernum].ch[channel].setHL_T((byte)(cbx.SelectedIndex), num);
                        break;
                    default: break;
                }
                UpdatePic();
            }
            hlTsMdown = false;
        }

        private void cbxHLTS_MDowm(object sender, MouseEventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                hlTsMdown = true;//鼠标点击
            }
            //this.ActiveControl = tbxHF;
        }
        #endregion


        #region 频率 增益 Q值
        public void TbxKeyDown_FGQ(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            TextBox tbx = (TextBox)sender;
            string tbxType = tbx.Name.Substring(3, 1);//F Q G
            PDownNum = Convert.ToInt32(tbx.Name.Substring(4, 1));// 0 1 2 3 4 5 

            double Value = 0;
            if (e.KeyCode == Keys.Enter)
            {
                if (tbxType == "F")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (double.TryParse(((TextBox)(this.Controls.Find("tbxF" + i, true)[0])).Text, out Value) && (Value >= _eq.getMinF() && Value <= _eq.getMaxF()))
                        {
                            if (PDownNum == i)
                            {
                                _dev.user[usernum].ch[channel].setEQ_F(Value, i);//user.channel[channelSet].PvuArray[i].ValueF = Value;
                                ((TextBox)(this.Controls.Find("tbxF" + i, true)[0])).Text = Math.Round(Value, 2).ToString();
                            }
                        }
                        else
                        {
                            ((TextBox)(this.Controls.Find("tbxF" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_F(i), 2).ToString();
                        }
                    }
                }
                else if (tbxType == "G")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (double.TryParse(((TextBox)(this.Controls.Find("tbxG" + i, true)[0])).Text, out Value) && (Value >= _eq.getMinG() && Value <= _eq.getMaxG()))
                        {
                            if (PDownNum == i)
                            {
                                _dev.user[usernum].ch[channel].setEQ_G(Value, i); //user.channel[channelSet].PvuArray[i].ValueG = Value;
                                ((TextBox)(this.Controls.Find("tbxG" + i, true)[0])).Text = Math.Round(Value, 2).ToString();
                                //GgetY(Value, ref user.channel[channelSet].PxyArray[i]);
                            }
                        }
                        else
                        {
                            ((TextBox)(this.Controls.Find("tbxG" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_G(i), 2).ToString();
                        }
                    }
                }
                else if (tbxType == "Q")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (double.TryParse(((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text, out Value) && (Value >= _eq.getMinQ() && Value <= _eq.getMaxQ()))
                        {
                            if (PDownNum == i)
                            {
                                _dev.user[usernum].ch[channel].setEQ_Q(Value, PDownNum); //user.channel[channelSet].PvuArray[i].ValueQ = Value;
                                ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text = Math.Round(Value, 2).ToString();
                            }
                        }
                        else
                        {
                            ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_Q(i), 2).ToString();
                        }
                    }
                }
                else
                { }
                //calcEqValue(user.channel[channelSet].PvuArray[PDownNum].ValueF, user.channel[channelSet].PvuArray[PDownNum].ValueG, user.channel[channelSet].PvuArray[PDownNum].ValueQ, Fs);
                UpdatePic();
            }

        }
        #endregion
        #region 类型选项 cbxT0-T5 EQ，LS，HS
        bool cmbType = false;//用于判断是否是手动改变选项，还是程序设置改变选项
        public void cbxTypeChanged(object sender, EventArgs e)
        {

            if (cmbType == false)
                return;
            ComboBox tbx = (ComboBox)sender;
            string tbxType = tbx.Name.Substring(3, 1);//F Q G T
            PDownNum = Convert.ToInt32(tbx.Name.Substring(4, 1));// 0 1 2 3 4 5 

            _dev.user[usernum].ch[channel].setEQ_T((byte)tbx.SelectedIndex, PDownNum);

            if (tbx.SelectedIndex != 0)
            {
                ((TextBox)(this.Controls.Find("tbxQ" + PDownNum, true)[0])).Text = 1.ToString();
                ((TextBox)(this.Controls.Find("tbxQ" + PDownNum, true)[0])).Enabled = false;
            }
            else
            {
                ((TextBox)(this.Controls.Find("tbxQ" + PDownNum, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_Q(PDownNum), 2).ToString();
                ((TextBox)(this.Controls.Find("tbxQ" + PDownNum, true)[0])).Enabled = true;
            }

            //MessageBox.Show(((ComboBox)sender).SelectedIndex.ToString());
            UpdatePic();
            cmbType = false;
        }
        private void cbxTypeDown(object sender, MouseEventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                cmbType = true;//鼠标点击
            }
        }
        #endregion
        #region TBX高低通频率
        public void TbxKeyDown_HL(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            double Value = 0;
            TextBox tbx = (TextBox)sender;

            string hl = tbx.Name.Substring(3, 1);   // L  --  H
            int num = (hl == "L") ? 0 : 1;          // L:0  ,  H:1
            if (e.KeyCode == Keys.Enter)
            {
                if (double.TryParse(tbx.Text, out Value) == false)
                {
                    tbx.Text = Math.Round(_dev.user[usernum].ch[channel].getHL_F(num), 2).ToString();
                    return;
                }
                if ((Value < _hl.getMinF()) || Value > _hl.getMaxF())
                {
                    tbx.Text = Math.Round(_dev.user[usernum].ch[channel].getHL_F(num), 2).ToString();
                    return;
                }
                _dev.user[usernum].ch[channel].setHL_F(Value, num);
            }
            UpdatePic();
        }
        #endregion
        #region btnOpen，点击连接
        private void btnOpen_Click(object sender, EventArgs e)
        {
            switch (_facty.getState())
            {
                case _comState.Break: _facty.setState(_comState.Link); break;
                case _comState.Link: _facty.setState(_comState.Break); break;
                default: break;
            }

            if (_facty.getState() == _comState.Link)
            {
                btnOpen.BackColor = Color.LightGreen;
                btnOpen.Text = "点击断开";
                cbxCom.Enabled = false;
                cbxDev.Enabled = false;

                string dataClr = "5A00A5FF0001080000A5FF005A";  //
                string dataDev = "5A00A5FF0001050000A5FF005A";  //读取设备号   "5A00A5FF" + "0001" + "05" + "0000" + "A5FF005A";
                string dataTempUser = "5A00A5FF0001000000A5FF005A";//读取温度buf[9]+用户组buf[10]
                _facty.sendnp(dataClr);
                Thread.Sleep(200);
                byte[] buf = _facty.read();
                if (buf.Length < 1) return;
                _facty.sendnp(dataDev);
                Thread.Sleep(200);
                buf = _facty.read();
                if (buf.Length < 12) return;
                devcbx = buf[9];//////////////? 忘了干嘛用的了

                //int dev_index = int.Parse(devcbx.ToString()) -1;
                /*
                int dev_index = buf[9] - 1;
                if(dev_index >= cbxDev.Items.Count)
                {
                    cbxDev.SelectedIndex = cbxDev.Items.Count - 1;
                }
                else
                {
                    cbxDev.SelectedIndex = dev_index;
                }
                */
                //cbxDev.SelectedIndex = dev_index;
                int index = cbxDev.FindString(devcbx.ToString());
                cbxDev.SelectedIndex = index;


                //温度数据，没卵用
                _facty.sendnp(dataTempUser);

                Thread.Sleep(200);
                buf = _facty.read();
                if (buf.Length < 12) return;
                usernum = buf[10];
                labelUser.Text = usernum + "组";
                usernumload = usernum;
                cbxUserLoad.SelectedIndex = usernum;
                if (LoadOperation() == 0)
                {
                    //MessageBox.
                    if ((channel == 0) || (channel == 1))
                    {
                        btnMute.Text = "静音";
                    }


                    if (channel == 2)
                    {
                        if (_dev.user[usernum].RecentMute_ch2 == "取消静音")
                        {
                            btnMute.Text = "取消静音";
                        }
                        else
                        {
                            btnMute.Text = "静音";

                        }
                    }
                    else if (channel == 3)
                    {
                        if (_dev.user[usernum].RecentMute_ch3 == "取消静音")
                        {
                            btnMute.Text = "取消静音";
                        }
                        else
                        {
                            btnMute.Text = "静音";

                        }
                    }
                    else if (channel == 4)
                    {
                        if (cbxIn1Out3.Enabled == false)
                        {
                            btnMute.Text = "取消静音";
                            _dev.user[usernum].RecentMute_ch4 = "取消静音";
                        }
                        else
                        {
                            btnMute.Text = "静音";
                            _dev.user[usernum].RecentMute_ch4 = "静音";

                        }
                    }
                    else if (channel == 5)
                    {
                        if (_dev.user[usernum].RecentMute_ch5 == "取消静音")
                        {
                            btnMute.Text = "取消静音";
                        }
                        else
                        {
                            btnMute.Text = "静音";

                        }
                    }


                    if (cbxIn1Out3.Enabled == false)
                    {
                        _dev.user[usernum].RecentMute_ch4 = "取消静音";

                    }
                    else
                    {
                        _dev.user[usernum].RecentMute_ch4 = "静音";

                    }
                }
                else
                {
                    MessageBox.Show("加载失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            else
            {
                btnOpen.BackColor = Control.DefaultBackColor;
                btnOpen.Text = "点击连接";
                cbxCom.Enabled = true;
                cbxDev.Enabled = true;
            }
            cbxIn1Out1.CheckStateChanged += new EventHandler(cbxIn1Out1_CheckStateChanged);
            cbxIn1Out2.CheckStateChanged += new EventHandler(cbxIn1Out2_CheckStateChanged);
            cbxIn1Out3.CheckStateChanged += new EventHandler(cbxIn1Out3_CheckStateChanged);
            cbxIn1Out4.CheckStateChanged += new EventHandler(cbxIn1Out4_CheckStateChanged);
            cbxIn2Out1.CheckStateChanged += new EventHandler(cbxIn2Out1_CheckStateChanged);
            cbxIn2Out2.CheckStateChanged += new EventHandler(cbxIn2Out2_CheckStateChanged);
            cbxIn2Out3.CheckStateChanged += new EventHandler(cbxIn2Out3_CheckStateChanged);
            cbxIn2Out4.CheckStateChanged += new EventHandler(cbxIn2Out4_CheckStateChanged);
        }
        #endregion






        #region StartLine
        private void myDrawLine()
        {
            //Graphics g = gbxAdjust.CreateGraphics();
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //g.DrawLines(new Pen(Color.FromArgb(255, 255, 255), 2), new PointF[] { new PointF(1, 40), new PointF(230, 40), new PointF(230, 8) });

            //if (_dev.user[usernum].getBtl() == true)
            //{
            //    g.DrawLines(new Pen(Color.FromArgb(255, 255, 255), 2), new PointF[] { new PointF(150, 32), new PointF(150, 36), new PointF(210, 36), new PointF(210, 32) });
            //}
            //g.Dispose();
        }


        //此定时器用于开机窗口画分割线，除此别无用处
        private void timer1_Tick(object sender, EventArgs e)
        {
            myDrawLine();
            this.timer1.Enabled = false;
        }
        #endregion
        #region MyMouseDown
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private void MyMouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        #endregion
        #region btn-Min-Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        bool WinMinimized = false;
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            WinMinimized = true;
        }
        #endregion
        #region SerialCommunication
        void RefreshCom()
        {
            List<string> comList = _facty.GetCom();
            if (comList.Count == 0)
            {
                goto RefreshEnd;
            }
            for (int i = 0; i < comList.Count; i++)
            {
                cbxCom.Items.Add(comList[i]);
            }
            RefreshEnd:
            cbxCom.Items.Add("重新搜索");
            if (cbxCom.Items.Count == 1)
            {
                //btnOpen.Enabled = false;
            }
            else
            {
                //btnOpen.Enabled = true;
                cbxCom.SelectedIndex = 0;
                _facty.setPort(cbxCom.SelectedItem.ToString());
            }
        }
        private void cbxCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCom.SelectedIndex == cbxCom.Items.Count - 1)
            {
                cbxCom.Items.Clear();
                RefreshCom();
            }
            else
            {
                _facty.setPort(cbxCom.SelectedItem.ToString());
            }
        }

        #endregion
        #region tbrDelay 延时滑动 事件
        private void tbrDelay_Scroll(object sender, EventArgs e)
        {
            SetDelay(tbrDelay.Value);
        }

        void SetDelay(int value)
        {
            //if (user.channel[channelSet].delay <= 0.01) user.channel[channelSet].delay = 0;
            if (channel == 2 || channel == 3 || channel == 4 || channel == 5)
            {
                tbrDelay.Maximum = 400;
            }
            else
            {
                tbrDelay.Maximum = 800;
            }

            _dev.user[usernum].ch[channel].setDelay((double)value/*tbrDelay.Value*/);
            lbDelaySet.Text = (((double)value/*tbrDelay.Value*/) / 100).ToString("F2") + "ms";
            if (_facty.getState() == _comState.Link)
            {
                //Send
            }
            Thread.Sleep(50);//调节频率不能太高，加个50ms延时
        }
        #endregion
        #region tbrGain 增益滑动 事件
        private void tbrGain_Scroll(object sender, EventArgs e)
        {
            SetGain(tbrGain.Value);
        }
        void SetGain(int value)
        {
            _dev.user[usernum].ch[channel].setGain((double)value/*tbrGain.Value*/);
            if (value/*tbrGain.Value*/ > 0)
            {
                lbGainSet.Text = "+ " + ((double)value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
            }
            else
            {
                if (value/*tbrGain.Value*/ > -100)
                {
                    lbGainSet.Text = "- " + (-(double)value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
                }
                else
                {
                    lbGainSet.Text = "-" + (-(double)value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
                }
            }
            Thread.Sleep(50);//调节频率不能太高，加个50ms延时
        }
        #endregion

        #region pic MouseDown,MouseMove,MouseUp事件
        PointF ptMove = new PointF();
        public static int PDownNum = 0;        //均衡参数第几个点按下了（0~5）        
        double distanceRefer = 10;      //鼠标触发6个点的距离        
        bool IsTouch = false;           //鼠标是否按下（拖动6个点）        
        private static float Distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        private void picDraw_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)//System.Windows.Forms.MouseButtons
            {
                return;
            }
            int i = 0;
            double distance = 0.0;
            ptMove.X = e.X;
            ptMove.Y = e.Y;
            for (i = 0; i < 8; i++) //for ( i = 0; i < 15; i++)
            {
                if (i < 6)
                {
                    distance = Distance(ptMove, _dev.user[usernum].ch[channel].getEQ_PointF(i));
                    if (distance > distanceRefer)
                    {
                        continue;
                    }
                }
                else
                {
                    distance = Distance(ptMove, _dev.user[usernum].ch[channel].getHL_PointF(i - 6));
                    if (distance > distanceRefer)
                    {
                        continue;
                    }
                }
                IsTouch = true;
                PDownNum = i;
                break;
            }
            if (i == 8)
            {
                IsTouch = false;
                PDownNum = -1;
                return;
            }
            this.Cursor = Cursors.Hand;//变换为手型图标
        }

        private void picDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsTouch == false || PDownNum < 0 || PDownNum >= 8)
            {
                return;
            }
            ptMove.X = e.X;
            ptMove.Y = e.Y;
            if (PDownNum < 6)
            {
                _dev.user[usernum].ch[channel].setEQ_PointF(ptMove, PDownNum);
                UpdatePic();
            }
            else // 6 7
            {
                _dev.user[usernum].ch[channel].setHL_X(ptMove.X, PDownNum - 6);
                UpdatePic();
            }

        }

        private void picDraw_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsTouch == false || PDownNum < 0 || PDownNum >= 8)
            {
                return;
            }
            //if (PDownNum < 15)
            //{
            //    _dev.user[usernum].ch[channel].SendEq(PDownNum);
            //}
            //else// 15 16
            //{
            //    _dev.user[usernum].ch[channel].SendHl(PDownNum - 15);
            //}
            IsTouch = false;
            this.Cursor = Cursors.Arrow;//箭头光标
        }
        #endregion
        #region UpdatePic更新Pic
        public void UpdatePic()
        {
            g.Clear(Color.Black);           //清屏          
            DrawLine();                     //画坐标网格
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //计算低高通 0-L  1-H
            _calc.GetHLPassValue(_dev.user[usernum].ch[channel].getHL_F(0), _dev.user[usernum].ch[channel].getHL_S(0), _dev.user[usernum].ch[channel].getHL_T(0), 0);
            _calc.GetHLPassValue(_dev.user[usernum].ch[channel].getHL_F(1), _dev.user[usernum].ch[channel].getHL_S(1), _dev.user[usernum].ch[channel].getHL_T(1), 1);
            //
            //像素点对应频率    
            double[] bufTmp = new double[calcWidth];

            for (int i = 0; i < 6; i++)
            {
                switch (_dev.user[usernum].ch[channel].getEQ_T(i)/*Config.eqtype[i]*/)
                {
                    case 0://eq
                        _calc.CalcPeak(bufTmp, wholeFreq, regondraw, _dev.user[usernum].ch[channel].getEQ_G(i), _dev.user[usernum].ch[channel].getEQ_F(i), _dev.user[usernum].ch[channel].getEQ_Q(i), 48000);
                        for (int j = 0; j < calcWidth; j++)
                        {
                            freq_y[i, j] = bufTmp[j];
                        }
                        break;
                    case 1://ls
                        _calc.CalcLowShelf(bufTmp, wholeFreq, regondraw, _dev.user[usernum].ch[channel].getEQ_G(i), _dev.user[usernum].ch[channel].getEQ_F(i), _dev.user[usernum].ch[channel].getEQ_Q(i), 48000, /*double slope =*/ 1.0);
                        for (int j = 0; j < calcWidth; j++)
                        {
                            freq_y[i, j] = bufTmp[j];
                        }
                        break;
                    case 2://hs
                        _calc.CalcHighShelf(bufTmp, wholeFreq, regondraw, _dev.user[usernum].ch[channel].getEQ_G(i), _dev.user[usernum].ch[channel].getEQ_F(i), _dev.user[usernum].ch[channel].getEQ_Q(i), 48000, /*double slope =*/ 1.0);
                        for (int j = 0; j < calcWidth; j++)
                        {
                            freq_y[i, j] = bufTmp[j];
                        }
                        break;
                    default: break;
                }
            }
            //绘制曲线
            for (int i = curveLeft; i < regondraw /*calcWidth*/; i++)// calcWidth = 790; -40 ,前40个点占的太宽了
            {
                DrawPoint[i - curveLeft].X = i;

                if (_dev.user[usernum].ch[channel].getHL_F(0) > 17000)
                {
                    if (_dev.user[usernum].ch[channel].getHL_F(1) < 16)
                    {
                        DrawPoint[i - curveLeft].Y = (float)(15 - (float)(freq_y[0, i] + freq_y[1, i] + freq_y[2, i] + freq_y[3, i] + freq_y[4, i] + freq_y[5, i])) * 6 + 20;// 
                    }
                    else
                    {
                        DrawPoint[i - curveLeft].Y = (float)(15 - (float)(freq_y[0, i] + freq_y[1, i] + freq_y[2, i] + freq_y[3, i] + freq_y[4, i] + freq_y[5, i] + HighDbValue[i])) * 6 + 20;// 
                    }
                }
                else
                {
                    if (_dev.user[usernum].ch[channel].getHL_F(1) < 16)
                    {
                        DrawPoint[i - curveLeft].Y = (float)(15 - (float)(freq_y[0, i] + freq_y[1, i] + freq_y[2, i] + freq_y[3, i] + freq_y[4, i] + freq_y[5, i] + LowDbValue[i])) * 6 + 20;// 
                    }
                    else
                    {
                        DrawPoint[i - curveLeft].Y = (float)(15 - (float)(freq_y[0, i] + freq_y[1, i] + freq_y[2, i] + freq_y[3, i] + freq_y[4, i] + freq_y[5, i] + LowDbValue[i] + HighDbValue[i])) * 6 + 20;// 
                    }
                }
            }
            DrawPoint[0].X = DrawPoint[0].X + 1;        //本来该整体右移一个坐标的

            switch (channel)
            {
                case 0: pp = new Pen(Color.Yellow, (float)1.5); break;
                case 1: pp = new Pen(Color.OrangeRed, (float)1.5); break;
                case 2: pp = new Pen(Color.Green, (float)1.5); break;
                case 3: pp = new Pen(Color.Blue, (float)1.5); break;
                case 4: pp = new Pen(Color.Red, (float)1.5); break;
                case 5: pp = new Pen(Color.Purple, (float)1.5); break;
                default: break;
            }

            g.DrawLines(pp, DrawPoint);
            g.FillPolygon(bb, DrawPointBB);

            //绘制点
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            for (int i = 0; i < 6; i++)
            {
                g.DrawString((i + 1).ToString(), new Font("宋体", 8, FontStyle.Bold), sbr[i], _dev.user[usernum].ch[channel].getEQ_PointF(i).X, _dev.user[usernum].ch[channel].getEQ_PointF(i).Y + 1, sf);
                g.DrawEllipse(new Pen(sbr[i], 2), _dev.user[usernum].ch[channel].getEQ_PointF(i).X - 9, _dev.user[usernum].ch[channel].getEQ_PointF(i).Y - 9, 18, 18);
            }
            g.DrawString("L", new Font("宋体", 8, FontStyle.Bold), hlsbr[0], _dev.user[usernum].ch[channel].getHL_X(0),/*0*/ _dev.user[usernum].ch[channel].getHL_Y(0) + 1, sf);
            g.DrawEllipse(new Pen(hlsbr[0], 2), _dev.user[usernum].ch[channel].getHL_X(0) - 9, _dev.user[usernum].ch[channel].getHL_Y(0) - 9, 18, 18);
            g.DrawString("H", new Font("宋体", 8, FontStyle.Bold), hlsbr[1], _dev.user[usernum].ch[channel].getHL_X(1),/*0*/ _dev.user[usernum].ch[channel].getHL_Y(1) + 1, sf);
            g.DrawEllipse(new Pen(hlsbr[1], 2), _dev.user[usernum].ch[channel].getHL_X(1) - 9, _dev.user[usernum].ch[channel].getHL_Y(1) - 9, 18, 18);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            //UpadateEQHL_Face();
            //Console.Write("coordPoint[coordPoint.length-1] = " + coordPoint[coordPoint.Length - 1] + "\r\n"); // calcWidth = 780; 时候为 736  // calcWidth = 790; 时候为 745
            picDraw.Image = bmp;
            updateCtr();
            g.DrawString("当前通道：" + /*(channel + 1)*/getChannelName(), this.Font, Brushes.Gray, new Point(40, 5));


        }
        string getChannelName()
        {
            if (channel == 0)
                return "输入通道1";
            else if (channel == 1)
                return "输入通道2";
            else if (channel == 2)
                return "输出通道1";
            else if (channel == 3)
                return "输出通道2";
            else if (channel == 4)
                return "输出通道3";
            else
                return "输出通道4";

        }
        #endregion
        #region 画坐标线
        public void drawTemp(int a, int b)
        {
            Font ft = new Font("楷体", 5);
            g.DrawString(a + " = " + b, ft, Brushes.Gray, new Point(40, 5));
        }
        public void DrawLine()
        {
            //左边界
            g.DrawLine(new Pen(Brushes.DimGray), new Point(curveLeft, curveUp), new Point(curveLeft, curveDown));
            //竖线
            for (int i = 0; i < coordPoint.Length; i++) g.DrawLine(new Pen(Brushes.DimGray), new Point(coordPoint[i], curveUp), new Point(coordPoint[i], curveDown));
            //右边界
            g.DrawLine(new Pen(Brushes.DimGray), new Point(coordPoint[coordPoint.Length - 1] + ExtendR, curveUp), new Point(coordPoint[coordPoint.Length - 1] + ExtendR, curveDown));
            //横线
            for (int i = 0; i < 11; i++) g.DrawLine(new Pen(Brushes.DimGray), new Point(curveLeft, curveUp + Interval_H * i), new Point(coordPoint[coordPoint.Length - 1] + ExtendR, curveUp + Interval_H * i));
            //重绘上横线
            g.DrawLine(new Pen(Brushes.DimGray), new Point(curveLeft, curveUp), new Point(coordPoint[coordPoint.Length - 1] + ExtendR, curveUp));
            //重绘下横线
            g.DrawLine(new Pen(Brushes.DimGray), new Point(curveLeft, curveUp + Interval_H * 10), new Point(coordPoint[coordPoint.Length - 1] + ExtendR, curveUp + Interval_H * 10));
            //设置字体
            Font font = new Font("宋体", 9, FontStyle.Regular);
            //横坐标 单位
            for (int i = 0; i < 11; i++) g.DrawString(((15 - (i * 5)).ToString() + "dB").PadLeft(5, ' '), font, Brushes.White, new Point(3, curveUp + Interval_H * i - 5));
            //纵坐标 单位  左移12点，下移5个点，位置刚好
            for (int i = 0; i < 10; i++) g.DrawString(FreqUnit[loca[i]], font, Brushes.White, new Point(coordPoint[loca[i]] - 12, curveDown + 5));
            picDraw.Image = bmp;
        }
        #endregion
        #region 初始化Pic
        private void InitPic()
        {
            //初始化 bmp
            bmp = new Bitmap(picDraw.Width, picDraw.Height);
            //初始化 g
            g = Graphics.FromImage(bmp);
            //初始化每个点的颜色
            for (int i = 0; i < 6; i++) sbr[i] = new SolidBrush(Color.FromArgb(160, pointColor[i]));
            for (int i = 0; i < 2; i++) hlsbr[i] = new SolidBrush(Color.FromArgb(160, hlColor[i]));
            //初始化每个点频率
            for (int i = 0; i < wholeFreq.Length; i++) { wholeFreq[i] = _coordinate.x2f(i); /*Console.Write(wholeFreq[i].ToString("F2") + "\t");*/ }// GetNewF(i);
            //初始化待画线频率的点坐标
            for (int i = 0; i < coordPoint.Length; i++) coordPoint[i] = (int)Math.Round(_coordinate.f2x(FreqLine[i]), 3);
            //右边线坐标
            curveRight = ((int)coordPoint[coordPoint.Length - 1] + ExtendR);
            //坐标线
            DrawLine();
        }
        #endregion

        void updateCtr()
        {
            //延时
            if (channel == 2 || channel == 3 || channel == 4 || channel == 5)
            {
                tbrDelay.Maximum = 400;

            }
            else
            {
                tbrDelay.Maximum = 800;
            }

            tbrDelay.Value = (int)_dev.user[usernum].ch[channel].getDealy();
            lbDelaySet.Text = (((double)tbrDelay.Value/*tbrDelay.Value*/) / 100).ToString("F2") + "ms";
            //增益
            tbrGain.Value = (int)_dev.user[usernum].ch[channel].getGain();
            if (tbrGain.Value/*tbrGain.Value*/ > 0)
            {
                lbGainSet.Text = "+ " + ((double)tbrGain.Value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
            }
            else
            {
                if (tbrGain.Value/*tbrGain.Value*/ > -100)
                {
                    lbGainSet.Text = "- " + (-(double)tbrGain.Value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
                }
                else
                {
                    lbGainSet.Text = "-" + (-(double)tbrGain.Value/*tbrGain.Value*/ / 10).ToString("F2").PadRight(3, '0') + "dB";
                }
            }
            //均衡
            for (int i = 0; i < 6; i++)
            {
                ((TextBox)(this.Controls.Find("tbxF" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_F(i), 2).ToString();
                ((TextBox)(this.Controls.Find("tbxG" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_G(i), 2).ToString();
                ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_Q(i), 2).ToString();
                ((ComboBox)(this.Controls.Find("CbxT" + i, true)[0])).SelectedIndex = _dev.user[usernum].ch[channel].getEQ_T(i);
                if (((ComboBox)(this.Controls.Find("CbxT" + i, true)[0])).SelectedIndex != 0)
                {
                    ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text = 1.ToString();
                    ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Enabled = false;
                }
                else
                {
                    ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Text = Math.Round(_dev.user[usernum].ch[channel].getEQ_Q(i), 2).ToString();
                    ((TextBox)(this.Controls.Find("tbxQ" + i, true)[0])).Enabled = true;
                }
            }
            //高低通
            tbxLF.Text = Math.Round(_dev.user[usernum].ch[channel].getHL_F(0), 2).ToString(); ;
            cbxLtype.SelectedIndex = _dev.user[usernum].ch[channel].getHL_T(0);
            cbxLstep.SelectedIndex = _dev.user[usernum].ch[channel].getHL_S(0);
            tbxHF.Text = Math.Round(_dev.user[usernum].ch[channel].getHL_F(1), 2).ToString(); ;
            cbxHtype.SelectedIndex = _dev.user[usernum].ch[channel].getHL_T(1);
            cbxHstep.SelectedIndex = _dev.user[usernum].ch[channel].getHL_S(1);
            //压限
            //if (channel != 0)
            //{                
            //    cbxVS.SelectedIndex = _dev.user[usernum].ch[channel].getLT_St();
            //    cbxVR.SelectedIndex = _dev.user[usernum].ch[channel].getLT_Re();
            //    cbxVT.SelectedIndex = _dev.user[usernum].ch[channel].getLT_Th();
            //}

            cbxVS.SelectedIndex = _dev.user[usernum].getLT_St();
            cbxVR.SelectedIndex = _dev.user[usernum].getLT_Re();
            cbxVT.SelectedIndex = _dev.user[usernum].getLT_Th();

            //静音,二进四出--由路由选择静音
            //if (_dev.user[usernum].ch[channel].getMute() == _mute.Mute) btnMute.BackColor = Color.Red;
            //else btnMute.BackColor = Control.DefaultBackColor;
            //相位
            if (_dev.user[usernum].ch[channel].getPhase() == _phase.Phase) btnPhase.BackColor = Color.Red;
            else btnPhase.BackColor = Control.DefaultBackColor;

            //噪声门限
            //_dev.user[usernum].setNoise(-900);
            //tbrNoise.Value = _dev.user[usernum].getNoise();
            //tbxNoise.Text = (Math.Round(((double)tbrNoise.Value) / 10, 1) - 10).ToString("F1");// -100 -- 0

            //广播模式
            if (_board_flag == true)
            {
                btnBoard.BackColor = Color.Red;
            }
            else
            {
                btnBoard.BackColor = Control.DefaultBackColor;
            }
            //BTL mode
            //if(_dev.user[usernum].getBtl() == true)
            //{
            //    btnBtl.BackColor = Color.Red;
            //    rbtnLow.Enabled = false;

            //}
            //else
            //{
            //    btnBtl.BackColor = Control.DefaultBackColor;
            //    rbtnLow.Enabled = true;
            //}
        }

        //private void btnMute_Click(object sender, EventArgs e)
        //{
        //    _mute mute = _dev.user[usernum].ch[channel].getMute();
        //    switch (mute)
        //    {
        //        case _mute.Mute: _dev.user[usernum].ch[channel].setMute(_mute.UnMute); break;
        //        case _mute.UnMute: _dev.user[usernum].ch[channel].setMute(_mute.Mute); break;
        //    }
        //    UpdatePic();
        //}
        #region BTN相位
        private void btnPhase_Click(object sender, EventArgs e)
        {
            _phase phase = _dev.user[usernum].ch[channel].getPhase();
            //_mute muted = _dev.user[usernum].ch[channel].getMute();
            //_dev.user[usernum].ch[channel].setMute(_mute.Mute);//反相操作前先静音
            switch (phase)
            {
                case _phase.Phase:
                    _dev.user[usernum].ch[channel].setPhase(_phase.UnPhase); //反相会有噼啪声，所以提前静音    
                    break;
                case _phase.UnPhase:
                    _dev.user[usernum].ch[channel].setPhase(_phase.Phase);
                    break;
            }
            Thread.Sleep(100);
            //if (muted == _mute.UnMute) _dev.user[usernum].ch[channel].setMute(_mute.UnMute);//恢复之前静音状态

            UpdatePic();
        }
        #endregion

        #region BTN 0dB
        void delay_10ms(int x)
        {
            //picDelay.Visible = true;
            //this.picDelay.Invalidate();
            //loadingCircle1.Visible = true;
            // loadingCircle1.Active = true;
            // picDelay.Refresh();
            for (int i = 0; i < x; i++)
            {
                delayi++;
                //if (delayi == 3) delayi = 0;
                // picDelay.Image = bitmapdelay[delayi];
                // loadingCircle1.Active = true;
                //this.picDelay.Invalidate();
                // picDelay.Refresh();
                Thread.Sleep(10);
            }

            //picDelay.Visible = false;
            //loadingCircle1.Visible = false;
            // this.picDelay.Invalidate();
            // picDelay.Refresh();
        }
        public static bool odbflag = false;
        private void btn0dB_Click(object sender, EventArgs e)
        {

            if (_facty.getState() == _comState.Break) return;
#if true
            odbflag = true;
            // _dev.user[usernum].ch[channel] = new _channel();
            for (int i = 0; i < 6; i++)
            {
                _dev.user[usernum].ch[channel].setEQ_G(0, i);
                delay_10ms(10);// Thread.Sleep(100);
            }

            _dev.user[usernum].ch[channel].setHL_F(15.5, 1);
            delay_10ms(10);//Thread.Sleep(200);
            _dev.user[usernum].ch[channel].setHL_F(20000, 0);
            delay_10ms(10);//Thread.Sleep(200);

            odbflag = false;


#else

            _dev.user[usernum].ch[channel] = new _channel();

            string start = "5A00A5FF";
            string devuser = devcbx.ToString("X2") + usernum.ToString("X2");
            string end = "A5FF005A";
            string eqData = "";
            for (int i = 0; i < 6; i++)
            {
                eqData += _dev.user[usernum].ch[channel].getEquData(devcbx, usernum, channel, i);
            }
            string sdev = devcbx.ToString("X2");
            string suser = usernum.ToString("X2");
            string scmd = "01";
            string slength = "01C2";//30 * 15
            string order = start + sdev + suser + scmd + slength + eqData + end;
            _facty.send(order);
#endif
            UpdatePic();
        }
        #endregion
        #region BTN btnDev，修改设备号
        private void btnDev_Click(object sender, EventArgs e)
        {
            if (_facty.getState() == _comState.Break) return;
            int newdevnum = 0;
            if (int.TryParse(tbxDevMoy.Text, out newdevnum) == false)
            {
                tbxDevMoy.Text = "";
                return;
            }
            //MessageBox.Show(newdevnum.ToString());
            //return;
            if (newdevnum < 1 || newdevnum > 255)
            {
                tbxDevMoy.Text = "";
                MessageBox.Show("请输入1-255编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = true;
                return;
            }
            if (_facty.getState() == _comState.Link)
            {
                string start = "5A00A5FF";
                string device = devcbx.ToString("X2");
                string usergroup = usernum.ToString("X2");
                string cmds = 4.ToString("X2");
                string length = "0001";
                string data = newdevnum.ToString("X2");
                string end = "A5FF005A";
                string order = start + device + usergroup + cmds + length + data + end;
                _facty.send(order);
                Thread.Sleep(200);
                byte[] buf = _facty.read();
                if (buf.Length > 0)
                {
                    int olddevnum = devcbx;
                    devcbx = newdevnum;
                    //cbxDev.SelectedIndex = newdevnum - 1;
                    if (cbxDev.Items.Contains(newdevnum) == false)
                    {
                        cbxDev.Items.Add(devcbx);
                        List<int> list = new List<int>();
                        for (int i = 0; i < cbxDev.Items.Count; i++)
                        {
                            list.Add(int.Parse(cbxDev.Items[i].ToString()));
                        }
                        list.Sort();
                        cbxDev.Items.Clear();
                        for (int i = 0; i < list.Count; i++)
                        {
                            cbxDev.Items.Add(list[i]);
                        }
                        int index = cbxDev.FindString(devcbx.ToString());
                        cbxDev.SelectedIndex = index;
                    }
                    else
                    {
                        int index = cbxDev.FindString(devcbx.ToString());
                        cbxDev.SelectedIndex = index;
                    }
                    if ((olddevnum != devcbx) && (cbxDev.Items.Contains(olddevnum) == true))
                    {
                        cbxDev.Items.Remove(olddevnum);
                    }
                    MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = true;
                }
            }
        }
        #endregion

        #region BTN保存到电脑
        private void btnSaveCom_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入要保存.DSP的文件名";
            sfd.InitialDirectory = @"C:\Users\obil\Desktop";
            sfd.Filter = "DSP参数|*.DSP";
            sfd.ShowDialog();
           
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
               
                BinaryFormatter bf = new BinaryFormatter();
                //MessageBox.Show(cbxIn1Out1.Checked.ToString() + cbxIn1Out2.Checked.ToString() + cbxIn1Out3.Checked.ToString() + cbxIn1Out4.Checked.ToString() + cbxIn2Out1.Checked.ToString() + cbxIn2Out2.Checked.ToString() + cbxIn2Out3.Checked.ToString() + cbxIn2Out4.Checked.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(_dev.user[usernum].switchChannel[0].getMute().ToString() + _dev.user[usernum].switchChannel[0].getMute().ToString() + _dev.user[usernum].switchChannel[2].getMute().ToString() + _dev.user[usernum].switchChannel[3].getMute().ToString() + _dev.user[usernum].switchChannel[4].getMute().ToString() + _dev.user[usernum].switchChannel[5].getMute().ToString() + _dev.user[usernum].switchChannel[6].getMute().ToString() + _dev.user[usernum].switchChannel[7].getMute().ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bf.Serialize(fs, _dev.user[usernum]);
                fs.Close();
            }


        }

        #endregion
        public static int nor = 0xff;
        #region BTN从电脑加载
        private void btnLoadCom_Click(object sender, EventArgs e)
        {
            if (_facty.getState() == _comState.Break) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择需要打开的.DSP文档";
            ofd.Multiselect = false;
            ofd.InitialDirectory = @"C:\Users\obil\Desktop";
            ofd.Filter = "DSP参数|*.DSP";
            ofd.ShowDialog();
            string path = ofd.FileName;
            if (path == "")
            {
                return;
            }
            lblState.Text = "正在从电脑加载，请稍等...";
            lblState.Show();
            // usernum = cbxUserLoad.SelectedIndex;
            using (FileStream fr = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {

                BinaryFormatter br = new BinaryFormatter();

                _dev.user[usernum] = (_user)br.Deserialize(fr);

              
               // fr.Close();
                
            }

        //    rbtnIn1.Checked = true;
           
            for (int i = 0; i < 8; i++)
            {
                _mute mute = _dev.user[usernum].switchChannel[i].getMute();
                switch (mute)
                {
                    case _mute.UnMute:
                        _dev.user[usernum].switchChannel[i].setMute(_mute.UnMute,nor);
                        if (i == 0)
                        {
                            // cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);
                            cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);
                            cbxIn1Out1.Checked = true;
                            //_dev.user[usernum].switchChannel[0].setMute(_mute.UnMute);
                        }
                        else if (i == 1)
                        {
                            cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);

                            cbxIn1Out2.Checked = true;
                        }
                        else if (i == 2)
                        {
                            cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);

                            cbxIn1Out3.Checked = true;
                        }
                        else if (i == 3)
                        {
                            cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);

                            cbxIn1Out4.Checked = true;
                        }
                        else if (i == 4)
                        {
                            cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);

                            cbxIn2Out1.Checked = true;
                        }
                        else if (i == 5)
                        {
                            cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);

                            cbxIn2Out2.Checked = true;
                        }
                        else if (i == 6)
                        {

                            cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);

                            cbxIn2Out3.Checked = true;
                        }
                        else
                        {
                            cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);

                            cbxIn2Out4.Checked = true;
                        }
                        break;
                    case _mute.Mute:
                        _dev.user[usernum].switchChannel[i].setMute(_mute.Mute,nor);
                        if (i == 0)
                        {
                            if (_dev.user[usernum].cbxIn1Out1_IsCheck == true)
                            {
                                cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);


                                cbxIn1Out1.Checked = true;
                            }
                            else
                            {
                                cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);

                                cbxIn1Out1.Checked = false;
                            }
                        }
                        else if (i == 1)
                        {
                            if (_dev.user[usernum].cbxIn1Out2_IsCheck == true)

                            {
                                cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);


                                cbxIn1Out2.Checked = true;
                            }
                            else
                            {
                                cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);

                                cbxIn1Out2.Checked = false;

                            }
                        }
                        else if (i == 2)
                        {
                            if (_dev.user[usernum].cbxIn1Out3_IsCheck == true)

                            {
                                cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);


                                cbxIn1Out3.Checked = true;
                            }
                            else
                            {
                                cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);

                                cbxIn1Out3.Checked = false;

                            }
                        }
                        else if (i == 3)
                        {
                            if (_dev.user[usernum].cbxIn1Out4_IsCheck == true)

                            {
                                cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);


                                cbxIn1Out4.Checked = true;
                            }
                            else
                            {
                                cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);

                                cbxIn1Out4.Checked = false;

                            }
                        }
                        else if (i == 4)
                        {
                            if (_dev.user[usernum].cbxIn2Out1_IsCheck == true)

                            {
                                cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);
                                cbxIn2Out1.Checked = true;
                            }
                            else
                            {
                                cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);

                                cbxIn2Out1.Checked = false;

                            }
                        }
                        else if (i == 5)
                        {
                            if (_dev.user[usernum].cbxIn2Out2_IsCheck == true)

                            {
                                cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);
                                cbxIn2Out2.Checked = true;
                            }
                            else
                            {
                                cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);

                                cbxIn2Out2.Checked = false;

                            }
                        }
                        else if (i == 6)
                        {
                            if (_dev.user[usernum].cbxIn2Out3_IsCheck == true)

                            {
                                cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);
                                cbxIn2Out3.Checked = true;
                            }
                            else
                            {
                                cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);

                                cbxIn2Out3.Checked = false;

                            }
                        }
                        else
                        {
                            if (_dev.user[usernum].cbxIn2Out4_IsCheck == true)
                            {
                                cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);
                                cbxIn2Out4.Checked = true;
                            }
                            else
                            {
                                cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);
                                cbxIn2Out4.Checked = false;

                            }
                        }
                        break;
                    default: break;
                }
            }

            cbxIn1Out1.CheckStateChanged += new EventHandler(cbxIn1Out1_CheckStateChanged);
            cbxIn1Out2.CheckStateChanged += new EventHandler(cbxIn1Out2_CheckStateChanged);
            cbxIn1Out3.CheckStateChanged += new EventHandler(cbxIn1Out3_CheckStateChanged);
            cbxIn1Out4.CheckStateChanged += new EventHandler(cbxIn1Out4_CheckStateChanged);
            cbxIn2Out1.CheckStateChanged += new EventHandler(cbxIn2Out1_CheckStateChanged);
            cbxIn2Out2.CheckStateChanged += new EventHandler(cbxIn2Out2_CheckStateChanged);
            cbxIn2Out3.CheckStateChanged += new EventHandler(cbxIn2Out3_CheckStateChanged);
            cbxIn2Out4.CheckStateChanged += new EventHandler(cbxIn2Out4_CheckStateChanged);
            if (_dev.user[usernum].RecentMute_ch2 == "取消静音")
            {
                cbxIn1Out1.Enabled = false;
                cbxIn2Out1.Enabled = false;
                if(rbtnOut1.Checked==true)
                {
                    btnMute.Text = "取消静音";
                }
                else
                { }
            }
            else
            {
                cbxIn1Out1.Enabled = true;
                cbxIn2Out1.Enabled = true;
                if (rbtnOut1.Checked == true)
                {
                    btnMute.Text = "静音";
                }
                else
                { }
            }
            if (_dev.user[usernum].RecentMute_ch3 == "取消静音")
            {
                cbxIn1Out2.Enabled = false;
                cbxIn2Out2.Enabled = false;
                if (rbtnOut2.Checked == true)
                {
                    btnMute.Text = "取消静音";
                }
                else
                { }
            }
            else
            {
                cbxIn1Out2.Enabled = true;
                cbxIn2Out2.Enabled = true;
                if (rbtnOut2.Checked == true)
                {
                    btnMute.Text = "静音";
                }
                else
                { }
            }
            if (_dev.user[usernum].RecentMute_ch4 == "取消静音")
            {
                cbxIn1Out3.Enabled = false;
                cbxIn2Out3.Enabled = false;
                if (rbtnOut3.Checked == true)
                {
                    btnMute.Text = "取消静音";
                }
                else
                { }
            }
            else
            {
                cbxIn1Out3.Enabled = true;
                cbxIn2Out3.Enabled = true;
                if (rbtnOut3.Checked == true)
                {
                    btnMute.Text = "静音";
                }
                else
                { }
            }
            if (_dev.user[usernum].RecentMute_ch5 == "取消静音")
            {
                cbxIn1Out4.Enabled = false;
                cbxIn2Out4.Enabled = false;
                if (rbtnOut4.Checked == true)
                {
                    btnMute.Text = "取消静音";
                }
                else
                { }
            }
            else
            {
                cbxIn1Out4.Enabled = true;
                cbxIn2Out4.Enabled = true;
                if (rbtnOut4.Checked == true)
                {
                    btnMute.Text = "静音";
                }
                else
                { }
            }



            

            UpdatePic();

            if (_facty.getState() == _comState.Break) return;

            //发送同步到设备
            //channelSend(0);
            //channelSend(1);
            //channelSend(2);
            //channelSend(3);
            //MessageBox.Show("从电脑加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //同时保存到设备
            if (SaveOpreation(usernum) == 0)
            {
                lblState.Text = "从电脑加载成功";
                MessageBox.Show("从电脑加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblState.Hide();
            }
            else
            {
                lblState.Text = "从电脑加载失败";
                MessageBox.Show("从电脑加载失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            //cbxIn1Out1.CheckStateChanged += new EventHandler(cbxIn1Out1_CheckStateChanged);
            //cbxIn1Out2.CheckStateChanged += new EventHandler(cbxIn1Out2_CheckStateChanged);
            //cbxIn1Out3.CheckStateChanged += new EventHandler(cbxIn1Out3_CheckStateChanged);
            //cbxIn1Out4.CheckStateChanged += new EventHandler(cbxIn1Out4_CheckStateChanged);
            //cbxIn2Out1.CheckStateChanged += new EventHandler(cbxIn2Out1_CheckStateChanged);
            //cbxIn2Out2.CheckStateChanged += new EventHandler(cbxIn2Out2_CheckStateChanged);
            //cbxIn2Out3.CheckStateChanged += new EventHandler(cbxIn2Out3_CheckStateChanged);
            //cbxIn2Out4.CheckStateChanged += new EventHandler(cbxIn2Out4_CheckStateChanged);

            timer1.Enabled = true;
        }
        #endregion

        public enum _cbxSwitch
        {
            cbxIn1Out1,
            cbxIn1Out2,
            cbxIn1Out3,
            cbxIn1Out4,
            cbxIn2Out1,
            cbxIn2Out2,
            cbxIn2Out3,
            cbxIn2Out4
        };

        /*
            int userSend(int _usernum)  此函数弃用，有问题
        */
        #region 发送函数，这里要改，因为要发送两次
        int userSend(int _usernum)
        {
            //无BTL模式
            //if (_dev.user[Form1.usernum].getBtl() == true)
            //{
            //    _dev.user[usernum].ch[3] = DeepCopyByBin<_channel>(_dev.user[usernum].ch[2]);
            //}
            string alldata = _dev.user[usernum].ch[channel].getAllData();
            int length = alldata.Length / 2;
            string start = "5A00A5FF";
            string device = devcbx.ToString("X2");
            string usergroup = _usernum.ToString("X2");
            string cmds = "01";
            string num = length.ToString("X4");
            string end = "A5FF005A";
            string order = start + device + usergroup + cmds + num + alldata + end;
            _facty.sendnp(order);
            delay_100ms(30);
            if (_board_flag == false)//非广播模式
            {
                byte[] buf = _facty.read();
                if (buf.Length == 0)
                {
                    return -1;// MessageBox.Show("从电脑加载失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return 0;// MessageBox.Show("从电脑加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return -1;
        }
        #endregion
        #region BTN广播模式
        int generalDev = 1;
        public static bool _board_flag = false;
        private void btnBoard_Click(object sender, EventArgs e)
        {
            if (_facty.getState() == _comState.Link)
            {
                if (btnBoard.BackColor != Color.Red)
                {
                    _board_flag = true;
                    //generalDev = Device.getDevNum();
                    //Device.setDevNum(0);
                    generalDev = devcbx;
                    devcbx = 0;
                    //btnBoard.BackColor = Color.Red;
                    cbxDev.Enabled = false;
                    btnSave.Enabled = false;
                    btnLoad.Enabled = false;
                    btnSaveCom.Enabled = false;
                    btnLoadCom.Enabled = false;
                    tbxDevMoy.Enabled = false;
                    btnDev.Enabled = false;
                }
                else
                {
                    _board_flag = false;
                    //Device.setDevNum(generalDev);
                    devcbx = generalDev;
                    //btnBoard.BackColor = Control.DefaultBackColor;
                    cbxDev.Enabled = false;
                    btnSave.Enabled = true;
                    btnLoad.Enabled = true;
                    btnSaveCom.Enabled = true;
                    btnLoadCom.Enabled = true;
                    tbxDevMoy.Enabled = true;
                    btnDev.Enabled = true;
                }
            }
            updateCtr();
        }
        #endregion

        void delay_5ms(int x)
        {
            //picDelay.Visible = true;
            // this.picDelay.Invalidate();
            // picDelay.Refresh();
            //loadingCircle1.Visible = true;
            for (int i = 0; i < x; i++)
            {
                delayi++;
                Thread.Sleep(5);
            }
        }


        #region ComboBox压限
        bool b_Lt_Down = false;
        private void cbxLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (b_Lt_Down == true)
            {
                b_Lt_Down = false;
                ComboBox cbx = (ComboBox)sender;
                string TRS = cbx.Name.Substring(4, 1);   // Thread Start Release
                switch (TRS)
                {
                    //case "T": _dev.user[usernum].ch[channel].setLT_Th(cbx.SelectedIndex); break;
                    case "T": _dev.user[usernum].setLT_Th(cbx.SelectedIndex); break;
                    //case "R": _dev.user[usernum].ch[channel].setLT_Re(cbx.SelectedIndex); break;
                    case "R": _dev.user[usernum].setLT_Re(cbx.SelectedIndex); break;
                    //case "S": _dev.user[usernum].ch[channel].setLT_St(cbx.SelectedIndex); break;
                    case "S": _dev.user[usernum].setLT_St(cbx.SelectedIndex); break;
                    default: break;
                }
            }

        }
        private void cbxLimit_MouseDown(object sender, MouseEventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                b_Lt_Down = true;
            }
        }
        #endregion
        #region BTN搜索设备
        List<int> DeviceList = new List<int>();
        void searchDevice()
        {
            int cycle = 0;
            try
            {
                bool collect = false;
                string orStr = "";
                if (_facty.getState() == _comState.Link)
                {
                    collect = true;
                    orStr = cbxDev.SelectedItem.ToString();
                }
                else
                {
                    collect = false;
                    _facty.setState(_comState.Link);
                }

                DeviceList.Clear();
                _facty.sendnp("5A00A5FF" + "0001" + "08" + "0000" + "A5FF005A");
                Thread.Sleep(200);
                byte[] readBuffer = _facty.read();
                int count = readBuffer.Length;
                if (count == 4)
                {
                    if (readBuffer[0] == 0X5A && readBuffer[1] == 0XA5 && readBuffer[2] == 0X5A && readBuffer[3] == 0XA5)
                    {
                        while (true)
                        {
                            _facty.sendnp("5A00A5FF" + "0001" + "05" + "0000" + "A5FF005A");
                            Thread.Sleep(200);
                            readBuffer = _facty.read();
                            count = readBuffer.Length;
                            if (count > 5)
                            {
                                cycle = 0;
                                DeviceList.Add(readBuffer[4]);
                            }
                            else
                            {
                                cycle++;
                                if (cycle == 3)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (DeviceList.Count > 0)
                {
                    string search = "";
                    DeviceList.Sort();
                    cbxDev.Items.Clear();

                    for (int i = 0; i < DeviceList.Count; i++)
                    {
                        search += "搜到设备号：" + DeviceList[i].ToString() + "\r\n";
                        cbxDev.Items.Add(DeviceList[i]);
                    }


                    if (collect == true)
                    {
                        int index = cbxDev.FindString(orStr);
                        cbxDev.SelectedIndex = index;
                        devcbx = int.Parse(cbxDev.SelectedItem.ToString());
                        //lbState.Text = "已连接";
                    }
                    else
                    {
                        cbxDev.SelectedIndex = 0;
                        devcbx = int.Parse(cbxDev.SelectedItem.ToString());
                        cbxCom.Enabled = true;

                    }
                    MessageBox.Show(search, "搜索结果");
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("没有搜到任何设备", "搜索结果");
                    timer1.Enabled = true;
                }
            }
            catch
            {

            }

            _facty.setState(_comState.Break);
            cbxCom.Enabled = true;
            cbxDev.Enabled = true;
            btnOpen.BackColor = Control.DefaultBackColor;
            btnOpen.Text = "点击连接";
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchDevice();
        }
        #endregion

        #region 自动编号
        private void btnCode_Click(object sender, EventArgs e)
        {
            int cycle = 0;
            try
            {
                int newNum = 1;
                if (_facty.getState() == _comState.Link)
                {
                }
                else
                {
                    _facty.setState(_comState.Link);
                }
                _facty.sendnp("5A00A5FF" + "0001" + "08" + "0000" + "A5FF005A");
                Thread.Sleep(200);
                byte[] readBuffer = _facty.read();
                int count = readBuffer.Length;
                if (count == 4)
                {
                    if (readBuffer[0] == 0X5A && readBuffer[1] == 0XA5 && readBuffer[2] == 0X5A && readBuffer[3] == 0XA5)
                    {
                        cbxDev.Items.Clear();
                        while (true)
                        {
                            SendSetDevice(4, newNum);
                            Thread.Sleep(200);
                            readBuffer = _facty.read();
                            count = readBuffer.Length;
                            if (count == 4)
                            {
                                cycle = 0;
                                if (readBuffer[0] == 0X5A && readBuffer[1] == 0XA5 && readBuffer[2] == 0X5A && readBuffer[3] == 0XA5)
                                {
                                    cbxDev.Items.Add(newNum.ToString());
                                    newNum++;
                                }
                            }
                            else
                            {
                                cycle++;
                                if (cycle == 3)
                                {
                                    if (cbxDev.Items.Count > 0)
                                    {
                                        cbxDev.SelectedIndex = 0;
                                        _facty.setState(_comState.Break);

                                        cbxCom.Enabled = true;
                                        MessageBox.Show("     编号成功\r\n一共有" + cbxDev.Items.Count.ToString() + "台设备");
                                        timer1.Enabled = true;
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("编号失败");
                                        timer1.Enabled = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer1.Enabled = true;
                _facty.setState(_comState.Break);
                cbxCom.Enabled = true;
                cbxDev.Enabled = true;
                btnOpen.BackColor = Control.DefaultBackColor;
                btnOpen.Text = "点击连接";
                //lbState.Text = "未连接";
            }
            _facty.setState(_comState.Break);
            cbxCom.Enabled = true;
            cbxDev.Enabled = true;
            btnOpen.BackColor = Control.DefaultBackColor;
            btnOpen.Text = "点击连接";
        }



        public void SendSetDevice(int GeneralCmd, int newDevice)
        {

            string start = "5A00A5FF";
            //string device = oldDevice.ToString("X2");
            string device = 0.ToString("X2");
            string usergroup = usernum.ToString("X2");
            string cmds = GeneralCmd.ToString("X2");
            string num_h = "00";
            string num_l = "01";
            string data = newDevice.ToString("X2");
            string end = "A5FF005A";
            string order = start + device + usergroup + cmds + num_h + num_l + data + end;
            if (GeneralCmd == 4)
            {
            }
            _facty.sendnp(order);
        }
        #endregion

        void delay(int x)
        {
            var t = Task.Run(async delegate
              {
                  await Task.Delay(x * 100);

              });

        }

        #region BTN保存到设备()组
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (_facty.getState() == _comState.Break) return;
            lblState.Show();
            lblState.Text = "正在保存，请稍等...";
            if (SaveOpreation(usernumsave) == 0)
            {
                lblState.Text = "保存成功！";
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //loadingCircle1.Visible = false;
                lblState.Hide();
                // loadingCircle2.Hide();

            }
            else
            {
                lblState.Text = "保存失败,请检查连接情况";
                MessageBox.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //loadingCircle1.Visible = false;
                lblState.Hide();
                // loadingCircle2.Hide();
            }

            this.timer1.Enabled = true;
        }
        #endregion
        #region 从设备加载（）组
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (_facty.getState() == _comState.Break) return;
            labelUser.Text = usernumload + "组";
            usernum = usernumload;
            lblState.Text = "正在加载,请稍等...";

            
            if (LoadOperation() == 0)
            {
                //先发送的指令，再弹的窗
                //  delay_100ms(30);
                #region

                #endregion
                if ((channel == 0) || (channel == 1))
                {
                    btnMute.Text = "静音";
                }
              

                if(channel==2)
                {
                    if(_dev.user[usernum].RecentMute_ch2=="取消静音")
                    {
                        btnMute.Text = "取消静音";
                    }
                    else
                    {
                        btnMute.Text = "静音";

                    }
                }
                else if (channel == 3)
                {
                    if (_dev.user[usernum].RecentMute_ch3 == "取消静音")
                    {
                        btnMute.Text = "取消静音";
                    }
                    else
                    {
                        btnMute.Text = "静音";

                    }
                }
                else if (channel == 4)
                {
                    if ( cbxIn1Out3.Enabled == false)
                    {
                        btnMute.Text = "取消静音";
                        _dev.user[usernum].RecentMute_ch4 ="取消静音";
                    }
                    else
                    {
                        btnMute.Text = "静音";
                        _dev.user[usernum].RecentMute_ch4 = "静音";

                    }
                }
                else if (channel == 5)
                {
                    if (_dev.user[usernum].RecentMute_ch5 == "取消静音")
                    {
                        btnMute.Text = "取消静音";
                    }
                    else
                    {
                        btnMute.Text = "静音";

                    }
                }


                if (cbxIn1Out3.Enabled == false)
                {
                    _dev.user[usernum].RecentMute_ch4 = "取消静音";

                }
                else
                {
                    _dev.user[usernum].RecentMute_ch4 = "静音";

                }


                lblState.Text = "加载成功";
                //tbxQ0.Enabled = false;

                MessageBox.Show("加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblState.Hide();

            }
            else
            {
                lblState.Text = "加载失败，请检查设备连接";
                MessageBox.Show("加载失败,请检查设备连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblState.Hide();
            }
            cbxIn1Out1.CheckStateChanged += new EventHandler(cbxIn1Out1_CheckStateChanged);
            cbxIn1Out2.CheckStateChanged += new EventHandler(cbxIn1Out2_CheckStateChanged);
            cbxIn1Out3.CheckStateChanged += new EventHandler(cbxIn1Out3_CheckStateChanged);
            cbxIn1Out4.CheckStateChanged += new EventHandler(cbxIn1Out4_CheckStateChanged);
            cbxIn2Out1.CheckStateChanged += new EventHandler(cbxIn2Out1_CheckStateChanged);
            cbxIn2Out2.CheckStateChanged += new EventHandler(cbxIn2Out2_CheckStateChanged);
            cbxIn2Out3.CheckStateChanged += new EventHandler(cbxIn2Out3_CheckStateChanged);
            cbxIn2Out4.CheckStateChanged += new EventHandler(cbxIn2Out4_CheckStateChanged);

            timer1.Enabled = true;
        }

        #endregion

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



        int SaveOpreation(int _usernum)
        {
            //二进四出要发两次，保存到设备要发两次真实数据，还要发一次界面数据
            //goto Symbol;
            //this.TopMost = true;
            //无BTL模式
            string alldata = _dev.user[usernum].ch[channel].getAllData();
            int length = alldata.Length / 2;
            string start = "5A00A5FF";
            string device = devcbx.ToString("X2");
            string usergroup = _usernum.ToString("X2");

            string cmds = "02";
            string num = length.ToString("X4");
            string end = "A5FF005A";
            //第一次发送的数据  
            string order = start + device + usergroup + cmds + num + alldata + end;

            _facty.sendnp(order);

            //loadingCircle1.Visible = true;
            //loadingCircle1.Active = true;
            //delay(30);


            delay_100ms(30);
            
            //第二次发送,EQ数据,注意帧格式
            string alldata_2 = _dev.user[usernum].ch[channel].getEQUdata();
            int length_2 = alldata_2.Length / 2;
            string start_2 = "5A00A5FF";
            string device_2 = devcbx.ToString("X2");
            string usergroup_2 = _usernum.ToString("X2");
            string cmds_2 = "0C";
            string num_2 = length_2.ToString("X4");
            //string end_2 = "A5FF005A";
            string order_2 = start_2 + device_2 + usergroup_2 + cmds_2 + num_2 + alldata_2 + end;
            _facty.sendnp(order_2);
            delay_100ms(30);
            if (_board_flag == false)//非广播模式
            {
                byte[] buf = _facty.read();
                //string str = "";
                if (buf.Length == 0)
                {
                    return -1;
                }
            }
            byte[] guidata = getAllguiData();//
            //MessageBox.Show("guidata.length=:" + guidata.Length);
            byte[] guiorder = new byte[9 + guidata.Length + 4];
            guiorder[0] = 0X5A;
            guiorder[1] = 0X00;
            guiorder[2] = 0XA5;
            guiorder[3] = 0XFF;
            guiorder[4] = (byte)devcbx;
            guiorder[5] = (byte)_usernum;
            guiorder[6] = 0X0A;
            //MessageBox.Show(guidata.Length.ToString());
            guiorder[7] = (byte)(guidata.Length / 256);// (byte)(((820- 13) /256 ));
            guiorder[8] = (byte)(guidata.Length % 256);// (byte)((820 - 13) & 0xff);
            Buffer.BlockCopy(guidata, 0, guiorder, 9, guidata.Length);
            guiorder[9 + guidata.Length + 0] = 0xA5;
            guiorder[9 + guidata.Length + 1] = 0xFF;
            guiorder[9 + guidata.Length + 2] = 0x00;
            guiorder[9 + guidata.Length + 3] = 0x5A;

            _facty.send(guiorder);
            //myDelay(300, "正在保存用户组信息……");
            //delayStart();

            delay_100ms(30); //Thread.Sleep(3000);30*100
            //Delay pbr1 = new Delay(this, "正在保存用户组信息……", Color.LightGray);
            //pbr1.Show();
            //this.Enabled = false;
            //pbr1.SubTest(300);
            //pbr1.Close();
            //this.Enabled = true;
            //#endregion 

            //this.TopMost = false;

            return 0;
        }

        public byte[] getCh2_Mute()
        {
            byte[] ch2 = new byte[1];
            ch2[0] = 0;
            //ch2=(byte)_dev.user[usernum].switchChannel[0].getMute();
            if (_dev.user[usernum].RecentMute_ch2 =="取消静音")
            {
                //ch2 |=BitConverter.GetBytes(0X01);
                ch2[0] |= 0X01;
            }
            else
            {
                ch2[0] &= 0XFE;
            }
            if (cbxIn1Out1.CheckState == CheckState.Checked)
            {
                ch2[0] |= 0X02;
            }
            else
            {
                ch2[0] &= 0XFD;
            }
            if (cbxIn2Out1.CheckState == CheckState.Checked)
            {
                ch2[0] |= 0X04;
            }
            else
            {
                ch2[0] &= 0XFB;
            }
            return ch2;

        }

        public byte[] getCh3_Mute()
        {
            byte[] ch3 = new byte[1];
            ch3[0] = 0X00;
            //ch2=(byte)_dev.user[usernum].switchChannel[0].getMute();
            if (_dev.user[usernum].RecentMute_ch3 == "取消静音")
            {
                //ch2 |=BitConverter.GetBytes(0X01);
                ch3[0] |= 0X01;
            }
            else
            {
                ch3[0] &= 0XFE;
            }
            if (cbxIn1Out2.CheckState == CheckState.Checked)
            {
                ch3[0] |= 0X02;
            }
            else
            {
                ch3[0] &= 0XFD;
            }
            if (cbxIn2Out2.CheckState == CheckState.Checked)
            {
                ch3[0] |= 0X04;
            }
            else
            {
                ch3[0] &= 0XFB;
            }
            return ch3;

        }
        public byte[] getCh4_Mute()
        {
            byte[] ch4 = new byte[1];
            ch4[0] = 0X00;
            //ch4=(byte)_dev.user[usernum].switchChannel[0].getMute();
            if (_dev.user[usernum].RecentMute_ch4 == "取消静音")
            {
                //ch4 |=BitConverter.GetBytes(0X01);
                ch4[0] |= 0X01;
            }
            else
            {
                ch4[0] &= 0XFE;
            }
            if (cbxIn1Out3.CheckState == CheckState.Checked)
            {
                ch4[0] |= 0X02;
            }
            else
            {
                ch4[0] &= 0XFD;
            }
            if (cbxIn2Out3.CheckState == CheckState.Checked)
            {
                ch4[0] |= 0X04;
            }
            else
            {
                ch4[0] &= 0XFB;
            }
            return ch4;

        }
        public byte[] getCh5_Mute()
        {
            byte[] ch5 = new byte[1];
            ch5[0] = 0X00;
            //ch5=(byte)_dev.user[usernum].switchChannel[0].getMute();
            if (_dev.user[usernum].RecentMute_ch5 == "取消静音")
            {
                //ch5 |=BitConverter.GetBytes(0X01);
                ch5[0] |= 0X01;
            }
            else
            {
                ch5[0] &= 0XFE;
            }
            if (cbxIn1Out4.CheckState == CheckState.Checked)
            {
                ch5[0] |= 0X02;
            }
            else
            {
                ch5[0] &= 0XFD;
            }
            if (cbxIn2Out4.CheckState == CheckState.Checked)
            {
                ch5[0] |= 0X04;
            }
            else
            {
                ch5[0] &= 0XFB;
            }
            return ch5;

        }

        //_user us = new _user();
        byte[] getAllguiData()
        {


            byte[] bufChMute_2=new byte[1];
            byte[] bufChMute_3 = new byte[1];
            byte[] bufChMute_4 = new byte[1];
            byte[] bufChMute_5 = new byte[1];

            // byte[] bufMute = new byte[4];
            byte[] bufMute = new byte[8];
            // byte[] bufMute_2 = new byte[8];

            //byte[] bufPhase = new byte[4];
            byte[] bufPhase = new byte[6];
            //byte[] bufLimit = new byte[12];
            byte[] bufLimit = new byte[18];


            // byte[] bufChannelMute = new byte[4];
            bufChMute_2 = getCh2_Mute();
            bufChMute_3 =getCh3_Mute();
            bufChMute_4 = getCh4_Mute();
            bufChMute_5 = getCh5_Mute();

            for (int j = 0; j < 8; j++)
            {
                bufMute[j] = (byte)((_dev.user[usernum].switchChannel[j].getMute() == _mute.Mute) ? 1 : 0);
            }

            //bufChannelMute[0] = (byte)((_dev.user[usernum].ch[2].getMute() == _mute.UnMute) ? 1 : 0);
            //bufChannelMute[1] = (byte)((_dev.user[usernum].ch[3].getMute() == _mute.UnMute) ? 1 : 0);
            //bufChannelMute[2] = (byte)((_dev.user[usernum].ch[4].getMute() == _mute.UnMute) ? 1 : 0);
            //bufChannelMute[3] = (byte)((_dev.user[usernum].ch[5].getMute() == _mute.UnMute) ? 1 : 0);


            //静音-相位-压限
            //for (int i = 0; i < 4; i++)
            for (int i = 0; i < 6; i++)
            {
                /*    bufMute[i] = (byte)((_dev.user[usernum].ch[i].getMute() == _mute.Mute) ? 1 : 0);     */       //  4



                bufPhase[i] = (byte)((_dev.user[usernum].ch[i].getPhase() == _phase.Phase) ? 1 : 0);        //  4
                bufLimit[i * 3 + 0] = (byte)(_dev.user[usernum].getLT_Th());
                bufLimit[i * 3 + 1] = (byte)(_dev.user[usernum].getLT_St());
                bufLimit[i * 3 + 2] = (byte)(_dev.user[usernum].getLT_Re());

            }
            //EQ                                                                                                
            //byte[][][][] bufeq = new byte[4][][][];
            byte[][][][] bufeq = new byte[6][][][];
            //for (int i = 0; i < 4; i++)                                                                        
            for (int i = 0; i < 6; i++)
            {
                bufeq[i] = new byte[6][][];//6 point                                                            
                for (int j = 0; j < 6; j++)
                {
                    bufeq[i][j] = new byte[4][];//F+G+Q+T                                                       
                }
            }
            //for (int ch = 0; ch < 4; ch++)                                                                      
            for (int ch = 0; ch < 6; ch++)
            {
                for (int pt = 0; pt < 6; pt++)
                {
                    bufeq[ch][pt][0] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getEQ_F(pt));        //  8
                    bufeq[ch][pt][1] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getEQ_G(pt));        //  8
                    bufeq[ch][pt][2] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getEQ_Q(pt));        //  8
                    bufeq[ch][pt][3] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getEQ_T(pt));        //  2

                }
            }
            //HL
            //byte[][][][] bufhl = new byte[4][][][];
            byte[][][][] bufhl = new byte[6][][][];
            //for (int i = 0; i < 4; i++)//ch
            for (int i = 0; i < 6; i++)
            {
                bufhl[i] = new byte[2][][];
                for (int j = 0; j < 2; j++)//H+L 
                {
                    bufhl[i][j] = new byte[3][];//F + STEP + TYPE
                }
            }
            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                for (int _hl = 0; _hl < 2; _hl++)
                {
                    bufhl[ch][_hl][0] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getHL_F(_hl));      //  8
                    bufhl[ch][_hl][1] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getHL_S(_hl));      //  2
                    bufhl[ch][_hl][2] = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getHL_T(_hl));      //  2
                }
            }
            //NOISE
            //byte[] bufnoise = BitConverter.GetBytes(_dev.user[usernum].getNoise());                                              // 4       
            //二进四出无噪声控制
            //byte[] bufnoise = new byte[4];
            //bufnoise[0] = (byte)(_dev.user[usernum].getNoise() % 256);
            //bufnoise[1] = (byte)(_dev.user[usernum].getNoise() / 256);

            //LOWMEND     二进四出无低音补偿LowMend                                                                                              
            //byte[] buflowmend = BitConverter.GetBytes((byte)((_dev.user[usernum].getLowMendState() == _lowmend.Mend) ? 0 : 1));// 2                                                                                                       
            //BTL 二进四出无BTL模式                                                                                                           
            //byte[] bufbtl = BitConverter.GetBytes((byte)((_dev.user[usernum].getBtl() == true) ? 0 : 1));                       // 2 

            //int len_mu = bufMute.Length;

            //int len_chMute = bufChannelMute.Length;
            int len_mu = bufMute.Length;
            int len_ph = bufPhase.Length;
            int len_lt = bufLimit.Length;

            int len_eqf = bufeq[0][0][0].Length;
            int len_eqg = bufeq[0][0][1].Length;
            int len_eqq = bufeq[0][0][2].Length;
            int len_eqt = bufeq[0][0][3].Length;

            int len_hlf = bufhl[0][0][0].Length;
            int len_hls = bufhl[0][0][1].Length;
            int len_hlt = bufhl[0][0][2].Length;



            //int len_delay = 8/*double*/* 4/*ch*/;
            int len_delay = 8 /*double*/* 6/*ch*/;
            //int len_gain = 8/*double*/* 4/*ch*/;
            int len_gain = 8/*double*/* 6/*ch*/;
            byte[] bufAll = new byte[
                //len_mu/*4*/+
                len_mu/*8*/+
                len_ph/*6*/+
                len_lt/*18*/+
                6 * 6 * (len_eqf + len_eqg + len_eqq + len_eqt)/*8+8+8+2*/+
                6 * 2 * (len_hlf + len_hls + len_hlt)/*8+2+2*/+
                len_delay +
                len_gain//+
                +4
                ];

            //void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);

            //Buffer.BlockCopy(bufMute, 0, bufAll, 0, len_mu);
            Buffer.BlockCopy(bufMute, 0, bufAll, 0, len_mu);
            //Buffer.BlockCopy(bufPhase, 0, bufAll, 0, len_ph);
            Buffer.BlockCopy(bufPhase, 0, bufAll, len_mu, len_ph);
            Buffer.BlockCopy(bufLimit, 0, bufAll, len_mu + len_ph, len_lt);

            byte[] bufTemp = new byte[6 * (len_eqf + len_eqg + len_eqq + len_eqt)];//一个通道的长度: 6*(8+8+8+2)
            int len_eq_one = bufTemp.Length;
            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                for (int pt = 0; pt < 6; pt++)
                {
                    Buffer.BlockCopy(bufeq[ch][pt][0], 0, bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + 0, len_eqf);//F
                    Buffer.BlockCopy(bufeq[ch][pt][1], 0, bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf, len_eqg);//G
                    Buffer.BlockCopy(bufeq[ch][pt][2], 0, bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf + len_eqg, len_eqq);//Q
                    Buffer.BlockCopy(bufeq[ch][pt][3], 0, bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf + len_eqg + len_eqq, len_eqt);//T
                }
                //Buffer.BlockCopy(bufTemp, 0, bufAll, len_mu + len_ph + len_lt + ch * len_eq_one, len_eq_one);//F
                //Buffer.BlockCopy(bufTemp, 0, bufAll, len_ph + len_lt + ch * len_eq_one, len_eq_one);//F
                Buffer.BlockCopy(bufTemp, 0, bufAll, len_mu + len_ph + len_lt + ch * len_eq_one, len_eq_one);
                bufTemp = new byte[6 * (len_eqf + len_eqg + len_eqq + len_eqt)];//清空数组
            }

            bufTemp = new byte[2 * (len_hlf + len_hls + len_hlt)];//一个通道的长度: 6*(8+8+8+2)
            int len_hl_one = bufTemp.Length;
            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                for (int _hl = 0; _hl < 2; _hl++)
                {
                    Buffer.BlockCopy(bufhl[ch][_hl][0], 0, bufTemp, _hl * (len_hlf + len_hls + len_hlt) + 0, len_hlf);//F
                    Buffer.BlockCopy(bufhl[ch][_hl][1], 0, bufTemp, _hl * (len_hlf + len_hls + len_hlt) + len_hlf, len_hls);//G
                    Buffer.BlockCopy(bufhl[ch][_hl][2], 0, bufTemp, _hl * (len_hlf + len_hls + len_hlt) + len_hlf + len_hls, len_hlt);//Q
                }
                //Buffer.BlockCopy(bufTemp, 0, bufAll, len_mu + len_ph + len_lt + 4 * len_eq_one + ch * len_hl_one, len_hl_one);
                Buffer.BlockCopy(bufTemp, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + ch * len_hl_one, len_hl_one);
                bufTemp = new byte[2 * (len_hlf + len_hls + len_hlt)];//清空数组
            }


            byte[] bufdelay = new byte[len_delay];
            byte[] bufgain = new byte[len_gain];


            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                byte[] buf1 = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getDealy());
                byte[] buf2 = BitConverter.GetBytes(_dev.user[usernum].ch[ch].getGain());

                Buffer.BlockCopy(buf1, 0, bufdelay, ch * 8, buf1.Length);
                Buffer.BlockCopy(buf2, 0, bufgain, ch * 8, buf2.Length);

                buf1 = new byte[8];//清空数组
                buf2 = new byte[8];//清空数组
            }

            //Buffer.BlockCopy(bufdelay, 0, bufAll, len_mu + len_ph + len_lt + 4 * len_eq_one + 4 * len_hl_one, len_delay);
            //Buffer.BlockCopy(bufdelay, 0, bufAll, len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one, len_delay);
            Buffer.BlockCopy(bufdelay, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one, len_delay);
            //Buffer.BlockCopy(bufgain, 0, bufAll, len_mu + len_ph + len_lt + 4 * len_eq_one + 4 * len_hl_one + len_delay, len_gain);
            //Buffer.BlockCopy(bufgain, 0, bufAll, len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay, len_gain);
            Buffer.BlockCopy(bufgain, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay, len_gain);

            Buffer.BlockCopy(bufChMute_2, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain, 1);
            Buffer.BlockCopy(bufChMute_3, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain+1, 1);
            Buffer.BlockCopy(bufChMute_4, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain + 1+1, 1);
            Buffer.BlockCopy(bufChMute_5, 0, bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain + 1 + 1+1, 1);

            return bufAll;
        }


        int LoadOperation()
        {
            //this.TopMost = true;

            string start = "5A00A5FF";
            string device = devcbx.ToString("X2");
            string usergroup = usernumload.ToString("X2");

            // usernum = usernumload;

            string cmds = "09";
            string num = "0000";
            string end = "A5FF005A";
            string order = start + device + usergroup + cmds + num + end;
            _facty.sendnp(order);

            //myDelay(350, "正在检测设备连接……");             //临时关闭 2018-05-14
            delay_100ms(30); //Thread.Sleep(3000);                                //临时关闭 2018-05-14

            byte[] buf = _facty.read();
            // MessageBox.Show("收到数据长度：" + buf.Length .ToString());
            //MessageBox.Show()
            if (buf.Length == 0)
            {
                return -1;
            }


            int ret = loadData(buf);
            if (ret != 0)
            {

                return -1;
            }

            

            #region
            //for (int i = 0; i < 8; i++)
            //{
            //    _mute mute = _dev.user[usernum].switchChannel[i].getMute();
            //    switch (mute)
            //    {
            //        case _mute.UnMute:
            //            if (i == 0)
            //            {

            //                cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);
            //                //cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);

            //                cbxIn1Out1.Checked = true;
            //                // cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);


            //            }
            //            else if (i == 1)
            //            {
            //                cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);


            //                cbxIn1Out2.Checked = true;
            //                //cbxIn1Out2.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);

            //            }
            //            else if (i == 2)
            //            {
            //                //cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);
            //                cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);


            //                cbxIn1Out3.Checked = true;
            //                //cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);

            //            }
            //            else if (i == 3)
            //            {

            //                cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);

            //                cbxIn1Out4.Checked = true;
            //                //cbxIn1Out4.CheckedChanged -= new EventHandler(cbxIn1Out4_CheckedChanged);

            //            }
            //            else if (i == 4)
            //            {
            //                cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);

            //                cbxIn2Out1.Checked = true;
            //                //cbxIn2Out1.CheckedChanged -= new EventHandler(cbxIn2Out1_CheckedChanged);

            //            }
            //            else if (i == 5)
            //            {
            //                cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);


            //                cbxIn2Out2.Checked = true;
            //                //cbxIn2Out2.CheckedChanged -= new EventHandler(cbxIn2Out2_CheckedChanged);

            //            }
            //            else if (i == 6)
            //            {
            //                cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);

            //                cbxIn2Out3.Checked = true;
            //                //cbxIn2Out3.CheckedChanged -= new EventHandler(cbxIn2Out3_CheckedChanged);

            //            }
            //            else if (i == 7)
            //            {
            //                cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);

            //                cbxIn2Out4.Checked = true;
            //                //cbxIn2Out4.CheckedChanged -= new EventHandler(cbxIn2Out4_CheckedChanged);

            //            }
            //            break;
            //        case _mute.Mute:
            //            if (i == 0)
            //            {
            //                cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);
            //                cbxIn1Out1.Checked = false;
            //                //cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);

            //            }
            //            else if (i == 1)
            //            {
            //                cbxIn1Out2.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);
            //                cbxIn1Out2.Checked = false;
            //                //cbxIn1Out2.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);

            //            }
            //            else if (i == 2)
            //            {
            //                cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);

            //                cbxIn1Out3.Checked = false;
            //                //cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);

            //            }
            //            else if (i == 3)
            //            {
            //                cbxIn1Out4.CheckedChanged -= new EventHandler(cbxIn1Out4_CheckedChanged);
            //                cbxIn1Out4.Checked = false;
            //                //cbxIn1Out4.CheckedChanged -= new EventHandler(cbxIn1Out4_CheckedChanged);

            //            }
            //            else if (i == 4)
            //            {
            //                cbxIn2Out1.CheckedChanged -= new EventHandler(cbxIn2Out1_CheckedChanged);
            //                cbxIn2Out1.Checked = false;
            //                //cbxIn2Out1.CheckedChanged -= new EventHandler(cbxIn2Out1_CheckedChanged);

            //            }
            //            else if (i == 5)
            //            {
            //                cbxIn2Out2.CheckedChanged -= new EventHandler(cbxIn2Out2_CheckedChanged);
            //                cbxIn2Out2.Checked = false;
            //                //cbxIn2Out2.CheckedChanged -= new EventHandler(cbxIn2Out2_CheckedChanged);

            //            }
            //            else if (i == 6)
            //            {
            //                cbxIn2Out3.CheckedChanged -= new EventHandler(cbxIn2Out3_CheckedChanged);

            //                cbxIn2Out3.Checked = false;
            //                //cbxIn2Out3.CheckedChanged -= new EventHandler(cbxIn2Out3_CheckedChanged);

            //            }
            //            else if (i == 7)
            //            {
            //                cbxIn2Out4.CheckedChanged -= new EventHandler(cbxIn2Out4_CheckedChanged);
            //                cbxIn2Out4.Checked = false;
            //                //cbxIn2Out4.CheckedChanged -= new EventHandler(cbxIn2Out4_CheckedChanged);

            //            }
            //            break;
            //        default: break;

            //    }

            //}
            #endregion

            //  MessageBox.Show(CbxT0.SelectedItem.ToString());








            UpdatePic();
            return 0;
        }



        int loadData(byte[] buf)
        {
            int ret = 0;
            int nores = 0xff;
            //if (buf.Length < 825/*761*/) return -1;
            //if (buf.Length < 1209) return -1;
            //if (buf.Length < 1213) return -1;

            //if (buf.Length < 1221) return -1;
            if (buf.Length < 1225) return -1;

            //byte[] bufAll = new byte[812/*748*/];
            //byte[] bufAll = new byte[1196];
            //byte[] bufAll = new byte[1200];

            //byte[] bufAll = new byte[1208];
            byte[] bufAll = new byte[1212];

            //Buffer.BlockCopy(buf, 9, bufAll, 0, 1196);
            //Buffer.BlockCopy(buf, 9, bufAll, 0, 1200);

            //Buffer.BlockCopy(buf, 9, bufAll, 0, 1208);
            Buffer.BlockCopy(buf, 9, bufAll, 0, 1212);

            //int len_mu = 4;

            //int len_chMute = 4;

            int len_ch_mute = 4;

            int len_mu = 8;

            //int len_ph = 4;
            int len_ph = 6;
            //int len_lt = 12;
            int len_lt = 18;

            int len_eqf = 8;
            int len_eqg = 8;
            int len_eqq = 8;
            int len_eqt = 2;

            int len_hlf = 8;
            int len_hls = 2;
            int len_hlt = 2;

            int len_eq_one = 156;
            int len_hl_one = 24;

            //int len_delay = 32;
            int len_delay = 48;
            //int len_gain = 32;
            int len_gain = 48;


            //int len_noise = 4;

            //int len_lmd = 2;
            //int len_btl = 2;

            byte[] bufChMute_ch2 = new byte[1];
            byte[] bufChMute_ch3 = new byte[1];
            byte[] bufChMute_ch4 = new byte[1];
            byte[] bufChMute_ch5 = new byte[1];

            //byte[] bufchMute = new byte[len_chMute];
            //byte[] bufMute = new byte[len_mu];
            byte[] bufMute = new byte[len_mu];

            byte[] bufPhase = new byte[len_ph];
            byte[] bufLimit = new byte[len_lt];


            //EQ                                                                                                
            //byte[][][][] bufeq = new byte[4][][][];
            byte[][][][] bufeq = new byte[6][][][];
            //for (int i = 0; i < 4; i++)
            for (int i = 0; i < 6; i++)
            {
                bufeq[i] = new byte[6][][];//6 point                                                            
                for (int j = 0; j < 6; j++)
                {
                    bufeq[i][j] = new byte[4][];//F+G+Q+T          
                    bufeq[i][j][0] = new byte[len_eqf];
                    bufeq[i][j][1] = new byte[len_eqg];
                    bufeq[i][j][2] = new byte[len_eqq];
                    bufeq[i][j][3] = new byte[len_eqt];
                }
            }
            //HL
            //byte[][][][] bufhl = new byte[4][][][];
            byte[][][][] bufhl = new byte[6][][][];
            //for (int i = 0; i < 4; i++)//ch
            for (int i = 0; i < 6; i++)
            {
                bufhl[i] = new byte[2][][];
                for (int j = 0; j < 2; j++)//H+L 
                {
                    bufhl[i][j] = new byte[3][];//F + STEP + TYPE
                    bufhl[i][j][0] = new byte[len_hlf];
                    bufhl[i][j][1] = new byte[len_hls];
                    bufhl[i][j][2] = new byte[len_hlt];
                }
            }

            Buffer.BlockCopy(bufAll, 0, bufMute, 0, len_mu);
            //Buffer.BlockCopy(bufAll, 0, bufPhase, 0, len_ph);
            Buffer.BlockCopy(bufAll, len_mu, bufPhase, 0, len_ph);

            Buffer.BlockCopy(bufAll, len_mu + len_ph, bufLimit, 0, len_lt);
            //Buffer.BlockCopy(bufAll, len_ph, bufLimit, 0, len_lt);

            //for (int i = 0; i < 4; i++)

            for (int j = 0; j < 8; j++)
            {
                _dev.user[usernum].switchChannel[j].setMute((bufMute[j] == 1) ? _mute.Mute : _mute.UnMute, nores);
            }

            for (int i = 0; i < 6; i++)
            {
                _dev.user[usernum].ch[i].setPhase((bufPhase[i] == 1) ? _phase.Phase : _phase.UnPhase, nores);
                _dev.user[usernum].setLT_Th(bufLimit[i * 3 + 0], nores);
                _dev.user[usernum].setLT_St(bufLimit[i * 3 + 1], nores);
                _dev.user[usernum].setLT_Re(bufLimit[i * 3 + 2], nores);
            }


            byte[] bufTemp = new byte[6 * (len_eqf + len_eqg + len_eqq + len_eqt)];//一个通道的长度: 6*(8+8+8+2)
            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                bufTemp = new byte[6 * (len_eqf + len_eqg + len_eqq + len_eqt)];//清空数组

                Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + ch * len_eq_one, bufTemp, 0, len_eq_one);//F
                //Buffer.BlockCopy(bufAll, len_ph + len_lt + ch * len_eq_one, bufTemp, 0, len_eq_one);

                for (int pt = 0; pt < 6; pt++)
                {
                    Buffer.BlockCopy(bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + 0, bufeq[ch][pt][0], 0, len_eqf);//F
                    Buffer.BlockCopy(bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf, bufeq[ch][pt][1], 0, len_eqg);//G
                    Buffer.BlockCopy(bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf + len_eqg, bufeq[ch][pt][2], 0, len_eqq);//Q
                    Buffer.BlockCopy(bufTemp, pt * (len_eqf + len_eqg + len_eqq + len_eqt) + len_eqf + len_eqg + len_eqq, bufeq[ch][pt][3], 0, len_eqt);//T

                    _dev.user[usernum].ch[ch].setEQ_F(BitConverter.ToDouble(bufeq[ch][pt][0], 0), pt, nores);
                    _dev.user[usernum].ch[ch].setEQ_G(BitConverter.ToDouble(bufeq[ch][pt][1], 0), pt, nores);
                    _dev.user[usernum].ch[ch].setEQ_Q(BitConverter.ToDouble(bufeq[ch][pt][2], 0), pt, nores);
                    _dev.user[usernum].ch[ch].setEQ_T(bufeq[ch][pt][3][0]/*byte[0]*/, pt, nores);
                }
            }
            bufTemp = new byte[2 * (len_hlf + len_hls + len_hlt)];//一个通道的长度: 6*(8+8+8+2)
            //for (int ch = 0; ch < 4; ch++)
            for (int ch = 0; ch < 6; ch++)
            {
                bufTemp = new byte[2 * (len_hlf + len_hls + len_hlt)];//清空数组
                //Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 4 * len_eq_one + ch * len_hl_one, bufTemp, 0, len_hl_one);
                //Buffer.BlockCopy(bufAll, len_ph + len_lt + 6 * len_eq_one + ch * len_hl_one, bufTemp, 0, len_hl_one);
                Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + ch * len_hl_one, bufTemp, 0, len_hl_one);
                for (int pt = 0; pt < 2; pt++)
                {
                    Buffer.BlockCopy(bufTemp, pt * (len_hlf + len_hls + len_hlt) + 0, bufhl[ch][pt][0], 0, len_hlf);//F
                    Buffer.BlockCopy(bufTemp, pt * (len_hlf + len_hls + len_hlt) + len_hlf, bufhl[ch][pt][1], 0, len_hls);//G
                    Buffer.BlockCopy(bufTemp, pt * (len_hlf + len_hls + len_hlt) + len_hlf + len_hls, bufhl[ch][pt][2], 0, len_hlt);//Q

                    _dev.user[usernum].ch[ch].setHL_F(BitConverter.ToDouble(bufhl[ch][pt][0], 0), pt, nores);
                    _dev.user[usernum].ch[ch].setHL_S(bufhl[ch][pt][1][0]/*byte[0]*/, pt, nores);
                    _dev.user[usernum].ch[ch].setHL_T(bufhl[ch][pt][2][0]/*byte[0]*/, pt, nores);
                }
            }


            byte[] bufdelay = new byte[len_delay];
            byte[] bufgain = new byte[len_gain];
            //Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 4 * len_eq_one + 4 * len_hl_one, bufdelay, 0, len_delay);
            //Buffer.BlockCopy(bufAll, len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one, bufdelay, 0, len_delay);
            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one, bufdelay, 0, len_delay);
            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay, bufgain, 0, len_gain);
            for (int ch = 0; ch < 6; ch++)
            {
                byte[] buf1 = new byte[8];//清空数组
                byte[] buf2 = new byte[8];//清空数组
                Buffer.BlockCopy(bufdelay, ch * 8, buf1, 0, 8);
                Buffer.BlockCopy(bufgain, ch * 8, buf2, 0, 8);
                _dev.user[usernum].ch[ch].setDelay(BitConverter.ToDouble(buf1, 0), nores);
                _dev.user[usernum].ch[ch].setGain(BitConverter.ToDouble(buf2, 0), nores);
            }
            int lenChMute_ch2 = bufChMute_ch2.Length;
            int lenChMute_ch3 = bufChMute_ch3.Length;
            int lenChMute_ch4 = bufChMute_ch4.Length;
            int lenChMute_ch5 = bufChMute_ch5.Length;

            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain,bufChMute_ch2, 0, 1);
            if ((bufChMute_ch2[0] & 0X01) == 1)
            {
                cbxIn1Out1.Enabled = false;
                cbxIn2Out1.Enabled = false;
                // rbtnOut1.Checked = true;


              //  btnMute.Text = "取消静音";
                //RecentMute_ch2 = "取消静音";
                _dev.user[usernum].RecentMute_ch2 = "取消静音";
              //  UpdatePic();
            }
            else
            {

             //   btnMute.Text = "静音";
                //RecentMute_ch2 = "静音";
                _dev.user[usernum].RecentMute_ch2 = "静音";
                cbxIn1Out1.Enabled = true;
                cbxIn2Out1.Enabled = true;
             //   UpdatePic();

            }
            if ( ((int)bufChMute_ch2[0] & 0x02) == 0x02)
            {
                cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);
                cbxIn1Out1.CheckState = CheckState.Checked;
            }
            else
            {
                cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);
                cbxIn1Out1.CheckState = CheckState.Unchecked;
            }
            
            if (((int)bufChMute_ch2[0] & 0x04) == 0x04)
            {
                cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);
                cbxIn2Out1.CheckState = CheckState.Checked;
            }
            else//0x06   0x07
            {
                cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);
                cbxIn2Out1.CheckState = CheckState.Unchecked;
            }
            
            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain+lenChMute_ch2,bufChMute_ch3, 0, 1);
            if ((bufChMute_ch3[0] & 0X01) == 1)
            {
                cbxIn1Out2.Enabled = false;
                cbxIn2Out2.Enabled = false;

             //   btnMute.Text = "取消静音";
                //RecentMute_ch3 = "取消静音";
                _dev.user[usernum].RecentMute_ch3 = "取消静音";
             //   UpdatePic();

            }
            else
            {
              //  btnMute.Text = "静音";
                //RecentMute_ch3 = "静音";
                _dev.user[usernum].RecentMute_ch3 = "静音";
                cbxIn1Out2.Enabled = true;
                cbxIn2Out2.Enabled = true;
              //  UpdatePic();

            }
            if (((int)bufChMute_ch3[0] & 0x02) == 0x02)
            {
                cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);
                cbxIn1Out2.CheckState = CheckState.Checked;
            }
            else
            {
                cbxIn1Out2.CheckStateChanged -= new EventHandler(cbxIn1Out2_CheckStateChanged);
                cbxIn1Out2.CheckState = CheckState.Unchecked;
            }

            if (((int)bufChMute_ch3[0] & 0x04) == 0x04)
            {
                cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);
                cbxIn2Out2.CheckState = CheckState.Checked;
            }
            else//0x06   0x07
            {
                cbxIn2Out2.CheckStateChanged -= new EventHandler(cbxIn2Out2_CheckStateChanged);
                cbxIn2Out2.CheckState = CheckState.Unchecked;
            }
            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain + lenChMute_ch2+lenChMute_ch3, bufChMute_ch4, 0, 1);
            if ((bufChMute_ch4[0] & 0X01) == 1)
            {

                cbxIn1Out3.Enabled = false;
                cbxIn2Out3.Enabled = false;
             //   btnMute.Text = "取消静音";
                _dev.user[usernum].RecentMute_ch4 = "取消静音";
                    //btnMute.Text = "取消静音";
               // UpdatePic();

            }
            else
            {
               
              //  btnMute.Text = "静音";

                cbxIn1Out3.Enabled = true;
                cbxIn2Out3.Enabled = true;
                _dev.user[usernum].RecentMute_ch4 = "静音";
                //   UpdatePic();

            }
            if (((int)bufChMute_ch4[0] & 0x02) == 0x02)
            {
                cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);
                cbxIn1Out3.CheckState = CheckState.Checked;
            }
            else
            {
                cbxIn1Out3.CheckStateChanged -= new EventHandler(cbxIn1Out3_CheckStateChanged);
                cbxIn1Out3.CheckState = CheckState.Unchecked;
            }

            if (((int)bufChMute_ch4[0] & 0x04) == 0x04)
            {
                cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);
                cbxIn2Out3.CheckState = CheckState.Checked;
            }
            else//0x06   0x07
            {
                cbxIn2Out3.CheckStateChanged -= new EventHandler(cbxIn2Out3_CheckStateChanged);
                cbxIn2Out3.CheckState = CheckState.Unchecked;
            }
            Buffer.BlockCopy(bufAll, len_mu + len_ph + len_lt + 6 * len_eq_one + 6 * len_hl_one + len_delay + len_gain + lenChMute_ch2 + lenChMute_ch3+lenChMute_ch4, bufChMute_ch5, 0, 1);
            if ((bufChMute_ch5[0] & 0X01) == 1)
            {
                cbxIn1Out4.Enabled = false;
                cbxIn2Out4.Enabled = false;

              //  btnMute.Text = "取消静音";
                //RecentMute_ch5 = "取消静音";
                _dev.user[usernum].RecentMute_ch5 = "取消静音";
               // UpdatePic();

            }
            else
            {
             //   btnMute.Text = "静音";
                //RecentMute_ch4 = "静音";
                _dev.user[usernum].RecentMute_ch5 = "静音";
                cbxIn1Out4.Enabled = true;
                cbxIn2Out4.Enabled = true;
              //  UpdatePic();

            }
            if (((int)bufChMute_ch5[0] & 0x02) == 0x02)
            {
                cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);
                cbxIn1Out4.CheckState = CheckState.Checked;
            }
            else
            {
                cbxIn1Out4.CheckStateChanged -= new EventHandler(cbxIn1Out4_CheckStateChanged);
                cbxIn1Out4.CheckState = CheckState.Unchecked;
            }

            if (((int)bufChMute_ch5[0] & 0x04) == 0x04)
            {
                cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);
                cbxIn2Out4.CheckState = CheckState.Checked;
            }
            else//0x06   0x07
            {
                cbxIn2Out4.CheckStateChanged -= new EventHandler(cbxIn2Out4_CheckStateChanged);
                cbxIn2Out4.CheckState = CheckState.Unchecked;
            }

            return ret;
        }
        //public void Restore_Ch2(byte[] buf)
        //{
            
        //    if(buf<<7)
        //}
        private void cbxUserLoad_SelectedIndexChanged(object sender, EventArgs e)
        {
            usernumload = cbxUserLoad.SelectedIndex;
        }

        private void cbxUserSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            usernumsave = cbxUserSave.SelectedIndex;
        }

        #region TBX 修改设备号
        private void tbxDevMoy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_facty.getState() == _comState.Break) return;
                int newdevnum = 0;
                if (int.TryParse(tbxDevMoy.Text, out newdevnum) == false)
                {
                    tbxDevMoy.Text = "";
                    return;
                }
                //MessageBox.Show(newdevnum.ToString());
                //return;
                if (newdevnum < 1 || newdevnum > 255)
                {
                    tbxDevMoy.Text = "";
                    MessageBox.Show("请输入1-255编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = true;
                    return;
                }
                if (_facty.getState() == _comState.Link)
                {
                    string start = "5A00A5FF";
                    string device = devcbx.ToString("X2");
                    string usergroup = usernum.ToString("X2");
                    string cmds = 4.ToString("X2");
                    string length = "0001";
                    string data = newdevnum.ToString("X2");
                    string end = "A5FF005A";
                    string order = start + device + usergroup + cmds + length + data + end;
                    _facty.send(order);
                    Thread.Sleep(200);
                    byte[] buf = _facty.read();
                    if (buf.Length > 0)
                    {
                        int olddevnum = devcbx;
                        devcbx = newdevnum;
                        //cbxDev.SelectedIndex = newdevnum - 1;
                        if (cbxDev.Items.Contains(newdevnum) == false)
                        {
                            cbxDev.Items.Add(devcbx);
                            List<int> list = new List<int>();
                            for (int i = 0; i < cbxDev.Items.Count; i++)
                            {
                                list.Add(int.Parse(cbxDev.Items[i].ToString()));
                            }
                            list.Sort();
                            cbxDev.Items.Clear();
                            for (int i = 0; i < list.Count; i++)
                            {
                                cbxDev.Items.Add(list[i]);
                            }
                            int index = cbxDev.FindString(devcbx.ToString());
                            cbxDev.SelectedIndex = index;
                        }
                        else
                        {
                            int index = cbxDev.FindString(devcbx.ToString());
                            cbxDev.SelectedIndex = index;
                        }
                        if ((olddevnum != devcbx) && (cbxDev.Items.Contains(olddevnum) == true))
                        {
                            cbxDev.Items.Remove(olddevnum);
                        }
                        MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        timer1.Enabled = true;
                    }
                }
            }
        }
        #endregion
        private void Form1_Resize(object sender, EventArgs e)
        {
            //窗体最小化时  
            if (this.WindowState == FormWindowState.Minimized)
            {

            }
            //窗体恢复正常时  
            if (this.WindowState == FormWindowState.Normal)
            {
                //启动定时器  
                if (WinMinimized == true)
                {
                    timer1.Enabled = true;
                    WinMinimized = false;
                }
            }

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //if (this.WindowState != FormWindowState.Minimized)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //}
            //else
            //{
            //    this.WindowState = FormWindowState.Normal;
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


       

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void rbtnIn1_MouseEnter(object sender, EventArgs e)
        {
            //this.toolTipRbtnIn1.ToolTipTitle = "输入通道1的tips";
            this.toolTipRbtnIn1.IsBalloon = false;
            this.toolTipRbtnIn1.UseFading = true;
            this.toolTipRbtnIn1.Show("输入通道1", this.rbtnIn1);
        }

        private void rbtnIn1_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipRbtnIn1.Hide(rbtnIn1);
        }

        private void rbtnIn2_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipRbtnIn1.IsBalloon = false;
            this.toolTipRbtnIn1.UseFading = true;
            this.toolTipRbtnIn1.Show("输入通道2", this.rbtnIn2);
        }

        private void rbtnIn2_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipRbtnIn2.Hide(rbtnIn2);
        }

        private void rbtnOut1_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipRbtnOut1.IsBalloon = false;
            this.toolTipRbtnOut1.UseFading = true;
            this.toolTipRbtnOut1.Show("输出通道1", this.rbtnOut1);
        }

        private void rbtnOut1_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipRbtnOut1.Hide(rbtnOut1);
        }

        private void rbtnOut2_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipRbtnOut2.IsBalloon = false;
            this.toolTipRbtnOut2.UseFading = true;
            this.toolTipRbtnOut2.Show("输出通道2", this.rbtnOut2);
        }

        private void rbtnOut2_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipRbtnOut2.Hide(rbtnOut2);
        }

        private void rbtnOut3_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipRbtnOut3.IsBalloon = false;
            this.toolTipRbtnOut3.UseFading = true;
            this.toolTipRbtnOut3.Show("输出通道3", this.rbtnOut3);
        }

        private void rbtnOut3_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipRbtnOut3.Hide(rbtnOut3);
        }

        private void rbtnOut4_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipRbtnOut4.IsBalloon = false;
            this.toolTipRbtnOut4.UseFading = true;
            this.toolTipRbtnOut4.Show("输出通道4", this.rbtnOut4);
        }


        private void btn0dB_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip0dB.IsBalloon = false;
            this.toolTip0dB.UseFading = true;
            this.toolTip0dB.Show("使曲线恢复初始状态", this.btn0dB);
        }

        private void btn0dB_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip0dB.Hide(btn0dB);
        }

        private void cbxIn1Out1_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipIn1Out1.IsBalloon = false;
            this.toolTipIn1Out1.UseFading = true;
            this.toolTipIn1Out1.Show("选中以使输入通道1与输出通道1连通，未选中则静音", this.cbxIn1Out1);
        }

        private void cbxIn1Out1_MouseLeave(object sender, EventArgs e)
        {
            this.toolTipIn1Out1.Hide(cbxIn1Out1);
        }

        private void cbxIn1Out2_MouseEnter(object sender, EventArgs e)
        {
            this.toolTipIn1Out2.IsBalloon = false;
            this.toolTipIn1Out2.UseFading = true;
            //this.toolTipIn1Out2.Show("选中以使输入通道1与")
        }

        private void cbxIn1Out2_MouseLeave(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.IsBalloon = false;
            this.toolTip1.UseFading = true;
            this.toolTip1.Show("选中则使对应通道连通，未选中则静音", this.tableLayoutPanel1);
        }

        private void tableLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(tableLayoutPanel1);
        }

        private void btnTool_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //contextMenuS.Show((Button)sender, new Point(e.X, e.Y));
                contextMenuStrip1.Show((Button)sender, new Point(e.X, e.Y));
            }
        }

        private void 计算器ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void 设备管理器ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("devmgmt.msc");
        }

        private void 恢复出厂设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Thread tt = new Thread(reset);
            //tt.Start();

        }

        void reset()
        {
            if (_facty.getState() == _comState.Link)
            {
                MessageBox.Show("请耐心等待恢复出厂设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("恢复失败，请检查设备连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void 帮助关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("若有疑问请联系邮箱18081244079ss@gmail.com", "帮助/关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbxF4_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxG2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxF0_TextChanged(object sender, EventArgs e)
        {

        }
        
        public static int FLAG_SELECTCHANNEL;

        //public static string RecentMute_ch2 = "静音";
        //public static string RecentMute_ch3 = "静音";
        //public static string RecentMute_ch4 = "静音";
        //public static string RecentMute_ch5 = "静音";

        
        //RecentMute rm = new RecentMute();


        public string getChannel_2_btnMute_MouseDown()
        {
            if (btnMute.Text == "静音")
            {
                //_channel.RecentMute_ch2 rm = _channel.RecentMute_ch2.Unmute;
                _dev.user[usernum].RecentMute_ch2 = "取消静音";
                return _dev.user[usernum].RecentMute_ch2;

            }
            else
            {
                _dev.user[usernum].RecentMute_ch2 = "静音";
                return _dev.user[usernum].RecentMute_ch2;
            }
        }

        public string getChannel_3_btnMute_MouseDown()
        {
            if (btnMute.Text == "静音")
            {
                _dev.user[usernum].RecentMute_ch3 = "取消静音";
                return _dev.user[usernum].RecentMute_ch3;

            }
            else
            {
                _dev.user[usernum].RecentMute_ch3 = "静音";
                return _dev.user[usernum].RecentMute_ch3;
            }
        }

        public string getChannel_4_btnMute_MouseDown()
        {
            if (btnMute.Text == "静音")
            {
                _dev.user[usernum].RecentMute_ch4= "取消静音";
                return _dev.user[usernum].RecentMute_ch4;

            }
            else
            {
                _dev.user[usernum].RecentMute_ch4= "静音";
                return _dev.user[usernum].RecentMute_ch4;
            }
        }
        public string getChannel_5_btnMute_MouseDown()
        {

            if (btnMute.Text == "静音")
            {
                _dev.user[usernum].RecentMute_ch5= "取消静音";
                return _dev.user[usernum].RecentMute_ch5;
            }
            else
            {
                _dev.user[usernum].RecentMute_ch5= "静音";
                return _dev.user[usernum].RecentMute_ch5;
            }
        }
        public static int FLAG_MUTE_CH2=0;
        public static int FLAG_MUTE_CH3=0;
        public static int FLAG_MUTE_CH4=0;
        public static int FLAG_MUTE_CH5=0;

        
        public static string start = "5A00A5FF";
        public static string end = "A5FF005A";
        ///*UnMute未静音数据*/{0X00,0X80,0X00,0X00 },
        //  /*Mute静音数据*/{ 0X00,0x00,0x00,0x00},
        public void SendSwitch_Mute(int n)
        {
            string addr_0 = "0091";
            string addr_1 = "0092";
            string addr_2 = "0093";
            string addr_3 = "0094";
            string addr_4 = "0095";
            string addr_5 = "0096";
            string addr_6 = "0097";
            string addr_7 = "0098";


            string sdata = "00000000";
            string sdev = devcbx.ToString("X2");
            string suser = usernum.ToString("X2");
            string scmd = "01";
            string slength = "0006";
            string order = start + sdev + suser + scmd + slength;
            string a= sdata + end;
            switch (n)
            {
                case 2:
                    if (cbxIn1Out1.CheckState == CheckState.Checked)
                    {
                        string s = order + addr_0 + a;
                        _facty.send(s);
                     }
                    else { }
            delay_10ms(9);
                    if (cbxIn2Out1.CheckState == CheckState.Checked)
                    {

                        string s1 = order + addr_4 + a;
                        _facty.send(s1);

                    }
                    break;
                case 3:
                    if (cbxIn1Out2.CheckState == CheckState.Checked)
                    {

                        string s2 = order + addr_1 + a;
                        _facty.send(s2);

                    }
                    delay_10ms(9);
                    if (cbxIn2Out2.CheckState == CheckState.Checked)
                    {

                        string s3 = order + addr_5 + a;
                        _facty.send(s3);

                   }
                    break;
                case 4:
                    if (cbxIn1Out3.CheckState == CheckState.Checked)
                    {

                        string s4 = order + addr_2 + a;
                        _facty.send(s4);

                    }
                    delay_10ms(9);

                    if (cbxIn2Out3.CheckState == CheckState.Checked)
                    {

                        string s5 = order + addr_6 + a;
                        _facty.send(s5);

                    }
                    break;
                case 5:
                    if (cbxIn1Out4.CheckState == CheckState.Checked)
                    {
                        string s6 = order + addr_3 + a;
                        _facty.send(s6);

                     }

            delay_10ms(9);
                    if (cbxIn2Out4.CheckState == CheckState.Checked)
                    {
                        string s7 = order + addr_7 + a;
                        _facty.send(s7);

                    }
                    break;

            }
        }
        public void SendSwitch_UnMute(int n)
        {
            string addr_0 = "0091";
            string addr_1 = "0092";
            string addr_2 = "0093";
            string addr_3 = "0094";
            string addr_4 = "0095";
            string addr_5 = "0096";
            string addr_6 = "0097";
            string addr_7 = "0098";


            string sdata = "00800000";
            string sdev = devcbx.ToString("X2");
            string suser = usernum.ToString("X2");
            string scmd = "01";
            string slength = "0006";
            string order = start + sdev + suser + scmd + slength;
            string a = sdata + end;
            switch (n)
            {
                case 2:
                    if(cbxIn1Out1.CheckState==CheckState.Checked)
                    {
                        string s = order + addr_0 + a;
                        _facty.send(s);
                        delay_10ms(9);
                    }
                    else
                    { }
                    if(cbxIn2Out1.CheckState==CheckState.Checked)
                    {
                        string s1 = order + addr_4 + a;
                        _facty.send(s1);
                        delay_10ms(9);
                    }
                    else
                    { }
                    break;
                case 3:
                    if (cbxIn1Out2.CheckState == CheckState.Checked)
                    {
                        string s2 = order + addr_1 + a;
                        _facty.send(s2);
                        delay_10ms(9);
                    }
                    else
                    { }
                    if (cbxIn2Out2.CheckState == CheckState.Checked)
                    {
                        string s3 = order + addr_5 + a;
                        _facty.send(s3);
                        delay_10ms(9);
                    }
                    else
                    { }
                    //string s2 = order + addr_1 + a;
                    //_facty.send(s2);
                    //string s3 = order + addr_5 + a;
                    //_facty.send(s3);
                    break;
                case 4:
                    if (cbxIn1Out3.CheckState == CheckState.Checked)
                    {
                        string s4 = order + addr_2 + a;
                        _facty.send(s4);
                        delay_10ms(9);
                    }
                    else
                    { }
                    if (cbxIn2Out3.CheckState == CheckState.Checked)
                    {
                        string s5 = order + addr_6 + a;
                        _facty.send(s5);
                        delay_10ms(9);
                    }
                    else
                    { }
                    //string s4 = order + addr_2 + a;
                    //_facty.send(s4);
                    //string s5 = order + addr_6 + a;
                    //_facty.send(s5);
                    break;
                case 5:
                    if (cbxIn1Out4.CheckState == CheckState.Checked)
                    {
                        string s6 = order + addr_3 + a;
                        _facty.send(s6);
                        delay_10ms(9);
                    }
                    else
                    { }
                    if (cbxIn2Out4.CheckState == CheckState.Checked)
                    {
                        string s7 = order + addr_7 + a;
                        _facty.send(s7);
                        delay_10ms(9);
                    }
                    else
                    { }
                    //string s6 = order + addr_3 + a;
                    //_facty.send(s6);
                    //string s7 = order + addr_7 + a;
                    //_facty.send(s7);
                    break;

            }
        }

        public static string CurrentMute_CH2 = "静音";
        public static string CurrentMute_CH3 = "静音";
        public static string CurrentMute_CH4 = "静音";
        public static string CurrentMute_CH5 = "静音";

        //public static _channel.RecentMute_ch2 rm;
        public void btn_Click(object sender, EventArgs e)
        {
            #region
           string btnMute_Text;
            switch (FLAG_SELECTCHANNEL)
            {

                case 1://channel 2
                    btnMute_Text = getChannel_2_btnMute_MouseDown();

                    if (btnMute_Text == "取消静音")//最新状态为取消静音,按下则静音
                    {
                      //  rm = _channel.RecentMute_ch2.Mute;
                        FLAG_MUTE_CH2 = 1;
                        btnMute.Text = "取消静音";
                        //
                        _dev.user[usernum].RecentMute_ch2 ="取消静音";
                        cbxIn1Out1.Enabled = false;
                        cbxIn2Out1.Enabled = false;
                        SendSwitch_Mute(2);
                      

                    }
                    else//最新状态为静音，按下则为取消静音
                    {
                        FLAG_MUTE_CH2 = 0;
                        btnMute.Text = "静音";
                        CurrentMute_CH2 = "静音";
                        _dev.user[usernum].RecentMute_ch2 = "静音";

                        // switchChannel = 0;
                        // switchChannel = 4;
                        cbxIn1Out1.Enabled = true;
                        cbxIn2Out1.Enabled = true;
                        SendSwitch_UnMute(2);
                        //if (cbxIn1Out1.CheckState == CheckState.Checked)
                        //{

                        //    //switchChannel = 0;
                        //    //_dev.user[usernum].switchChannel[0].setMute(_mute.UnMute);
                        //}
                        //else
                        //{ }
                        //delay_10ms(9);
                        //if (cbxIn2Out1.CheckState == CheckState.Checked)
                        //{
                        //    switchChannel = 4;
                        //    _dev.user[usernum].switchChannel[4].setMute(_mute.UnMute);
                        //}
                        //else
                        //{ }


                        #region
                        //if (cbxIn1Out1.CheckState == CheckState.Indeterminate)
                        //{
                        //    //_dev.user[usernum].switchChannel[0].setMute(_mute.UnMute);
                        //    cbxIn1Out1.CheckState = CheckState.Checked;
                        //    //cbxIn1Out1.CheckStateChanged += new EventHandler(cbxIn1Out1_CheckStateChanged);

                        //}
                        //else
                        //{
                        //    cbxIn1Out1.CheckState = CheckState.Unchecked;
                        //    cbxIn1Out1.CheckStateChanged -= new EventHandler(cbxIn1Out1_CheckStateChanged);

                        //}
                        //if (cbxIn2Out1.CheckState == CheckState.Indeterminate)
                        //{
                        //    //_dev.user[usernum].switchChannel[4].setMute(_mute.UnMute);
                        //    cbxIn2Out1.CheckState = CheckState.Checked;
                        //   // cbxIn2Out1.CheckStateChanged += new EventHandler(cbxIn2Out1_CheckStateChanged);

                        //}
                        //else
                        //{
                        //    cbxIn2Out1.CheckState = CheckState.Unchecked;
                        //    cbxIn2Out1.CheckStateChanged -= new EventHandler(cbxIn2Out1_CheckStateChanged);
                        //}
                        #endregion
                    }

                    break;
                case 2:
                    btnMute_Text = getChannel_3_btnMute_MouseDown();
                    if (btnMute_Text == "取消静音")//最新状态为取消静音,按下则静音
                    {
                        FLAG_MUTE_CH3 = 1;

                        btnMute.Text = "取消静音";
                        CurrentMute_CH3 = "取消静音";
                        _dev.user[usernum].RecentMute_ch3 = "取消静音";

                        cbxIn1Out2.Enabled = false;
                        cbxIn2Out2.Enabled = false;
                        SendSwitch_Mute(3);
                        //switchChannel = 1;
                        //_dev.user[usernum].switchChannel[1].setMute(_mute.Mute);
                      //  delay_10ms(9);
                        //switchChannel = 5;
                        //_dev.user[usernum].switchChannel[5].setMute(_mute.Mute);

                    }
                    else//最新状态为静音，按下则为取消静音
                    {
                        FLAG_MUTE_CH3 = 0;

                        btnMute.Text = "静音";
                        CurrentMute_CH3 = "静音";
                        _dev.user[usernum].RecentMute_ch3 = "静音";

                        cbxIn1Out2.Enabled = true;
                        cbxIn2Out2.Enabled = true;
                        SendSwitch_UnMute(3);

                    }
                    break;
                case 3:
                    btnMute_Text = getChannel_4_btnMute_MouseDown();
                    if (btnMute_Text == "取消静音")//最新状态为取消静音,按下则静音
                    {
                        FLAG_MUTE_CH4 = 1;

                        btnMute.Text = "取消静音";
                        CurrentMute_CH4 = "取消静音";
                        _dev.user[usernum].RecentMute_ch4 = "取消静音";

                        cbxIn1Out3.Enabled = false;
                        cbxIn2Out3.Enabled = false;
                        SendSwitch_Mute(4);

                        //FLAG_MUTE_CH3 = 1;

                        //btnMute.Text = "取消静音";
                        //CurrentMute_CH3 = "取消静音";
                        //cbxIn1Out2.Enabled = false;
                        //cbxIn2Out2.Enabled = false;
                        //SendSwitch_Mute(3);

                    }
                    else//最新状态为静音，按下则为取消静音
                    {
                        FLAG_MUTE_CH4 = 0;

                        btnMute.Text = "静音";
                        CurrentMute_CH4 = "静音";
                        _dev.user[usernum].RecentMute_ch4 = "静音";

                        cbxIn1Out3.Enabled = true;
                        cbxIn2Out3.Enabled = true;
                        SendSwitch_UnMute(4);
                     
                    }
                    break;
                case 4:
                    //getChannel_5();

                    btnMute_Text = getChannel_5_btnMute_MouseDown();
                    if (btnMute_Text == "取消静音")//最新状态为取消静音,按下则静音
                    {
                        FLAG_MUTE_CH5 = 1;

                        btnMute.Text = "取消静音";
                        CurrentMute_CH5 = "取消静音";
                        _dev.user[usernum].RecentMute_ch5 = "取消静音";

                        cbxIn1Out4.Enabled = false;
                        cbxIn2Out4.Enabled = false;
                        SendSwitch_Mute(5);
                        //switchChannel = 3;
                        //_dev.user[usernum].switchChannel[3].setMute(_mute.Mute);
                        //delay_10ms(9);
                        //switchChannel = 7;
                        //_dev.user[usernum].switchChannel[7].setMute(_mute.Mute);

                    }
                    else//最新状态为静音，按下则为取消静音
                    {
                        FLAG_MUTE_CH5 = 0;
                        btnMute.Text = "静音";
                        CurrentMute_CH5 = "静音";
                        _dev.user[usernum].RecentMute_ch5 = "静音";

                        cbxIn1Out4.Enabled = true;
                        cbxIn2Out4.Enabled = true;
                        SendSwitch_UnMute(5);
                        //if (cbxIn1Out4.CheckState == CheckState.Checked)
                        //{
                        //    switchChannel = 3;
                        //    _dev.user[usernum].switchChannel[3].setMute(_mute.UnMute);
                        //}
                        //else
                        //{ }
                        //delay_10ms(9);
                        //if (cbxIn2Out4.CheckState == CheckState.Checked)
                        //{
                        //    switchChannel = 7;
                        //    _dev.user[usernum].switchChannel[7].setMute(_mute.UnMute);
                        //}
                    }
                    break;
                    #endregion

                    #region
                    //case 1:
                    //    if (btnMute.Text == "静音")
                    //    {

                    //        btnMute.Text = "取消静音";
                    //        cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);
                    //        cbxIn2Out1.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);
                    //        cbxIn1Out1.Enabled = false;
                    //        cbxIn2Out1.Enabled = false;
                    //        switch (cbxIn1Out1.CheckState)
                    //        {
                    //            case CheckState.Checked:
                    //                cbxIn1Out1.CheckState = CheckState.Unchecked;
                    //                break;
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out1.CheckState = CheckState.Indeterminate;
                    //                break;
                    //        }
                    //        if (cbxIn2Out1.CheckState == CheckState.Checked)
                    //        {
                    //            cbxIn2Out1.CheckState = CheckState.Unchecked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out1.CheckState = CheckState.Indeterminate;
                    //        }
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        btnMute.Text = "静音";
                    //        cbxIn1Out1.CheckedChanged -= new EventHandler(cbxIn1Out1_CheckedChanged);
                    //        cbxIn2Out1.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);
                    //        cbxIn1Out1.Enabled = true;
                    //        cbxIn2Out1.Enabled = true;
                    //        switch (cbxIn1Out1.CheckState)
                    //        {
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out1.CheckState = CheckState.Checked;
                    //                break;
                    //            case CheckState.Indeterminate:
                    //                cbxIn1Out1.CheckState = CheckState.Unchecked;
                    //                break;
                    //        }
                    //        if (cbxIn2Out1.CheckState == CheckState.Unchecked)
                    //        {
                    //            cbxIn2Out1.CheckState = CheckState.Checked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out1.CheckState = CheckState.Unchecked;
                    //        }

                    //    }
                    //    break;
                    //case 2:
                    //    if (btnMute.Text == "静音")
                    //    {
                    //        btnMute.Text = "取消静音";
                    //        cbxIn1Out2.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);
                    //        cbxIn2Out2.CheckedChanged -= new EventHandler(cbxIn2Out2_CheckedChanged);
                    //        cbxIn1Out2.Enabled = false;
                    //        cbxIn2Out2.Enabled = false;
                    //        switch (cbxIn1Out2.CheckState)
                    //        {
                    //            case CheckState.Checked:
                    //                cbxIn1Out2.CheckState = CheckState.Unchecked;
                    //                break;
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out2.CheckState = CheckState.Indeterminate;
                    //                break;
                    //        }
                    //        if (cbxIn2Out2.CheckState == CheckState.Checked)
                    //        {
                    //            cbxIn2Out2.CheckState = CheckState.Unchecked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out2.CheckState = CheckState.Indeterminate;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        btnMute.Text = "静音";
                    //        cbxIn1Out2.CheckedChanged -= new EventHandler(cbxIn1Out2_CheckedChanged);
                    //        cbxIn2Out2.CheckedChanged -= new EventHandler(cbxIn2Out2_CheckedChanged);
                    //        cbxIn1Out2.Enabled = true;
                    //        cbxIn2Out2.Enabled = true;
                    //        switch (cbxIn1Out2.CheckState)
                    //        {
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out2.CheckState = CheckState.Checked;
                    //                break;
                    //            case CheckState.Indeterminate:
                    //                cbxIn1Out2.CheckState = CheckState.Unchecked;
                    //                break;
                    //        }
                    //        if (cbxIn2Out2.CheckState == CheckState.Unchecked)
                    //        {
                    //            cbxIn2Out2.CheckState = CheckState.Checked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out2.CheckState = CheckState.Unchecked;
                    //        }
                    //    }
                    //    break;
                    //case 3:
                    //    if (btnMute.Text == "静音")
                    //    {
                    //        btnMute.Text = "取消静音";
                    //        cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);
                    //        cbxIn2Out3.CheckedChanged -= new EventHandler(cbxIn2Out3_CheckedChanged);
                    //        cbxIn1Out3.Enabled = false;
                    //        cbxIn2Out3.Enabled = false;
                    //        switch (cbxIn1Out3.CheckState)
                    //        {
                    //            case CheckState.Checked:
                    //                cbxIn1Out3.CheckState = CheckState.Unchecked;
                    //                break;
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out3.CheckState = CheckState.Indeterminate;
                    //                break;
                    //        }
                    //        if (cbxIn2Out3.CheckState == CheckState.Checked)
                    //        {
                    //            cbxIn2Out3.CheckState = CheckState.Unchecked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out3.CheckState = CheckState.Indeterminate;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        btnMute.Text = "静音";
                    //        cbxIn1Out3.CheckedChanged -= new EventHandler(cbxIn1Out3_CheckedChanged);
                    //        cbxIn2Out3.CheckedChanged -= new EventHandler(cbxIn2Out3_CheckedChanged);
                    //        cbxIn1Out3.Enabled = true;
                    //        cbxIn2Out3.Enabled = true;
                    //        switch (cbxIn1Out3.CheckState)
                    //        {
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out3.CheckState = CheckState.Checked;
                    //                break;
                    //            case CheckState.Indeterminate:
                    //                cbxIn1Out3.CheckState = CheckState.Unchecked;
                    //                break;
                    //        }
                    //        if (cbxIn2Out3.CheckState == CheckState.Unchecked)
                    //        {
                    //            cbxIn2Out3.CheckState = CheckState.Checked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out3.CheckState = CheckState.Unchecked;
                    //        }
                    //    }
                    //    break;
                    //case 4:
                    //    if (btnMute.Text == "静音")
                    //    {
                    //        btnMute.Text = "取消静音";
                    //        cbxIn1Out4.CheckedChanged -= new EventHandler(cbxIn1Out4_CheckedChanged);
                    //        cbxIn2Out4.CheckedChanged -= new EventHandler(cbxIn2Out4_CheckedChanged);
                    //        cbxIn1Out4.Enabled = false;
                    //        cbxIn2Out4.Enabled = false;
                    //        switch (cbxIn1Out4.CheckState)
                    //        {
                    //            case CheckState.Checked:
                    //                cbxIn1Out4.CheckState = CheckState.Unchecked;
                    //                break;
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out4.CheckState = CheckState.Indeterminate;
                    //                break;
                    //        }
                    //        if (cbxIn2Out4.CheckState == CheckState.Checked)
                    //        {
                    //            cbxIn2Out4.CheckState = CheckState.Unchecked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out4.CheckState = CheckState.Indeterminate;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        btnMute.Text = "静音";
                    //        cbxIn1Out4.CheckedChanged -= new EventHandler(cbxIn1Out4_CheckedChanged);
                    //        cbxIn2Out4.CheckedChanged -= new EventHandler(cbxIn2Out4_CheckedChanged);
                    //        cbxIn1Out4.Enabled = true;
                    //        cbxIn2Out4.Enabled = true;
                    //        switch (cbxIn1Out4.CheckState)
                    //        {
                    //            case CheckState.Unchecked:
                    //                cbxIn1Out4.CheckState = CheckState.Checked;
                    //                break;
                    //            case CheckState.Indeterminate:
                    //                cbxIn1Out4.CheckState = CheckState.Unchecked;
                    //                break;
                    //        }
                    //        if (cbxIn2Out4.CheckState == CheckState.Unchecked)
                    //        {
                    //            cbxIn2Out4.CheckState = CheckState.Checked;
                    //        }
                    //        else
                    //        {
                    //            cbxIn2Out4.CheckState = CheckState.Unchecked;
                    //        }
                    //    }
                    //    break;
                    #endregion

            }

            switchChannel = 0;



        }



        private void cbxIn1Out1_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn1Out1.CheckState == CheckState.Checked)
            {
                switchChannel = 0;
                _dev.user[usernum].cbxIn1Out1_IsCheck = true;
                _dev.user[usernum].switchChannel[0].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 0;
                _dev.user[usernum].cbxIn1Out1_IsCheck = false;

                _dev.user[usernum].switchChannel[0].setMute(_mute.Mute);
            }
            UpdatePic();

        }

        private void cbxIn1Out2_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn1Out2.CheckState == CheckState.Checked)
            {
                switchChannel = 1;
                _dev.user[usernum].cbxIn1Out2_IsCheck = true;

                _dev.user[usernum].switchChannel[1].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 1;
                _dev.user[usernum].cbxIn1Out2_IsCheck = false;

                _dev.user[usernum].switchChannel[1].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn1Out3_CheckStateChanged(object sender, EventArgs e)
        {

            if (cbxIn1Out3.CheckState == CheckState.Checked)
            {
                switchChannel = 2;
                _dev.user[usernum].cbxIn1Out3_IsCheck = true;

                _dev.user[usernum].switchChannel[2].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 2;
                _dev.user[usernum].cbxIn1Out3_IsCheck = false;

                _dev.user[usernum].switchChannel[2].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn1Out4_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn1Out4.CheckState == CheckState.Checked)
            {
                switchChannel = 3;
                _dev.user[usernum].cbxIn1Out4_IsCheck = true;

                _dev.user[usernum].switchChannel[3].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 3;
                _dev.user[usernum].cbxIn1Out4_IsCheck = false;

                _dev.user[usernum].switchChannel[3].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn2Out1_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn2Out1.CheckState == CheckState.Checked)
            {
                switchChannel = 4;
                _dev.user[usernum].cbxIn2Out1_IsCheck = true;

                _dev.user[usernum].switchChannel[4].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 4;
                _dev.user[usernum].cbxIn2Out1_IsCheck = false;

                _dev.user[usernum].switchChannel[4].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn2Out2_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn2Out2.CheckState == CheckState.Checked)
            {
                switchChannel = 5;
                _dev.user[usernum].cbxIn2Out2_IsCheck = true;

                _dev.user[usernum].switchChannel[5].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 5;
                _dev.user[usernum].cbxIn2Out2_IsCheck = false;

                _dev.user[usernum].switchChannel[5].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn2Out3_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn2Out3.CheckState == CheckState.Checked)
            {
                switchChannel = 6;
                _dev.user[usernum].cbxIn2Out3_IsCheck = true;

                _dev.user[usernum].switchChannel[6].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 6;
                _dev.user[usernum].cbxIn2Out3_IsCheck = false;

                _dev.user[usernum].switchChannel[6].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void cbxIn2Out4_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbxIn2Out4.CheckState == CheckState.Checked)
            {
                switchChannel = 7;
                _dev.user[usernum].cbxIn2Out4_IsCheck = true;

                _dev.user[usernum].switchChannel[7].setMute(_mute.UnMute);
            }
            else
            {
                switchChannel = 7;
                _dev.user[usernum].cbxIn2Out4_IsCheck = false;

                _dev.user[usernum].switchChannel[7].setMute(_mute.Mute);
            }
            UpdatePic();
        }

        private void gbxUser_Enter(object sender, EventArgs e)
        {

        }

        private void gbxDev_Enter(object sender, EventArgs e)
        {

        }

        private void panelMid_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void cbxHtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxHstep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void tbxHF_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cbxLtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxLstep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tbxLF_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            WinMinimized = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void picDraw_Click(object sender, EventArgs e)
        {

        }

        
        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }
    }
}


        


    

