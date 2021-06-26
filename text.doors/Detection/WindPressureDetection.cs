using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Common;
using text.doors.dal;
using text.doors.Default;
using text.doors.Model;
using text.doors.Model.DataBase;
using text.doors.Service;

namespace text.doors.Detection
{
    public partial class WindPressureDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;

        private string _tempCode = ""; //检验编号
        public DateTime dtNow { get; set; }
        private PublicEnum.WindPressureTest? windPressureTest = null; //抗风压数据位置
        public static string selectTestType = "";//选择ABC检测类型
        private List<DefKFYPa> defKFYPa = new List<DefKFYPa>();  //默认
        public static bool IsGCJC = false;//工程检测

        private static DAL_dt_kfy_Info dalKfyInfo = new DAL_dt_kfy_Info();
        private static DAL_dt_kfy_res_Info dalKfyResInfo = new DAL_dt_kfy_res_Info();
        private static DAL_dt_Info dalDtInfo = new DAL_dt_Info();

        public static System.Timers.Timer tim_fy_A;
        public static System.Timers.Timer tim_static_A;

        public static System.Timers.Timer tim_fy_B;
        public static System.Timers.Timer tim_static_B;

        public static System.Timers.Timer tim_fy_C;
        public static System.Timers.Timer tim_static_C;

        private DataTable tabSettings = new DataTable();
        public List<WindPressureDGV> windPressureDGV_A = new List<WindPressureDGV>();
        public List<WindPressureDGV> windPressureDGV_B = new List<WindPressureDGV>();
        public List<WindPressureDGV> windPressureDGV_C = new List<WindPressureDGV>();

        private bool IsStartUp = false;//是否启动

        #region A组
        private bool isComplete_A = true;
        private int currentPoint_A = 0; //记录当前锚点
        private List<int> complete_A = new List<int>();
        private int indexCollection_A = 0; //稳压次数

        private List<Tuple<double, double, double>> average_A = new List<Tuple<double, double, double>>();
        #endregion

        #region B组
        private bool isComplete_B = true;
        private int currentPoint_B = 0;
        private List<int> complete_B = new List<int>();
        private int indexCollection_B = 0;
        private List<Tuple<double, double, double>> average_B = new List<Tuple<double, double, double>>();
        #endregion

        #region C组
        private bool isComplete_C = true;
        private int currentPoint_C = 0;
        private List<int> complete_C = new List<int>();
        private int indexCollection_C = 0;
        private List<Tuple<double, double, double>> average_C = new List<Tuple<double, double, double>>();

        #endregion


        public static System.Threading.Timer timRead;

        public WindPressureDetection(SerialPortClient serialPortClient, string tempCode)
        {
            InitializeComponent();

            this._serialPortClient = serialPortClient;
            this._tempCode = tempCode;

            BindDefInit();

            InitWindPressureDGV(PublicEnum.KFY_DGVENUM.DGV_A);
            InitWindPressureDGV(PublicEnum.KFY_DGVENUM.DGV_B);
            InitWindPressureDGV(PublicEnum.KFY_DGVENUM.DGV_C);

            BindData_A();

            BindData_B();

            BindData_C();

            CreateTimer();
        }

        private void CreateTimer()
        {
            try
            {
                timRead = new System.Threading.Timer(readTimer, null, 900, 0);
            }
            catch (Exception ex)
            {
            }
        }

        public void readTimer(object state)
        {
            try
            {
                if (selectTestType.Contains("A"))
                {
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
                }

                if (selectTestType.Contains("B"))
                {
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
                }
                if (selectTestType.Contains("C"))
                {
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
                }

                timRead.Change(100, 0);
            }
            catch (Exception ex)
            {
                timRead.Dispose();
                timRead = null;
            }
        }

        /// <summary>
        /// 绑定默认
        /// </summary>
        private void BindDefInit()
        {
            // 绑定设定压力
            lbl_title.Text = string.Format("门窗抗风压性能检测  第{0}号", this._tempCode);

            tabSettings = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);
            lbl_a.Text = tabSettings.Rows[0]["ganAchang"].ToString();
            lbl_b.Text = tabSettings.Rows[0]["ganBchang"].ToString();
            lbl_c.Text = tabSettings.Rows[0]["ganCchang"].ToString();
            selectTestType = tabSettings.Rows[0]["ganjianABC"].ToString();

            var kfyResDataInfo = dalKfyInfo.GetKFYResInfo(_tempCode);
            if (kfyResDataInfo != null)
            {
                //添加上次检测默认
                this.txt_gbjc.Text = kfyResDataInfo.defJC.ToString();
                this.txt_desc.Text = kfyResDataInfo.desc;
                this.txt_p1.Text = kfyResDataInfo.p1;
                this.txt_p2.Text = kfyResDataInfo.p2;
                this.txt_p3.Text = kfyResDataInfo.p3;
                this.txt_f_p1.Text = kfyResDataInfo._p1;
                this.txt_f_p2.Text = kfyResDataInfo._p2;
                this.txt_f_p3.Text = kfyResDataInfo._p3;
                this.txt_zpmax.Text = kfyResDataInfo.pMax;
                this.txt_fpmax.Text = kfyResDataInfo._pMax;
                this.txt_lx_a.Text = kfyResDataInfo.lx_a;
                this.txt_lx_b.Text = kfyResDataInfo.lx_b;
                this.txt_lx_c.Text = kfyResDataInfo.lx_c;

                IsGCJC = kfyResDataInfo.testtype == 2 ? true : false;

                if (IsGCJC)
                    btn_gcjc.BackColor = Color.Green;
                else
                    btn_gcjc.BackColor = Color.Transparent;
            }
            else
            {
                var jcValue = _serialPortClient.GetKFYjC();
                txt_gbjc.Text = jcValue.ToString();
            }

            var jcvalue = int.Parse(txt_gbjc.Text);
            for (int i = 1; i < 9; i++)
            {
                defKFYPa.Add(new DefKFYPa() { Value = jcvalue * i });
            }

            dtNow = DateTime.Now;

            //风速图表
            qm_Line.GetVertAxis.SetMinMax(-8000, 8000);
        }



        //委托
        public delegate void SetRealTimeDataGroup(
            string value1, string value2, string value3,
            string value4, string value5, string value6,
            string value7, string value8, string value9);



        #region 绑定 列表控件
        /// <summary>
        /// A
        /// </summary>
        private void BindData_A()
        {
            dgv_WindPressure_A.DataSource = windPressureDGV_A;
            dgv_WindPressure_A.RowHeadersVisible = false;
            dgv_WindPressure_A.AllowUserToResizeColumns = false;
            dgv_WindPressure_A.AllowUserToResizeRows = false;

            dgv_WindPressure_A.Columns[0].HeaderText = "国际检测";
            dgv_WindPressure_A.Columns[0].Width = 85;
            dgv_WindPressure_A.Columns[0].ReadOnly = true;
            dgv_WindPressure_A.Columns[0].DataPropertyName = "Pa";

            dgv_WindPressure_A.Columns[1].HeaderText = "位移A1";
            dgv_WindPressure_A.Columns[1].Width = 75;
            dgv_WindPressure_A.Columns[1].DataPropertyName = "zwy1";
            dgv_WindPressure_A.Columns[1].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[2].HeaderText = "位移A2";
            dgv_WindPressure_A.Columns[2].Width = 75;
            dgv_WindPressure_A.Columns[2].DataPropertyName = "zwy2";
            dgv_WindPressure_A.Columns[2].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[3].HeaderText = "位移A3";
            dgv_WindPressure_A.Columns[3].Width = 75;
            dgv_WindPressure_A.Columns[3].DataPropertyName = "zwy3";
            dgv_WindPressure_A.Columns[3].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[4].HeaderText = "挠度";
            dgv_WindPressure_A.Columns[4].Width = 75;
            dgv_WindPressure_A.Columns[4].DataPropertyName = "zzd";
            dgv_WindPressure_A.Columns[4].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[5].HeaderText = "l/X";
            dgv_WindPressure_A.Columns[5].Width = 75;
            dgv_WindPressure_A.Columns[5].DataPropertyName = "zlx";
            dgv_WindPressure_A.Columns[5].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[6].HeaderText = "位移A1";
            dgv_WindPressure_A.Columns[6].Width = 75;
            dgv_WindPressure_A.Columns[6].DataPropertyName = "fwy1";
            dgv_WindPressure_A.Columns[6].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[7].HeaderText = "位移A2";
            dgv_WindPressure_A.Columns[7].Width = 74;
            dgv_WindPressure_A.Columns[7].DataPropertyName = "fwy2";
            dgv_WindPressure_A.Columns[7].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[8].HeaderText = "位移A3";
            dgv_WindPressure_A.Columns[8].Width = 75;
            dgv_WindPressure_A.Columns[8].DataPropertyName = "fwy3";
            dgv_WindPressure_A.Columns[8].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[9].HeaderText = "挠度";
            dgv_WindPressure_A.Columns[9].Width = 75;
            dgv_WindPressure_A.Columns[9].DataPropertyName = "fzd";
            dgv_WindPressure_A.Columns[9].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[10].HeaderText = "l/X";
            dgv_WindPressure_A.Columns[10].Width = 75;
            dgv_WindPressure_A.Columns[10].DataPropertyName = "flx";
            dgv_WindPressure_A.Columns[10].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_A.Columns[11].Visible = false;
            dgv_WindPressure_A.Columns[12].Visible = false;

            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_WindPressure_A.Refresh();
                }));
            }
        }

        /// <summary>
        /// B
        /// </summary>
        private void BindData_B()
        {
            dgv_WindPressure_B.DataSource = windPressureDGV_B;
            dgv_WindPressure_B.RowHeadersVisible = false;
            dgv_WindPressure_B.AllowUserToResizeColumns = false;
            dgv_WindPressure_B.AllowUserToResizeRows = false;

            dgv_WindPressure_B.Columns[0].HeaderText = "国际检测";
            dgv_WindPressure_B.Columns[0].Width = 85;
            dgv_WindPressure_B.Columns[0].ReadOnly = true;
            dgv_WindPressure_B.Columns[0].DataPropertyName = "Pa";


            dgv_WindPressure_B.Columns[1].HeaderText = "位移B1";
            dgv_WindPressure_B.Columns[1].Width = 75;
            dgv_WindPressure_B.Columns[1].DataPropertyName = "zwy1";
            dgv_WindPressure_B.Columns[1].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_B.Columns[2].HeaderText = "位移B2";
            dgv_WindPressure_B.Columns[2].Width = 75;
            dgv_WindPressure_B.Columns[2].DataPropertyName = "zwy2";
            dgv_WindPressure_B.Columns[2].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[3].HeaderText = "位移B3";
            dgv_WindPressure_B.Columns[3].Width = 75;
            dgv_WindPressure_B.Columns[3].DataPropertyName = "zwy3";
            dgv_WindPressure_B.Columns[3].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[4].HeaderText = "挠度";
            dgv_WindPressure_B.Columns[4].Width = 75;
            dgv_WindPressure_B.Columns[4].DataPropertyName = "zzd";
            dgv_WindPressure_B.Columns[4].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[5].HeaderText = "l/X";
            dgv_WindPressure_B.Columns[5].Width = 75;
            dgv_WindPressure_B.Columns[5].DataPropertyName = "zlx";
            dgv_WindPressure_B.Columns[5].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_B.Columns[6].HeaderText = "位移B1";
            dgv_WindPressure_B.Columns[6].Width = 75;
            dgv_WindPressure_B.Columns[6].DataPropertyName = "fwy1";
            dgv_WindPressure_B.Columns[6].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_B.Columns[7].HeaderText = "位移B2";
            dgv_WindPressure_B.Columns[7].Width = 74;
            dgv_WindPressure_B.Columns[7].DataPropertyName = "fwy2";
            dgv_WindPressure_B.Columns[7].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[8].HeaderText = "位移B3";
            dgv_WindPressure_B.Columns[8].Width = 75;
            dgv_WindPressure_B.Columns[8].DataPropertyName = "fwy3";
            dgv_WindPressure_B.Columns[8].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[9].HeaderText = "挠度";
            dgv_WindPressure_B.Columns[9].Width = 75;
            dgv_WindPressure_B.Columns[9].DataPropertyName = "fzd";
            dgv_WindPressure_B.Columns[9].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[10].HeaderText = "l/X";
            dgv_WindPressure_B.Columns[10].Width = 75;
            dgv_WindPressure_B.Columns[10].DataPropertyName = "flx";
            dgv_WindPressure_B.Columns[10].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_B.Columns[11].Visible = false;
            dgv_WindPressure_B.Columns[12].Visible = false;

            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_WindPressure_B.Refresh();
                }));
            }
        }


        /// <summary>
        /// C
        /// </summary>
        private void BindData_C()
        {
            dgv_WindPressure_C.DataSource = windPressureDGV_C;
            dgv_WindPressure_C.RowHeadersVisible = false;
            dgv_WindPressure_C.AllowUserToResizeColumns = false;
            dgv_WindPressure_C.AllowUserToResizeRows = false;

            dgv_WindPressure_C.Columns[0].HeaderText = "国际检测";
            dgv_WindPressure_C.Columns[0].Width = 85;
            dgv_WindPressure_C.Columns[0].ReadOnly = true;
            dgv_WindPressure_C.Columns[0].DataPropertyName = "Pa";


            dgv_WindPressure_C.Columns[1].HeaderText = "位移C1";
            dgv_WindPressure_C.Columns[1].Width = 75;
            dgv_WindPressure_C.Columns[1].DataPropertyName = "zwy1";
            dgv_WindPressure_C.Columns[1].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_C.Columns[2].HeaderText = "位移C2";
            dgv_WindPressure_C.Columns[2].Width = 75;
            dgv_WindPressure_C.Columns[2].DataPropertyName = "zwy2";
            dgv_WindPressure_C.Columns[2].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_C.Columns[3].HeaderText = "位移C3";
            dgv_WindPressure_C.Columns[3].Width = 75;
            dgv_WindPressure_C.Columns[3].DataPropertyName = "zwy3";
            dgv_WindPressure_C.Columns[3].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[4].HeaderText = "挠度";
            dgv_WindPressure_C.Columns[4].Width = 75;
            dgv_WindPressure_C.Columns[4].DataPropertyName = "zzd";
            dgv_WindPressure_C.Columns[4].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[5].HeaderText = "l/X";
            dgv_WindPressure_C.Columns[5].Width = 75;
            dgv_WindPressure_C.Columns[5].DataPropertyName = "zlx";
            dgv_WindPressure_C.Columns[5].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[6].HeaderText = "位移C1";
            dgv_WindPressure_C.Columns[6].Width = 75;
            dgv_WindPressure_C.Columns[6].DataPropertyName = "fwy1";
            dgv_WindPressure_C.Columns[6].DefaultCellStyle.Format = "N2";


            dgv_WindPressure_C.Columns[7].HeaderText = "位移C2";
            dgv_WindPressure_C.Columns[7].Width = 74;
            dgv_WindPressure_C.Columns[7].DataPropertyName = "fwy2";
            dgv_WindPressure_C.Columns[7].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[8].HeaderText = "位移C3";
            dgv_WindPressure_C.Columns[8].Width = 75;
            dgv_WindPressure_C.Columns[8].DataPropertyName = "fwy3";
            dgv_WindPressure_C.Columns[8].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[9].HeaderText = "挠度";
            dgv_WindPressure_C.Columns[9].Width = 75;
            dgv_WindPressure_C.Columns[9].DataPropertyName = "fzd";
            dgv_WindPressure_C.Columns[9].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[10].HeaderText = "l/X";
            dgv_WindPressure_C.Columns[10].Width = 75;
            dgv_WindPressure_C.Columns[10].DataPropertyName = "flx";
            dgv_WindPressure_C.Columns[10].DefaultCellStyle.Format = "N2";

            dgv_WindPressure_C.Columns[11].Visible = false;
            dgv_WindPressure_C.Columns[12].Visible = false;

            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_WindPressure_C.Refresh();
                }));
            }
        }
        #endregion

        #region  获取列表数据
        private void InitWindPressureDGV(PublicEnum.KFY_DGVENUM kfy_dgveEnum)
        {
            int lengA = int.Parse(tabSettings.Rows[0]["ganAchang"].ToString());
            int lengB = int.Parse(tabSettings.Rows[0]["ganBchang"].ToString());
            int lengC = int.Parse(tabSettings.Rows[0]["ganCchang"].ToString());

            var kfyTable = dalKfyInfo.GetkfyListByCode(_tempCode);
            if (kfyTable != null && kfyTable.Rows.Count > 0)
            {
                if (int.Parse(txt_gbjc.Text) == 0)
                {
                    if (kfy_dgveEnum == PublicEnum.KFY_DGVENUM.DGV_A)
                        windPressureDGV_A = GetDefData(lengA);
                    else if (kfy_dgveEnum == PublicEnum.KFY_DGVENUM.DGV_B)
                        windPressureDGV_B = GetDefData(lengB);
                    else if (kfy_dgveEnum == PublicEnum.KFY_DGVENUM.DGV_C)
                        windPressureDGV_C = GetDefData(lengC);
                    return;
                }

                foreach (DataRow dr in kfyTable.Rows)
                {
                    var level = dr["level"].ToString();
                    if (level == "A")
                        windPressureDGV_A = GetGroupData(dr, lengA);
                    else if (level == "B")
                        windPressureDGV_B = GetGroupData(dr, lengB);
                    else if (level == "C")
                        windPressureDGV_C = GetGroupData(dr, lengC);
                }
                if (windPressureDGV_A.Count == 0)
                    windPressureDGV_A = GetDefData(lengA);
                if (windPressureDGV_B.Count == 0)
                    windPressureDGV_B = GetDefData(lengB);
                if (windPressureDGV_C.Count == 0)
                    windPressureDGV_C = GetDefData(lengC);
            }
            else
            {
                windPressureDGV_A = GetDefData(lengA);
                windPressureDGV_B = GetDefData(lengB);
                windPressureDGV_C = GetDefData(lengC);
            }
        }


        /// <summary>
        /// 分组获取数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<WindPressureDGV> GetGroupData(DataRow dr, int ganJianChangDu)
        {
            List<WindPressureDGV> tempWindPressureDGV = new List<WindPressureDGV>();

            for (int i = 0; i < defKFYPa.Count; i++)
            {
                var paInfo = defKFYPa[i];
                var dtValue = (i + 1) * 250;

                tempWindPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = paInfo.Value + "Pa",
                    PaValue = paInfo.Value,
                    zwy1 = string.IsNullOrWhiteSpace(dr["z_one_" + dtValue].ToString()) ? 0d : double.Parse(dr["z_one_" + dtValue].ToString()),
                    zwy2 = string.IsNullOrWhiteSpace(dr["z_two_" + dtValue].ToString()) ? 0d : double.Parse(dr["z_two_" + dtValue].ToString()),
                    zwy3 = string.IsNullOrWhiteSpace(dr["z_three_" + dtValue].ToString()) ? 0d : double.Parse(dr["z_three_" + dtValue].ToString()),

                    fwy1 = string.IsNullOrWhiteSpace(dr["f_one_" + dtValue].ToString()) ? 0d : double.Parse(dr["f_one_" + dtValue].ToString()),
                    fwy2 = string.IsNullOrWhiteSpace(dr["f_two_" + dtValue].ToString()) ? 0d : double.Parse(dr["f_two_" + dtValue].ToString()),
                    fwy3 = string.IsNullOrWhiteSpace(dr["f_three_" + dtValue].ToString()) ? 0d : double.Parse(dr["f_three_" + dtValue].ToString()),
                    GanJianChangDu = ganJianChangDu
                }); ;
            }

            //极差
            for (int i = 0; i < 2; i++)
            {
                var name = "";
                var field = "";
                int pa = 0;
                if (i == 0)
                {
                    pa = -1;
                    name = "P3";
                    field = "p3";
                }
                else if (i == 1)
                {
                    pa = -2;
                    name = "P3Max";
                    field = "p3max";
                }
                tempWindPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = name,
                    PaValue = pa,
                    zwy1 = string.IsNullOrWhiteSpace(dr["z_one_" + field].ToString()) ? 0d : double.Parse(dr["z_one_" + field].ToString()),
                    zwy2 = string.IsNullOrWhiteSpace(dr["z_two_" + field].ToString()) ? 0d : double.Parse(dr["z_two_" + field].ToString()),
                    zwy3 = string.IsNullOrWhiteSpace(dr["z_three_" + field].ToString()) ? 0d : double.Parse(dr["z_three_" + field].ToString()),

                    fwy1 = string.IsNullOrWhiteSpace(dr["f_one_" + field].ToString()) ? 0d : double.Parse(dr["f_one_" + field].ToString()),
                    fwy2 = string.IsNullOrWhiteSpace(dr["f_two_" + field].ToString()) ? 0d : double.Parse(dr["f_two_" + field].ToString()),
                    fwy3 = string.IsNullOrWhiteSpace(dr["f_three_" + field].ToString()) ? 0d : double.Parse(dr["f_three_" + field].ToString()),
                    GanJianChangDu = ganJianChangDu
                });
            }

            return tempWindPressureDGV;
        }


        /// <summary>
        ///获取默认数据
        /// </summary>
        /// <returns></returns>
        private List<WindPressureDGV> GetDefData(int leng)
        {
            List<WindPressureDGV> tempWindPressureDGV = new List<WindPressureDGV>();
            foreach (var paInfo in defKFYPa)
            {
                tempWindPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = paInfo.Value + "Pa",
                    PaValue = paInfo.Value,
                    zwy1 = 0.00,
                    zwy2 = 0.00,
                    zwy3 = 0.00,
                    fwy1 = 0.00,
                    fwy2 = 0.00,
                    fwy3 = 0.00,
                    GanJianChangDu = leng
                });
            }
            //极差
            for (int i = 0; i < 2; i++)
            {
                var name = "";
                var paValue = 0;
                if (i == 0)
                {
                    name = "P3";
                    paValue = -1;
                }
                else if (i == 1)
                {
                    name = "P3Max";
                    paValue = -2;
                }
                tempWindPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = name,
                    PaValue = paValue,
                    zwy1 = 0.00,
                    zwy2 = 0.00,
                    zwy3 = 0.00,
                    fwy1 = 0.00,
                    fwy2 = 0.00,
                    fwy3 = 0.00,
                });
            }
            return tempWindPressureDGV;
        }
        #endregion

        #region 按钮控制
        private void btn_zff_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            int value = 0;

            int.TryParse(txt_p2.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正反复数值, BFMCommand.正反复, value);
            if (!res)
            {
                MessageBox.Show("正反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            windPressureTest = PublicEnum.WindPressureTest.ZRepeatedly;
            DisableBtnType();
            btn_zff.BackColor = Color.Green;
        }

        private void btn_zyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.风压正压预备);
            if (!res)
            {
                MessageBox.Show("正压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            windPressureTest = PublicEnum.WindPressureTest.ZReady;
            DisableBtnType();
            btn_zyyb.BackColor = Color.Green;
        }

        private void btn_zyks_Click(object sender, EventArgs e)
        {
            tim_fy_A = null;
            tim_fy_B = null;
            tim_fy_C = null;
            if (!_serialPortClient.sp.IsOpen)
                return;

            var jc = int.Parse(txt_gbjc.Text);
            if (jc == 0)
            {
                MessageBox.Show("请选择极差！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.风压正压开始);
            if (!res)
                return;

            windPressureTest = PublicEnum.WindPressureTest.ZStart;
            DisableBtnType();
            btn_zyks.BackColor = Color.Green;

            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();

            currentPoint_A = 0;
            currentPoint_B = 0;
            currentPoint_C = 0;

            indexCollection_A = 0;
            indexCollection_B = 0;
            indexCollection_C = 0;

            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;

            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;

            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;

        }

        private void btn_fyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.风压负压预备);
            if (!res)
            {
                MessageBox.Show("负压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FReady;
            DisableBtnType();
            btn_fyyb.BackColor = Color.Green;
        }

        private void btn_fyks_Click(object sender, EventArgs e)
        {
            tim_fy_A = null;
            tim_fy_B = null;
            tim_fy_C = null;

            if (!_serialPortClient.sp.IsOpen)
                return;

            var jc = int.Parse(txt_gbjc.Text);
            if (jc == 0)
            {
                MessageBox.Show("请选择极差！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var res = _serialPortClient.SendBtnSingleCoil(BFMCommand.风压负压开始);
            if (!res)
            {
                MessageBox.Show("负压开始异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FStart;
            DisableBtnType();
            btn_fyks.BackColor = Color.Green;

            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();
            currentPoint_A = 0;
            currentPoint_B = 0;
            currentPoint_C = 0;
            indexCollection_A = 0;
            indexCollection_B = 0;
            indexCollection_C = 0;

            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;

            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;

            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;

        }

        private void btn_datahandle_Click(object sender, EventArgs e)
        {
            List<double> zyList = new List<double>();
            List<double> fyList = new List<double>();

            if (selectTestType.Contains("A"))
            {
                double lx_a = 0;
                double.TryParse(txt_lx_a.Text, out lx_a);
                double p1 = 0d;
                double _p1 = 0d;
                int lengA = int.Parse(tabSettings.Rows[0]["ganAchang"].ToString());
                GetP1(windPressureDGV_A, lengA, lx_a, ref p1, ref _p1);

                if (p1 > 0)
                    zyList.Add(p1);
                if (_p1 > 0)
                    fyList.Add(_p1);
                currentPoint_A = 0;
            }
            if (selectTestType.Contains("B"))
            {
                double lx_b = 0;
                double.TryParse(txt_lx_b.Text, out lx_b);
                double p1 = 0d;
                double _p1 = 0d;
                int lengA = int.Parse(tabSettings.Rows[0]["ganBchang"].ToString());
                GetP1(windPressureDGV_B, lengA, lx_b, ref p1, ref _p1);
                if (p1 > 0)
                    zyList.Add(p1);
                if (_p1 > 0)
                    fyList.Add(_p1);
                currentPoint_B = 0;
            }
            if (selectTestType.Contains("C"))
            {
                double lx_c = 0;
                double.TryParse(txt_lx_c.Text, out lx_c);
                double p1 = 0d;
                double _p1 = 0d;
                int lengA = int.Parse(tabSettings.Rows[0]["ganCchang"].ToString());

                GetP1(windPressureDGV_C, lengA, lx_c, ref p1, ref _p1);
                if (p1 > 0)
                    zyList.Add(p1);
                if (_p1 > 0)
                    fyList.Add(_p1);
                currentPoint_C = 0;
            }
            var zyV = 0d;
            var fyV = 0d;
            if (zyList?.FindAll(t => t > 0).Count > 0)
            {
                //正负最小的在值
                zyList.AddRange(fyList);
                zyV = zyList.FindAll(t => t > 0).Min();
            }
            if (fyList?.FindAll(t => t > 0).Count > 0)
            {
                fyV = fyList.FindAll(t => t > 0).Min();
            }

            txt_p1.Text = zyV > 0 ? Math.Round(zyV, 0).ToString() : "0";
            txt_f_p1.Text = fyV > 0 ? Math.Round(fyV, 0).ToString() : "0";

            txt_p2.Text = Math.Round(zyV * 1.5, 0).ToString();
            txt_p3.Text = Math.Round(zyV * 2.5, 0).ToString();

            txt_f_p2.Text = Math.Round(fyV * 1.5, 0).ToString();
            txt_f_p3.Text = Math.Round(fyV * 2.5, 0).ToString();
        }


        private void GetP1(List<WindPressureDGV> data, int leng, double lx,
            ref double p1, ref double _p1)
        {
            var zdefPa = 2000;
            var fdefPa = 2000;
            int lengA = leng;

            var tempData_z = data.FindAll(t => t.zlx > 0 && t.Pa != "P3" && t.Pa != "P3Max").ToList();
            if (tempData_z != null && tempData_z.Count() > 0)
            {
                if (tempData_z[tempData_z.Count - 1] != null)
                {
                    zdefPa = tempData_z[tempData_z.Count - 1].PaValue;
                }
            }
            var tempData_f = data.FindAll(t => t.flx > 0 && t.Pa != "P3" && t.Pa != "P3Max").ToList();
            if (tempData_f != null && tempData_f.Count() > 0)
            {
                if (tempData_f[tempData_f.Count - 1] != null)
                {
                    fdefPa = tempData_f[tempData_f.Count - 1].PaValue;
                }
            }

            var zone = new WindPressureDGV();
            var ztwo = new WindPressureDGV();
            zone = data.Find(t => t.Pa == (zdefPa - 250) + "Pa");
            ztwo = data.Find(t => t.Pa == zdefPa + "Pa");
            if (zone != null && ztwo != null && zone.zzd > 0 && ztwo.zzd > 0)
            {
                var x1 = float.Parse(zone.zzd.ToString());
                var x2 = float.Parse(ztwo.zzd.ToString());
                var y1 = zdefPa - 250;
                var y2 = zdefPa;

                var p = Calculate(x1, x2, y1, y2, lengA, lx);

                p1 = Math.Round(p, 0, MidpointRounding.AwayFromZero);
            }

            var fone = new WindPressureDGV();
            var ftwo = new WindPressureDGV();
            fone = data.Find(t => t.Pa == (fdefPa - 250) + "Pa");
            ftwo = data.Find(t => t.Pa == fdefPa + "Pa");
            if (zone != null && ztwo != null && fone.zzd > 0 && ftwo.zzd > 0)
            {
                var _x1 = float.Parse(fone.fzd.ToString());
                var _x2 = float.Parse(ftwo.fzd.ToString());
                var y1 = fdefPa - 250;
                var y2 = fdefPa;
                var _p = Calculate(_x1, _x2, y1, y2, lengA, lx);
                _p1 = Math.Round(_p, 0, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <returns></returns> 

        private double Calculate(float x1, float x2, int y1, int y2, int gjcd, double lx)
        {
            float k = 0, b = 0;

            Formula.Calculate(x1, x2, y1, y2, ref k, ref b);

            double x = lx;

            x = gjcd / x;

            if (k == 0 && b == 0)
                return x;

            return k * x + b;
        }


        private void btn_fff_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int value = 0;
            int.TryParse(txt_f_p2.Text, out value);

            if (value == 0)
                return;
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负反复数值, BFMCommand.负反复, value);
            if (!res)
            {
                MessageBox.Show("负反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            windPressureTest = PublicEnum.WindPressureTest.FRepeatedly;
            DisableBtnType();
            btn_fff.BackColor = Color.Green;
        }

        private void btn_zaq_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int value = 0;
            int.TryParse(txt_p3.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正安全数值, BFMCommand.正安全, value);
            if (!res)
            {
                MessageBox.Show("正安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();

            currentPoint_A = 0;
            currentPoint_B = 0;
            currentPoint_C = 0;

            indexCollection_A = 0;
            indexCollection_B = 0;
            indexCollection_C = 0;

            windPressureTest = PublicEnum.WindPressureTest.ZSafety;
            DisableBtnType();
            btn_zaq.BackColor = Color.Green;


            //if (selectTestType.Contains("A"))
            //{
            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;
            //}
            //if (selectTestType.Contains("B"))
            //{
            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;
            //}
            //if (selectTestType.Contains("C"))
            //{
            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;
            //}
        }

        private void btnfaq_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int value = 0;
            int.TryParse(txt_f_p3.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.负安全数值, BFMCommand.负安全, value);
            if (!res)
            {
                MessageBox.Show("负安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();

            currentPoint_A = 0;
            currentPoint_B = 0;
            currentPoint_C = 0;

            indexCollection_A = 0;
            indexCollection_B = 0;
            indexCollection_C = 0;

            windPressureTest = PublicEnum.WindPressureTest.FSafety;
            DisableBtnType();

            btnfaq.BackColor = Color.Green;
            //if (selectTestType.Contains("A"))
            //{
            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;
            //}
            //if (selectTestType.Contains("B"))
            //{
            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;
            //}
            //if (selectTestType.Contains("C"))
            //{
            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;
            //}
        }

        private void btn_wygl_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            _serialPortClient.SendDisplaceZero(BFMCommand.位移1标零);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
            OpenBtnType();
            windPressureTest = PublicEnum.WindPressureTest.Stop;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var jc = int.Parse(txt_gbjc.Text);
            if (Add(jc))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();
        }

        private void btn_zpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_zpmax.Text, out value);
            var res = _serialPortClient.Set_FY_Value(BFMCommand.正PMAX值, BFMCommand.正PMAX, value);
            if (!res)
            {
                MessageBox.Show("正pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();


            windPressureTest = PublicEnum.WindPressureTest.ZPmax;
            DisableBtnType();
            btn_zpmax.BackColor = Color.Green;

            //if (selectTestType.Contains("A"))
            //{
            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;
            //}
            //if (selectTestType.Contains("B"))
            //{
            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;
            //}
            //if (selectTestType.Contains("C"))
            //{
            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;
            //}

        }

        private void btn_fpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_fpmax.Text, out value);
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负PMAX值, BFMCommand.负PMAX, value);
            if (!res)
            {
                MessageBox.Show("负pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            complete_A = new List<int>();
            complete_B = new List<int>();
            complete_C = new List<int>();

            windPressureTest = PublicEnum.WindPressureTest.FPmax;
            DisableBtnType();
            btn_fpmax.BackColor = Color.Green;
            //if (selectTestType.Contains("A"))
            //{
            tim_fy_A = new System.Timers.Timer(1000);
            tim_fy_A.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_A);
            tim_fy_A.Enabled = true;
            //}
            //if (selectTestType.Contains("B"))
            //{
            tim_fy_B = new System.Timers.Timer(1000);
            tim_fy_B.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_B);
            tim_fy_B.Enabled = true;
            //}
            //if (selectTestType.Contains("C"))
            //{
            tim_fy_C = new System.Timers.Timer(1000);
            tim_fy_C.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer_C);
            tim_fy_C.Enabled = true;
            // }

        }
        private void button12_Click(object sender, EventArgs e)
        {
            timRead.Dispose();
            this.Close();
        }
        private void btn_gcjc_Click(object sender, EventArgs e)
        {
            IsGCJC = !IsGCJC;

            if (IsGCJC)
                btn_gcjc.BackColor = Color.Green;
            else
                btn_gcjc.BackColor = Color.Transparent;
        }

        private void btn_gbjc_Click(object sender, EventArgs e)
        {
            var value = int.Parse(txt_gbjc.Text);
            var res = _serialPortClient.SendGBJC(value);
            if (!res)
            {
                MessageBox.Show("改变级差异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            defKFYPa = new List<DefKFYPa>();
            for (int i = 1; i < 9; i++)
            {
                defKFYPa.Add(new DefKFYPa() { Value = value * i });
            }
            var lengA = int.Parse(tabSettings.Rows[0]["ganAchang"].ToString());
            var lengB = int.Parse(tabSettings.Rows[0]["ganBchang"].ToString());
            var lengC = int.Parse(tabSettings.Rows[0]["ganCchang"].ToString());
            windPressureDGV_A = GetDefData(lengA);
            windPressureDGV_B = GetDefData(lengB);
            windPressureDGV_C = GetDefData(lengC);

            BindData_A();
            BindData_B();
            BindData_C();
        }

        //private void btn_zp3cybx_Click(object sender, EventArgs e)
        //{
        //    if (selectTestType.Contains("A"))
        //    {
        //        var info = windPressureDGV_A.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceA1;
        //            info.zwy2 = RegisterData.DisplaceA2;
        //            info.zwy3 = RegisterData.DisplaceA3;
        //        }
        //        BindData_A();
        //    }
        //    if (selectTestType.Contains("B"))
        //    {
        //        var info = windPressureDGV_B.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceB1;
        //            info.zwy2 = RegisterData.DisplaceB2;
        //            info.zwy3 = RegisterData.DisplaceB3;
        //        }
        //        BindData_B();
        //    }
        //    if (selectTestType.Contains("C"))
        //    {
        //        var info = windPressureDGV_C.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceC1;
        //            info.zwy2 = RegisterData.DisplaceC2;
        //            info.zwy3 = RegisterData.DisplaceC3;
        //        }
        //        BindData_C();
        //    }
        //}

        //private void btn_fp3cybx_Click(object sender, EventArgs e)
        //{
        //    if (selectTestType.Contains("A"))
        //    {
        //        var info = windPressureDGV_A.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceA1;
        //            info.zwy2 = RegisterData.DisplaceA2;
        //            info.zwy3 = RegisterData.DisplaceA3;
        //        }
        //        BindData_A();
        //    }
        //    if (selectTestType.Contains("B"))
        //    {
        //        var info = windPressureDGV_B.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceB1;
        //            info.zwy2 = RegisterData.DisplaceB2;
        //            info.zwy3 = RegisterData.DisplaceB3;
        //        }
        //        BindData_B();
        //    }
        //    if (selectTestType.Contains("C"))
        //    {
        //        var info = windPressureDGV_C.Find(t => t.PaValue == -2);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceC1;
        //            info.zwy2 = RegisterData.DisplaceC2;
        //            info.zwy3 = RegisterData.DisplaceC3;
        //        }
        //        BindData_C();
        //    }
        //}

        //private void btn_zpmax_cybx_Click(object sender, EventArgs e)
        //{
        //    if (selectTestType.Contains("A"))
        //    {
        //        var info = windPressureDGV_A.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceA1;
        //            info.zwy2 = RegisterData.DisplaceA2;
        //            info.zwy3 = RegisterData.DisplaceA3;
        //        }
        //        BindData_A();
        //    }
        //    if (selectTestType.Contains("B"))
        //    {
        //        var info = windPressureDGV_B.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceB1;
        //            info.zwy2 = RegisterData.DisplaceB2;
        //            info.zwy3 = RegisterData.DisplaceB3;
        //        }
        //        BindData_B();
        //    }
        //    if (selectTestType.Contains("C"))
        //    {
        //        var info = windPressureDGV_C.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceC1;
        //            info.zwy2 = RegisterData.DisplaceC2;
        //            info.zwy3 = RegisterData.DisplaceC3;
        //        }
        //        BindData_C();
        //    }
        //}

        //private void btn_fpmax_cybx_Click(object sender, EventArgs e)
        //{
        //    if (selectTestType.Contains("A"))
        //    {
        //        var info = windPressureDGV_A.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceA1;
        //            info.zwy2 = RegisterData.DisplaceA2;
        //            info.zwy3 = RegisterData.DisplaceA3;
        //        }
        //        BindData_A();
        //    }
        //    if (selectTestType.Contains("B"))
        //    {
        //        var info = windPressureDGV_B.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceB1;
        //            info.zwy2 = RegisterData.DisplaceB2;
        //            info.zwy3 = RegisterData.DisplaceB3;
        //        }
        //        BindData_B();
        //    }
        //    if (selectTestType.Contains("C"))
        //    {
        //        var info = windPressureDGV_C.Find(t => t.PaValue == -3);
        //        if (info != null)
        //        {
        //            info.zwy1 = RegisterData.DisplaceC1;
        //            info.zwy2 = RegisterData.DisplaceC2;
        //            info.zwy3 = RegisterData.DisplaceC3;
        //        }
        //        BindData_C();
        //    }
        //}
        #endregion

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZReady)
            {
                AnimateSeries(this.tChart_qm, RegisterData.CY_Low_Value);
                lbl_dqyl.Text = RegisterData.CY_Low_Value.ToString();
            }
            else
            {
                AnimateSeries(this.tChart_qm, RegisterData.CY_High_Value);
                lbl_dqyl.Text = RegisterData.CY_High_Value.ToString();
            }
        }


        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.qm_Line.Add(DateTime.Now, yl);
            this.tChart_qm.Axes.Bottom.SetMinMax(dtNow, DateTime.Now.AddSeconds(20));
        }

        private void fyTimer_A(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!isComplete_A)
                return;

            var cyValue = RegisterData.CY_High_Value;
            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                var val = defKFYPa.Find(t => cyValue >= t.MinValue && cyValue <= t.MaxValue);
                if (val != null && !complete_A.Exists(t => t == val.Value))
                {
                    complete_A.Add(val.Value);
                    currentPoint_A = val.Value;

                    tim_static_A = new System.Timers.Timer(500);
                    tim_static_A.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_A);
                    tim_static_A.Enabled = true;

                }
            }
            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                var val = defKFYPa.Find(t => cyValue >= t._MinValue && cyValue <= t._MaxValue);
                if (val != null && !complete_A.Exists(t => t == val.Value))
                {
                    complete_A.Add(val.Value);
                    currentPoint_A = val.Value;

                    tim_static_A = new System.Timers.Timer(500);
                    tim_static_A.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_A);
                    tim_static_A.Enabled = true;

                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                if (!complete_A.Exists(t => t == -1))
                {
                    complete_A.Add(-1);
                    currentPoint_A = -1;

                    tim_static_A = new System.Timers.Timer(500);
                    tim_static_A.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_A);
                    tim_static_A.Enabled = true;

                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZPmax || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                if (!complete_A.Exists(t => t == -2))
                {
                    complete_A.Add(-2);
                    currentPoint_A = -2;

                    tim_static_A = new System.Timers.Timer(500);
                    tim_static_A.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_A);
                    tim_static_A.Enabled = true;

                }
            }
        }

        private void staticTimer_A(object source, System.Timers.ElapsedEventArgs e)
        {
            isComplete_A = false;

            int maxIndex = 0;
            string common = "";
            if (!GetIsTimeStart(ref maxIndex, ref common))
            {
                return;
            }

            if (indexCollection_A < maxIndex)
            {
                var _displace1 = RegisterData.DisplaceA1;
                var _displace2 = RegisterData.DisplaceA2;
                var _displace3 = RegisterData.DisplaceA3;
                average_A.Add(new Tuple<double, double, double>(_displace1, _displace2, _displace3));
            }
            else
            {
                double ave1 = 0, ave2 = 0, ave3 = 0;
                foreach (var item in average_A)
                {
                    ave1 += item.Item1;
                    ave2 += item.Item2;
                    ave3 += item.Item3;
                }
                var pa = windPressureDGV_A.Find(t => t.PaValue == currentPoint_A);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
                {
                    pa.zwy1 = Math.Round(ave1 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy2 = Math.Round(ave2 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy3 = Math.Round(ave3 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart || windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
                {
                    pa.fwy1 = Math.Round(ave1 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy2 = Math.Round(ave2 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy3 = Math.Round(ave3 / average_A.Count, 2, MidpointRounding.AwayFromZero);
                }

                //清空初始化
                BindData_A();

                tim_static_A.Enabled = false;
                average_A = new List<Tuple<double, double, double>>();
                indexCollection_A = 0;
                isComplete_A = true;
            }
            indexCollection_A++;
        }

        private bool GetIsTimeStart(ref int maxIndex, ref string common)
        {
            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                common = BFMCommand.风压_正压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                common = BFMCommand.风压_负压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
            {
                common = BFMCommand.风压安全_正压是否计时;
                maxIndex = 4;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                common = BFMCommand.风压安全_负压是否计时;
                maxIndex = 4;
            }
            return _serialPortClient.Read_FY_Static_IsStart(common);
        }

        private void fyTimer_B(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!isComplete_B)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                var cyValue = RegisterData.CY_High_Value;
                var val = defKFYPa.Find(t => cyValue >= t.MinValue && cyValue <= t.MaxValue);
                if (val != null && !complete_B.Exists(t => t == val.Value))
                {
                    complete_B.Add(val.Value);
                    currentPoint_B = val.Value;

                    tim_static_B = new System.Timers.Timer(500);
                    tim_static_B.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_B);
                    tim_static_B.Enabled = true;
                }
            }
            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                var cyValue = RegisterData.CY_High_Value;
                var val = defKFYPa.Find(t => cyValue >= t._MinValue && cyValue <= t._MaxValue);
                if (val != null && !complete_B.Exists(t => t == val.Value))
                {
                    complete_B.Add(val.Value);
                    currentPoint_B = val.Value;

                    tim_static_B = new System.Timers.Timer(500);
                    tim_static_B.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_B);
                    tim_static_B.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                if (!complete_B.Exists(t => t == -1))
                {
                    complete_B.Add(-1);
                    currentPoint_B = -1;

                    tim_static_B = new System.Timers.Timer(500);
                    tim_static_B.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_B);
                    tim_static_B.Enabled = true;
                }
            }
            if (windPressureTest == PublicEnum.WindPressureTest.ZPmax || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                if (!complete_B.Exists(t => t == -2))
                {
                    complete_B.Add(-2);
                    currentPoint_B = -2;

                    tim_static_B = new System.Timers.Timer(500);
                    tim_static_B.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_B);
                    tim_static_B.Enabled = true;
                }
            }
        }

        private void staticTimer_B(object source, System.Timers.ElapsedEventArgs e)
        {
            isComplete_B = false;

            int maxIndex = 0;
            string common = "";
            if (!GetIsTimeStart(ref maxIndex, ref common))
            {
                return;
            }

            if (indexCollection_B < maxIndex)
            {
                var _displace1 = RegisterData.DisplaceB1;
                var _displace2 = RegisterData.DisplaceB2;
                var _displace3 = RegisterData.DisplaceB3;
                average_B.Add(new Tuple<double, double, double>(_displace1, _displace2, _displace3));
            }
            else
            {
                double ave1 = 0, ave2 = 0, ave3 = 0;
                foreach (var item in average_B)
                {
                    ave1 += item.Item1;
                    ave2 += item.Item2;
                    ave3 += item.Item3;
                }
                var pa = windPressureDGV_B.Find(t => t.PaValue == currentPoint_B);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
                {
                    pa.zwy1 = Math.Round(ave1 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy2 = Math.Round(ave2 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy3 = Math.Round(ave3 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart || windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
                {
                    pa.fwy1 = Math.Round(ave1 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy2 = Math.Round(ave2 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy3 = Math.Round(ave3 / average_B.Count, 2, MidpointRounding.AwayFromZero);
                }

                //清空初始化
                BindData_B();

                tim_static_B.Enabled = false;
                average_B = new List<Tuple<double, double, double>>();
                indexCollection_B = 0;
                isComplete_B = true;
            }
            indexCollection_B++;
        }

        private void fyTimer_C(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!isComplete_C)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                var cyValue = RegisterData.CY_High_Value;
                var val = defKFYPa.Find(t => cyValue >= t.MinValue && cyValue <= t.MaxValue);
                if (val != null && !complete_C.Exists(t => t == val.Value))
                {
                    complete_C.Add(val.Value);
                    currentPoint_C = val.Value;

                    tim_static_C = new System.Timers.Timer(500);
                    tim_static_C.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_C);
                    tim_static_C.Enabled = true;
                }
            }
            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                var cyValue = RegisterData.CY_High_Value;
                var val = defKFYPa.Find(t => cyValue >= t._MinValue && cyValue <= t._MaxValue);
                if (val != null && !complete_C.Exists(t => t == val.Value))
                {
                    complete_C.Add(val.Value);
                    currentPoint_C = val.Value;

                    tim_static_C = new System.Timers.Timer(500);
                    tim_static_C.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_C);
                    tim_static_C.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                if (!complete_C.Exists(t => t == -1))
                {
                    complete_C.Add(-1);
                    currentPoint_C = -1;

                    tim_static_C = new System.Timers.Timer(500);
                    tim_static_C.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_C);
                    tim_static_C.Enabled = true;
                }
            }
            if (windPressureTest == PublicEnum.WindPressureTest.ZPmax || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                if (!complete_C.Exists(t => t == -2))
                {
                    complete_C.Add(-2);
                    currentPoint_C = -2;

                    tim_static_C = new System.Timers.Timer(500);
                    tim_static_C.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer_C);
                    tim_static_C.Enabled = true;
                }
            }
        }

        private void staticTimer_C(object source, System.Timers.ElapsedEventArgs e)
        {
            isComplete_C = false;

            int maxIndex = 0;
            string common = "";
            if (!GetIsTimeStart(ref maxIndex, ref common))
            {
                return;
            }

            if (indexCollection_C < maxIndex)
            {
                var _displace1 = RegisterData.DisplaceC1;
                var _displace2 = RegisterData.DisplaceC2;
                var _displace3 = RegisterData.DisplaceC3;
                average_C.Add(new Tuple<double, double, double>(_displace1, _displace2, _displace3));
            }
            else
            {
                double ave1 = 0, ave2 = 0, ave3 = 0;
                foreach (var item in average_C)
                {
                    ave1 += item.Item1;
                    ave2 += item.Item2;
                    ave3 += item.Item3;
                }
                var pa = windPressureDGV_C.Find(t => t.PaValue == currentPoint_C);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
                {
                    pa.zwy1 = Math.Round(ave1 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy2 = Math.Round(ave2 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy3 = Math.Round(ave3 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart || windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
                {
                    pa.fwy1 = Math.Round(ave1 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy2 = Math.Round(ave2 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy3 = Math.Round(ave3 / average_C.Count, 2, MidpointRounding.AwayFromZero);
                }

                //清空初始化
                BindData_C();

                tim_static_C.Enabled = false;
                average_C = new List<Tuple<double, double, double>>();
                indexCollection_C = 0;
                isComplete_C = true;
            }
            indexCollection_C++;

        }

        private void tim_btnType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            if (windPressureTest == null)
                return;

            var IsSeccess = false;
            if (windPressureTest == PublicEnum.WindPressureTest.ZReady)
            {
                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.风压正压预备结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    OpenBtnType();

                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                double value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.风压正压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压正压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value >= 15)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    IsStartUp = false;
                    OpenBtnType();

                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FReady)
            {
                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.风压负压预备结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压预备结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    OpenBtnType();
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                double value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.风压负压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value >= 15)
                {
                    IsStartUp = false;
                    OpenBtnType();

                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZRepeatedly)
            {
                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.正反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value == 5)
                {
                    OpenBtnType();

                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FRepeatedly)
            {
                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.负反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value == 5)
                {
                    OpenBtnType();
                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
            {

                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.正安全结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    return;
                }
                if (value > 10)
                {
                    OpenBtnType();
                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                int value = _serialPortClient.ReadHoldingRegistersBtnType(BFMCommand.负安全结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value > 10)
                {
                    OpenBtnType();
                    tim_fy_A.Enabled = false;
                    tim_static_A.Enabled = false;

                    tim_fy_B.Enabled = false;
                    tim_static_B.Enabled = false;

                    tim_fy_C.Enabled = false;
                    tim_static_C.Enabled = false;
                }
            }
        }

        #region 私有方法
        /// <summary>
        /// 开启按钮
        /// </summary>
        private void OpenBtnType()
        {
            this.btn_zyyb.Enabled = true;
            this.btn_zyks.Enabled = true;
            this.btn_fyyb.Enabled = true;
            this.btn_fyks.Enabled = true;
            this.btn_zff.Enabled = true;
            this.btn_fff.Enabled = true;
            this.btn_zaq.Enabled = true;
            this.btnfaq.Enabled = true;
            this.btn_datahandle.Enabled = true;
            this.btn_zpmax.Enabled = true;
            this.btn_fpmax.Enabled = true;

            this.btn_zyyb.BackColor = Color.Transparent;
            this.btn_zyks.BackColor = Color.Transparent;
            this.btn_fyyb.BackColor = Color.Transparent;
            this.btn_fyks.BackColor = Color.Transparent;
            this.btn_zff.BackColor = Color.Transparent;
            this.btn_fff.BackColor = Color.Transparent;
            this.btn_zaq.BackColor = Color.Transparent;
            this.btnfaq.BackColor = Color.Transparent;
            this.btn_datahandle.BackColor = Color.Transparent;

            this.btn_zpmax.BackColor = Color.Transparent;
            this.btn_fpmax.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableBtnType()
        {
            this.btn_zyyb.Enabled = false;
            this.btn_zyks.Enabled = false;
            this.btn_fyyb.Enabled = false;
            this.btn_fyks.Enabled = false;
            this.btn_zff.Enabled = false;
            this.btn_fff.Enabled = false;
            this.btn_zaq.Enabled = false;
            this.btnfaq.Enabled = false;
            this.btn_datahandle.Enabled = false;

            this.btn_zpmax.Enabled = false;

            this.btn_fpmax.Enabled = false;

            this.btn_zyyb.BackColor = Color.Transparent;
            this.btn_zyks.BackColor = Color.Transparent;
            this.btn_fyyb.BackColor = Color.Transparent;
            this.btn_fyks.BackColor = Color.Transparent;

            this.btn_zff.BackColor = Color.Transparent;
            this.btn_fff.BackColor = Color.Transparent;
            this.btn_zaq.BackColor = Color.Transparent;
            this.btnfaq.BackColor = Color.Transparent;
            this.btn_datahandle.BackColor = Color.Transparent;

            this.btn_zpmax.BackColor = Color.Transparent;

            this.btn_fpmax.BackColor = Color.Transparent;

        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.SendSingleCoilControl(BFMCommand.急停);
            if (!res) { }
        }
        #endregion

        #region 控件操作方法

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.tChart_qm.Export.ShowExportDialog();
        }

        private void tChart_qm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.char_cms_click.Show(MousePosition.X, MousePosition.Y);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                              this.panel1.ClientRectangle,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                              this.panel2.ClientRectangle,
                             Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid,
                              Color.Black, 1,
                              ButtonBorderStyle.Solid);
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel2.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel5.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel4.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel6.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel9.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel8.ClientRectangle,
                           Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid,
                            Color.Black, 1,
                            ButtonBorderStyle.Solid);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                               this.panel7.ClientRectangle,
                              Color.Black, 1,
                               ButtonBorderStyle.Solid,
                               Color.Black, 1,
                               ButtonBorderStyle.Solid,
                               Color.Black, 1,
                               ButtonBorderStyle.Solid,
                               Color.Black, 1,
                               ButtonBorderStyle.Solid);

        }
        private void dgv_WindPressure_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //获得当前选中的行   
            int rowindex = e.RowIndex;
            if (windPressureDGV_A != null && windPressureDGV_A.Count() > 0)
            {
                var name = dgv_WindPressure_A.Rows[rowindex].Cells[0].Value.ToString();

                var z_wy1 = dgv_WindPressure_A.Rows[rowindex].Cells[1].Value.ToString();
                var z_wy2 = dgv_WindPressure_A.Rows[rowindex].Cells[2].Value.ToString();
                var z_wy3 = dgv_WindPressure_A.Rows[rowindex].Cells[3].Value.ToString();

                var f_wy1 = dgv_WindPressure_A.Rows[rowindex].Cells[6].Value.ToString();
                var f_wy2 = dgv_WindPressure_A.Rows[rowindex].Cells[7].Value.ToString();
                var f_wy3 = dgv_WindPressure_A.Rows[rowindex].Cells[8].Value.ToString();

                var item = windPressureDGV_A.Find(t => t.Pa == name);
                item.zwy1 = double.Parse(z_wy1);
                item.zwy2 = double.Parse(z_wy2);
                item.zwy3 = double.Parse(z_wy3);
                item.fwy1 = double.Parse(f_wy1);
                item.fwy2 = double.Parse(f_wy2);
                item.fwy3 = double.Parse(f_wy3);
                BindData_A();
            }
        }

        private void dgv_WindPressure_B_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
            if (windPressureDGV_B != null && windPressureDGV_B.Count() > 0)
            {
                var name = dgv_WindPressure_B.Rows[rowindex].Cells[0].Value.ToString();
                var z_wy1 = dgv_WindPressure_B.Rows[rowindex].Cells[1].Value.ToString();
                var z_wy2 = dgv_WindPressure_B.Rows[rowindex].Cells[2].Value.ToString();
                var z_wy3 = dgv_WindPressure_B.Rows[rowindex].Cells[3].Value.ToString();
                var f_wy1 = dgv_WindPressure_B.Rows[rowindex].Cells[6].Value.ToString();
                var f_wy2 = dgv_WindPressure_B.Rows[rowindex].Cells[7].Value.ToString();
                var f_wy3 = dgv_WindPressure_B.Rows[rowindex].Cells[8].Value.ToString();

                var item = windPressureDGV_B.Find(t => t.Pa == name);
                item.zwy1 = double.Parse(z_wy1);
                item.zwy2 = double.Parse(z_wy2);
                item.zwy3 = double.Parse(z_wy3);
                item.fwy1 = double.Parse(f_wy1);
                item.fwy2 = double.Parse(f_wy2);
                item.fwy3 = double.Parse(f_wy3);
                BindData_B();
            }
        }

        private void dgv_WindPressure_C_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
            if (windPressureDGV_C != null && windPressureDGV_C.Count() > 0)
            {
                var name = dgv_WindPressure_C.Rows[rowindex].Cells[0].Value.ToString();
                var z_wy1 = dgv_WindPressure_C.Rows[rowindex].Cells[1].Value.ToString();
                var z_wy2 = dgv_WindPressure_C.Rows[rowindex].Cells[2].Value.ToString();
                var z_wy3 = dgv_WindPressure_C.Rows[rowindex].Cells[3].Value.ToString();
                var f_wy1 = dgv_WindPressure_C.Rows[rowindex].Cells[6].Value.ToString();
                var f_wy2 = dgv_WindPressure_C.Rows[rowindex].Cells[7].Value.ToString();
                var f_wy3 = dgv_WindPressure_C.Rows[rowindex].Cells[8].Value.ToString();

                var item = windPressureDGV_C.Find(t => t.Pa == name);
                item.zwy1 = double.Parse(z_wy1);
                item.zwy2 = double.Parse(z_wy2);
                item.zwy3 = double.Parse(z_wy3);
                item.fwy1 = double.Parse(f_wy1);
                item.fwy2 = double.Parse(f_wy2);
                item.fwy3 = double.Parse(f_wy3);

                BindData_C();
            }
        }

        private void dgv_WindPressure_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        }
        #endregion

        #region  添加数据库
        private bool Add(int defJC)
        {
            var kfy = AddKfy();

            var resRes = AddKfyRes(defJC);

            dalDtInfo.UpdateTestType(_tempCode, PublicEnum.SystemItem.AirPressure, 1);
            return true;
        }

        private bool AddKfy()
        {
            List<Model_dt_kfy_Info> list = new List<Model_dt_kfy_Info>();

            if (selectTestType.Contains("A"))
            {
                Model_dt_kfy_Info model = new Model_dt_kfy_Info();
                for (int i = 0; i < 10; i++)
                {
                    model.level = "A";
                    model.dt_Code = _tempCode;
                    #region 获取 A
                    if (i == 0)
                    {
                        model.z_one_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 1)
                    {
                        model.z_one_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 2)
                    {
                        model.z_one_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 3)
                    {
                        model.z_one_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 4)
                    {
                        model.z_one_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1250 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 5)
                    {
                        model.z_one_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1500 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 6)
                    {
                        model.z_one_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1750 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 7)
                    {
                        model.z_one_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_2000 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 8)
                    {
                        model.z_one_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");


                        model.f_one_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3 = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 9)
                    {
                        model.z_one_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3max = double.Parse(this.dgv_WindPressure_A.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }

                    #endregion
                }
                list.Add(model);
            }
            if (selectTestType.Contains("B"))
            {
                Model_dt_kfy_Info model = new Model_dt_kfy_Info();
                for (int i = 0; i < 10; i++)
                {
                    model.dt_Code = _tempCode;
                    model.level = "B";
                    #region 获取 b
                    if (i == 0)
                    {
                        model.z_one_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 1)
                    {
                        model.z_one_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 2)
                    {
                        model.z_one_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 3)
                    {
                        model.z_one_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 4)
                    {
                        model.z_one_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1250 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 5)
                    {
                        model.z_one_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1500 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 6)
                    {
                        model.z_one_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1750 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 7)
                    {
                        model.z_one_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_2000 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 8)
                    {
                        model.z_one_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");

                        model.f_one_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3 = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 9)
                    {
                        model.z_one_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3max = double.Parse(this.dgv_WindPressure_B.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }

                    #endregion
                }
                list.Add(model);
            }
            //C
            if (selectTestType.Contains("C"))
            {
                Model_dt_kfy_Info model = new Model_dt_kfy_Info();
                for (int i = 0; i < 10; i++)
                {
                    model.dt_Code = _tempCode;
                    model.level = "C";

                    #region 获取 C
                    if (i == 0)
                    {
                        model.z_one_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 1)
                    {
                        model.z_one_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 2)
                    {
                        model.z_one_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 3)
                    {
                        model.z_one_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 4)
                    {
                        model.z_one_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1250 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 5)
                    {
                        model.z_one_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1500 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 6)
                    {
                        model.z_one_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_1750 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 7)
                    {
                        model.z_one_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_2000 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 8)
                    {
                        model.z_one_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");

                        model.f_one_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3 = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }
                    if (i == 9)
                    {
                        model.z_one_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                        model.z_two_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                        model.z_three_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                        model.z_nd_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                        model.z_ix_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                        model.f_one_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                        model.f_two_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                        model.f_three_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                        model.f_nd_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                        model.f_ix_p3max = double.Parse(this.dgv_WindPressure_C.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                    }

                    #endregion
                }
                list.Add(model);
            }
            return dalKfyInfo.Add_kfy_Info(list, _tempCode);
        }

        public bool AddKfyRes(int defJC)
        {
            Model_dt_kfy_res_Info model = new Model_dt_kfy_res_Info();
            model.dt_Code = _tempCode;
            model.defJC = defJC;
            model.p1 = txt_p1.Text;
            model.p2 = txt_p2.Text;
            model.p3 = txt_p3.Text;
            model._p1 = txt_f_p1.Text;
            model._p2 = txt_f_p2.Text;
            model._p3 = txt_f_p3.Text;
            model.testtype = IsGCJC == true ? 2 : 1;
            model.pMax = txt_zpmax.Text;
            model._pMax = txt_fpmax.Text;
            model.desc = txt_desc.Text;
            model.lx_a = txt_lx_a.Text;
            model.lx_b = txt_lx_b.Text;
            model.lx_c = txt_lx_c.Text;
            return dalKfyResInfo.Add_kfy_res_Info(model);
        }

        #endregion

        private void WindPressureDetection_FormClosing(object sender, FormClosingEventArgs e)
        {
            timRead.Dispose();
        }

    }
}

public class DefKFYPa
{

    public int Value { get; set; }

    public int MinValue
    {
        get
        {
            return this.Value - 10;
        }
    }

    public int MaxValue
    {
        get
        {
            return this.Value + 10;
        }
    }

    public int _MinValue
    {
        get
        {
            return -this.Value - 10;
        }
    }

    public int _MaxValue
    {
        get
        {
            return -this.Value + 10;
        }
    }
}