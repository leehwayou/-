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
    public partial class 选择认证 : Form
    {
        public 选择认证()
        {
            InitializeComponent();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            指纹指静脉校验1 jy1 = new 指纹指静脉校验1();
            jy1.Show();
        }
    }
}
