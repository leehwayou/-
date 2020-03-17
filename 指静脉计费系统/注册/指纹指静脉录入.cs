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
    public partial class 指纹指静脉录入 : Form
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
        private static int mEnrollCnt = 3;
        private int mEnrollIdx = 0;
        private int mFingerID = 1;
        private static int mMaxTempLen = 2048;

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

        public 指纹指静脉录入()
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
        private void skinButton2_Click(object sender, EventArgs e)
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

        private void 指纹指静脉录入_Load(object sender, EventArgs e)
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
                        this.picFP.Image = bmpfp;


                        MemoryStream msFV = new MemoryStream();
                        BitmapFormat.GetBitmap(mfvImg, mfvWidth, mfvHeight, ref msFV);
                        Bitmap bmpfv = new Bitmap(msFV);
                        this.picFV.Image = bmpfv;

                        Array.Copy(mfpTemp, mPreRegFPRegTemps[mEnrollIdx], mfpTempLen);
                        Array.Copy(mfvTemp, mPreRegFVRegTemps[mEnrollIdx], mfvTempLen);
                        mPreRegFPTempLen[mEnrollIdx] = mfpTempLen;
                        mPreRegFVTempLen[mEnrollIdx] = mfvTempLen;
                        mEnrollIdx++;

                        if (mEnrollIdx >= 3)
                        {
                            mEnrollIdx = 0;
                            mbRegister = false;
                            byte[] temp = new byte[mMaxTempLen];
                            int nTempLen = mMaxTempLen;
                            zkfv.DBMergeFP(mDBHandle, mPreRegFPRegTemps, temp, ref nTempLen);//temp获取到最优指纹模板
                            Array.Copy(temp, mRegFPRegTemp, nTempLen);
                            mRegFPRegTempLen = nTempLen;
                            for (int i = 0; i < mEnrollCnt; i++)
                            {
                                Array.Copy(mPreRegFVRegTemps[i], mRegFVRegTemps[i], mPreRegFVTempLen[i]);
                                mRegFVTempLen[i] = mPreRegFVTempLen[i];
                            }
                            mFingerID++;
                        }
                        else
                        {
                            txtResult.Text = "你需要再录入 " + (mEnrollCnt - mEnrollIdx) + " 次指纹指静脉信息！";
                        }
                        if (!mbRegister)
                        {
                            ///<要做的事情>
                            ///将指纹模板和指静脉模板录入数据库
                            ///<要做的事情>

                            string ID = CommonData.SID;

                            byte[] byteFP = new byte[mRegFPRegTempLen];
                            byte[] byteFV = new byte[mRegFVTempLen[2]];
                            Array.Copy(mRegFVRegTemps[2], byteFV, mRegFVTempLen[2]);
                            Array.Copy(mRegFPRegTemp, byteFP, mRegFPRegTempLen);

                            string strConn = @"Data Source =.;Initial Catalog = 指静脉;Integrated Security = true";
                            SqlConnection con = new SqlConnection(strConn);
                            con.Open();
                            SqlParameter par1 = new SqlParameter("@FPTemp", SqlDbType.Image);
                            SqlParameter par2 = new SqlParameter("@FVTemp", SqlDbType.Image);
                            SqlCommand cmd = new SqlCommand("insert into 指纹指静脉(电话号,学号,指纹,指静脉) values('"+CommonData.PhoneNum+"','" + ID + "',@FPTemp,@FVTemp)", con);
                            cmd.Parameters.Add(par1);
                            cmd.Parameters.Add(par2);
                            par1.Value = byteFP;
                            par2.Value = byteFV;
                            cmd.ExecuteNonQuery();
                            txtResult.Text = "录入成功！";
                            if(MessageBox.Show("录入成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information)==DialogResult.OK)
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
                            this.Close();
                        }
                        break;
                    }
                    default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
