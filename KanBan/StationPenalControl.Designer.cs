namespace KanBan
{
    partial class StationPenalControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LB_ErrorTimes = new System.Windows.Forms.Label();
            this.LB_ErrorNotice = new System.Windows.Forms.Label();
            this.LB_PanelStation = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Lime;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.LB_PanelStation);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(138, 114);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LB_ErrorTimes);
            this.panel2.Controls.Add(this.LB_ErrorNotice);
            this.panel2.Location = new System.Drawing.Point(3, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(126, 77);
            this.panel2.TabIndex = 1;
            // 
            // LB_ErrorTimes
            // 
            this.LB_ErrorTimes.AutoSize = true;
            this.LB_ErrorTimes.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LB_ErrorTimes.Location = new System.Drawing.Point(52, 46);
            this.LB_ErrorTimes.Name = "LB_ErrorTimes";
            this.LB_ErrorTimes.Size = new System.Drawing.Size(0, 16);
            this.LB_ErrorTimes.TabIndex = 1;
            // 
            // LB_ErrorNotice
            // 
            this.LB_ErrorNotice.AutoSize = true;
            this.LB_ErrorNotice.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LB_ErrorNotice.Location = new System.Drawing.Point(3, 9);
            this.LB_ErrorNotice.Name = "LB_ErrorNotice";
            this.LB_ErrorNotice.Size = new System.Drawing.Size(0, 19);
            this.LB_ErrorNotice.TabIndex = 0;
            // 
            // LB_PanelStation
            // 
            this.LB_PanelStation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LB_PanelStation.AutoSize = true;
            this.LB_PanelStation.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LB_PanelStation.Location = new System.Drawing.Point(42, 9);
            this.LB_PanelStation.Name = "LB_PanelStation";
            this.LB_PanelStation.Size = new System.Drawing.Size(0, 19);
            this.LB_PanelStation.TabIndex = 0;
            // 
            // StationPenalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "StationPenalControl";
            this.Size = new System.Drawing.Size(138, 115);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label LB_PanelStation;
        public System.Windows.Forms.Label LB_ErrorNotice;
        public System.Windows.Forms.Label LB_ErrorTimes;
        public System.Windows.Forms.Panel panel1;
    }
}
