using System;
using System.Windows.Forms;

namespace 指静脉计费系统
{
    public partial class 主页 : Form
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
        public 主页()
        {
            InitializeComponent();
        }
        说明界面 smjm = new 说明界面();
        购物界面 gw = new 购物界面();
        选择认证 xzrz = new 选择认证();
        认证服务 ksrz = new 认证服务();

        private bool CheckFormIsOpen(string asFormName)
        {
            bool bResult = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == asFormName)
                {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }
        private void skinButton1_Click(object sender, EventArgs e)
        {
            bool a = CheckFormIsOpen("购物界面");
            bool b = CheckFormIsOpen("选择认证");
            bool c = CheckFormIsOpen("认证服务");
            if (a)
            {
                Application.OpenForms["购物界面"].Close();
            }
            else if (b)
            { Application.OpenForms["选择认证"].Close(); }
            else if (c)
            { Application.OpenForms["认证服务"].Close(); }
            else if (a && b)
            {
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["选择认证"].Close();
            }
            else if (b && c)
            {
                Application.OpenForms["选择认证"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && c)
            {
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && b && c)
            {
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["选择认证"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            说明界面 smjm = new 说明界面();
            smjm.FormBorderStyle = FormBorderStyle.None;
            smjm.Dock = DockStyle.Fill;
            smjm.TopLevel = false;
            this.panel6.Controls.Clear();
            this.panel6.Controls.Add(smjm);
            smjm.Show();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            bool a = CheckFormIsOpen("说明界面");
            bool b = CheckFormIsOpen("选择认证");
            bool c = CheckFormIsOpen("认证服务");
            if (a)
            {
                Application.OpenForms["说明界面"].Close();
            }
            else if (b)
            { Application.OpenForms["选择认证"].Close(); }
            else if (c)
            { Application.OpenForms["认证服务"].Close(); }
            else if (a && b)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["选择认证"].Close();
            }
            else if (b && c)
            {
                Application.OpenForms["选择认证"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && b && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["选择认证"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            购物界面 gw = new 购物界面();
            gw.FormBorderStyle = FormBorderStyle.None;
            gw.Dock = DockStyle.Fill;
            gw.TopLevel = false;
            this.panel6.Controls.Clear();
            this.panel6.Controls.Add(gw);
            gw.Show();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            bool a = CheckFormIsOpen("说明界面");
            bool b = CheckFormIsOpen("购物界面");
            bool c = CheckFormIsOpen("认证服务");
            if (a)
            {
                Application.OpenForms["说明界面"].Close();
            }
            else if (b)
            { Application.OpenForms["购物界面"].Close(); }
            else if (c)
            { Application.OpenForms["认证服务"].Close(); }
            else if (a && b)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["购物界面"].Close();
            }
            else if (b && c)
            {
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            else if (a && b && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["认证服务"].Close();
            }
            选择认证 xzrz = new 选择认证();
            xzrz.FormBorderStyle = FormBorderStyle.None;
            xzrz.Dock = DockStyle.Fill;
            xzrz.TopLevel = false;
            this.panel6.Controls.Clear();
            this.panel6.Controls.Add(xzrz);
            xzrz.Show();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            bool a = CheckFormIsOpen("说明界面");
            bool b = CheckFormIsOpen("购物界面");
            bool c = CheckFormIsOpen("选择认证");
            if (a)
            {
                Application.OpenForms["说明界面"].Close();
            }
            else if (b)
            { Application.OpenForms["购物界面"].Close(); }
            else if (c)
            { Application.OpenForms["选择认证"].Close(); }
            else if (a && b)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["购物界面"].Close();
            }
            else if (b && c)
            {
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["选择认证"].Close();
            }
            else if (a && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["选择认证"].Close();
            }
            else if (a && b && c)
            {
                Application.OpenForms["说明界面"].Close();
                Application.OpenForms["购物界面"].Close();
                Application.OpenForms["选择认证"].Close();
            }
            认证服务 ksrz = new 认证服务();
            ksrz.FormBorderStyle = FormBorderStyle.None;
            ksrz.Dock = DockStyle.Fill;
            ksrz.TopLevel = false;
            this.panel6.Controls.Clear();
            this.panel6.Controls.Add(ksrz);
            ksrz.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 主页_Load(object sender, EventArgs e)
        {

        }
    }
}
