using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Model.DataBase;
using text.doors.Default;

namespace text.doors.Detection
{
    public partial class DetectionSet : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        //当前温度
        public double _temperature { set; get; }
        //当前压力
        public double _temppressure { set; get; }

        private string _tempCode = "";
        private SerialPortClient _serialPortClient;

        public DetectionSet() { }
        public DetectionSet(SerialPortClient serialPortClient, double temperature, double temppressure, string tempCode)
        {
            this._serialPortClient = serialPortClient;
            this._temperature = temperature;
            this._temppressure = temppressure;
            this._tempCode = tempCode;
            InitializeComponent();
            Init();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string code = txt_JianYanBianHao.Text;

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("请设置当前编号！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DAL_dt_Settings dal = new DAL_dt_Settings();
            try
            {
                var setting = GetSettings();
                if (dal.Add(setting))
                {
                    //获取樘号
                    deleBottomTypeEvent(GetBottomType(true));
                    DefaultBase.base_TestItem = cb_JianCeXiangMu.Text;

                    this.btn_add.Enabled = true;
                    this.btn_select.Enabled = true;
                    this.btn_delete.Enabled = true;
                    this.btn_Ok.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据异常！", "异常", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Error(ex);
            }
        }

        public void GetDangHaoTrigger()
        {
            //获取樘号
            deleBottomTypeEvent(GetBottomType(false));
        }

        private void Init()
        {
            BindInfoText();
        }
        private void SelectDangHao(object sender, text.doors.Detection.Select_Code.TransmitEventArgs e)
        {
            _tempCode = e.Code;
            Init();
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            Select_Code sc = new Select_Code();
            sc.Transmit += new Select_Code.TransmitHandler(SelectDangHao);
            sc.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {

            MessageBox.Show("您是否删除", " 永久性删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);

            if (new DAL_dt_Info().delete_dt_Info(txt_JianYanBianHao.Text))
            {
                _tempCode = "";
                Init();
            }
        }


        #region Info
        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindInfoText()
        {
            try
            {
                DataTable dt = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);
                if (dt != null)
                {
                    this.txt_weituodianhua.Text = dt.Rows[0]["weituodianhua"].ToString();
                    this.txt_songyangriqi.Text = dt.Rows[0]["songyangriqi"].ToString();
                    this.txt_naihoujiao.Text = dt.Rows[0]["naihoujiao"].ToString();
                    this.txt_litingxilie.Text = dt.Rows[0]["litingxilie"].ToString();
                    this.txt_yangpinbianhao.Text = dt.Rows[0]["yangpinbianhao"].ToString();
                    this.txt_mianbancaizhi.Text = dt.Rows[0]["mianbancaizhi"].ToString();
                    this.txt_mianbanxiangqianfangshi.Text = dt.Rows[0]["mianbanxiangqianfangshi"].ToString();
                    this.txt_mianbanxiangqiancailiao.Text = dt.Rows[0]["mianbanxiangqiancailiao"].ToString();
                    this.txt_kuangshanmifengcailiao.Text = dt.Rows[0]["kuangshanmifengcailiao"].ToString();
                    this.txt_kekaibishijianmianji.Text = dt.Rows[0]["kekaibishijianmianji"].ToString();
                    this.txt_caiyangfangshi.Text = dt.Rows[0]["caiyangfangshi"].ToString();
                    this.txt_weituoren.Text = dt.Rows[0]["weituoren"].ToString();
                    this.txt_zuidamianban.Text = dt.Rows[0]["zuidamianban"].ToString();
                    this.txt_jianlidanwei.Text = dt.Rows[0]["jianlidanwei"].ToString();
                    this.txt_jianshedanwei.Text = dt.Rows[0]["jianshedanwei"].ToString();
                    this.txt_shejidanwei.Text = dt.Rows[0]["shejidanwei"].ToString();
                    this.txt_ganCchang.Text = dt.Rows[0]["ganCchang"].ToString();
                    this.txt_ganBchang.Text = dt.Rows[0]["ganBchang"].ToString();
                    this.txt_ganAchang.Text = dt.Rows[0]["ganAchang"].ToString();
                    this.txt_kekaimianji.Text = dt.Rows[0]["kekaimianji"].ToString();
                    this.cb_KaiQiFangShi.Text = dt.Rows[0]["KaiQiFangShi"].ToString();
                    this.txt_kekaifengchang.Text = dt.Rows[0]["kekaifengchang"].ToString();
                    this.txt_jiegoujiao.Text = dt.Rows[0]["jiegoujiao"].ToString();
                    this.txt_shijiancenggao.Text = dt.Rows[0]["shijiancenggao"].ToString();
                    this.txt_gudingmianji.Text = dt.Rows[0]["gudingmianji"].ToString();
                    this.txt_gudingfengchang.Text = dt.Rows[0]["gudingfengchang"].ToString();
                    this.txt_shijiangaodu.Text = dt.Rows[0]["shijiangaodu"].ToString();
                    this.txt_gongchengmingcheng.Text = dt.Rows[0]["gongchengmingcheng"].ToString();
                    this.txt_shijiankuandu.Text = dt.Rows[0]["shijiankuandu"].ToString();
                    this.txt_GuiGeXingHao.Text = dt.Rows[0]["GuiGeXingHao"].ToString();
                    this.txt_YangPinMingCheng.Text = dt.Rows[0]["YangPinMingCheng"].ToString();
                    this.cb_jianceyiju.Text = dt.Rows[0]["jianceyiju"].ToString();
                    this.txt_shijianmianji.Text = dt.Rows[0]["shijianmianji"].ToString();
                    this.txt_DaQiYaLi.Text = dt.Rows[0]["DaQiYaLi"].ToString();
                    this.txt_DangQianWenDu.Text = dt.Rows[0]["DangQianWenDu"].ToString();
                    this.txt_shengchandanwei.Text = dt.Rows[0]["shengchandanwei"].ToString();
                    this.txt_WeiTuoBianHao.Text = dt.Rows[0]["WeiTuoBianHao"].ToString();
                    this.txt_baogaoriqi.Text = dt.Rows[0]["baogaoriqi"].ToString();
                    this.cb_JianCeXiangMu.Text = dt.Rows[0]["JianCeXiangMu"].ToString();
                    this.txt_ganjianABC.Text = dt.Rows[0]["ganjianABC"].ToString();
                    this.txt_JianCeRiQi.Text = dt.Rows[0]["JianCeRiQi"].ToString();
                    this.txt_WeiTuoDanWei.Text = dt.Rows[0]["WeiTuoDanWei"].ToString();

                    _tempCode = dt.Rows[0]["dt_Code"].ToString();

                    txt_JianYanBianHao.Text = dt.Rows[0]["dt_Code"].ToString();
                    this.cb_JianCeXiangMu.Enabled = false;
                }
                else
                {
                    txt_JianYanBianHao.Text = DateTime.Now.ToString("yyyyMMdd") + "-01";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Error(ex);
            }
        }


        private Model_dt_Settings GetSettings()
        {
            Model_dt_Settings model = new Model_dt_Settings();
            model.dt_Create = DateTime.Now;
            model.dt_Code = this.txt_JianYanBianHao.Text;

            model.weituodianhua = this.txt_weituodianhua.Text;
            model.songyangriqi = this.txt_songyangriqi.Text;
            model.naihoujiao = this.txt_naihoujiao.Text;
            model.litingxilie = this.txt_litingxilie.Text;
            model.yangpinbianhao = this.txt_yangpinbianhao.Text;
            model.mianbancaizhi = this.txt_mianbancaizhi.Text;
            model.mianbanxiangqianfangshi = this.txt_mianbanxiangqianfangshi.Text;
            model.mianbanxiangqiancailiao = this.txt_mianbanxiangqiancailiao.Text;
            model.kuangshanmifengcailiao = this.txt_kuangshanmifengcailiao.Text;
            model.kekaibishijianmianji = this.txt_kekaibishijianmianji.Text;
            model.caiyangfangshi = this.txt_caiyangfangshi.Text;
            model.weituoren = this.txt_weituoren.Text;
            model.zuidamianban = this.txt_zuidamianban.Text;
            model.jianlidanwei = this.txt_jianlidanwei.Text;
            model.jianshedanwei = this.txt_jianshedanwei.Text;
            model.shejidanwei = this.txt_shejidanwei.Text;
            model.ganCchang = this.txt_ganCchang.Text;
            model.ganBchang = this.txt_ganBchang.Text;
            model.ganAchang = this.txt_ganAchang.Text;
            model.kekaimianji = this.txt_kekaimianji.Text;
            model.KaiQiFangShi = this.cb_KaiQiFangShi.Text;
            model.kekaifengchang = this.txt_kekaifengchang.Text;
            model.jiegoujiao = this.txt_jiegoujiao.Text;
            model.shijiancenggao = this.txt_shijiancenggao.Text;
            model.gudingmianji = this.txt_gudingmianji.Text;
            model.gudingfengchang = this.txt_gudingfengchang.Text;
            model.shijiangaodu = this.txt_shijiangaodu.Text;
            model.gongchengmingcheng = this.txt_gongchengmingcheng.Text;
            model.shijiankuandu = this.txt_shijiankuandu.Text;
            model.GuiGeXingHao = this.txt_GuiGeXingHao.Text;
            model.YangPinMingCheng = this.txt_YangPinMingCheng.Text;
            model.jianceyiju = this.cb_jianceyiju.Text;
            model.shijianmianji = this.txt_shijianmianji.Text;
            model.DaQiYaLi = this.txt_DaQiYaLi.Text;
            model.DangQianWenDu = this.txt_DangQianWenDu.Text;
            model.shengchandanwei = this.txt_shengchandanwei.Text;
            model.WeiTuoBianHao = this.txt_WeiTuoBianHao.Text;
            model.baogaoriqi = this.txt_baogaoriqi.Text;
            model.JianCeXiangMu = this.cb_JianCeXiangMu.Text;
            model.ganjianABC = this.txt_ganjianABC.Text;
            model.JianCeRiQi = this.txt_JianCeRiQi.Text;
            model.WeiTuoDanWei = this.txt_WeiTuoDanWei.Text;
            return model;
        }

        #endregion


        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_JianYanBianHao.Text))
            {
                MessageBox.Show("请输入编号！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var arr = txt_JianYanBianHao.Text.Split('-');
            if (arr.Length == 1)
            {
                MessageBox.Show("编号格式有误，请输入如d-1格式！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txt_JianYanBianHao.Text = arr[0] + "-" + (int.Parse(arr[1]) + 1).ToString();
            this.cb_JianCeXiangMu.Enabled = true;
            this.btn_add.Enabled = false;
            this.btn_select.Enabled = false;
            this.btn_delete.Enabled = false;
            this.btn_Ok.Enabled = true;

            txt_DaQiYaLi.Text = RegisterData.AtmospherePa_Value.ToString(); //_serialPortClient.GetDQYLXS().ToString();

            txt_DangQianWenDu.Text = RegisterData.Temperature_Value.ToString();//_serialPortClient.GetWDXS().ToString();
        }



        #region 底部状态栏赋值

        private BottomType GetBottomType(bool ISOK)
        {
            BottomType bt = new BottomType(txt_JianYanBianHao.Text, ISOK);
            return bt;
        }

        //委托
        public delegate void deleBottomType(BottomType bottomType);
        public deleBottomType deleBottomTypeEvent;//委托事件

        /// <summary>
        /// 底部状态栏
        /// </summary>
        public class BottomType
        {
            private string _code;
            private bool _isok;

            public BottomType(string code, bool isOK)
            {
                this._code = code;
                this._isok = isOK;

            }
            public string Code { get { return _code; } }
            public bool ISOK { get { return _isok; } }

        }

        #endregion

        AutoSizeFormClass asc = new AutoSizeFormClass();
        private void DetectionSet_Load(object sender, EventArgs e)
        {
            //asc.controllInitializeSize(this);
        }

        private void DetectionSet_SizeChanged(object sender, EventArgs e)
        {
            //asc.controlAutoSize(this);
            //this.WindowState = (System.Windows.Forms.FormWindowState)(2);//记录完控件的初始位置和大小后，再最大化

        }
    }
}
