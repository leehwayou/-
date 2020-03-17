using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace 指静脉计费系统
{
    public partial class 说明界面 : Form
    {
        private string url = "http://utf8.sms.webchinese.cn/?";
        private string strUid = "Uid=myonlylove";
        private string strKey = "&key=d795c6e443d027461418";
        private string strMob = "&smsMob=";
        private string strContent = "&smsText=";
        public static string strConn = "Data Source=(local);Initial Catalog=指静脉;Integrated Security=true";

        public string PhoneNum
        {
            get { return phone.Text; }
        }

        public string SID
        {
            get { return sId.Text; }
        }

        public 说明界面()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (phone.Text.Trim() == "")
            {
                MessageBox.Show("请输入手机号！", "警告", 0);
            }
            else
            {
                Random rad = new Random();
                int value = rad.Next(100000, 1000000);
                string a = value.ToString();
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO 验证码(手机号,验证码) VALUES ('" + phone.Text.Trim() + "','" + a + "')", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                string b = "欢迎使用本系统，验证码：" + a;
                if (phone.Text.ToString().Trim() != "" )
                {
                    url = url + strUid + strKey + strMob + phone.Text + strContent +b;
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
                CommonData.PhoneNum = PhoneNum;
                CommonData.SID = SID;
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

        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (sId.Text.Trim() == "" || ID.Text.Trim() == "" || phone.Text.Trim() == "" || check.Text.Trim() == "")
            {
                MessageBox.Show("请输入完整信息！", "警告", 0);
            }
            else
            {
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
               
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM 学生基本信息 where 学号 = '" + sId.Text.Trim() + "' and 身份证号='"+ID.Text.Trim()+"'", conn);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if(sdr1.HasRows)
                {
                    sdr1.Close();
                    cmd1.Clone();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM 验证码 where 手机号 = '" + phone.Text.Trim() + "'", conn);
                    cmd.ExecuteNonQuery();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        cmd.Clone();
                        sdr.Close();
                        SqlCommand cmd3 = new SqlCommand("SELECT * FROM 学生账户信息 where 学号 = '" + sId.Text.Trim() + "'", conn);
                        cmd.ExecuteNonQuery();
                        SqlDataReader sdr3 = cmd3.ExecuteReader();
                        if (sdr3.HasRows)
                        {
                            MessageBox.Show("该用户已注册","提示",0);
                        }
                        else
                        {
                            cmd3.Clone();
                            sdr3.Close();
                            float c = 0;
                            SqlCommand cmd2 = new SqlCommand("insert into 学生账户信息(学号,账户余额) values ('" + sId.Text.Trim() + "'," + c + ")", conn);
                            cmd.ExecuteNonQuery();
                            SqlDataReader sdr2 = cmd2.ExecuteReader();
                            MessageBox.Show("验证成功，请录入指纹指静脉信息！", "提示", 0);
                            this.Hide();
                            指纹指静脉录入 lr = new 指纹指静脉录入();
                            lr.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("验证码输入错误，请重试！", "提示", 0);
                    }
                    conn.Close();
                }
                else
                    MessageBox.Show("查无此人，请确认学号、身份证信息！", "提示", 0);

            }
        }
    }
}
