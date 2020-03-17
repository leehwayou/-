using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkfvcsharp;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;

namespace 指静脉计费系统
{
    public partial class 超市 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private IntPtr mDevHandle = IntPtr.Zero;
        private IntPtr mDBHandle = IntPtr.Zero;
        private byte[] mRegFPRegTemp = null;
        private int mRegFPRegTempLen = 0;
        private byte[][] mRegFVRegTemps = new byte[3][];
        private int[] mRegFVTempLen = null;
        private bool mbStop = false;
        private bool mbRegister = true;
        private bool mbIdentify = true;
        private static int mEnrollCnt = 3;
        private int mEnrollIdx = 0;
        private int mFingerID = 1;
        private static int mMaxTempLen = 2048;
        private static int value = 0;

        IntPtr FormHandle = IntPtr.Zero;

        private byte[] mfpImg = null;
        private byte[] mfvImg = null;
        private int mfpWidth = 0;
        private int mfpHeight = 0;
        private int mfvWidth = 0;
        private int mfvHeight = 0;
        private byte[] mfpTemp = null;
        private byte[] mfvTemp = null;
        private int mfpTempLen = 0;
        private int mfvTempLen = 0;
        private byte[][] mPreRegFPRegTemps = new byte[3][];
        private int[] mPreRegFPTempLen = null;
        private byte[][] mPreRegFVRegTemps = new byte[3][];
        private int[] mPreRegFVTempLen = null;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public 超市()
        {
            InitializeComponent();
            mRegFVTempLen = new int[mEnrollCnt];
            mPreRegFPTempLen = new int[mEnrollCnt];
            mPreRegFVTempLen = new int[mEnrollCnt];
            mfpTemp = new byte[mMaxTempLen];
            mfvTemp = new byte[mMaxTempLen];
            mRegFPRegTemp = new byte[mMaxTempLen];
            mRegFPRegTempLen = 0;
            for (int i = 0; i < mEnrollCnt; i++)
            {
                mRegFVRegTemps[i] = new byte[mMaxTempLen];
                mPreRegFPRegTemps[i] = new byte[mMaxTempLen];
                mPreRegFVRegTemps[i] = new byte[mMaxTempLen];
                mRegFVTempLen[i] = 0;
                mPreRegFPTempLen[i] = 0;
                mPreRegFVTempLen[i] = 0;
            }
        }

        private void goods_1_Click(object sender, EventArgs e)
        {
            lab_name1.Text = "可口可乐";
            label1.Visible = true;
            lab_name1.Visible = true;
            lab_price1.Text = price1.Text;
            lab_price1.Visible = true;
            button1.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void price1_Click(object sender, EventArgs e)
        {

        }

        private void goods_2_Click(object sender, EventArgs e)
        {
            lab_name2.Text = "美好火腿肠";
            lab_name2.Visible = true;
            label2.Visible = true;
            lab_price2.Text = price2.Text;
            lab_price2.Visible = true;
            button2.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();

        }

        private void goods_4_Click(object sender, EventArgs e)
        {
            lab_name4.Text = "王老吉";
            lab_name4.Visible = true;
            label7.Visible = true;
            lab_price4.Text = price4.Text;
            lab_price4.Visible = true;
            button4.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();

        }

        private void goods_3_Click(object sender, EventArgs e)
        {
            lab_name3.Text = "威化饼干";
            lab_name3.Visible = true;
            label5.Visible = true;
            lab_price3.Text = price3.Text;
            lab_price3.Visible = true;
            button3.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();

        }

        private void goods_5_Click(object sender, EventArgs e)
        {
            lab_name5.Text = "鸡翅";
            lab_name5.Visible = true;
            label8.Visible = true;
            lab_price5.Text = price5.Text;
            lab_price5.Visible = true;
            button5.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void goods_6_Click(object sender, EventArgs e)
        {
            lab_name6.Text = "卤蛋";
            lab_name6.Visible = true;
            label10.Visible = true;
            lab_price6.Text = price6.Text;
            lab_price6.Visible = true;
            button6.Visible = true;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void 超市_Load(object sender, EventArgs e)
        {
            FormHandle = this.Handle;
            zkfv.Init();
            mDevHandle = zkfv.OpenDevice(0);
            mDBHandle = zkfv.DBInit();

            byte[] retParam = new byte[4];
            int size = 4;
            int ret = 0;
            ret = zkfv.GetParameters(mDevHandle, 5001, retParam, ref size);
            zkfv.ByteArray2Int(retParam, ref mfvWidth);
            size = 4;
            ret = zkfv.GetParameters(mDevHandle, 5002, retParam, ref size);
            zkfv.ByteArray2Int(retParam, ref mfvHeight);
            size = 4;
            ret = zkfv.GetParameters(mDevHandle, 5004, retParam, ref size);
            zkfv.ByteArray2Int(retParam, ref mfpWidth);
            size = 4;
            ret = zkfv.GetParameters(mDevHandle, 5005, retParam, ref size);
            zkfv.ByteArray2Int(retParam, ref mfpHeight);

            mfvImg = new byte[mfvWidth * mfvHeight];
            mfpImg = new byte[mfpWidth * mfpHeight];

            mbStop = false;
            Thread captureThread = new Thread(new ThreadStart(DoCapture));
            captureThread.IsBackground = true;
            captureThread.Start();
        }

        private void DoCapture()
        {
            while (!mbStop)
            {
                mfpTempLen = mfpTemp.Length;
                mfvTempLen = mfvTemp.Length;
                int ret = zkfv.AcquireFingerVein(mDevHandle, mfpImg, mfvImg, mfpTemp, ref mfpTempLen, mfvTemp, ref mfvTempLen);
                if (ret == zkfverrdef.ZKFV_ERR_OK)
                {
                    SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                }
                Thread.Sleep(200);
            }
        }
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        MemoryStream msFP = new MemoryStream();
                        BitmapFormat.GetBitmap(mfpImg, mfpWidth, mfpHeight, ref msFP);
                        Bitmap bmpfp = new Bitmap(msFP);

                        MemoryStream msFV = new MemoryStream();
                        BitmapFormat.GetBitmap(mfvImg, mfvWidth, mfvHeight, ref msFV);
                        Bitmap bmpfv = new Bitmap(msFV);

                        //转移
                        int FPThreshold = 0;
                        zkfv.DBGetThreshold(mDBHandle, 2, ref FPThreshold);
                        int FVThreshold = 0;
                        zkfv.DBGetThreshold(mDBHandle, 4, ref FVThreshold);
                        int FPScore = 0;
                        int FVScore = 0;

                        string strConn = @"Data Source =.;Initial Catalog = 指静脉;Integrated Security = true";
                        SqlConnection conn = new SqlConnection(strConn);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("select 学号,指纹,指静脉 from 指纹指静脉", conn);
                        SqlDataAdapter adt = new SqlDataAdapter(cmd);
                        DataSet FPFVDS = new DataSet();
                        adt.Fill(FPFVDS, "指纹指静脉对比表");

                        //要遍历数据库搜寻到某个人，并提取他的余额出来
                        int rowsNum = FPFVDS.Tables[0].Rows.Count;
                        bool exist = false;
                        if (mbIdentify)
                        {
                            for (int i = 0; i < rowsNum; i++)
                            {
                                byte[] DBfptemp = (byte[])FPFVDS.Tables[0].Rows[i]["指纹"];
                                byte[] DBfvtemp = (byte[])FPFVDS.Tables[0].Rows[i]["指静脉"];

                                FPScore = zkfv.DBMatchFP(mDBHandle, DBfptemp, mfpTemp);
                                FVScore = zkfv.DBMatchFV(mDBHandle, DBfvtemp, mfvTemp);

                                //1：N的识别，找出是哪个人的指纹指静脉，如果成功识别则是第i个
                                if (FPScore >= FPThreshold || FVScore >= FVThreshold)
                                {
                                    string a = (string)FPFVDS.Tables[0].Rows[i]["学号"];
                                    mbIdentify = false;
                                    cmd.Clone();
                                    SqlCommand cmd1 = new SqlCommand("SELECT 账户余额 FROM 学生账户信息 where 学号 = '" + a + "'", conn);
                                    SqlDataAdapter adt1 = new SqlDataAdapter(cmd1);
                                    DataSet ds = new DataSet();
                                    adt1.Fill(ds);

                                    float b = Convert.ToInt64(ds.Tables[0].Rows[0]["账户余额"].ToString());
                                    float c = Convert.ToInt64(this.allprice.Text.Trim());
                                    float d = b - c;
                                    if (d < 0)
                                    {
                                        if (MessageBox.Show("余额不足！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                                        {
                                            if (!mbStop)
                                            {
                                                mbStop = true;
                                                Thread.Sleep(1000);
                                            }
                                            if (IntPtr.Zero != mDBHandle)
                                            {
                                                zkfv.DBFree(mDBHandle);
                                                mDBHandle = IntPtr.Zero;
                                            }
                                            if (IntPtr.Zero != mDevHandle)
                                            {
                                                zkfv.CloseDevice(mDevHandle);
                                                mDevHandle = IntPtr.Zero;
                                            }
                                            mRegFPRegTempLen = 0;
                                        }
                                    }
                                    else
                                    {
                                        cmd1.Clone();
                                        SqlCommand cmd2 = new SqlCommand("update 学生账户信息 set 账户余额='" + d + "'where 学号='" + a + "'", conn);
                                        SqlDataReader sdr1 = cmd2.ExecuteReader();
                                        cmd2.Clone();
                                        sdr1.Close();
                                        DateTime DT = System.DateTime.Now;
                                        string spot = "自动贩卖机";
                                        string commodity = lab_name1.Text.Trim() + " " + lab_name2.Text.Trim() + " " + lab_name3.Text.Trim() + " " + lab_name4.Text.Trim() + " " + lab_name5.Text.Trim() + " " + lab_name6.Text.Trim();
                                        SqlCommand cmd3 = new SqlCommand("insert into 消费记录(消费时间,学号,消费地点,购买商品,消费金额) values ('" + DT + "','" + a + "','" +spot+"','"+ commodity.TrimStart() + "','" + c.ToString() + "')", conn);
                                        SqlDataReader sdr3 = cmd3.ExecuteReader();
                                        if (MessageBox.Show("消费成功！余额为："+d, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                                        {
                                            if (!mbStop)
                                            {
                                                mbStop = true;
                                                Thread.Sleep(1000);
                                            }
                                            if (IntPtr.Zero != mDBHandle)
                                            {
                                                zkfv.DBFree(mDBHandle);
                                                mDBHandle = IntPtr.Zero;
                                            }
                                            if (IntPtr.Zero != mDevHandle)
                                            {
                                                zkfv.CloseDevice(mDevHandle);
                                                mDevHandle = IntPtr.Zero;
                                            }
                                            mRegFPRegTempLen = 0;
                                        }
                                    }
                                    exist = true;
                                    return;
                                }
                            }
                            if (exist == false)
                            {
                                if (MessageBox.Show("您尚未注册哦！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                                {
                                    if (!mbStop)
                                    {
                                        mbStop = true;
                                        Thread.Sleep(1000);
                                    }
                                    if (IntPtr.Zero != mDBHandle)
                                    {
                                        zkfv.DBFree(mDBHandle);
                                        mDBHandle = IntPtr.Zero;
                                    }
                                    if (IntPtr.Zero != mDevHandle)
                                    {
                                        zkfv.CloseDevice(mDevHandle);
                                        mDevHandle = IntPtr.Zero;
                                    }
                                    mRegFPRegTempLen = 0;
                                }
                                mbIdentify = false;
                            }
                        }
                        break;
                    }
                    default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

       

        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            if (!mbStop)
            {
                mbStop = true;
                Thread.Sleep(1000);
            }
            if (IntPtr.Zero != mDBHandle)
            {
                zkfv.DBFree(mDBHandle);
                mDBHandle = IntPtr.Zero;
            }
            if (IntPtr.Zero != mDevHandle)
            {
                zkfv.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
            }
            mRegFPRegTempLen = 0;
       
            this.Close();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            this.lab_name1.Text = "";
            this.lab_price1.Text = "0";
            this.lab_price1.Visible = false;
            label1.Visible = false;
            this.button1.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.lab_name2.Text = "";
            this.lab_price2.Text = "0";
            this.lab_price2.Visible = false;
            label2.Visible = false;
            this.button2.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.lab_name3.Text = "";
            this.lab_price3.Text = "0";
            this.lab_price3.Visible = false;
            label5.Visible = false;
            this.button3.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.lab_name4.Text = "";
            this.lab_price4.Text = "0";
            this.lab_price4.Visible = false;
            label7.Visible = false;
            this.button4.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.lab_name5.Text = "";
            this.lab_price5.Text = "0";
            this.lab_price5.Visible = false;
            label8.Visible = false;
            this.button5.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.lab_name6.Text = "";
            this.lab_price6.Text = "0";
            this.lab_price6.Visible = false;
            label10.Visible = false;
            this.button6.Visible = false;
            float price_1 = Convert.ToInt64(lab_price1.Text);
            float price_2 = Convert.ToInt64(lab_price2.Text);
            float price_3 = Convert.ToInt64(lab_price3.Text);
            float price_4 = Convert.ToInt64(lab_price4.Text);
            float price_5 = Convert.ToInt64(lab_price5.Text);
            float price_6 = Convert.ToInt64(lab_price6.Text);
            this.allprice.Text = (price_1 + price_2 + price_3 + price_4 + price_5 + price_6).ToString();
        }
    }
}
