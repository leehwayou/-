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
    public partial class 购物界面 : Form
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
        public 购物界面()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            购物2 gw = new 购物2();
            gw.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            购物1 gw = new 购物1();
            gw.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            超市 cs = new 超市();
            cs.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            洗衣 xy = new 洗衣();
            xy.Show();
        }
    }
}
