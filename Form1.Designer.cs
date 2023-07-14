
using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Form1
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.SuspendLayout();
            //// 
            //// Form1
            //// 
            //this.ClientSize = new System.Drawing.Size(282, 253);
            //this.Name = "Form1";
            //this.ResumeLayout(false);
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

        }

       
        #endregion
    }
}

