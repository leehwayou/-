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
    public partial class 洗衣计时 : Form
    {
        string strConn = @"Data Source =.;Initial Catalog = 指静脉;Integrated Security = true";
        private string url = "http://utf8.sms.webchinese.cn/?";
        private string strUid = "Uid=myonlylove";
        private string strKey = "&key=d795c6e443d027461418";
        private string strMob = "&smsMob=";
        private string strContent = "&smsText=";

        public 洗衣计时()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float b;
            float a = Convert.ToInt64(time.Text.Trim());
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("select 手机号 from 学生基本信息 where 学号='" + CommonData.SID + "'", conn);
            SqlDataAdapter adt2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            adt2.Fill(ds2);
            string phone = ds2.Tables[0].Rows[0]["手机号"].ToString();
            cmd2.Clone();
            SqlCommand cmd = new SqlCommand("select 账户余额 from 学生账户信息 where 学号='" + CommonData.SID+ "'", conn);
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds);
            string c = ds.Tables[0].Rows[0]["账户余额"].ToString();
            float d = Convert.ToInt64(c);
            a = a - 1;
            b = (40 - a) * 2;
            cmd.Clone();
            if (a > 0)
            {
                if (d > b)
                {
                    time.Text = a.ToString();
                    price.Text = b.ToString();
                }
                else if (d == b)
                {
                    this.timer1.Enabled = false;
                    string m = "衣物已清洗完毕！请回收！已花费" + b;
                    url = url + strUid + strKey + strMob + phone + strContent + m;
                    string Result = GetHtmlFromUrl(url);
                    SqlCommand cmd1 = new SqlCommand("update 学生账户信息 set 账户余额='" + (d - b) + "'where 学号='" + CommonData.SID + "'", conn);
                    SqlDataReader sdr1 = cmd1.ExecuteReader();
                    MessageBox.Show("账户余额为：" + (d - b), "提示", 0);
                    this.Hide();
                }
                else
                {
                    this.timer1.Enabled = false;
                    string m = "余额不足！请回收！";                 
                    SqlCommand cmd1 = new SqlCommand("update 学生账户信息 set 账户余额='" + (d - b) + "'where 学号='" + CommonData.SID + "'", conn);
                    SqlDataReader sdr1 = cmd1.ExecuteReader();
                    url = url + strUid + strKey + strMob + phone + strContent + m;
                    string Result = GetHtmlFromUrl(url);
                    MessageBox.Show("余额不足！", "提示", 0);
                    this.Hide();
                }
            }
            else
            {

                this.timer1.Enabled = false;
                string m = "衣物已清洗完毕！请回收！已花费"+b;
                url = url + strUid + strKey + strMob + phone + strContent + m;
                string Result = GetHtmlFromUrl(url);
                SqlCommand cmd1 = new SqlCommand("update 学生账户信息 set 账户余额='" + (d-b) + "'where 学号='" + CommonData.SID + "'", conn);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                MessageBox.Show("账户余额为：" + (d-b), "提示", 0);
                this.Hide();
            }
        }

        private void 洗衣计时_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            float b;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("select 手机号 from 学生基本信息 where 学号='" + CommonData.SID + "'", conn);
            SqlDataAdapter adt2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            adt2.Fill(ds2);
            string phone = ds2.Tables[0].Rows[0]["手机号"].ToString();
            cmd2.Clone();
            SqlCommand cmd = new SqlCommand("select 账户余额 from 学生账户信息 where 学号='" + CommonData.SID + "'", conn);
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds);
            string c = ds.Tables[0].Rows[0]["账户余额"].ToString();
            float d = Convert.ToInt64(c);
            b =Convert.ToInt64(price.Text.Trim());
            cmd.Clone();
            SqlCommand cmd1 = new SqlCommand("update 学生账户信息 set 账户余额='" + (d - b) + "'where 学号='" + CommonData.SID + "'", conn);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            sdr1.Close();
            MessageBox.Show("账户余额为：" + (d - b), "提示", 0);

            string spot = "洗衣机";
            DateTime DT = DateTime.Now;
            SqlCommand cmd4 = new SqlCommand("insert into 消费记录(消费时间,学号,消费地点,消费金额) values ('" + DT + "','" + CommonData.SID + "','" + spot + "','" + b.ToString() + "')", conn);
            SqlDataReader sdr4 = cmd4.ExecuteReader();
            sdr4.Close();

            string m = "衣物已清洗完毕！请回收！已花费" +b;
            url = url + strUid + strKey + strMob + phone + strContent + m;
            string Result = GetHtmlFromUrl(url);
            this.Hide();
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
    }
}
