using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KanBan
{
  public  class TimeHelper
    {
        Thread thread;
        private TimeSpan time;  //计时时间
        private TimeSpan endTime;   //到点时间
        private System.Windows.Forms.Label lb;
        private bool whereExit = true;

        /// <summary>
        /// 设定计时器计时的时间
        /// </summary>
        /// <param name="StartTime">计时器时间，如：01:00:00 既1小时</param>
        public TimeHelper(TimeSpan StartTime, Label lb)
        {
            time = StartTime;
            this.lb = lb;
        }

        public void ShowLabel()
        {
            lb.Text = time.ToString();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTime()
        {
            return time;
        }

        /// <summary>
        /// 开启计时器
        /// </summary>
        public void Open()
        {
            //计算到点时间
            TimeSpan tsNow = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
          //  TimeSpan tsAdd = time;
            endTime = tsNow;// +tsAdd;
            //线程开始
            whereExit = false;
            thread = new Thread(TimeThreadStart);
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 关闭计时器
        /// </summary>
        public void Close()
        {
            whereExit = true;
            thread.Join(1000);
        }

        private void TimeThreadStart()
        {
            while (!whereExit)
            {
                RunTime();
                Thread.Sleep(1000);
            }
        }

        private delegate void RunTimeDelegate();
        private void RunTime()
        {
            if (lb.InvokeRequired)
            {
                RunTimeDelegate d = RunTime;
                lb.Invoke(d);
            }
            else
            {
             //   time = endTime - TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                time =TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"))- endTime;
                lb.Text = time.ToString();
            }
        }
    }
}
