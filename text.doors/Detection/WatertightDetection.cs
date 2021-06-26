
using Modbus.Device;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Young.Core.Common;
using text.doors.Model.DataBase;
using text.doors.Default;
using text.doors.Service;
using static text.doors.Default.PublicEnum;
using NPOI.SS.Formula.Functions;

namespace text.doors.Detection
{
    public partial class WatertightDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";

        /// <summary>
        /// 水密按钮位置
        /// </summary>
        private PublicEnum.WaterTightPropertyTest? waterTightPropertyTest = null;

        /// <summary>
        /// 操作状态
        /// </summary>
        private bool IsSeccess = false;

        private DAL_dt_sm_Info dal_dt_sm_Info = new DAL_dt_sm_Info();


        public DateTime dtnow { get; set; }

        public WatertightDetection(SerialPortClient serialPortClient, string tempCode)
        {
            InitializeComponent();
            this._serialPortClient = serialPortClient;
            this._tempCode = tempCode;
            Init();
        }

        private void Init()
        {
            Initial();
            Title();
            SMchartInit();
        }
        #region 数据绑定

        /// <summary>
        /// 绑定水密初始值
        /// </summary>
        private void Initial()
        {
            var smInfo = dal_dt_sm_Info.GetSMListByCode(_tempCode);

            if (smInfo != null)
            {
                #region 绑定
                var checkDesc = smInfo.sm_PaDesc;
                var sm_pa = smInfo.sm_Pa;
                var remark = smInfo.sm_Remark;
                var method = smInfo.Method;

                var sm_pa2 = smInfo.sm_Pa2;
                var checkDesc2 = smInfo.sm_PaDesc2;
                if (method == "稳定加压")
                {
                    this.rdb_wdjy.Checked = true;
                }
                else if (method == "波动加压")
                {
                    this.rdb_bdjy.Checked = true;

                    txt_maxValue.Text = smInfo.sxyl;
                    txt_minValue.Text = smInfo.xxyl;
                }
                else
                    this.rdb_wdjy.Checked = true;

                txt_ycjy.Text = smInfo.gongchengjiance;

                //可开
                if (!string.IsNullOrWhiteSpace(checkDesc))
                {
                    var flish = "";
                    var two = "";

                    SplitDest(checkDesc, ref flish, ref two);
                    if (checkDesc.Contains("●") || checkDesc.Contains("▲"))
                    {
                        if (sm_pa == 250)
                        {
                            cbb_1_350Pa.Text = flish;
                            cbb_2_350Pa.Text = two;
                        }

                        if (sm_pa == 350)
                        {
                            cbb_1_500Pa.Text = flish;
                            cbb_2_500Pa.Text = two;
                        }

                        if (sm_pa == 500)
                        {
                            cbb_1_700Pa.Text = flish;
                            cbb_2_700Pa.Text = two;
                        }

                        if (sm_pa == 700)
                        {
                            cbb_1_1000Pa.Text = flish;
                            cbb_2_1000Pa.Text = two;
                        }
                        if (sm_pa == 1000)
                        {
                            cbb_1_1000Pa.Text = flish;
                            cbb_2_1000Pa.Text = two;
                        }
                    }
                    else
                    {
                        if (sm_pa == 250)
                        {
                            cbb_1_250Pa.Text = flish;
                            cbb_2_250Pa.Text = two;
                        }

                        if (sm_pa == 350)
                        {
                            cbb_1_350Pa.Text = flish;
                            cbb_2_350Pa.Text = two;
                        }

                        if (sm_pa == 500)
                        {
                            cbb_1_500Pa.Text = flish;
                            cbb_2_500Pa.Text = two;
                        }

                        if (sm_pa == 700)
                        {
                            cbb_1_700Pa.Text = flish;
                            cbb_2_700Pa.Text = two;
                        }
                        if (sm_pa == 1000)
                        {
                            cbb_1_1000Pa.Text = flish;
                            cbb_2_1000Pa.Text = two;
                        }
                    }
                    txt_fjzb_kk.Text = sm_pa.ToString();
                    txt_desc.Text = remark;
                }
                if (!string.IsNullOrWhiteSpace(checkDesc2))
                {
                    var flish = "";
                    var two = "";

                    SplitDest(checkDesc2, ref flish, ref two);
                    if (checkDesc2.Contains("●") || checkDesc2.Contains("▲"))
                    {
                        if (sm_pa2 == 500)
                        {
                            cbb_1_700Pa_cf.Text = flish;
                            cbb_2_700Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 700)
                        {
                            cbb_1_1000Pa_cf.Text = flish;
                            cbb_2_1000Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 1000)
                        {
                            cbb_1_1500Pa_cf.Text = flish;
                            cbb_2_1500Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 1500)
                        {
                            cbb_1_2000Pa_cf.Text = flish;
                            cbb_2_2000Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 2000)
                        {
                            cbb_1_2000Pa_cf.Text = flish;
                            cbb_2_2000Pa_cf.Text = two;
                        }
                    }
                    else
                    {
                        if (sm_pa2 == 500)
                        {
                            cbb_1_500Pa_cf.Text = flish;
                            cbb_2_500Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 700)
                        {
                            cbb_1_700Pa_cf.Text = flish;
                            cbb_2_700Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 1000)
                        {
                            cbb_1_1000Pa_cf.Text = flish;
                            cbb_2_1000Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 1500)
                        {
                            cbb_1_1500Pa_cf.Text = flish;
                            cbb_2_1500Pa_cf.Text = two;
                        }
                        if (sm_pa2 == 2000)
                        {
                            cbb_1_2000Pa_cf.Text = flish;
                            cbb_2_2000Pa_cf.Text = two;
                        }
                    }
                }
                txt_fjzb_gd.Text = sm_pa2.ToString();
                txt_desc_cf.Text = remark;
                #endregion
            }
        }

        /// <summary>
        /// 拆分desc
        /// </summary>
        /// <param name="checkDesc"></param>
        /// <param name="flish"></param>
        /// <param name="two"></param>
        private void SplitDest(string checkDesc, ref string flish, ref string two)
        {
            string[] temp = null;
            if (checkDesc.Contains("〇"))
            {
                temp = checkDesc.Split(new char[] { '〇' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = "〇" + temp[1];
            }
            else if (checkDesc.Contains("□"))
            {
                temp = checkDesc.Split(new char[] { '□' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = "□" + temp[1];
            }
            else if (checkDesc.Contains("△"))
            {
                temp = checkDesc.Split(new char[] { '△' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = "△" + temp[1];
            }
            else if (checkDesc.Contains("▲"))
            {
                temp = checkDesc.Split(new char[] { '▲' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = "▲" + temp[1];
            }
            else if (checkDesc.Contains("●"))
            {
                temp = checkDesc.Split(new char[] { '●' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = "●" + temp[1];
            }
            else
            {
                temp = checkDesc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                flish = temp[0];
                two = temp[1];
            }
        }
        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void Title()
        {
            lbl_smjc.Text = string.Format("门窗水密性能检测  第{0}号 ", this._tempCode);
            btn_ksbd.Enabled = false;
            btn_tzbd.Enabled = false;
            _serialPortClient.qiehuanTab(false);
        }

        #endregion

        #region 图表控制
        private void tim_sm_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var c = 0;
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
                c = RegisterData.CY_Low_Value;
            else
            {
                c = RegisterData.CY_High_Value;
            }
            int value = int.Parse(c.ToString());

            lbldqyl.Text = value.ToString();
            AnimateSeries(this.tChart_sm, value);
        }

        /// <summary>
        /// 水密
        /// </summary>
        private void SMchartInit()
        {
            dtnow = DateTime.Now;
            sm_Line.GetVertAxis.SetMinMax(-2500, 2500);
        }

        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.sm_Line.Add(DateTime.Now, yl);
            this.tChart_sm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(20));
        }

        //private void tim_PainPic_Tick(object sender, EventArgs e)
        //{
        //    if (!_serialPortClient.sp.IsOpen)
        //        return;
            
        //    var c = 0;
        //    if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
        //        c = RegisterData.CY_Low_Value;
        //    else
        //    {
        //        c = RegisterData.CY_Low_Value;
        //    }
        //    int value = int.Parse(c.ToString());

        //    lbldqyl.Text = value.ToString();
        //    AnimateSeries(this.tChart_sm, value);
        //}
        #endregion


        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_sm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 导出图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo
            //this.tChart_qm.Export.ShowExportDialog();
        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.SendSingleCoilControl(BFMCommand.急停);
            if (!res)
                MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            var yl = _serialPortClient.GetSMYBSDYL(ref IsSeccess, "XYJ");
            if (this.rdb_bdjy.Checked == true)
            {
                if (yl == 350)
                {
                    Stop();
                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;

                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                    return;
                }
            }
            else
            {
                if (yl == 2000)
                {
                    Stop();
                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;

                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                    return;
                }
            }
            //var res = _serialPortClient.SendSMXXYJ();
            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.下一级);
            if (!res)
            {
                MessageBox.Show("设置水密性下一级异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Next;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();

            this.btn_ready.Enabled = true;
            this.btn_start.Enabled = true;
            this.btn_next.Enabled = true;
            this.btn_upKpa.Enabled = true;
            this.btn_shuibeng.Enabled = true;

            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;
            btn_upKpa.BackColor = Color.Transparent;
            btn_shuibeng.BackColor = Color.Transparent;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
        }


        #region 水密性能检测按钮事件
        private void btn_ready_Click(object sender, EventArgs e)
        {
            //var res = _serialPortClient.SetSMYB();
            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.水密性预备加压);
            if (!res)
            {
                MessageBox.Show("水密预备异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            this.btn_ready.Enabled = false;
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = false;
            this.btn_next.Enabled = false;
            this.btn_shuibeng.Enabled = false;
            this.btn_upKpa.Enabled = false;

            btn_ready.BackColor = Color.Green;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Ready;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.水密性开始);
            // var res = _serialPortClient.SendSMXKS();
            if (!res)
            {
                MessageBox.Show("水密开始异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = true;
            this.tim_upNext.Enabled = true;
            this.btn_ready.Enabled = false;

            btn_start.BackColor = Color.Green;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Start;
        }
        /// <summary>
        /// 依次加压
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private bool ycjyType = true;

        private void btn_upKpa_Click(object sender, EventArgs e)
        {
            var value = int.Parse(txt_ycjy.Text);
            var res = _serialPortClient.SendSMYCJY(value);
            if (!res)
                return;

            ycjyType = (ycjyType ? false : true);
            if (!ycjyType)
            {
                btn_upKpa.Text = "停止";
            }
            else
            {
                btn_upKpa.Text = "依次加压";

                Stop();
                this.btn_ready.Enabled = true;
                this.btn_start.Enabled = true;
                this.btn_next.Enabled = true;
                waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                return;
            }
            tim_upNext.Enabled = false;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.CycleLoading;
        }
        #endregion

        private void tim_upNext_Tick(object sender, EventArgs e)
        {
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Stop)
                return;

            if (!_serialPortClient.sp.IsOpen)
                return;

            string TEMP = "";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
                TEMP = "SMYB";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.CycleLoading)
                TEMP = "SMKS";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Start)
                TEMP = "SMKS";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next)
                TEMP = "XYJ";

            //double yl = _serialPortClient.GetSMYBSDYL(ref IsSeccess, TEMP);

            //if (!IsSeccess)
            //{
            //    return;
            //}
            //lbl_sdyl.Text = yl.ToString();


            //if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Start ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.SrartBD)
            //{
            //    if (this.rdb_bdjy.Checked == true)
            //    {
            //        lbl_max.Visible = true;

            //        var minVal = 0;
            //        var maxVal = 0;

            //        _serialPortClient.GetCYXS_BODONG(ref IsSeccess, ref minVal, ref maxVal);

            //        lbl_sdyl.Text = minVal.ToString();
            //        lbl_max.Text = maxVal.ToString();
            //    }
            //}
        }

        #region -- 水密选择

        /// <summary>
        /// 位置
        /// </summary>
        private string CheckPosition = "";

        /// <summary>
        /// 问题
        /// </summary>
        private string CheckProblem = "";

        /// <summary>
        /// 数值
        /// </summary>
        private int CheckValue = 0;



        private string CheckPosition2 = "";

        /// <summary>
        /// 问题2
        /// </summary>
        private string CheckProblem2 = "";
        /// <summary>
        /// 数值2
        /// </summary>
        private int CheckValue2 = 0;

        private void cbb_2_250Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_250Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_250Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_250Pa.Text;
            CheckProblem = cbb_2_250Pa.Text;
            CheckValue = 250;

            if (cbb_2_250Pa.Text.Contains("▲") || cbb_2_250Pa.Text.Contains("●"))
            {
                CheckValue = 200;
            }
            txt_fjzb_kk.Text = CheckValue.ToString();
        }

        private void cbb_2_350Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_350Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_350Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_350Pa.Text;
            CheckProblem = cbb_2_350Pa.Text;
            CheckValue = 350;

            if (cbb_2_350Pa.Text.Contains("▲") || cbb_2_350Pa.Text.Contains("●"))
            {
                CheckValue = 250;
            }
            txt_fjzb_kk.Text = CheckValue.ToString();
        }

        private void cbb_2_500Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_500Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_500Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_500Pa.Text;
            CheckProblem = cbb_2_500Pa.Text;
            CheckValue = 500;

            if (cbb_2_500Pa.Text.Contains("▲") || cbb_2_500Pa.Text.Contains("●"))
            {
                CheckValue = 350;
            }
            txt_fjzb_kk.Text = CheckValue.ToString();
        }

        private void cbb_2_700Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_700Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_700Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_700Pa.Text;
            CheckProblem = cbb_2_700Pa.Text;
            CheckValue = 700;

            if (cbb_2_700Pa.Text.Contains("▲") || cbb_2_700Pa.Text.Contains("●"))
            {
                CheckValue = 500;
            }
            txt_fjzb_kk.Text = CheckValue.ToString();
        }


        private void cbb_2_1000Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_1000Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_1000Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_1000Pa.Text;
            CheckProblem = cbb_2_1000Pa.Text;
            CheckValue = 1000;

            if (cbb_2_1000Pa.Text.Contains("▲") || cbb_2_1000Pa.Text.Contains("●"))
            {
                CheckValue = 700;
            }
            txt_fjzb_kk.Text = CheckValue.ToString();
        }

        private void txt_zgfy_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                int zgfy = int.Parse(txt_fjzb_kk.Text);
                CheckValue = zgfy;
            }
            catch
            {
                MessageBox.Show("请输入数字", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 水密数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_2sjcl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckPosition) || string.IsNullOrWhiteSpace(CheckProblem))
            {
                MessageBox.Show("选择失去焦点，请重新选择检测记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Model_dt_sm_Info model = new Model_dt_sm_Info();
            model.dt_Code = _tempCode;
            model.sm_Pa = CheckValue;
            model.sm_PaDesc = CheckPosition + "," + CheckProblem;
            model.sm_Pa2 = CheckValue2;
            model.sm_PaDesc2 = CheckPosition2 + "," + CheckProblem2;


            model.sm_Remark = txt_desc.Text;



            model.gongchengjiance = txt_ycjy.Text;

            if (this.rdb_bdjy.Checked == true)
            {
                model.Method = "波动加压";
                model.sxyl = txt_maxValue.Text;
                model.xxyl = txt_minValue.Text;
            }
            else
            {
                model.Method = "稳定加压";
            }

            if (new DAL_dt_sm_Info().Add(model))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void tChart_sm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_sm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void export_image_sm_Click(object sender, EventArgs e)
        {
            this.tChart_sm.Export.ShowExportDialog();
        }

       

        private void btn_ksbd_Click(object sender, EventArgs e)
        {
            int minValue = -1;
            int maxValue = -1;

            int.TryParse(txt_minValue.Text, out maxValue);

            int.TryParse(txt_maxValue.Text, out minValue);
            if (minValue == 0 || maxValue == 0)
            {
                MessageBox.Show("上线-下线压力请设置大于零数字", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            var res = _serialPortClient.SendBoDongksjy(maxValue, minValue);
            if (!res)
            {
                MessageBox.Show("水密波动开始异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            this.btn_ksbd.Enabled = false;
            this.btn_tzbd.Enabled = true;
            this.btn_ready.Enabled = false;
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = false;

            tim_upNext.Enabled = false;

            btn_ksbd.BackColor = Color.Green;
            btn_tzbd.BackColor = Color.Transparent;
            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;

            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.SrartBD;
        }

        private void btn_tzbd_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendSingleCoilControl(BFMCommand.工程检测水密性停止加压);
            // var res = _serialPortClient.StopBoDong();
            if (!res)
            {
                MessageBox.Show("停止波动", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            this.btn_tzbd.Enabled = false;

            Thread.Sleep(5000);

            this.btn_ksbd.Enabled = true;
            this.btn_ready.Enabled = true;
            this.btn_start.Enabled = true;
            this.btn_next.Enabled = true;


            btn_tzbd.BackColor = Color.Green;
            btn_ksbd.BackColor = Color.Transparent;
            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;

            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.StopBD;
        }

        private void tim_getType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //水密预备
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
            {
                int value = _serialPortClient.ReadEndState(BFMCommand.水密预备结束);
                if (value == 3)
                {
                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;

                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;
                    this.btn_shuibeng.Enabled = true;
                    this.btn_upKpa.Enabled = true;
                    this.btn_shuibeng.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;
                    btn_shuibeng.BackColor = Color.Transparent;
                    btn_upKpa.BackColor = Color.Transparent;
                    btn_shuibeng.BackColor = Color.Transparent;
                }
            }
        }

        private void rdb_bdjy_CheckedChanged(object sender, EventArgs e)
        {
            btn_upKpa.Enabled = false;
            btn_ksbd.Enabled = true;
            btn_tzbd.Enabled = true;

            if (this.rdb_bdjy.Checked == true)
                _serialPortClient.qiehuanTab(true);
            else
                _serialPortClient.qiehuanTab(false);
        }

        private void rdb_wdjy_CheckedChanged(object sender, EventArgs e)
        {
            btn_upKpa.Enabled = true;
            btn_ksbd.Enabled = false;
            btn_tzbd.Enabled = false;
            if (this.rdb_bdjy.Checked == true)
                _serialPortClient.qiehuanTab(true);
            else
                _serialPortClient.qiehuanTab(false);

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.tabControl1.SelectedTab.Name == "可开部分")
            //{
            //    Initial();
            //}
        }

        private void cbb_2_500Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_500Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_500Pa_cf.Text = "";
                CheckProblem2 = "";
                CheckPosition2 = "";
                CheckValue2 = 0;
                return;
            }
            CheckPosition2 = cbb_1_500Pa_cf.Text;
            CheckProblem2 = cbb_2_500Pa_cf.Text;
            CheckValue2 = 500;

            if (cbb_2_500Pa_cf.Text.Contains("▲") || cbb_2_500Pa_cf.Text.Contains("●"))
            {
                CheckValue2 = 400;
            }
            txt_fjzb_gd.Text = CheckValue2.ToString();
        }

        private void cbb_2_700Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_700Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_700Pa_cf.Text = "";
                CheckProblem2 = "";
                CheckPosition2 = "";
                CheckValue2 = 0;
                return;
            }
            CheckPosition2 = cbb_1_700Pa_cf.Text;
            CheckProblem2 = cbb_2_700Pa_cf.Text;
            CheckValue2 = 700;

            if (cbb_2_700Pa_cf.Text.Contains("▲") || cbb_2_700Pa_cf.Text.Contains("●"))
            {
                CheckValue2 = 500;
            }
            txt_fjzb_gd.Text = CheckValue2.ToString();
        }

        private void cbb_2_1000Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_1000Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_1000Pa_cf.Text = "";
                CheckProblem2 = "";
                CheckPosition2 = "";
                CheckValue2 = 0;
                return;
            }
            CheckPosition2 = cbb_1_1000Pa_cf.Text;
            CheckProblem2 = cbb_2_1000Pa_cf.Text;
            CheckValue2 = 1000;

            if (cbb_2_1000Pa_cf.Text.Contains("▲") || cbb_2_1000Pa_cf.Text.Contains("●"))
            {
                CheckValue2 = 700;
            }
            txt_fjzb_gd.Text = CheckValue2.ToString();
        }

        private void cbb_2_1500Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_1500Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_1500Pa_cf.Text = "";
                CheckProblem2 = "";
                CheckPosition2 = "";
                CheckValue2 = 0;
                return;
            }
            CheckPosition2 = cbb_1_1500Pa_cf.Text;
            CheckProblem2 = cbb_2_1500Pa_cf.Text;
            CheckValue2 = 1500;

            if (cbb_2_1500Pa_cf.Text.Contains("▲") || cbb_2_1500Pa_cf.Text.Contains("●"))
            {
                CheckValue2 = 1000;
            }
            txt_fjzb_gd.Text = CheckValue2.ToString();
        }

        private void cbb_2_2000Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_2000Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_2000Pa_cf.Text = "";
                CheckProblem2 = "";
                CheckPosition2 = "";
                CheckValue2 = 0;
                return;
            }
            CheckPosition2 = cbb_1_2000Pa_cf.Text;
            CheckProblem2 = cbb_2_2000Pa_cf.Text;
            CheckValue2 = 2000;

            if (cbb_2_2000Pa_cf.Text.Contains("▲") || cbb_2_2000Pa_cf.Text.Contains("●"))
            {
                CheckValue2 = 1500;
            }
            txt_fjzb_gd.Text = CheckValue2.ToString();
        }



        private void btn_1sjcl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckPosition) || string.IsNullOrWhiteSpace(CheckProblem))
            {
                MessageBox.Show("选择失去焦点，请重新选择检测记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Model_dt_sm_Info model = new Model_dt_sm_Info();
            model.dt_Code = _tempCode;
            model.sm_Pa = CheckValue;
            model.sm_PaDesc = CheckPosition + "," + CheckProblem;
            //以前是重复水密，现在改成【可开、固定】
            model.sm_Pa2 = CheckValue2;
            model.sm_PaDesc = CheckPosition2 + "," + CheckProblem2;

            model.sm_Remark = txt_desc_cf.Text;
            model.gongchengjiance = txt_ycjy.Text;

            if (this.rdb_bdjy.Checked == true)
            {
                model.Method = "波动加压";
                model.sxyl = txt_maxValue.Text;
                model.xxyl = txt_minValue.Text;
            }
            else
            {
                model.Method = "稳定加压";
            }

            if (new DAL_dt_sm_Info().Add(model))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool _ShuiBengQiDong = false;
        private void btn_shuibeng_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendShuiBengQiDong(ref _ShuiBengQiDong);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_shuibeng.BackColor = _ShuiBengQiDong ? Color.Green : Color.Transparent;
        }

        private void btn_next_MouseDown(object sender, MouseEventArgs e)
        {
            btn_next.BackColor = Color.Green;
        }

        private void btn_next_MouseUp(object sender, MouseEventArgs e)
        {
            btn_next.BackColor = Color.Transparent;
        }

        private void chart_cms_sm_click_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
