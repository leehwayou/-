using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Net;

namespace 指静脉计费系统
{
    public partial class 学生考勤 : Form
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

        string strConn = @"Data Source =.;Initial Catalog = 指静脉;Integrated Security = true";
        private string url = "http://utf8.sms.webchinese.cn/?";
        private string strUid = "Uid=myonlylove";
        private string strKey = "&key=d795c6e443d027461418";
        private string strMob = "&smsMob=";
        private string strContent = "&smsText=";

        public 学生考勤()
        {
            InitializeComponent();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from 考勤表 where 到勤状态='1'", conn);
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds);
            string tnumber = ds.Tables[0].Rows[0]["教师职工号"].ToString().Trim();
            int count1 = ds.Tables[0].Rows.Count;
            cmd.Clone();
            SqlCommand cmd1 = new SqlCommand("select * from 考勤表", conn);
            SqlDataAdapter adt1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adt1.Fill(ds1);          
            int count2 = ds1.Tables[0].Rows.Count;
            cmd1.Clone();
            SqlCommand cmd2 = new SqlCommand("select 电话号 from 教师基本信息 where 职工号='"+ tnumber + "'", conn);
            SqlDataAdapter adt2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            adt2.Fill(ds2);

            string b = "考勤结果：应到"+count2+"人，实到"+count1+"人，缺勤"+(count2-count1)+"人";
            string phone= ds2.Tables[0].Rows[0]["电话号"].ToString().Trim();

            url = url + strUid + strKey + strMob + phone + strContent + b;
            string Result = GetHtmlFromUrl(url);
            if (Result == "1")
            {
                MessageBox.Show("发送成功", "提示", 0);
            }
            else
            {
                MessageBox.Show("发送失败，请检查手机号是否正确！", "提示", 0);
            }
        }

        public string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)HttpWebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.skinButton1.Enabled = true;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update 考勤表 set 到勤状态='0'", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            MessageBox.Show("信息已清空，可考勤！","提示",0);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.skinButton2.Enabled = true;
            指纹指静脉校验3 jy3 = new 指纹指静脉校验3();
            jy3.Show();
        }
    }
}
