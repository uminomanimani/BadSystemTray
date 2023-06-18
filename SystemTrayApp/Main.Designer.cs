using System.Drawing;

namespace SystemTrayApp
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            button = new System.Windows.Forms.Button();
            timer = new System.Windows.Forms.Timer(this.components);


            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            for (int i = 0; i < notifyIcons.Length; i++)
            {
                notifyIcons[i] = new System.Windows.Forms.NotifyIcon(this.components);
            }

            for (int i = 0; i != notifyIcons.Length; i++)
            {
                this.notifyIcons[i].Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
                this.notifyIcons[i].Text = i.ToString();
                this.notifyIcons[i].Visible = true;
            }

            this.SuspendLayout();
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            //button
            button.Size = new System.Drawing.Size(Width / 2, Height / 2);
            button.Location = new System.Drawing.Point(75, 75);
            button.Click += ButtonClick;
            button.Text = "はじめよう!";
            button.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            //

            //timer
            timer.Interval = 25;
            timer.Tick += TimerTick;
            timer.Enabled = false;
            timer.Enabled = true;
            //

            Controls.Add(button);
            this.Name = "Main";
            this.Text = "BadSystemTray!!";
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.NotifyIcon[] notifyIcons = new System.Windows.Forms.NotifyIcon[49];
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Timer timer;
    }
}

