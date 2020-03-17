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
    public partial class 人工界面 : Form
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
        public 人工界面()
        {
            InitializeComponent();
        }
        public static string strConn = "Data Source=(local);Initial Catalog=指静脉;Integrated Security=true";
        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void 人工界面_Load(object sender, EventArgs e)
        {
            string a = CommonData.SID;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM 学生基本信息 where 学号 = '" + a + "'", conn);
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds);
            this.name.Text = (string)ds.Tables[0].Rows[0]["姓名"];
            this.SID.Text = a;

            cmd.Clone();
            ds.Clone();
            
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM 学生账户信息 where 学号 = '" + a + "'", conn);
            SqlDataAdapter adt1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adt1.Fill(ds1);
            this.ye.Text = ds1.Tables[0].Rows[0]["账户余额"].ToString();
           
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (money.Text.Trim() == "")
            {
                MessageBox.Show("输入充值金额", "提示", 0);
            }
            else
            {
                string a = CommonData.SID;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT 账户余额 FROM 学生账户信息 where 学号 = '" + a + "'", conn);
                SqlDataAdapter adt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adt.Fill(ds);
                float b = Convert.ToInt64(ds.Tables[0].Rows[0]["账户余额"].ToString());
                float c = Convert.ToInt64(this.money.Text.Trim());
                float d = b + c;
                cmd.Clone();

                SqlCommand cmd1 = new SqlCommand("update 学生账户信息 set 账户余额='" + d + "'where 学号='" + a + "'", conn);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                MessageBox.Show("账户余额为：" + d, "提示", 0);
                this.Close();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
