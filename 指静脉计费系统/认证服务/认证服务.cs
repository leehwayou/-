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
    public partial class 认证服务 : Form
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
        public 认证服务()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            考试认证 ksrz = new 考试认证();
            ksrz.Show();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            学生考勤 xskq = new 学生考勤();
            xskq.Show();
        }
    }
}
