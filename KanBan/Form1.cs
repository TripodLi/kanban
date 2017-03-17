using MyOPC;
using MySql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace KanBan
{
    public partial class Form1 : Form
    {
        // 根据配置文件获得产品类型
        public String productionTypeconfig = BussinessFacde.GetConfigXml("ProductionType");
        //根据配置文件获取今日计划产量
        public String productionTodayPlan = BussinessFacde.GetConfigXml("TodayPlan");
        public String showText = BussinessFacde.GetConfigXml("ShowText");
        public static OPCCreate OPC = null;
        DbUtility db = new DbUtility("Data Source=" + GetXml("DataSource", "value") + ";Initial Catalog=" + GetXml("InitialCatalog", "value") + ";User ID=" + GetXml("UserId", "value") + ";pwd=" + GetXml("Password", "value"), DbProviderType.SqlServer);
        int stopSecondTimes;
        int stopMinTimers;
        public Form1()
        {
            InitializeComponent();
            //必须全部的父控件都是窗体
            panel1.Parent = this;
            panel13.Parent = this;
            panel16.Parent = this;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 窗体拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        /// <summary>
        /// 鼠标在Panel3上拖动窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }
        /// <summary>
        /// 鼠标在Panel1上拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }
        /// <summary>
        /// 窗体上显示菜单
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, e.Location);
            }
        }
        /// <summary>
        /// 菜单项的关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
        /// <summary>
        /// 菜单项的显示配置窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 显示配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataConfig dc = new DataConfig();
            dc.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            panel1.Visible = true;
            panel13.Visible = false;
            panel16.Visible = false;
            //显示产品种类
            LB_ProductionType.Text = productionTypeconfig;
            //显示今日计划
            LB_TodayPlan.Text = productionTodayPlan + " ea";
            StartBussinessThread(); //开启线程
            this.Resize += new EventHandler(Form1_Resize);
            X = this.Width;
            Y = this.Height;
            setTag(this);
            Form1_Resize(new object(), new EventArgs());//x,y可在实例化时赋值,最后这句是新加的，在MDI时有用

        }
        /// <summary>
        /// 开启业务处理的线程
        /// </summary>
        public void StartBussinessThread()
        {
            //开启页面跳转线程
            String speed = BussinessFacde.GetConfigXml("Speed"); //获取配置的页面跳转速度
            if (!String.IsNullOrEmpty(speed) && Convert.ToInt32(speed) > 0)   //配置的速度不为空
            {
                OverrideTimer tt = new OverrideTimer();
                //将用户配置的时间转换为毫秒
                int speedControl = Convert.ToInt32(speed) * 1000;
                tt.Interval = speedControl;
                tt.Elapsed += new System.Timers.ElapsedEventHandler(ChangePageBusinessProcess);
                tt.Enabled = true;
                tt.AutoReset = false;
            }
            else   //如果用户没有配置，则默认10秒切换页面
            {
                OverrideTimer tt = new OverrideTimer();
                //将用户配置的时间转换为毫秒
                tt.Interval = 10000;
                tt.Elapsed += new System.Timers.ElapsedEventHandler(ChangePageBusinessProcess);
                tt.Enabled = true;
                tt.AutoReset = false;
            }

            //开启地址监听线程，实时刷新看板数据
            String stationText = BussinessFacde.GetConfigXml("Station");
            if (!String.IsNullOrEmpty(stationText))
            {
               
                String[] stationArray = stationText.Split(',');
                for (int i = 0; i < stationArray.Length; i++)
                {
                    if (i < 8)  //一个页面放8个工位的图片
                    {
                        if (i < 4)
                        {
                            StationPenalControl spc = new StationPenalControl();
                            panel1.Controls.Add(spc);
                            spc.Location = new Point(28 + 200 * i, 139);
                            spc.LB_PanelStation.Text = "OP" + stationArray[i];
                            TimeSpan time = TimeSpan.Parse("00:00:00");
                            TimeHelper myTime = new TimeHelper(time, spc.LB_ErrorTimes);
                            OverrideTimer tt = new OverrideTimer();
                            tt.MYTIME = myTime;
                            tt.Spc = spc;
                            tt.Type = "1";
                            tt.Station = stationArray[i];
                            tt.Interval = 1000;
                            tt.Elapsed += new System.Timers.ElapsedEventHandler(ShowBusinessProcess);
                            tt.Enabled = true;
                            tt.AutoReset = false;
                        }
                        else
                        {
                            StationPenalControl spc = new StationPenalControl();
                            spc.LB_PanelStation.Text = "OP" + stationArray[i];
                            panel1.Controls.Add(spc);
                            spc.Location = new Point(28 + 200 *(i-4), 139*2);
                            TimeSpan time = TimeSpan.Parse("00:00:00");
                            TimeHelper myTime = new TimeHelper(time, spc.LB_ErrorTimes);
                            OverrideTimer tt = new OverrideTimer();
                            tt.MYTIME = myTime;
                            tt.Spc = spc;
                            tt.Type = "1";
                            tt.Station = stationArray[i];
                            tt.Interval = 1000;
                            tt.Elapsed += new System.Timers.ElapsedEventHandler(ShowBusinessProcess);
                            tt.Enabled = true;
                            tt.AutoReset = false;
                        }
                       
                    }
                    else if (i >= 8 && i<16)
                    {
                        if (i < 12)
                        {
                            StationPenalControl spc = new StationPenalControl();
                            panel16.Controls.Add(spc);
                            spc.Location = new Point(28 + 200 * (i-8), 139);
                            spc.LB_PanelStation.Text = "OP" + stationArray[i];
                            TimeSpan time = TimeSpan.Parse("00:00:00");
                            TimeHelper myTime = new TimeHelper(time, spc.LB_ErrorTimes);
                            OverrideTimer tt = new OverrideTimer();
                            tt.MYTIME = myTime;
                            tt.Spc = spc;
                            tt.Type = "1";
                            tt.Station = stationArray[i];
                            tt.Interval = 1000;
                            tt.Elapsed += new System.Timers.ElapsedEventHandler(ShowBusinessProcess);
                            tt.Enabled = true;
                            tt.AutoReset = false;
                        }
                        else
                        {
                            StationPenalControl spc = new StationPenalControl();
                            panel16.Controls.Add(spc);
                            spc.Location = new Point(28 + 200 * (i-12), 139*2);
                            spc.LB_PanelStation.Text = "OP" + stationArray[i];
                            TimeSpan time = TimeSpan.Parse("00:00:00");
                            TimeHelper myTime = new TimeHelper(time, spc.LB_ErrorTimes);
                            OverrideTimer tt = new OverrideTimer();
                            tt.MYTIME = myTime;
                            tt.Spc = spc;
                            tt.Type = "1";
                            tt.Station = stationArray[i];
                            tt.Interval = 1000;
                            tt.Elapsed += new System.Timers.ElapsedEventHandler(ShowBusinessProcess);
                            tt.Enabled = true;
                            tt.AutoReset = false;
                        }
                       
                    }
                   
                  
                    //    tt.Type = BussinessFacde.GetConfigXml("ProductionType");
                    //   tt.Plan = BussinessFacde.GetConfigXml("TodayPlan");
                   
                }
            }

            //开启产品上产状态线程
            OverrideTimer t1 = new OverrideTimer();
            //将用户配置的时间转换为毫秒
            t1.Interval = 2000;
            t1.Elapsed += new System.Timers.ElapsedEventHandler(showProductionState);
            t1.Enabled = true;
            t1.AutoReset = false;
        }
        /// <summary>
        /// 显示设备状态信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void ShowBusinessProcess(object source, System.Timers.ElapsedEventArgs e)
        {
            OverrideTimer tt = (OverrideTimer)source;
            tt.Stop();
            try
            {
                String pruductionType = tt.Type;
                String todayPlan = tt.Plan;
                String station = tt.Station;
                StationPenalControl spc = tt.Spc;
                TimeHelper myTime = tt.MYTIME;
                //显示工位
          //      BeginInvoke((MethodInvoker)delegate() { spc.LB_PanelStation.Text = "OP "+station; });
                int eqstateControl = Convert.ToInt32(BussinessFacde.GetOpcConfigXml(station, "1", "1"));
                String eqstate = OPC.ReadItem(eqstateControl).ToString(); //获取PLC传过来的设备状态信息
                if (eqstate == "1") //表示设备正常
                {
                    BeginInvoke((MethodInvoker)delegate() { spc.panel1.BackColor = System.Drawing.Color.Lime; });
                    if (spc.LB_ErrorTimes.Text != "")
                    {
                        BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorTimes.Text = ""; });
                        BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorNotice.Text = ""; });
                        myTime.Close();
                    }
                    
                }
                else if(eqstate=="2")  //设备故障
                {
                    BeginInvoke((MethodInvoker)delegate() { spc.panel1.BackColor = System.Drawing.Color.Red; });
                    BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorNotice.Text = "停机时间："; });
                    //显示停机时间
                    //for (int i = 1; i <= 100; i++)
                    //{
                    //    if (i == 100)
                    //    {
                    //        stopSecondTimes++; 
                    //    }
                    //    stopMinTimers = i;
                    //    BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorTimes.Text = stopSecondTimes + "'" + stopMinTimers+"''"; });
                    //}
                    if (spc.LB_ErrorTimes.Text != "")
                    {
                       
                    }
                    else
                    {
                        myTime.Open();
                    }
                    
                }
                else if (eqstate == "3") //设备缺料
                {
                    BeginInvoke((MethodInvoker)delegate() { spc.panel1.BackColor = System.Drawing.Color.Yellow; });
                    BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorNotice.Text = "等待时间："; });
                    //显示等待时间
                    //for (int i = 1; i <= 100; i++)
                    //{
                    //    if (i == 100)
                    //    {
                    //        stopSecondTimes++;
                    //    }
                    //    stopMinTimers = i;
                    //    BeginInvoke((MethodInvoker)delegate() { spc.LB_ErrorTimes.Text = stopSecondTimes + "'" + stopMinTimers + "''"; });
                    //}
                    if (spc.LB_ErrorTimes.Text != "")
                    {

                    }
                    else
                    {
                        myTime.Open();
                    }
                }

            }
            catch
            {

               
            }
            finally
            {
                tt.Start();
            }
        }
        /// <summary>
        /// 页面切换业务处理方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void ChangePageBusinessProcess(object source, System.Timers.ElapsedEventArgs e)
        {
            OverrideTimer tt = (OverrideTimer)source;
            tt.Stop();
            int pageNo=0;
            try
            {
                if (panel1.Visible)
                {
                    pageNo = 1;
                   
                     
                }
                else if(panel16.Visible)
                {
                    pageNo = 2;
                    //panel1.Visible = true;
                    //panel13.Visible = false;
                   
                }
                else if (panel13.Visible)
                {
                    pageNo = 3;
                   
                }
                switch (pageNo)
                { 
                    case 1:
                       
                         BeginInvoke((MethodInvoker)delegate() { this.panel13.Visible = false; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel1.Visible = false; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel16.Visible = true; });
                      //   BeginInvoke((MethodInvoker)delegate() { this.panel13.BringToFront(); });
                          break;
                    case 2:
                        
                         BeginInvoke((MethodInvoker)delegate() { this.panel13.Visible = true; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel16.Visible = false; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel1.Visible = false; });
                     //    BeginInvoke((MethodInvoker)delegate() { this.panel16.BringToFront(); });
                          break;
                    case 3:
                       
                         BeginInvoke((MethodInvoker)delegate() { this.panel16.Visible = false; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel13.Visible = false; });
                         BeginInvoke((MethodInvoker)delegate() { this.panel1.Visible = true; });
                     //    BeginInvoke((MethodInvoker)delegate() { this.panel1.BringToFront(); });
                         break;
                }
               
            }
            catch
            {


            }
            finally
            {
                tt.Start();
            }
        }
        /// <summary>
        /// 显示产品状态的业务方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void showProductionState(object source, System.Timers.ElapsedEventArgs e)
        {
            OverrideTimer tt = (OverrideTimer)source;
            tt.Stop();
            try
            {
                //显示产品种类
                BeginInvoke((MethodInvoker)delegate() { LB_ProductionType.Text = productionTypeconfig; });
                
                //显示今日计划
                BeginInvoke((MethodInvoker)delegate() { LB_TodayPlan.Text = productionTodayPlan + " ea"; });
                
                //显示已经完成的产量
                int fishedControl = Convert.ToInt32(BussinessFacde.GetOpcConfigXml("", "1", "2"));
                //if(Convert.ToInt32(OPC.ReadItem(fishedControl).ToString())==1)
                //{
                    string sql = "select distinct sn from OfflinePack where DT between  '" + DateTime.Now.ToString("yyyy.MM.dd") + " 00:00:00 ' and  '" + DateTime.Now.ToString("yyyy.MM.dd  HH:mm:ss")+"'" ;
                     DataTable dt = new DataTable();
                     dt = db.ExecuteDataTable(sql);
                     int finishedNumber = dt.Rows.Count;

                     BeginInvoke((MethodInvoker)delegate() { LB_Fished.Text = finishedNumber + " ea"; });
                     double fishedP =(double) finishedNumber/ Convert.ToInt32(productionTodayPlan);

                     BeginInvoke((MethodInvoker)delegate() { LB_FishenP.Text = fishedP.ToString("P"); });
                     
                //}
                int moduleAddr = Convert.ToInt32(BussinessFacde.GetOpcConfigXml("", "4", "2"));
                string moduleS=OPC.ReadItem(moduleAddr).ToString();
                if (moduleS == "1")
                {

                    BeginInvoke((MethodInvoker)delegate() { moduleStatus.Text = showText; });
                    BeginInvoke((MethodInvoker)delegate() { moduleStatus.ForeColor = Color.Red; });
                    
                }
                else
                {
                    BeginInvoke((MethodInvoker)delegate() { moduleStatus.Text = ""; });
                }
            }
            catch
            { 
            
            }
            finally
            {
                tt.Start();
            }
        }
        /// <summary>
        /// 显示当前的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
         LB_PanelDate.Text = DateTime.Now.ToString("yyyy.MM.dd  HH:mm:ss");
         LB_Panel1Date.Text = DateTime.Now.ToString("yyyy.MM.dd  HH:mm:ss");
         LB_Panel16Date.Text = DateTime.Now.ToString("yyyy.MM.dd  HH:mm:ss ");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                OPC = new OPCCreate();
               
            }
            catch
            {
                MessageBox.Show("OPC初始化失败！请检查OPC配置文件或网络是否连通！");
            }
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void panel13_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
        // 以下为控件全屏是自适应方法
        private float X;

        private float Y;

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {

                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy);
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }

        }

        void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
            this.Text = this.Width.ToString() + " " + this.Height.ToString();

        }
        /// <summary>
        /// 读取数据库和功能配置文件文件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetXml(string node, string attribute)
        {
            string result = null;
            XmlDocument xmlDoc = new XmlDocument();
            string addr = "DataBaseMap.xml";
            xmlDoc.Load(addr);
            XmlNode nd;
            nd = xmlDoc.SelectSingleNode("Config");
            XmlNodeList xnl = nd.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == node)
                {
                    result = xe.GetAttribute(attribute);
                }
            }
            return result;
        }
    }
}
