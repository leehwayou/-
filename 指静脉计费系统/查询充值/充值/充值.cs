using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 指静脉计费系统
{
    public partial class 充值 : Form
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
        public 充值()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            人工界面 rgjm = new 人工界面();
            rgjm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            二维码 ewm = new 二维码();
            ewm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            二维码 ewm = new 二维码();
            ewm.Show();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
