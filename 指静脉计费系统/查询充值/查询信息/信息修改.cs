using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 指静脉计费系统
{
    public partial class 信息修改 : Form
    {
        public static string strConn = "Data Source=(local);Initial Catalog=指静脉;Integrated Security=true";

        public 信息修改()
        {
            InitializeComponent();
        }

        信息查询 xxcx = new 信息查询();
        public 信息修改(信息查询 formFrm)//这个构造方法里有参数 
        {
            xxcx = formFrm; //这个必须要有 
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            string a = CommonData.SID;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            SqlCommand cmd1 = new SqlCommand("update 学生基本信息 set 手机号='" + this.Telenumber.Text.Trim() + "'where 学号='" + a + "'", conn);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            MessageBox.Show("修改成功!" , "提示", 0);
            xxcx.StrLabel1 = this.Telenumber.Text;
            this.Close();
        }
    }
}
