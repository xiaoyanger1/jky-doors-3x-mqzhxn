
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
using text.doors.Model.DataBase;
using text.doors.Default;
using static text.doors.Default.PublicEnum;
using NPOI.SS.Formula.Functions;

namespace text.doors.Detection
{
    public partial class AirtightDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";

        /// <summary>
        /// 气密数据载体
        /// </summary>
        WindSpeedInfo windSpeedInfo = new WindSpeedInfo();

        /// <summary>
        /// 气密数据位置
        /// </summary>
        private PublicEnum.AirtightPropertyTest? airtightPropertyTest = null;

        /// <summary>
        /// 分级指标
        /// </summary>
        double zFc = 0, fFc = 0, zMj = 0, fMj = 0;

        private DAL_dt_qm_Info dal_dt_qm_Info = new DAL_dt_qm_Info();

        public List<ReadT> _readT = new List<ReadT>();


        private static List<WindSpeedInfo> windSpeedInfoList = new List<WindSpeedInfo>();


        /// <summary>
        /// 检测设定
        /// </summary>
        private DataTable tab_settings = new DataTable();

        /// <summary>
        /// 图标定时
        /// </summary>
        public DateTime dtnow { get; set; }

        public AirtightDetection()
        {
            tc_RealTimeSurveillance.Anchor = AnchorStyles.Top;
        }

        public AirtightDetection(SerialPortClient tcpClient, string tempCode, string tempTong)
        {
            InitializeComponent();
            this._serialPortClient = tcpClient;
            this._tempCode = tempCode;

            Init();
            GetWindSpeed();
            BindFlowBase();

            GetDatabaseLevelIndex();
            BindLevelIndex();

        }
        #region 初始化

        private void Init()
        {
            tab_settings = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);

            InitControl();
            QMchartInit();
            BindSetTitle();
            Clear();
        }

        private void InitControl()
        {
            if (tab_settings.Rows[0]["KaiQiFangShi"].ToString() == "无")
                rdb_gfzh.Enabled = false;

            this.lbl_jlgzj.Text = GetConfigSetting("PipeDiameter");
            this.lbl_sjmj.Text = tab_settings.Rows[0]["shijianmianji"].ToString();
            this.lbl_kkfc.Text = tab_settings.Rows[0]["kekaifengchang"].ToString();
        }


        private string GetConfigSetting(string value)
        {
            return System.Configuration.ConfigurationSettings.AppSettings[value].ToString();
        }
        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void BindSetTitle()
        {
            lbl_title.Text = string.Format("门窗气密性能检测  第{0}号", this._tempCode);
        }
        private void Clear()
        {
            windSpeedInfo = new WindSpeedInfo();
            airtightPropertyTest = null;
            tim_Top10.Enabled = false;
        }
        #endregion

        #region 数据绑定

        /// <summary>
        /// 获取风速数据
        /// </summary>
        /// <returns></returns>
        public void GetWindSpeed()
        {
            windSpeedInfoList = new List<WindSpeedInfo>();
            var qm_Info = dal_dt_qm_Info.GetQMListByCode(_tempCode);

            var qm_zb_info = dal_dt_qm_Info.GetQMZB(_tempCode);

            if (qm_Info == null || qm_Info.Count == 0)
            {
                windSpeedInfoList = new WindSpeedInfo().GetWindSpeed();
                return;
            }
            if (qm_zb_info == null)
            {
                windSpeedInfoList = new WindSpeedInfo().GetWindSpeed();
                return;
            }

            gv_list.Enabled = false;

            #region 排序插入

            //if (qm_Info.First().testtype == "1")
            //{

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "50" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "100" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "150" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "100" && t.PaType.ToString() == "2").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "50" && t.PaType.ToString() == "2").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "-50" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "-100" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());
            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "-150" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());
            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "-100" && t.PaType.ToString() == "2").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());
            windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "-50" && t.PaType.ToString() == "2").Select(t => new WindSpeedInfo()
            {
                Pa = t.Pa,
                PaType = t.PaType,
                FJST = double.Parse(t.FJST),
                GFZH = double.Parse(t.GFZH),
                ZDST = double.Parse(t.ZDST)
            }).ToList());

            //设计值
            //windSpeedInfoList.AddRange(qm_Info?.FindAll(t => t.Pa == "设计值" && t.PaType.ToString() == "1").Select(t => new WindSpeedInfo()
            //{
            //    Pa = t.Pa,
            //    PaType = t.PaType,
            //    FJST = 0d,
            //    GFZH = 0d,
            //    ZDST = 0d
            //}).ToList());
            //}
            //else if (qm_Info.First().testtype == "2")//工程检测  不存在监控数据
            //{
            //    this.btn_justready.Enabled = false;
            //    this.btn_loseready.Enabled = false;
            //    this.btn_losestart.Enabled = false;
            //    this.btn_juststart.Enabled = false;

            //    txt_ycjy_z.Text = qm_zb_info.sjz_value.ToString();
            //    txt_ycjy_f.Text = qm_zb_info.sjz_value.ToString();

            //    //绑定空监测数据

            //    windSpeedInfoList = new WindSpeedInfo().GetWindSpeed();

            //    //绑定设计值
            //    var sjz = windSpeedInfoList.Find(t => t.Pa == "设计值");
            //    if (sjz != null)
            //    {
            //        sjz.Pressure_F = double.Parse(qm_zb_info.sjz_f_fj);
            //        sjz.Pressure_F_Z = double.Parse(qm_zb_info.sjz_f_zd);
            //        sjz.Pressure_Z = double.Parse(qm_zb_info.sjz_z_fj);
            //        sjz.Pressure_Z_Z = double.Parse(qm_zb_info.sjz_z_zd);
            //    }
            #endregion
        }

        /// <summary>
        /// 绑定风速
        /// </summary>
        private void BindFlowBase()
        {
            dgv_ll.DataSource = null;
            dgv_ll.DataSource = windSpeedInfoList;

            dgv_ll.RowHeadersVisible = false;
            dgv_ll.AllowUserToResizeColumns = false;
            dgv_ll.AllowUserToResizeRows = false;
            dgv_ll.Columns[0].HeaderText = "压力Pa";
            dgv_ll.Columns[0].Width = 53;
            dgv_ll.Columns[0].ReadOnly = true;
            dgv_ll.Columns[0].DataPropertyName = "Pa";

            dgv_ll.Columns[1].HeaderText = "类型";
            dgv_ll.Columns[1].Width = 2;
            dgv_ll.Columns[1].ReadOnly = true;
            dgv_ll.Columns[1].Visible = false;
            dgv_ll.Columns[1].DataPropertyName = "PaType";


            dgv_ll.Columns[2].HeaderText = "附加渗透";
            dgv_ll.Columns[2].Width = 54;
            dgv_ll.Columns[2].DataPropertyName = "FJST";

            dgv_ll.Columns[3].HeaderText = "固附之和";
            dgv_ll.Columns[3].Width = 54;
            dgv_ll.Columns[3].DataPropertyName = "GFZH";

            dgv_ll.Columns[4].HeaderText = "总的渗透量";
            dgv_ll.Columns[4].Width = 58;
            dgv_ll.Columns[4].DataPropertyName = "ZDST";

            dgv_ll.Columns[5].HeaderText = "幕墙整体";
            dgv_ll.Columns[5].Width = 54;
            dgv_ll.Columns[5].ReadOnly = true;
            dgv_ll.Columns[5].DataPropertyName = "MQZT";

            dgv_ll.Columns[6].HeaderText = "可开渗透";
            dgv_ll.Columns[6].Width = 54;
            dgv_ll.Columns[6].ReadOnly = true;
            dgv_ll.Columns[6].DataPropertyName = "KKST";


            dgv_ll.Columns[2].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns[3].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns[4].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns[5].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns[6].DefaultCellStyle.Format = "N2";
        }

        /// <summary>
        /// 绑定分级指标
        /// </summary>
        private void BindLevelIndex()
        {
            this.lbl_z_mj.Text = zMj.ToString();
            this.lbl_f_mj.Text = fMj.ToString();
            this.lbl_z_fc.Text = zFc.ToString();
            this.lbl_f_fc.Text = fFc.ToString();
        }

        #endregion

        #region 图表控制
        /// <summary>
        /// 风速图标
        /// </summary>
        private void QMchartInit()
        {
            dtnow = DateTime.Now;
            qm_Line.GetVertAxis.SetMinMax(-600, 600);
        }

        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.qm_Line.Add(DateTime.Now, yl);
            this.tChart_qm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(40));
        }

        /// <summary>
        /// 确定当前读取的压力状态
        /// </summary>
        private PublicEnum.Kpa_Level? kpa_Level = null;

        /// <summary>
        /// 差压读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tim_qm_Tick(object sender, EventArgs e)
        {
            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart || airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {
                if (this.tim_Top10.Enabled == false)
                    SetCurrType();
            }
        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int c = _serialPortClient.GetCY_Low();


            lbl_dqyl.Text = c.ToString();

            AnimateSeries(this.tChart_qm, c);
        }

        int index = 0;
        private void tim_Top10_Tick(object sender, EventArgs e)
        {
            index++;

            if (index < 4)
            {
                return;
            }

            if (index > 8)
            {
                index = 0;
                this.tim_Top10.Enabled = false;
                this.gv_list.Enabled = false;
                return;
            }
            gv_list.Enabled = true;

            var cyvalue = _serialPortClient.GetCY_Low();

            //获取风速
            var fsvalue = _serialPortClient.GetFSXS();

            if (rdb_fjstl.Checked)
            {
                if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        windSpeedInfo.AddFY_FJST(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter100);
                    else
                        windSpeedInfo.AddFY_FJST(fsvalue, PublicEnum.Kpa_Level.liter100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter150);
                    else
                        windSpeedInfo.AddFY_FJST(fsvalue, PublicEnum.Kpa_Level.liter150);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.drop100);
                    else
                        windSpeedInfo.AddFY_FJST(fsvalue, PublicEnum.Kpa_Level.drop100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.drop50);
                    else
                        windSpeedInfo.AddFY_FJST(fsvalue, PublicEnum.Kpa_Level.drop50);
                }
            }
            else if (rdb_gfzh.Checked)
            {
                if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter100);
                    else
                        windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.drop100);
                    else
                        windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_FJST(fsvalue, PublicEnum.Kpa_Level.drop50);
                    else
                        windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop50);
                }
            }
            else if (rdb_zdstl.Checked)
            {
                //填充固附之和
                if (rdb_gfzh.Enabled == false)
                {
                    if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                    {
                        if (cyvalue > 0)
                            windSpeedInfo.AddZY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter50);
                        else
                            windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                    {
                        if (cyvalue > 0)
                            windSpeedInfo.AddZY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter100);
                        else
                            windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                    {
                        if (cyvalue > 0)
                            windSpeedInfo.AddZY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter150);
                        else
                            windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.liter150);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                    {
                        if (cyvalue > 0)
                            windSpeedInfo.AddZY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop100);
                        else
                            windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                    {
                        if (cyvalue > 0)
                            windSpeedInfo.AddZY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop50);
                        else
                            windSpeedInfo.Add_FY_GFZH(fsvalue, PublicEnum.Kpa_Level.drop50);
                    }
                }
                if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        windSpeedInfo.Add_FY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter100);
                    else
                        windSpeedInfo.Add_FY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter150);
                    else
                        windSpeedInfo.Add_FY_ZDST(fsvalue, PublicEnum.Kpa_Level.liter150);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_ZDST(fsvalue, PublicEnum.Kpa_Level.drop100);
                    else
                        windSpeedInfo.Add_FY_ZDST(fsvalue, PublicEnum.Kpa_Level.drop100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                {
                    if (cyvalue > 0)
                        windSpeedInfo.AddZY_ZDST(fsvalue, PublicEnum.Kpa_Level.drop50);
                    else
                        windSpeedInfo.Add_FY_ZDST(fsvalue, PublicEnum.Kpa_Level.drop50);
                }
            }
        }


        /// <summary>
        /// 设置添加数据状态
        /// </summary>
        /// <param name="value"></param>
        private void SetCurrType()
        {
            bool start = false;
            var notReadList = _readT.FindAll(t => t.IsRead == false);
            if (notReadList == null || notReadList.Count == 0)
            {
                return;
            }
            var notRead = notReadList?.OrderBy(t => t.Order)?.First();

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart)
            {
                if (notRead?.Key == BFMCommand.正压50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }

                else if (notRead?.Key == BFMCommand.正压100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压150TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter150;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }

                else if (notRead?.Key == BFMCommand.正压_50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }

            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {
                if (notRead?.Key == BFMCommand.负压50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压150TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter150;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
            }
        }

        #endregion


        #region 气密性能检测按钮事件

        /// <summary>
        /// 判断是否开启正压开始或负压开始
        /// </summary>
        //private bool IsStart = false;

        private void btn_justready_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "ZYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //lbl_setYL.Text = yl.ToString();

            var res = _serialPortClient.SetZYYB();
            if (!res)
            {
                return;
            }
            DisableBtnType();
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZReady;

            btn_justready.BackColor = Color.Green;
        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.Stop();
            if (!res)
                MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 正压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_juststart_Click(object sender, EventArgs e)
        {
            index = 0;

            var res = _serialPortClient.SendZYKS();
            if (!res)
                return;

            DisableBtnType();

            _readT = new List<ReadT>();

            _readT.Add(new ReadT() { Key = BFMCommand.正压50TimeStart, Order = 1, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压100TimeStart, Order = 2, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压150TimeStart, Order = 3, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_100TimeStart, Order = 4, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_50TimeStart, Order = 5, IsRead = false });


            BindFlowBase();

            btn_juststart.BackColor = Color.Green;

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZStart;

        }

        private void btn_loseready_Click(object sender, EventArgs e)
        {
            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "FYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //lbl_setYL.Text = "-" + yl.ToString();

            var res = _serialPortClient.SendFYYB();
            if (!res)
                return;

            DisableBtnType();
            btn_loseready.BackColor = Color.Green;
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FReady;
        }
        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableBtnType()
        {
            this.btn_justready.Enabled = false;
            this.btn_loseready.Enabled = false;
            this.btn_losestart.Enabled = false;
            this.btn_juststart.Enabled = false;



            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 开启按钮
        /// </summary>
        private void OpenBtnType()
        {
            this.btn_justready.Enabled = true;
            this.btn_loseready.Enabled = true;
            this.btn_losestart.Enabled = true;
            this.btn_datadispose.Enabled = true;
            this.btn_juststart.Enabled = true;

            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
        }


        /// <summary>
        /// 负压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_losestart_Click(object sender, EventArgs e)
        {
            index = 0;

            var res = _serialPortClient.SendFYKS();
            if (!res)
                return;
            DisableBtnType();

            _readT = new List<ReadT>();

            _readT.Add(new ReadT() { Key = BFMCommand.负压50TimeStart, Order = 1, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压100TimeStart, Order = 2, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压150TimeStart, Order = 3, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_100TimeStart, Order = 4, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_50TimeStart, Order = 5, IsRead = false });


            //if (rdb_fjstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.FJST = 0;
            //    }
            //}
            //else
            //if (rdb_gfzh.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.GFZH = 0;
            //    }
            //}
            //else if (rdb_zdstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.ZDST = 0;
            //    }
            //}
            BindFlowBase();

            btn_losestart.BackColor = Color.Green;
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FStart;
        }


        #endregion

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_qm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void export_image_qm_Click(object sender, EventArgs e)
        {
            this.tChart_qm.Export.ShowExportDialog();
        }


        private void btn_datadispose_Click(object sender, EventArgs e)
        {
            GetDatabaseLevelIndex();
            BindLevelIndex();

            //获取分级指标
            var qmZBInfo = GetQMZBInfo();

            //获取实验结果
            var qmInfoList = GetQMInfoList();
            if (qmInfoList != null && qmInfoList.Count > 0)
            {
                var res = dal_dt_qm_Info.AddQM(qmInfoList, qmZBInfo);
                if (res)
                {
                    MessageBox.Show("处理完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("暂无数据处理！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// 获取气密结果
        /// </summary>
        /// <returns></returns>
        private List<Model_dt_qm_Info> GetQMInfoList()
        {
            List<Model_dt_qm_Info> list = new List<Model_dt_qm_Info>();
            var sjzValue = txt_ycjy_z.Text;
            for (int i = 0; i <= 9; i++)
            {
                Model_dt_qm_Info model = new Model_dt_qm_Info();
                model.dt_Code = _tempCode;
                model.Pa = this.dgv_ll.Rows[i].Cells["pa"].Value.ToString();

                model.PaType = int.Parse(this.dgv_ll.Rows[i].Cells[1].Value.ToString());
                model.FJST = double.Parse(this.dgv_ll.Rows[i].Cells[2].Value.ToString()).ToString("f2");
                model.GFZH = double.Parse(this.dgv_ll.Rows[i].Cells[3].Value.ToString()).ToString("f2");
                model.ZDST = double.Parse(this.dgv_ll.Rows[i].Cells[4].Value.ToString()).ToString("f2");
                model.MQZT = double.Parse(this.dgv_ll.Rows[i].Cells[5].Value.ToString()).ToString("f2");
                model.KKST = double.Parse(this.dgv_ll.Rows[i].Cells[6].Value.ToString()).ToString("f2");
                model.testtype = int.Parse(sjzValue) > 0 ? "2" : "1";
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        /// 获取指标
        /// </summary>
        /// <returns></returns>
        private Model_dt_qm_zb_Info GetQMZBInfo()
        {
            Model_dt_qm_zb_Info model = new Model_dt_qm_zb_Info();
            model.dt_Code = _tempCode;
            model.Z_MJ = this.lbl_z_mj.Text;
            model.F_MJ = this.lbl_f_mj.Text;
            model.Z_FC = this.lbl_z_fc.Text;
            model.F_FC = this.lbl_f_fc.Text;
            return model;
        }

        /// <summary>
        /// 获取分级指标
        /// </summary>
        /// <param name="zFc">正压缝长</param>
        /// <param name="fFc">负压缝长</param>
        /// <param name="zMj">正压面积</param>
        /// <param name="fMj">负压面积</param>
        private void GetDatabaseLevelIndex()
        {
            // double kPa = 0;
            // double tempTemperature = 0;
            // double stitchLength = 0;
            // double sumArea = 0;

            // if (tab_settings != null && tab_settings.Rows.Count > 0)
            // {
            //     kPa = double.Parse(tab_settings.Rows[0]["DaQiYaLi"].ToString());
            //     tempTemperature = double.Parse(tab_settings.Rows[0]["DangQianWenDu"].ToString());
            //     stitchLength = double.Parse(tab_settings.Rows[0]["KaiQiFengChang"].ToString());
            //     sumArea = double.Parse(tab_settings.Rows[0]["shijianmianji"].ToString());
            // }
            //// List<AirtightCalculation> airtightCalculation = new List<AirtightCalculation>();


            // zFc = Formula.GetIndexStitchLengthAndArea(
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z"].Value.ToString()),
            //     true, kPa, tempTemperature, stitchLength, sumArea);

            // fFc = Formula.GetIndexStitchLengthAndArea(
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F"].Value.ToString()),
            //      true, kPa, tempTemperature, stitchLength, sumArea);

            // zMj = Formula.GetIndexStitchLengthAndArea(
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z"].Value.ToString()),
            //     false, kPa, tempTemperature, stitchLength, sumArea);

            // fMj = Formula.GetIndexStitchLengthAndArea(
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
            //     double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F"].Value.ToString()),
            //     false, kPa, tempTemperature, stitchLength, sumArea);


            //获取分级指标
            //var indexStitchLengthAndArea = Formula.GetJK_IndexStitchLengthAndArea(airtightCalculation, stitchLength, sumArea);
            //if (indexStitchLengthAndArea != null)
            //{
            //    zFc = indexStitchLengthAndArea.ZY_FC;
            //    fFc = indexStitchLengthAndArea.FY_FC;
            //    zMj = indexStitchLengthAndArea.ZY_MJ;
            //    fMj = indexStitchLengthAndArea.FY_MJ;
            //}
            // }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.Stop();
            if (!res)
                return;
            this.btn_justready.Enabled = true;
            this.btn_loseready.Enabled = true;
            this.btn_losestart.Enabled = true;
            this.btn_datadispose.Enabled = true;
            this.btn_juststart.Enabled = true;

            btn_datadispose.BackColor = Color.Transparent;
            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;

            this.tim_Top10.Enabled = false;
            BindFlowBase();
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
        }

        /// <summary>
        /// 控制气密性能检测按钮显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tim_getType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            if (airtightPropertyTest == null) { return; }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZReady)
            {
                int value = _serialPortClient.GetZYYBJS();
                if (value == 3)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    OpenBtnType();
                }
            }
            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart)
            {
                double value = _serialPortClient.GetZYKSJS();

                if (value >= 15)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    OpenBtnType();
                }
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FReady)
            {
                int value = _serialPortClient.GetFYYBJS();

                if (value == 3)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    OpenBtnType();
                }
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {
                double value = _serialPortClient.GetFYKSJS();

                if (value >= 15)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    OpenBtnType();
                }
            }
        }

        private void gv_list_Tick(object sender, EventArgs e)
        {
            try
            {
                var windSpeed = windSpeedInfo.GetWindSpeed();

                for (int i = 0; i < windSpeed.Count; i++)
                {
                    if (windSpeed[i].GFZH > 0)
                    {
                        windSpeedInfoList[i].GFZH = windSpeed[i].GFZH;
                    }
                    if (windSpeed[i].FJST > 0)
                    {
                        windSpeedInfoList[i].FJST = windSpeed[i].FJST;
                    }
                    if (windSpeed[i].ZDST > 0)
                    {
                        windSpeedInfoList[i].ZDST = windSpeed[i].ZDST;
                    }
                }

                BindFlowBase();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btn_ycjy_z_Click(object sender, EventArgs e)
        {
            //index = 0;
            //this.btn_ycjy_z.Enabled = false;
            //int value = 0;
            //int.TryParse(txt_ycjy_z.Text, out value);

            //if (value == 0)
            //{
            //    this.btn_ycjy_z.Enabled = true;
            //    return;
            //}
            //var res = _serialPortClient.Set_FY_Value(BFMCommand.正依次加压值, BFMCommand.正依次加压, value);
            //if (!res)
            //{
            //    MessageBox.Show("正依次加压！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            ////重复做
            //if (rdb_fjstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.Pressure_Z = 0;
            //    }
            //}
            //else if (rdb_zdstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.Pressure_Z_Z = 0;
            //    }

            //}
            //BindFlowBase();


            ////本程序控制
            //btn_ycjy_z.BackColor = Color.Green;

            //airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZYCJY;


            ////关闭监控按钮
            //this.btn_justready.Enabled = false;
            //this.btn_loseready.Enabled = false;
            //this.btn_losestart.Enabled = false;
            //this.btn_juststart.Enabled = false;


            //btn_justready.BackColor = Color.Transparent;
            //btn_loseready.BackColor = Color.Transparent;
            //btn_losestart.BackColor = Color.Transparent;
            //btn_juststart.BackColor = Color.Transparent;
        }

        private void btn_ycjyf_Click(object sender, EventArgs e)
        {
            //index = 0;
            //this.btn_ycjyf.Enabled = false;
            //int value = 0;

            //int.TryParse(txt_ycjy_f.Text, out value);

            //if (value == 0)
            //{
            //    this.btn_ycjyf.Enabled = true;
            //    return;
            //}

            //var res = _serialPortClient.Set_FY_Value(BFMCommand.负依次加压值, BFMCommand.负依次加压, value);
            //if (!res)
            //{
            //    MessageBox.Show("负依次加压异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //if (rdb_fjstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.Pressure_F = 0;
            //    }
            //}
            //else if (rdb_zdstl.Checked)
            //{
            //    foreach (var item in windSpeedInfoList)
            //    {
            //        item.Pressure_F_Z = 0;
            //    }
            //}
            //BindFlowBase();

            ////本程序控制
            //btn_ycjyf.BackColor = Color.Green;
            //airtightPropertyTest = PublicEnum.AirtightPropertyTest.FYCJY;

            ////关闭监控按钮
            //this.btn_justready.Enabled = false;
            //this.btn_loseready.Enabled = false;
            //this.btn_losestart.Enabled = false;
            //this.btn_juststart.Enabled = false;

            //btn_justready.BackColor = Color.Transparent;
            //btn_loseready.BackColor = Color.Transparent;
            //btn_losestart.BackColor = Color.Transparent;
            //btn_juststart.BackColor = Color.Transparent;
        }

        private void tChart_sm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_qm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.tChart_qm.Export.ShowExportDialog();
        }


        private void btn_exit_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.Stop();

            this.Close();
        }
    }


    public class ReadT
    {
        public string Key { get; set; }
        public int Order { get; set; }
        public bool IsRead { get; set; }
    }
}
