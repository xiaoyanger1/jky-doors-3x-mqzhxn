using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Common;

namespace text.doors.Detection
{
    public partial class PlaneDeformation : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";
        /// <summary>
        /// 图标定时
        /// </summary>
        public DateTime dtnow { get; set; }

        public PlaneDeformation(SerialPortClient serialPortClient, string tempCode)
        {
            InitializeComponent();
            this._serialPortClient = serialPortClient;
            this._tempCode = tempCode;
            Init();

        }

        public void Init()
        {
            PMchartInit();
            BindSetTitle();
        }

        #region 图表控制
        /// <summary>
        /// 风速图标
        /// </summary>
        private void PMchartInit()
        {
            dtnow = DateTime.Now;
            pm_Line.GetVertAxis.SetMinMax(-600, 600);
        }

        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.pm_Line.Add(DateTime.Now, yl);
            this.tChart_pm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(40));
        }


        #endregion

        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void BindSetTitle()
        {
            lbl_title.Text = string.Format("幕墙平面内变形检测  第{0}号", this._tempCode);
        }

        /// <summary>
        /// 绑定风速
        /// </summary>
        private void BindLevel()
        {
            dgv_level.DataSource = null;

            dgv_level.RowHeadersVisible = false;
            dgv_level.AllowUserToResizeColumns = false;
            dgv_level.AllowUserToResizeRows = false;
            dgv_level.Columns[0].HeaderText = "压力Pa";
            dgv_level.Columns[0].Width = 53;
            dgv_level.Columns[0].ReadOnly = true;
            dgv_level.Columns[0].DataPropertyName = "Pa";

            dgv_level.Columns[1].HeaderText = "类型";
            dgv_level.Columns[1].Width = 2;
            dgv_level.Columns[1].ReadOnly = true;
            dgv_level.Columns[1].Visible = false;
            dgv_level.Columns[1].DataPropertyName = "PaType";


            dgv_level.Columns[2].HeaderText = "附加渗透";
            dgv_level.Columns[2].Width = 54;
            dgv_level.Columns[2].DataPropertyName = "FJST";

            dgv_level.Columns[3].HeaderText = "固附之和";
            dgv_level.Columns[3].Width = 54;
            dgv_level.Columns[3].DataPropertyName = "GFZH";

            dgv_level.Columns[4].HeaderText = "总的渗透量";
            dgv_level.Columns[4].Width = 58;
            dgv_level.Columns[4].DataPropertyName = "ZDST";

            dgv_level.Columns[5].HeaderText = "幕墙整体";
            dgv_level.Columns[5].Width = 54;
            dgv_level.Columns[5].ReadOnly = true;
            dgv_level.Columns[5].DataPropertyName = "MQZT";

            dgv_level.Columns[6].HeaderText = "可开渗透";
            dgv_level.Columns[6].Width = 54;
            dgv_level.Columns[6].ReadOnly = true;
            dgv_level.Columns[6].DataPropertyName = "KKST";


            dgv_level.Columns[2].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[3].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[4].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[5].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[6].DefaultCellStyle.Format = "N2";
        }
        private void btn_qt_Click(object sender, EventArgs e)
        {

        }

        private void btn_hl_Click(object sender, EventArgs e)
        {

        }

        private void btn_zore_Click(object sender, EventArgs e)
        {

        }

        private void btn_level1_Click(object sender, EventArgs e)
        {

        }

        private void btn_level2_Click(object sender, EventArgs e)
        {

        }

        private void btn_level3_Click(object sender, EventArgs e)
        {

        }

        private void btn_level4_Click(object sender, EventArgs e)
        {

        }

        private void btn_level5_Click(object sender, EventArgs e)
        {

        }

        private void btn_stop1_Click(object sender, EventArgs e)
        {

        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int c = 0;
            //if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZReady || airtightPropertyTest == PublicEnum.AirtightPropertyTest.FReady)
            //{
            //    c = _serialPortClient.GetCY_High();
            //}
            //else
            //{
            //    c = _serialPortClient.GetCY_Low();
            //}


            lbl_dqyl.Text = c.ToString();

            AnimateSeries(this.tChart_pm, c);
        }
    }
}
