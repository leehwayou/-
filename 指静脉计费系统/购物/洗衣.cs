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
    public partial class 洗衣 : Form
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
        public 洗衣()
        {
            InitializeComponent();
        }


        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            指纹指静脉校验2 xy2 = new 指纹指静脉校验2();
            xy2.Show();
        }
    }
}
