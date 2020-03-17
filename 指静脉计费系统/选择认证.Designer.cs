namespace 指静脉计费系统
{
    partial class 选择认证
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(选择认证));
            this.skinButton4 = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // skinButton4
            // 
            this.skinButton4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.skinButton4.BackColor = System.Drawing.Color.Transparent;
            this.skinButton4.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton4.DownBack = null;
            this.skinButton4.Font = new System.Drawing.Font("宋体", 35F, System.Drawing.FontStyle.Bold);
            this.skinButton4.Location = new System.Drawing.Point(302, 188);
            this.skinButton4.Margin = new System.Windows.Forms.Padding(4);
            this.skinButton4.MouseBack = null;
            this.skinButton4.Name = "skinButton4";
            this.skinButton4.NormlBack = null;
            this.skinButton4.Size = new System.Drawing.Size(504, 181);
            this.skinButton4.TabIndex = 2;
            this.skinButton4.Text = "身份认证";
            this.skinButton4.UseVisualStyleBackColor = false;
            this.skinButton4.Click += new System.EventHandler(this.skinButton4_Click);
            // 
            // 选择认证
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1069, 554);
            this.Controls.Add(this.skinButton4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "选择认证";
            this.Text = "选择认证";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton skinButton4;
    }
}