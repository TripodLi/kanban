using MyOPC;
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
        public static OPCCreate OPC = null;
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
            try
            {
                //写入产品名称
                BussinessFacde.SetConfigXml("ProductionType", productionType.Text);
               // OPC.WriteItem(17, productionType.Text);
                //写入产品种类
                BussinessFacde.SetConfigXml("materialType", materialType.Text);
               // OPC.WriteItem(18, materialType.Text);
                //写入来料方式
                BussinessFacde.SetConfigXml("materialWay", materialWay.Text);
               // OPC.WriteItem(19, materialWay.Text);
                //写入物料PN
                BussinessFacde.SetConfigXml("materialPn", materialPn.Text);
               // OPC.WriteItem(20, materialPn.Text);
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
            catch
            {
                MessageBox.Show("写入数据出错，请检查基础配置是否正确或是否有中文！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            productionType.Text = "";
            materialType.Text = "";
            materialWay.Text = "";
            materialPn.Text = "";
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
            try
            {
                OPC = new OPCCreate();
                productionType.Text = BussinessFacde.GetConfigXml("ProductionType");
                materialType.Text = BussinessFacde.GetConfigXml("materialType");
                //写入来料方式
                materialWay.Text = BussinessFacde.GetConfigXml("materialWay");
                //写入物料PN
                materialPn.Text = BussinessFacde.GetConfigXml("materialPn");
                number.Text = BussinessFacde.GetConfigXml("TodayPlan");
                speed.Text = BussinessFacde.GetConfigXml("Speed");
            }
            catch
            {
                //关闭窗口
                this.Dispose();
                this.Close();
            }
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
        /// <summary>
        /// 限制不能输入中文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productionType_KeyPress(object sender, KeyPressEventArgs e)
        {
       

            //允许输入字母、点、回退键、数字
            if (((int)e.KeyChar >= (int)'a' && (int)e.KeyChar <= (int)'z') || (((int)e.KeyChar > 48 && (int)e.KeyChar < 57) || (int)e.KeyChar == 8 || (int)e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else e.Handled = true ;


            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (productionType.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(productionType.Text, out oldf);
                    b2 = float.TryParse(productionType.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }

        private void materialType_KeyPress(object sender, KeyPressEventArgs e)
        {
            //允许输入字母、点、回退键、数字
            if (((int)e.KeyChar >= (int)'a' && (int)e.KeyChar <= (int)'z') || (((int)e.KeyChar > 48 && (int)e.KeyChar < 57) || (int)e.KeyChar == 8 || (int)e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else e.Handled = true;


            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (materialType.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(materialType.Text, out oldf);
                    b2 = float.TryParse(materialType.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }

        private void materialWay_KeyPress(object sender, KeyPressEventArgs e)
        {
            //允许输入字母、点、回退键、数字
            if (((int)e.KeyChar >= (int)'a' && (int)e.KeyChar <= (int)'z') || (((int)e.KeyChar > 48 && (int)e.KeyChar < 57) || (int)e.KeyChar == 8 || (int)e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else e.Handled = true;


            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (materialWay.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(materialWay.Text, out oldf);
                    b2 = float.TryParse(materialWay.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }

        private void materialPn_KeyPress(object sender, KeyPressEventArgs e)
        {
            //允许输入字母、点、回退键、数字
            if (((int)e.KeyChar >= (int)'a' && (int)e.KeyChar <= (int)'z') || (((int)e.KeyChar > 48 && (int)e.KeyChar < 57) || (int)e.KeyChar == 8 || (int)e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else e.Handled = true;


            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (materialPn.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(materialPn.Text, out oldf);
                    b2 = float.TryParse(materialPn.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }


      
    }
}
