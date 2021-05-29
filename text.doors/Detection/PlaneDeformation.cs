using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using text.doors.Model.DataBase;
using static text.doors.Detection.WindPressureDetection;

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

        public static Thread td;

        public PlaneDeformation(SerialPortClient serialPortClient, string tempCode)
        {
            InitializeComponent();
            this._serialPortClient = serialPortClient;
            this._tempCode = tempCode;
            Init();

            td = new Thread(BindFromInput);
            td.IsBackground = true;
            td.Start();

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

        /// <summary>
        /// 实时绑定数据
        /// </summary>
        private void BindFromInput()
        {
            SetRealTimeData st1 = new SetRealTimeData(Update_wy10_Text);

            while (true)
            {
                try
                {
                    if (txt_biaojilingdian.InvokeRequired)
                        txt_biaojilingdian.Invoke(st1, _serialPortClient.GetDisplace10().ToString());
                    else
                        txt_biaojilingdian.Text = _serialPortClient.GetDisplace10().ToString();
                }
                catch (Exception ex)
                {
                    td.Abort();
                }
            }
        }
        public delegate void SetRealTimeData(string value);

        private void Update_wy10_Text(string value)
        {
            txt_biaojilingdian.Text = value;
        }


        #region 图表控制
        /// <summary>
        /// 风速图标
        /// </summary>
        private void PMchartInit()
        {
            dtnow = DateTime.Now;
            pm_Line.GetVertAxis.SetMinMax(-200, 200);
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
            dgv_level.Columns[0].Width = 60;
            dgv_level.Columns[0].ReadOnly = true;
            dgv_level.Columns[0].DataPropertyName = "Level";

            dgv_level.Columns[1].HeaderText = "振幅";
            dgv_level.Columns[1].Width = 60;
            dgv_level.Columns[1].DataPropertyName = "ZhenFu";


            dgv_level.Columns[2].HeaderText = "修正";
            dgv_level.Columns[2].Width = 60;
            dgv_level.Columns[2].DataPropertyName = "XiuZheng";


            dgv_level.Columns[1].DefaultCellStyle.Format = "N2";
            dgv_level.Columns[2].DefaultCellStyle.Format = "N2";
        }

        private List<PlaneDeformationInfo> GetPlaneDeformationInfo()
        {
            List<PlaneDeformationInfo> list = new List<PlaneDeformationInfo>();

            var pdInfoModel = dal_dt_pd_Info.GetPDListByCode(_tempCode);
            if (pdInfoModel != null)
            {
                for (int i = 1; i < 6; i++)
                {
                    PlaneDeformationInfo model = new PlaneDeformationInfo();
                    model.Level = $"第{i}级";
                    if (i == 1)
                    {
                        model.ZhenFu = double.Parse(pdInfoModel.zf1);
                        model.XiuZheng = double.Parse(pdInfoModel.xz1);
                    }
                    else if (i == 2)
                    {
                        model.ZhenFu = double.Parse(pdInfoModel.zf2);
                        model.XiuZheng = double.Parse(pdInfoModel.xz2);
                    }
                    else if (i == 3)
                    {
                        model.ZhenFu = double.Parse(pdInfoModel.zf3);
                        model.XiuZheng = double.Parse(pdInfoModel.xz3);
                    }
                    else if (i == 4)
                    {
                        model.ZhenFu = double.Parse(pdInfoModel.zf4);
                        model.XiuZheng = double.Parse(pdInfoModel.xz4);
                    }
                    else if (i == 5)
                    {
                        model.ZhenFu = double.Parse(pdInfoModel.zf5);
                        model.XiuZheng = double.Parse(pdInfoModel.xz5);
                    }
                    list.Add(model);
                }
            }
            else
            {
                for (int i = 1; i < 6; i++)
                {
                    PlaneDeformationInfo model = new PlaneDeformationInfo();
                    model.Level = $"第{i}级";
                    model.XiuZheng = 0d;
                    model.ZhenFu = 0d;
                    list.Add(model);
                }
            }
            return list;
        }


        private void btn_zore_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            _serialPortClient.SendWYGL_PM();
        }

        private void btn_level1_Click(object sender, EventArgs e)
        {
            double v1 = 0d;
            double v2 = 0d;
            GetDGVValue(1, ref v1, ref v2);

            var res = _serialPortClient.SendLevelValueBtn(1, v1, v2);
            if (!res)
            {
                MessageBox.Show("第一级赋值异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var res1 = _serialPortClient.SendLevelBtn(1);
            if (!res1)
            {
                MessageBox.Show("第一级开始异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            double v1 = 0d;
            double v2 = 0d;
            GetDGVValue(2, ref v1, ref v2);
            var res = _serialPortClient.SendLevelValueBtn(2, v1, v2);
            if (!res)
            {
                MessageBox.Show("第二级赋值异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var res1 = _serialPortClient.SendLevelBtn(2);
            if (!res1)
            {
                MessageBox.Show("第二级开始异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            double v1 = 0d;
            double v2 = 0d;
            GetDGVValue(3, ref v1, ref v2);
            var res = _serialPortClient.SendLevelValueBtn(3, v1, v2);
            if (!res)
            {
                MessageBox.Show("第三级赋值异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var res1 = _serialPortClient.SendLevelBtn(3);
            if (!res1)
            {
                MessageBox.Show("第三级开始异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            double v1 = 0d;
            double v2 = 0d;
            GetDGVValue(4, ref v1, ref v2);
            var res = _serialPortClient.SendLevelValueBtn(4, v1, v2);
            if (!res)
            {
                MessageBox.Show("第四级赋值异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var res1 = _serialPortClient.SendLevelBtn(4);
            if (!res1)
            {
                MessageBox.Show("第四级开始异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            double v1 = 0d;
            double v2 = 0d;
            GetDGVValue(5, ref v1, ref v2);
            var res = _serialPortClient.SendLevelValueBtn(5, v1, v2);
            if (!res)
            {
                MessageBox.Show("第五级赋值异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var res1 = _serialPortClient.SendLevelBtn(5);
            if (!res1)
            {
                MessageBox.Show("第五级开始异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


        private void GetDGVValue(int level, ref double v1, ref double v2)
        {

            if (level == 1)
            {
                v1 = double.Parse(this.dgv_level.Rows[0].Cells[1].Value.ToString());
                v2 = double.Parse(this.dgv_level.Rows[0].Cells[2].Value.ToString());
            }
            else if (level == 2)
            {
                v1 = double.Parse(this.dgv_level.Rows[1].Cells[1].Value.ToString());
                v2 = double.Parse(this.dgv_level.Rows[1].Cells[2].Value.ToString());
            }
            else if (level == 3)
            {
                v1 = double.Parse(this.dgv_level.Rows[2].Cells[1].Value.ToString());
                v2 = double.Parse(this.dgv_level.Rows[2].Cells[2].Value.ToString());
            }
            else if (level == 4)
            {
                v1 = double.Parse(this.dgv_level.Rows[3].Cells[1].Value.ToString());
                v2 = double.Parse(this.dgv_level.Rows[3].Cells[2].Value.ToString());
            }
            else if (level == 5)
            {
                v1 = double.Parse(this.dgv_level.Rows[4].Cells[1].Value.ToString());
                v2 = double.Parse(this.dgv_level.Rows[4].Cells[2].Value.ToString());
            }
            v1 = v1 * 100;
            v2 = v2 * 100;
        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            double c = _serialPortClient.GetDisplace10();

            lbl_dqyl.Text = c.ToString();

            AnimateSeries(this.tChart_pm, (int)c);
        }

        private void btn_datadispose_Click(object sender, EventArgs e)
        {
            if (txt_reslevel.Text == "")
            {
                MessageBox.Show("请选择检测结果", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Model_dt_pd_Info model = new Model_dt_pd_Info();
            model.dt_Code = _tempCode;
            model.test_result = txt_reslevel.Text;
            model.test_desc = txt_desc.Text;


            model.zf1 = this.dgv_level.Rows[0].Cells[1].Value.ToString();
            model.zf2 = this.dgv_level.Rows[1].Cells[1].Value.ToString();
            model.zf3 = this.dgv_level.Rows[2].Cells[1].Value.ToString();
            model.zf4 = this.dgv_level.Rows[3].Cells[1].Value.ToString();
            model.zf5 = this.dgv_level.Rows[4].Cells[1].Value.ToString();

            model.xz1 = this.dgv_level.Rows[0].Cells[2].Value.ToString();
            model.xz2 = this.dgv_level.Rows[1].Cells[2].Value.ToString();
            model.xz3 = this.dgv_level.Rows[2].Cells[2].Value.ToString();
            model.xz4 = this.dgv_level.Rows[3].Cells[2].Value.ToString();
            model.xz5 = this.dgv_level.Rows[4].Cells[2].Value.ToString();


            if (dal_dt_pd_Info.AddPD(model))
            {
                MessageBox.Show("处理完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("处理失败！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void tim_WY_Tick(object sender, EventArgs e)
        {

        }

        private void btn_tgstart_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendTuiGanQiDong(1);
            if (!res)
            {
                MessageBox.Show("推杆启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_tgstart.BackColor = Color.Green;
            btn_stop.BackColor = Color.Transparent;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendTuiGanQiDong(0);
            if (!res)
            {
                MessageBox.Show("推杆停止异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_tgstart.BackColor = Color.Transparent;
            btn_stop.BackColor = Color.Green;
        }

        private void tim_getType_Tick(object sender, EventArgs e)
        {

        }
    }
}
