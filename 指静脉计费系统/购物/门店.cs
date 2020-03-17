using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkfvcsharp;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;

namespace 指静脉计费系统
{
    public partial class 购物1 : Form
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

        //Device Handle
        private IntPtr mDevHandle = IntPtr.Zero;
        //DB Handle
        private IntPtr mDBHandle = IntPtr.Zero;
        //Last reg-fingerprint template
        private byte[] mRegFPRegTemp = null;
        //Last reg-fingerprint template length
        private int mRegFPRegTempLen = 0;
        //Last reg-fingervein templates(3 fingervein temlates)
        private byte[][] mRegFVRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mRegFVTempLen = null;
        //stop thread
        private bool mbStop = false;
        //Enroll
        private bool mbRegister = true;
        //Identify(!Verify)
        private bool mbIdentify = true;
        //Enroll count
        private static int mEnrollCnt = 3;
        //Enroll index
        private int mEnrollIdx = 0;
        //register finger id(must > 0)
        private int mFingerID = 1;
        //Max template length
        private static int mMaxTempLen = 2048;
        private static int value = 0;

        //From handle
        IntPtr FormHandle = IntPtr.Zero;

        //Acquire info form sdk begin
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
        //prereg-fingerprint templates
        private byte[][] mPreRegFPRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mPreRegFPTempLen = null;
        //Last reg-fingervein templates(3 fingervein temlates)
        private byte[][] mPreRegFVRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mPreRegFVTempLen = null;
        //Acquire info form sdk end

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public 购物1()
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 购物_Load(object sender, EventArgs e)
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
                                        float c = Convert.ToInt64(this.textBox1.Text.Trim());
                                        float d = b - c;
                                    if (d < 0)
                                    {
                                        if (MessageBox.Show("余额不足!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
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
                                        sdr1.Close();

                                        string spot = "五食堂";
                                        DateTime DT = DateTime.Now;
                                        SqlCommand cmd5 = new SqlCommand("insert into 消费记录(消费时间,学号,消费地点,消费金额) values ('" + DT + "','" + a + "','" + spot + "','" + c.ToString() + "')", conn);
                                        SqlDataReader sdr5 = cmd5.ExecuteReader();
                                        sdr5.Close();

                                        this.textBox2.Text = d.ToString();
                                        if (MessageBox.Show("消费成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information) == DialogResult.OK)
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
                                            cmd2.Clone();
                                            sdr1.Close();
                                            SqlCommand cmd3 = new SqlCommand("SELECT 账户余额 FROM 学生账户信息 where 学号 = '" + a + "'", conn);
                                            SqlDataAdapter adt2 = new SqlDataAdapter(cmd3);
                                            DataSet ds1 = new DataSet();
                                            adt2.Fill(ds1);
                                            this.textBox2.Text = ds1.Tables[0].Rows[0]["账户余额"].ToString();
                                        }
                                    }
                                    exist = true;
                                    return;
                                }
                            }
                            if (exist == false)
                            {
                                if (MessageBox.Show("您尚未注册哦!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
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

        private void skinButton1_Click(object sender, EventArgs e)
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
    }
}
