using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using text.doors.Model.DataBase;

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

        private DAL_dt_pd_Info dal_dt_pd_Info = new DAL_dt_pd_Info();

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
            BindRes();
            BindDGV();
        }

        private void BindRes()
        {
            var model = dal_dt_pd_Info.GetPDListByCode(_tempCode);
            if (model != null)
            {
                txt_desc.Text = model.test_desc;
                txt_reslevel.Text = model.test_result;
            }
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
        private void BindDGV()
        {
            dgv_level.DataSource = GetPlaneDeformationInfo();

            dgv_level.RowHeadersVisible = false;
            dgv_level.AllowUserToResizeColumns = false;
            dgv_level.AllowUserToResizeRows = false;
            dgv_level.Columns[0].HeaderText = "等级";
            dgv_level.Columns[0].Width = 53;
            dgv_level.Columns[0].ReadOnly = true;
            dgv_level.Columns[0].DataPropertyName = "Level";

            dgv_level.Columns[1].HeaderText = "振幅";
            dgv_level.Columns[1].Width = 2;
            dgv_level.Columns[1].DataPropertyName = "ZhenFu";


            dgv_level.Columns[2].HeaderText = "修正";
            dgv_level.Columns[2].Width = 54;
            dgv_level.Columns[2].DataPropertyName = "XiuZheng";


            dgv_level.Columns[1].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[2].DefaultCellStyle.Format = "N2";
        }

        private List<PlaneDeformationInfo> GetPlaneDeformationInfo()
        {
            List<PlaneDeformationInfo> list = new List<PlaneDeformationInfo>();

            for (int i = 1; i < 6; i++)
            {
                PlaneDeformationInfo model = new PlaneDeformationInfo();
                model.Level = $"第{i}级";
                model.ZhenFu = 0d;
                model.XiuZheng = 0d;
                list.Add(model);
            }
            return list;
        }

        private bool _TuiGanQianTui = false;
        private void btn_qt_Click(object sender, EventArgs e)
        {

        }
        private bool _TuiGanHouLa = false;
        private void btn_hl_Click(object sender, EventArgs e)
        {

        }

        private void btn_zore_Click(object sender, EventArgs e)
        {

        }

        private void btn_level1_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendLevelBtn(1);
            if (!res)
            {
                MessageBox.Show("第一级异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_level1.BackColor = Color.Green;
            btn_level2.BackColor = Color.Transparent;
            btn_level3.BackColor = Color.Transparent;
            btn_level4.BackColor = Color.Transparent;
            btn_level5.BackColor = Color.Transparent;
        }

        private void btn_level2_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendLevelBtn(2);
            if (!res)
            {
                MessageBox.Show("第二级异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_level1.BackColor = Color.Transparent;
            btn_level2.BackColor = Color.Green;
            btn_level3.BackColor = Color.Transparent;
            btn_level4.BackColor = Color.Transparent;
            btn_level5.BackColor = Color.Transparent;
        }

        private void btn_level3_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendLevelBtn(3);
            if (!res)
            {
                MessageBox.Show("第三级异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_level1.BackColor = Color.Transparent;
            btn_level2.BackColor = Color.Transparent;
            btn_level3.BackColor = Color.Green;
            btn_level4.BackColor = Color.Transparent;
            btn_level5.BackColor = Color.Transparent;
        }

        private void btn_level4_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendLevelBtn(4);
            if (!res)
            {
                MessageBox.Show("第四级异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_level1.BackColor = Color.Transparent;
            btn_level2.BackColor = Color.Transparent;
            btn_level3.BackColor = Color.Transparent;
            btn_level4.BackColor = Color.Green;
            btn_level5.BackColor = Color.Transparent;
        }

        private void btn_level5_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendLevelBtn(5);
            if (!res)
            {
                MessageBox.Show("第五级异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_level1.BackColor = Color.Transparent;
            btn_level2.BackColor = Color.Transparent;
            btn_level3.BackColor = Color.Transparent;
            btn_level4.BackColor = Color.Transparent;
            btn_level5.BackColor = Color.Green;
        }

        private void btn_stop1_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SP_Stop();
            if (!res)
            {
                MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_level1.BackColor = Color.Transparent;
            btn_level2.BackColor = Color.Transparent;
            btn_level3.BackColor = Color.Transparent;
            btn_level4.BackColor = Color.Transparent;
            btn_level5.BackColor = Color.Transparent;
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

        private void btn_datadispose_Click(object sender, EventArgs e)
        {
            if (txt_reslevel.Text == "")
            {
                MessageBox.Show("清选择检测结果", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Model_dt_pd_Info model = new Model_dt_pd_Info();
            model.dt_Code = _tempCode;
            model.test_result = txt_reslevel.Text;
            model.test_desc = txt_desc.Text;
            dal_dt_pd_Info.AddPD(model);
        }

        private void btn_qt_MouseDown(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongKai(true);
            if (!res)
            {
                MessageBox.Show("推杆前推异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_qt.BackColor = Color.Green;
        }

        private void btn_qt_MouseUp(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongKai(false);
            if (!res)
            {
                MessageBox.Show("推杆前推异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_qt.BackColor = Color.Transparent;
        }

        private void btn_hl_MouseDown(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(true);
            if (!res)
            {
                MessageBox.Show("推杆后拉异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_hl.BackColor = Color.Green;
        }

        private void btn_hl_MouseUp(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(false);
            if (!res)
            {
                MessageBox.Show("推杆后拉异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_hl.BackColor = Color.Transparent;
        }
    }
}
