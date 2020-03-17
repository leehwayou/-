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
    public partial class 信息查询 : Form
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

        public static string strConn = "Data Source=(local);Initial Catalog=指静脉;Integrated Security=true";

        public 信息查询()
        {
            InitializeComponent();
        }

        string strLabel1 = "";
        public string StrLabel1
        {
            get
            {
                return strLabel1;
            }
            set
            {
                strLabel1 = value;
                this.Telephone.Text = strLabel1;
            }
        }

        private void 信息查询_Load(object sender, EventArgs e)
        {
            // 调整字体  
            dataGridView1.Font = new Font("宋体", 11, FontStyle.Bold);
            // 调整行高  
            //dataGridView1.Rows[0].Height = 100;  
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.Update();
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            string a = CommonData.SID;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM 学生基本信息 where 学号 = '" + a + "'", conn);
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds);
            this.name.Text = (string)ds.Tables[0].Rows[0]["姓名"];
            this.sId.Text = a;
            this.academy.Text = (string)ds.Tables[0].Rows[0]["学院"];
            this.profession.Text = (string)ds.Tables[0].Rows[0]["专业班级"];
            this.Telephone.Text = (string)ds.Tables[0].Rows[0]["手机号"];
            cmd.Clone();
            SqlCommand cmd1 = new SqlCommand("SELECT 消费时间,消费地点,购买商品,消费金额 FROM 消费记录 where 学号 = '" + a + "'", conn);
            SqlDataAdapter adt1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adt1.Fill(ds1);
            dataGridView1.DataSource = ds1.Tables[0];
            cmd1.Clone();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM 学生账户信息 where 学号 = '" + a + "'", conn);
            SqlDataAdapter adt2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            adt2.Fill(ds2);
            this.balance.Text =ds2.Tables[0].Rows[0]["账户余额"].ToString();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            信息修改 xxxg = new 信息修改(this);
            xxxg.StartPosition = FormStartPosition.CenterScreen;
            xxxg.Show();
        }

        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
