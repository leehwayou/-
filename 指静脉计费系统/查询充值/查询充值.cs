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
    public partial class 查询充值 : Form
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
        public 查询充值()
        {
            InitializeComponent();
        }

        private void 查询充值_Load(object sender, EventArgs e)
        {

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            信息查询 xxcx = new 信息查询();
            xxcx.Show();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            充值 cz = new 充值();
            cz.Show();
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
