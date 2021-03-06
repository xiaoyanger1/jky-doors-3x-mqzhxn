
using text.doors.Common;
using text.doors.Detection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Default;

namespace text.doors
{
    public partial class MainForm : Form
    {
        private static SerialPortClient _serialPortClient = new SerialPortClient();
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();


        /// <summary>
        /// 检验编号
        /// </summary>
        private string _tempCode = "";
        /// <summary>
        /// 当前樘号
        /// </summary>
        private string _tempTong = "";

        public static System.Threading.Timer timRead;

        AutoSizeFormClass asc = new AutoSizeFormClass();

        public MainForm()
        {
            InitializeComponent();
            OpenSerialPortClient();
            ShowDetectionSet();

            timRead = new System.Threading.Timer(readTimer, null, 100, 0);
        }
        public void readTimer(object state)
        {
            try
            {
                if (_serialPortClient.sp.IsOpen)
                {
                    List<double> showList = _serialPortClient.ReadHoldingRegisters_Show();

                    if (showList != null && showList.Count > 0 && showList.Count == 4)
                    {
                        RegisterData.WindSpeed_Value = showList[0];
                        // RegisterData.CY_High_Value = (int)showList[1];
                        RegisterData.Temperature_Value = showList[2];
                        RegisterData.AtmospherePa_Value = showList[3];
                        RegisterData.CY_Low_Value = _serialPortClient.GetCY_Low();
                    }
                    RegisterData.CY_High_Value = _serialPortClient.GetCY_High();
                    List<double> displace = _serialPortClient.ReadHoldingRegisters();
                    if (displace != null && displace.Count > 0)
                    {
                        if (displace.Count > 0)
                        {
                            RegisterData.DisplaceA1 = displace[0];
                            RegisterData.DisplaceA2 = displace[1];
                            RegisterData.DisplaceA3 = displace[2];
                            RegisterData.DisplaceB1 = displace[3];
                            RegisterData.DisplaceB2 = displace[4];
                            RegisterData.DisplaceB3 = displace[5];
                            RegisterData.DisplaceC1 = displace[6];
                            RegisterData.DisplaceC2 = displace[7];
                            RegisterData.DisplaceC3 = displace[8];
                            RegisterData.Displace10 = displace[9];
                        }
                    }


                    lbl_wy1.Invoke(new Action<string>(t =>
                    {
                        lbl_wy1.Text = t;
                    }), RegisterData.DisplaceA1.ToString());
                    lbl_wy2.Invoke(new Action<string>(t =>
                    {
                        lbl_wy2.Text = t;
                    }), RegisterData.DisplaceA2.ToString());

                    lbl_wy3.Invoke(new Action<string>(t =>
                    {
                        lbl_wy3.Text = t;
                    }), RegisterData.DisplaceA3.ToString());

                    lbl_wy4.Invoke(new Action<string>(t =>
                    {
                        lbl_wy4.Text = t;
                    }), RegisterData.DisplaceB1.ToString());
                    lbl_wy5.Invoke(new Action<string>(t =>
                    {
                        lbl_wy5.Text = t;
                    }), RegisterData.DisplaceB2.ToString());

                    lbl_wy6.Invoke(new Action<string>(t =>
                    {
                        lbl_wy6.Text = t;
                    }), RegisterData.DisplaceB3.ToString());

                    lbl_wy7.Invoke(new Action<string>(t =>
                    {
                        lbl_wy7.Text = t;
                    }), RegisterData.DisplaceC1.ToString());
                    lbl_wy8.Invoke(new Action<string>(t =>
                    {
                        lbl_wy8.Text = t;
                    }), RegisterData.DisplaceC2.ToString());

                    lbl_wy9.Invoke(new Action<string>(t =>
                    {
                        lbl_wy9.Text = t;
                    }), RegisterData.DisplaceC3.ToString());
                    lbl_wypm.Invoke(new Action<string>(t =>
                    {
                        lbl_wypm.Text = t;
                    }), RegisterData.Displace10.ToString());


                    //风速
                    lbl_fscgq.Invoke(new Action<string>(t =>
                    {
                        lbl_fscgq.Text = t;
                    }), RegisterData.WindSpeed_Value.ToString());
                    //差压高
                    lbl_cygcgq.Invoke(new Action<string>(t =>
                    {
                        lbl_cygcgq.Text = t;
                    }), RegisterData.CY_High_Value.ToString());

                    //差压低
                    lbl_cydcgq.Invoke(new Action<string>(t =>
                    {
                        lbl_cydcgq.Text = t;
                    }), RegisterData.CY_Low_Value.ToString());

                    //温度
                    lbl_wdcgq.Invoke(new Action<string>(t =>
                    {
                        lbl_wdcgq.Text = t;
                    }), RegisterData.Temperature_Value.ToString());
                    //大气压力
                    lbl_dqylcgq.Invoke(new Action<string>(t =>
                    {
                        lbl_dqylcgq.Text = t;
                    }), RegisterData.AtmospherePa_Value.ToString());

                    timRead.Change(500, 0);
                }
            }
            catch (Exception ex)
            {
                timRead.Dispose();
                timRead = null;
            }
        }


        private void OpenSerialPortClient()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    using (BackgroundWorker bw = new BackgroundWorker())
                    {
                        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SerialPort_RunWorkerCompleted);
                        bw.DoWork += new DoWorkEventHandler(SerialPort_DoWork);
                        bw.RunWorkerAsync();
                    }
                    if (!_serialPortClient.sp.IsOpen)
                    {
                        _serialPortClient.SerialPortOpen();
                    }

                    Thread.Sleep(500);
                }
            }));
            thread.Start();
        }

        void SerialPort_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _serialPortClient.sp.IsOpen ? "串口连接：成功" : "串口连接：失败";
        }

        void SerialPort_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tsl_tcpclient.Text = e.Result.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        private void SelectDangHao(text.doors.Detection.DetectionSet.BottomType bt)
        {
            this.tssl_SetCode.Text = string.Format("{0}", bt.Code);
            if (bt.ISOK == true)
            { this.tsl_type.Visible = false; }
            else { this.tsl_type.Visible = true; }

            _tempCode = bt.Code;
            DefaultBase.IsSetTong = bt.ISOK;
            if (bt.ISOK)
            {
                ShowAirtightDetection();
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        private void ShowDetectionSet()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }

            }

            this.pl_showItem.Controls.Clear();
            DetectionSet ds = new DetectionSet(_serialPortClient, RegisterData.Temperature_Value, RegisterData.AtmospherePa_Value, _tempCode);
            ds.deleBottomTypeEvent += new DetectionSet.deleBottomType(SelectDangHao);
            ds.GetDangHaoTrigger();
            ds.TopLevel = false;
            ds.Parent = this.pl_showItem;
            ds.Show();
        }

        /// <summary>
        /// 水密监控
        /// </summary>
        private void ShowWatertightDetection()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();
            WatertightDetection rts = new WatertightDetection(_serialPortClient, _tempCode);

            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;
            rts.Show();
        }

        /// <summary>
        /// 平面内变形检测
        /// </summary>
        private void ShowPlaneDeformation()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is PlaneDeformation)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();
            PlaneDeformation rts = new PlaneDeformation(_serialPortClient, _tempCode);

            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;
            rts.Show();
        }
        /// <summary>
        /// 气密监控
        /// </summary>
        private void ShowAirtightDetection()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();

            AirtightDetection rts = new AirtightDetection(_serialPortClient, _tempCode, _tempTong);
            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;

            rts.Show();

        }
        /// <summary>
        /// 抗风压
        /// </summary>
        private void ShowWindPressure()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();

            WindPressureDetection rts = new WindPressureDetection(_serialPortClient, _tempCode);
            this.pl_showItem.Controls.Clear();
            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;
            rts.Show();
        }

        private void hsb_WindControl_Scroll(object sender, ScrollEventArgs e)
        {
            if (hsb_WindControl.Value == 0)
                txt_hz.Text = "0.00";
            else
                txt_hz.Text = (hsb_WindControl.Value).ToString();

            double value = (hsb_WindControl.Value) * 640;

            var res = _serialPortClient.SendFJKZ(value);

            if (!res)
            {
                MessageBox.Show("风机控制异常,请确认服务器连接是否成功!", "风机", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //关闭
        private void tsm_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //收起
        private void tsb_fewer_Click(object sender, EventArgs e)
        {
            this.pl_set.Visible = false;
            this.tsb_fewer.Visible = false;
            this.tsb_open.Visible = true;
        }

        //打开
        private void tsb_open_Click(object sender, EventArgs e)
        {
            this.pl_set.Visible = true;
            this.tsb_open.Visible = false;
            this.tsb_fewer.Visible = true;
        }


        private void tsm_surveillance_Click(object sender, EventArgs e)
        {

        }

        //检测设定
        private void tsb_DetectionSet_Click(object sender, EventArgs e)
        {
            DefaultBase.IsSetTong = false;
            ShowDetectionSet();
        }

        private void tms_DetectionSet_Click(object sender, EventArgs e)
        {
            ShowDetectionSet();
        }

        private void tsm_UpdatePassWord_Click(object sender, EventArgs e)
        {
            UpdatePassWord up = new UpdatePassWord();
            up.Show();
            up.TopMost = true;
        }


        /// <summary>解决关闭按钮bug
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            try
            {
                const int WM_SYSCOMMAND = 0x0112;
                const int SC_CLOSE = 0xF060;
                if (msg.Msg == WM_SYSCOMMAND && ((int)msg.WParam == SC_CLOSE))
                {
                    // 点击winform右上关闭按钮 
                    this.Dispose();
                    // 加入想要的逻辑处理
                    System.Environment.Exit(0);
                    return;
                }
                base.WndProc(ref msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        private void tsb_生成报告_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                ExportReport exportReport = new ExportReport(_tempCode);
                exportReport.Show();
            }
            else
            {
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 高压归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool _GaoYaGuiLing = false;
        private void btn_gyZero_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.SendGYBD(ref _GaoYaGuiLing))
            {
                MessageBox.Show("高压归零异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btn_gyZero.BackColor = _GaoYaGuiLing ? Color.Green : Color.Transparent;
        }

        /// <summary>
        /// 低压归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private bool _DiyaGuiLing = false;
        private void btn_dyZero_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.SendDYBD(ref _DiyaGuiLing))
            {
                MessageBox.Show("低压归零异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btn_dyZero.BackColor = _DiyaGuiLing ? Color.Green : Color.Transparent;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
            a.TopMost = true;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (!DefaultBase.IsSetTong)
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SystemManager sm = new SystemManager();
            sm.Show();
            sm.TopMost = true;
        }

        private void 水密监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowWatertightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void 气密监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowAirtightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void 抗风压监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                ShowWindPressure();
            }
            else
            {
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsb_watertight_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowAirtightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tsbwatertight_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowWatertightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tsb_WindPressure_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                ShowWindPressure();
            }
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        void hsb_DoWork(object sender, DoWorkEventArgs e)
        {
            var IsSeccess = false;
            var diffPress = _serialPortClient.ReadFJSD(ref IsSeccess);
            if (!IsSeccess) return;

            //需要计算
            e.Result = diffPress;
        }

        void hsb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
                return;
            var value = int.Parse(e.Result.ToString()) / 640;
            txt_hz.Text = value.ToString();
        }


        void hsb_lqf_DoWork(object sender, DoWorkEventArgs e)
        {
            var IsSeccess = false;
            var diffPress = _serialPortClient.ReadLQFShow(ref IsSeccess);
            if (!IsSeccess) return;

            //需要计算
            e.Result = diffPress;
        }

        void hsb_lqf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
                return;
            var value = int.Parse(e.Result.ToString()) / 320;
            txt_lqfhz.Text = value.ToString();
            hsb_lqfControl.Value = value;
        }


        private void pID设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 系数设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tim_panelValue_Tick(object sender, EventArgs e)
        {
            if (_serialPortClient.sp.IsOpen)
            {
                //风机控制
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hsb_RunWorkerCompleted);
                    bw.DoWork += new DoWorkEventHandler(hsb_DoWork);
                    bw.RunWorkerAsync();
                }

                //漏气阀门
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hsb_lqf_RunWorkerCompleted);
                    bw.DoWork += new DoWorkEventHandler(hsb_lqf_DoWork);
                    bw.RunWorkerAsync();
                }
            }
        }


        private void btn_kglkz_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.SendSingleCoilControl(BFMCommand.开关量控制))
            {
                MessageBox.Show("开关量控制异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void 传感器设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensorSet ss = new SensorSet(_serialPortClient);
            ss.Show();
            ss.TopMost = true;
        }

        private void 系数设定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CorrectionFactor correctionFactor = new CorrectionFactor();
            correctionFactor.Show();
        }

        private void pID设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PIDManager p = new PIDManager(_serialPortClient);
            p.Show();
        }

        private void btn_fjqd_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendFengJiQiDong(1);
            if (!res)
            {
                MessageBox.Show("风机启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_fjqd.BackColor = Color.Green;
            btn_fjtz.BackColor = Color.Transparent;
        }

        private void btn_fjtz_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendFengJiQiDong(0);
            if (!res)
            {
                MessageBox.Show("风机启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            btn_fjtz.BackColor = Color.Green;
            btn_fjqd.BackColor = Color.Transparent;
        }

        private void btn_sbqd_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendShuiBengQiDong(1);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_sbqd.BackColor = Color.Green;
            btn_sbtz.BackColor = Color.Transparent;
        }

        private void btn_sbtz_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendShuiBengQiDong(0);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_sbtz.BackColor = Color.Green;
            btn_sbqd.BackColor = Color.Transparent;
        }

        private void btn_bhfk_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendBaoHuFaTong(1);
            if (!res)
            {
                MessageBox.Show("保护阀通启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_bhfk.BackColor = Color.Green;
            btn_bhfg.BackColor = Color.Transparent;
        }

        private void btn_bhfg_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendBaoHuFaTong(0);
            if (!res)
            {
                MessageBox.Show("保护阀通启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_bhfk.BackColor = Color.Transparent;
            btn_bhfg.BackColor = Color.Green;
        }
        private void btn_qmfk_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendQiMiFaKai(1);
            if (!res)
            {
                MessageBox.Show("气密阀启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_qmfg.BackColor = Color.Transparent;
            btn_qmfk.BackColor = Color.Green;
        }
        private void btn_qmfg_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendQiMiFaKai(0);
            if (!res)
            {
                MessageBox.Show("气密阀启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_qmfk.BackColor = Color.Transparent;
            btn_qmfg.BackColor = Color.Green;
        }
        private void btn_stfk_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendSiTongFaKai(1);
            if (!res)
            {
                MessageBox.Show("四通阀开异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_stfk.BackColor = Color.Green;
            btn_stfg.BackColor = Color.Transparent;
        }
        private void btn_stfg_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendSiTongFaKai(0);
            if (!res)
            {
                MessageBox.Show("四通阀开异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_stfk.BackColor = Color.Transparent;
            btn_stfg.BackColor = Color.Green;
        }


        private void hsb_lqfControl_Scroll(object sender, ScrollEventArgs e)
        {
            if (hsb_lqfControl.Value == 0)
                txt_lqfhz.Text = "0.00";
            else
                txt_lqfhz.Text = (hsb_lqfControl.Value).ToString();

            double value = (hsb_lqfControl.Value) * 320;

            var res = _serialPortClient.SendLQFKZ(value);

            if (!res)
            {
                MessageBox.Show("漏气阀控制异常,请确认服务器连接是否成功!", "风机", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsb_PlaneDeformation_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowPlaneDeformation();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void 平面内变形检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowPlaneDeformation();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_tgqt_MouseDown(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongKai(true);
            if (!res)
            {
                MessageBox.Show("推杆前推异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_tgqt.BackColor = Color.Green;
        }

        private void btn_tgqt_MouseUp(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongKai(false);
            if (!res)
            {
                MessageBox.Show("推杆前推异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_tgqt.BackColor = Color.Transparent;
        }

        private void btn_tghl_MouseDown(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(true);
            if (!res)
            {
                MessageBox.Show("推杆后拉异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_tghl.BackColor = Color.Green;
        }

        private void btn_tghl_MouseUp(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(false);
            if (!res)
            {
                MessageBox.Show("推杆后拉异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_tghl.BackColor = Color.Transparent;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //asc.controlAutoSize(this);
            //this.WindowState = FormWindowState.Maximized;//记录完控件的初始位置和大小后，再最大化
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //asc.controllInitializeSize(this);
            //asc.controlAutoSize(this);
            //this.WindowState = FormWindowState.Maximized;//记录完控件的初始位置和大小后，再最大化
        }
    }
}
