using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KanBan
{
    public partial class DataConfig : Form
    {
        public DataConfig()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataConfig_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }
        /// <summary>
        /// 提交用户的配置到到XML文件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //写入产品种类
            BussinessFacde.SetConfigXml("ProductionType", productionType.Text);
            //写入今日计划
            BussinessFacde.SetConfigXml("TodayPlan", number.Text);
            //写入页面速度
            BussinessFacde.SetConfigXml("Speed", speed.Text);
          
            //关闭窗口
            this.Dispose();
            this.Close();
            //重新启动
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            productionType.Text = "";
            number.Text = "";
            speed.Text = "";
        }

        /// <summary>
        /// 限制速度只能输入数字
        /// </summary>
        private bool nonNumberEntered = false;
        private void speed_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;
            if ((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9 && e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9))
            {
                if (e.KeyCode != Keys.Back)
                {
                    nonNumberEntered = true;
                }
            }
        }

        private void speed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered)
            {
                e.Handled = true;
            }
        }

        private void DataConfig_Load(object sender, EventArgs e)
        {
            productionType.Text = BussinessFacde.GetConfigXml("ProductionType");
            number.Text = BussinessFacde.GetConfigXml("TodayPlan");
            speed.Text = BussinessFacde.GetConfigXml("Speed");
        }

        private void number_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;
            if ((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9 && e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9))
            {
                if (e.KeyCode != Keys.Back)
                {
                    nonNumberEntered = true;
                }
            }
        }

        private void number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered)
            {
                e.Handled = true;
            }
        }

      
    }
}
