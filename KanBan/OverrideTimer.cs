using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KanBan
{
    public class OverrideTimer : System.Timers.Timer
    {
        private String productionType;
        private String todayPlan;
        private String station;
        private String type;
        private StationPenalControl spc;
        private Panel pe;
        private TimeHelper myTime;
        public TimeHelper MYTIME
        {
            set { myTime = value; }
            get { return myTime; }
        }
        public Panel Pe
        {
            set { pe = value; }
            get { return pe; }
        }
        public StationPenalControl Spc
        {
            set { spc = value; }
            get { return spc; }
        }
        public String ProductionType
        {
            get { return productionType; }
            set { productionType = value; }
        }
        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        public String Plan
        {
            set { todayPlan = value; }
            get { return todayPlan; }
        }
        public String Station
        {
            set { station = value; }
            get { return station; }
        }
        public OverrideTimer()
            : base()
        {

        }
    }
}
