using Microsoft.Office.Interop.Word;
using text.doors.Common;
using text.doors.dal;
using text.doors.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Young.Core.Common;
using text.doors.Default;
using System.Linq;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using text.doors.Service;
using text.doors.Model;

namespace text.doors.Detection
{
    public partial class ExportReport : Form
    {
        private string _tempCode = "";
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();

        Formula formula = new Formula();


        public ExportReport(string code)
        {
            InitializeComponent();
            this._tempCode = code;
            cm_Report.SelectedIndex = 0;



        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Eexport(cm_Report.SelectedItem.ToString());
        }


        private void Eexport(string fileName)
        {
            try
            {
                string strResult = string.Empty;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\template";
                string strFile = string.Format(@"{0}\{1}", strPath, fileName);

                FolderBrowserDialog path = new FolderBrowserDialog();
                path.ShowDialog();

                lbl_message.Visible = true;
                if (string.IsNullOrWhiteSpace(path.SelectedPath))
                {
                    return;
                }
                btn_ok.Enabled = false;
                cm_Report.Enabled = false;
                btn_close.Enabled = false;

                string[] name = fileName.Split('.');

                string _name = name[0] + "_" + _tempCode + "." + name[1];

                var saveExcelUrl = path.SelectedPath + "\\" + _name;

                Model_dt_Settings settings = new DAL_dt_Settings().GetInfoByCode(_tempCode);

                if (settings == null)
                {
                    MessageBox.Show("未查询到相关编号!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dc = new Dictionary<string, string>();
                if (fileName == "建筑幕墙抗风压性能检测记录表.doc")
                {
                    dc = GetDWDetectionReport(settings);
                }

                WordUtility wu = new WordUtility(strFile, saveExcelUrl);
                if (wu.GenerateWordByBookmarks(dc))
                {
                    DrawLine(saveExcelUrl, settings.dt_Code, settings.dt_kfy_Info);
                    lbl_message.Visible = false;
                    this.Hide();
                    MessageBox.Show("导出成功", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.None);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("数据出现问题，导出失败!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }


        /// <summary>
        /// 获取门窗检测报告文档
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetDWDetectionReport(Model_dt_Settings settings)
        {
            Dictionary<string, string> dc = new Dictionary<string, string>();


            dc.Add("样品名称重复1", settings.YangPinMingCheng);
            dc.Add("样品名称重复2", settings.YangPinMingCheng);
            dc.Add("样品名称重复3", settings.YangPinMingCheng);
            dc.Add("样品名称重复4", settings.YangPinMingCheng);
            dc.Add("样品名称重复5", settings.YangPinMingCheng);
            dc.Add("样品名称重复6", settings.YangPinMingCheng);
            dc.Add("检测地址1", "");
            dc.Add("检测地址2", "");
            dc.Add("检测地址3", "");
            dc.Add("检测地址4", "");
            dc.Add("检测地址5", "");
            dc.Add("检测地址6", "");
            dc.Add("检测地址7", "");

            dc.Add("检验编号重复1", settings.dt_Code + "             ");
            dc.Add("检验编号重复2", settings.dt_Code + "             ");
            dc.Add("检验编号重复3", settings.dt_Code + "             ");
            dc.Add("检验编号重复4", settings.dt_Code + "             ");
            dc.Add("检验编号重复5", settings.dt_Code + "             ");
            dc.Add("检验编号重复6", settings.dt_Code + "             ");
            dc.Add("检验编号重复7", settings.dt_Code + "             ");
            dc.Add("检验设备重复1", "");
            dc.Add("检验设备重复2", "");
            dc.Add("检验设备重复3", "");
            dc.Add("检验设备重复4", "");
            dc.Add("检验设备重复5", "");
            dc.Add("检验设备重复6", "");
            dc.Add("检验设备重复7", "");
            dc.Add("规格型号重复1", settings.GuiGeXingHao);
            dc.Add("规格型号重复2", settings.GuiGeXingHao);
            dc.Add("规格型号重复3", settings.GuiGeXingHao);
            dc.Add("规格型号重复4", settings.GuiGeXingHao);
            dc.Add("规格型号重复5", settings.GuiGeXingHao);
            dc.Add("规格型号重复6", settings.GuiGeXingHao);
            dc.Add("大气压力", settings.DaQiYaLi);
            dc.Add("环境温度", settings.DangQianWenDu);
            dc.Add("试件层高", settings.shijiancenggao);
            dc.Add("试件面级", settings.shijianmianji + "mm");
            dc.Add("可开逢长", settings.kekaifengchang);

            dc.Add("杆件长度A", settings.ganAchang);
            dc.Add("杆件长度B", settings.ganBchang);
            dc.Add("杆件长度C", settings.ganCchang);


            if (settings.dt_kfy_Info != null && settings.dt_kfy_Info.Count > 0)
            {
                var kfyInfo = settings.dt_kfy_Info.OrderBy(t => t.level);
                foreach (var item in settings.dt_kfy_Info)
                {
                    if (item.level == "A")
                    {
                        dc.Add("A_Z250A1", item.z_one_250 == "0.00" ? "--" : item.z_one_250);
                        dc.Add("A_Z250A2", item.z_two_250 == "0.00" ? "--" : item.z_two_250);
                        dc.Add("A_Z250A3", item.z_three_250 == "0.00" ? "--" : item.z_three_250);
                        dc.Add("A_Z250ND", item.z_nd_250 == "0.00" ? "--" : item.z_nd_250);

                        dc.Add("A_Z500A1", item.z_one_500 == "0.00" ? "--" : item.z_one_500);
                        dc.Add("A_Z500A2", item.z_two_500 == "0.00" ? "--" : item.z_two_250);
                        dc.Add("A_Z500A3", item.z_three_500 == "0.00" ? "--" : item.z_three_250);
                        dc.Add("A_Z500ND", item.z_nd_500 == "0.00" ? "--" : item.z_nd_500);

                        dc.Add("A_Z750A1", item.z_one_750 == "0.00" ? "--" : item.z_one_750);
                        dc.Add("A_Z750A2", item.z_two_750 == "0.00" ? "--" : item.z_two_750);
                        dc.Add("A_Z750A3", item.z_three_750 == "0.00" ? "--" : item.z_three_750);
                        dc.Add("A_Z750ND", item.z_nd_750 == "0.00" ? "--" : item.z_nd_750);

                        dc.Add("A_Z1000A1", item.z_one_1000 == "0.00" ? "--" : item.z_one_1000);
                        dc.Add("A_Z1000A2", item.z_two_1000 == "0.00" ? "--" : item.z_two_1000);
                        dc.Add("A_Z1000A3", item.z_three_1000 == "0.00" ? "--" : item.z_three_1000);
                        dc.Add("A_Z1000ND", item.z_nd_1000 == "0.00" ? "--" : item.z_nd_1000);

                        dc.Add("A_Z1250A1", item.z_one_1250 == "0.00" ? "--" : item.z_one_1250);
                        dc.Add("A_Z1250A2", item.z_two_1250 == "0.00" ? "--" : item.z_two_1250);
                        dc.Add("A_Z1250A3", item.z_three_1250 == "0.00" ? "--" : item.z_three_1250);
                        dc.Add("A_Z1250ND", item.z_nd_1250 == "0.00" ? "--" : item.z_nd_1250);

                        dc.Add("A_Z1500A1", item.z_one_1500 == "0.00" ? "--" : item.z_one_1500);
                        dc.Add("A_Z1500A2", item.z_two_1500 == "0.00" ? "--" : item.z_two_1500);
                        dc.Add("A_Z1500A3", item.z_three_1500 == "0.00" ? "--" : item.z_three_1500);
                        dc.Add("A_Z1500ND", item.z_nd_1500 == "0.00" ? "--" : item.z_nd_1500);

                        dc.Add("A_Z1750A1", item.z_one_1750 == "0.00" ? "--" : item.z_one_1750);
                        dc.Add("A_Z1750A2", item.z_two_1750 == "0.00" ? "--" : item.z_two_1750);
                        dc.Add("A_Z1750A3", item.z_three_1750 == "0.00" ? "--" : item.z_three_1750);
                        dc.Add("A_Z1750ND", item.z_nd_1750 == "0.00" ? "--" : item.z_nd_1750);

                        dc.Add("A_Z2000A1", item.z_one_2000 == "0.00" ? "--" : item.z_one_2000);
                        dc.Add("A_Z2000A2", item.z_two_2000 == "0.00" ? "--" : item.z_two_2000);
                        dc.Add("A_Z2000A3", item.z_three_2000 == "0.00" ? "--" : item.z_three_2000);
                        dc.Add("A_Z2000ND", item.z_nd_2000 == "0.00" ? "--" : item.z_nd_2000);

                        dc.Add("A_F250A1", item.f_one_250 == "0.00" ? "--" : item.f_one_250);
                        dc.Add("A_F250A2", item.f_two_250 == "0.00" ? "--" : item.f_two_250);
                        dc.Add("A_F250A3", item.f_three_250 == "0.00" ? "--" : item.f_three_250);
                        dc.Add("A_F250ND", item.f_nd_250 == "0.00" ? "--" : item.f_nd_250);

                        dc.Add("A_F500A1", item.f_one_500 == "0.00" ? "--" : item.f_one_500);
                        dc.Add("A_F500A2", item.f_two_500 == "0.00" ? "--" : item.f_two_500);
                        dc.Add("A_F500A3", item.f_three_500 == "0.00" ? "--" : item.f_three_500);
                        dc.Add("A_F500ND", item.f_nd_500 == "0.00" ? "--" : item.f_nd_500);

                        dc.Add("A_F750A1", item.f_one_750 == "0.00" ? "--" : item.f_one_750);
                        dc.Add("A_F750A2", item.f_two_750 == "0.00" ? "--" : item.f_two_750);
                        dc.Add("A_F750A3", item.f_three_750 == "0.00" ? "--" : item.f_three_750);
                        dc.Add("A_F750ND", item.f_nd_750 == "0.00" ? "--" : item.f_nd_750);

                        dc.Add("A_F1000A1", item.f_one_1000 == "0.00" ? "--" : item.f_one_1000);
                        dc.Add("A_F1000A2", item.f_two_1000 == "0.00" ? "--" : item.f_two_1000);
                        dc.Add("A_F1000A3", item.f_three_1000 == "0.00" ? "--" : item.f_three_1000);
                        dc.Add("A_F1000ND", item.f_nd_1000 == "0.00" ? "--" : item.f_nd_1000);

                        dc.Add("A_F1250A1", item.f_one_1250 == "0.00" ? "--" : item.f_one_1250);
                        dc.Add("A_F1250A2", item.f_two_1250 == "0.00" ? "--" : item.f_two_1250);
                        dc.Add("A_F1250A3", item.f_three_1250 == "0.00" ? "--" : item.f_three_1250);
                        dc.Add("A_F1250ND", item.f_nd_1250 == "0.00" ? "--" : item.f_nd_1250);

                        dc.Add("A_F1500A1", item.f_one_1500 == "0.00" ? "--" : item.f_one_1500);
                        dc.Add("A_F1500A2", item.f_two_1500 == "0.00" ? "--" : item.f_two_1500);
                        dc.Add("A_F1500A3", item.f_three_1500 == "0.00" ? "--" : item.f_three_1500);
                        dc.Add("A_F1500ND", item.f_nd_1500 == "0.00" ? "--" : item.f_nd_1500);

                        dc.Add("A_F1750A1", item.f_one_1750 == "0.00" ? "--" : item.f_one_1750);
                        dc.Add("A_F1750A2", item.f_two_1750 == "0.00" ? "--" : item.f_two_1750);
                        dc.Add("A_F1750A3", item.f_three_1750 == "0.00" ? "--" : item.f_three_1750);
                        dc.Add("A_F1750ND", item.f_nd_1750 == "0.00" ? "--" : item.f_nd_1750);

                        dc.Add("A_F2000A1", item.f_one_2000 == "0.00" ? "--" : item.f_one_2000);
                        dc.Add("A_F2000A2", item.f_two_2000 == "0.00" ? "--" : item.f_two_2000);
                        dc.Add("A_F2000A3", item.f_three_2000 == "0.00" ? "--" : item.f_three_2000);
                        dc.Add("A_F2000ND", item.f_nd_2000 == "0.00" ? "--" : item.f_nd_2000);

                        dc.Add("Ap3A1", item.z_one_p3 == "0.00" ? "--" : item.z_one_p3);
                        dc.Add("Ap3A2", item.z_two_p3 == "0.00" ? "--" : item.z_two_p3);
                        dc.Add("Ap3A3", item.z_three_p3 == "0.00" ? "--" : item.z_three_p3);
                        dc.Add("Ap3ND", item.z_nd_p3 == "0.00" ? "--" : item.z_nd_p3);

                        dc.Add("A_p3A1", item.f_one_p3 == "0.00" ? "--" : item.f_one_p3);
                        dc.Add("A_p3A2", item.f_two_p3 == "0.00" ? "--" : item.f_two_p3);
                        dc.Add("A_p3A3", item.f_three_p3 == "0.00" ? "--" : item.f_three_p3);
                        dc.Add("A_p3ND", item.f_nd_p3 == "0.00" ? "--" : item.f_nd_p3);
                    }
                    else if (item.level == "B")
                    {
                        dc.Add("B_Z250B1", item.z_one_250 == "0.00" ? "--" : item.z_one_250);
                        dc.Add("B_Z250B2", item.z_two_250 == "0.00" ? "--" : item.z_two_250);
                        dc.Add("B_Z250B3", item.z_three_250 == "0.00" ? "--" : item.z_three_250);
                        dc.Add("B_Z250ND", item.z_nd_250 == "0.00" ? "--" : item.z_nd_250);

                        dc.Add("B_Z500B1", item.z_one_500 == "0.00" ? "--" : item.z_one_500);
                        dc.Add("B_Z500B2", item.z_two_500 == "0.00" ? "--" : item.z_two_250);
                        dc.Add("B_Z500B3", item.z_three_500 == "0.00" ? "--" : item.z_three_250);
                        dc.Add("B_Z500ND", item.z_nd_500 == "0.00" ? "--" : item.z_nd_500);

                        dc.Add("B_Z750B1", item.z_one_750 == "0.00" ? "--" : item.z_one_750);
                        dc.Add("B_Z750B2", item.z_two_750 == "0.00" ? "--" : item.z_two_750);
                        dc.Add("B_Z750B3", item.z_three_750 == "0.00" ? "--" : item.z_three_750);
                        dc.Add("B_Z750ND", item.z_nd_750 == "0.00" ? "--" : item.z_nd_750);

                        dc.Add("B_Z1000B1", item.z_one_1000 == "0.00" ? "--" : item.z_one_1000);
                        dc.Add("B_Z1000B2", item.z_two_1000 == "0.00" ? "--" : item.z_two_1000);
                        dc.Add("B_Z1000B3", item.z_three_1000 == "0.00" ? "--" : item.z_three_1000);
                        dc.Add("B_Z1000ND", item.z_nd_1000 == "0.00" ? "--" : item.z_nd_1000);

                        dc.Add("B_Z1250B1", item.z_one_1250 == "0.00" ? "--" : item.z_one_1250);
                        dc.Add("B_Z1250B2", item.z_two_1250 == "0.00" ? "--" : item.z_two_1250);
                        dc.Add("B_Z1250B3", item.z_three_1250 == "0.00" ? "--" : item.z_three_1250);
                        dc.Add("B_Z1250ND", item.z_nd_1250 == "0.00" ? "--" : item.z_nd_1250);

                        dc.Add("B_Z1500B1", item.z_one_1500 == "0.00" ? "--" : item.z_one_1500);
                        dc.Add("B_Z1500B2", item.z_two_1500 == "0.00" ? "--" : item.z_two_1500);
                        dc.Add("B_Z1500B3", item.z_three_1500 == "0.00" ? "--" : item.z_three_1500);
                        dc.Add("B_Z1500ND", item.z_nd_1500 == "0.00" ? "--" : item.z_nd_1500);

                        dc.Add("B_Z1750B1", item.z_one_1750 == "0.00" ? "--" : item.z_one_1750);
                        dc.Add("B_Z1750B2", item.z_two_1750 == "0.00" ? "--" : item.z_two_1750);
                        dc.Add("B_Z1750B3", item.z_three_1750 == "0.00" ? "--" : item.z_three_1750);
                        dc.Add("B_Z1750BND", item.z_nd_1750 == "0.00" ? "--" : item.z_nd_1750);

                        dc.Add("B_Z2000B1", item.z_one_2000 == "0.00" ? "--" : item.z_one_2000);
                        dc.Add("B_Z2000B2", item.z_two_2000 == "0.00" ? "--" : item.z_two_2000);
                        dc.Add("B_Z2000B3", item.z_three_2000 == "0.00" ? "--" : item.z_three_2000);
                        dc.Add("B_Z2000ND", item.z_nd_2000 == "0.00" ? "--" : item.z_nd_2000);

                        dc.Add("B_F250B1", item.f_one_250 == "0.00" ? "--" : item.f_one_250);
                        dc.Add("B_F250B2", item.f_two_250 == "0.00" ? "--" : item.f_two_250);
                        dc.Add("B_F250B3", item.f_three_250 == "0.00" ? "--" : item.f_three_250);
                        dc.Add("B_F250ND", item.f_nd_250 == "0.00" ? "--" : item.f_nd_250);

                        dc.Add("B_F500B1", item.f_one_500 == "0.00" ? "--" : item.f_one_500);
                        dc.Add("B_F500B2", item.f_two_500 == "0.00" ? "--" : item.f_two_500);
                        dc.Add("B_F500B3", item.f_three_500 == "0.00" ? "--" : item.f_three_500);
                        dc.Add("B_F500ND", item.f_nd_500 == "0.00" ? "--" : item.f_nd_500);

                        dc.Add("B_F750B1", item.f_one_750 == "0.00" ? "--" : item.f_one_750);
                        dc.Add("B_F750B2", item.f_two_750 == "0.00" ? "--" : item.f_two_750);
                        dc.Add("B_F750B3", item.f_three_750 == "0.00" ? "--" : item.f_three_750);
                        dc.Add("B_F750ND", item.f_nd_750 == "0.00" ? "--" : item.f_nd_750);

                        dc.Add("B_F1000B1", item.f_one_1000 == "0.00" ? "--" : item.f_one_1000);
                        dc.Add("B_F1000B2", item.f_two_1000 == "0.00" ? "--" : item.f_two_1000);
                        dc.Add("B_F1000B3", item.f_three_1000 == "0.00" ? "--" : item.f_three_1000);
                        dc.Add("B_F1000ND", item.f_nd_1000 == "0.00" ? "--" : item.f_nd_1000);

                        dc.Add("B_F1250B1", item.f_one_1250 == "0.00" ? "--" : item.f_one_1250);
                        dc.Add("B_F1250B2", item.f_two_1250 == "0.00" ? "--" : item.f_two_1250);
                        dc.Add("B_F1250B3", item.f_three_1250 == "0.00" ? "--" : item.f_three_1250);
                        dc.Add("B_F1250ND", item.f_nd_1250 == "0.00" ? "--" : item.f_nd_1250);

                        dc.Add("B_F1500B1", item.f_one_1500 == "0.00" ? "--" : item.f_one_1500);
                        dc.Add("B_F1500B2", item.f_two_1500 == "0.00" ? "--" : item.f_two_1500);
                        dc.Add("B_F1500B3", item.f_three_1500 == "0.00" ? "--" : item.f_three_1500);
                        dc.Add("B_F1500ND", item.f_nd_1500 == "0.00" ? "--" : item.f_nd_1500);

                        dc.Add("B_F1750B1", item.f_one_1750 == "0.00" ? "--" : item.f_one_1750);
                        dc.Add("B_F1750B2", item.f_two_1750 == "0.00" ? "--" : item.f_two_1750);
                        dc.Add("B_F1750B3", item.f_three_1750 == "0.00" ? "--" : item.f_three_1750);
                        dc.Add("B_F1750ND", item.f_nd_1750 == "0.00" ? "--" : item.f_nd_1750);

                        dc.Add("B_F2000B1", item.f_one_2000 == "0.00" ? "--" : item.f_one_2000);
                        dc.Add("B_F2000B2", item.f_two_2000 == "0.00" ? "--" : item.f_two_2000);
                        dc.Add("B_F2000B3", item.f_three_2000 == "0.00" ? "--" : item.f_three_2000);
                        dc.Add("B_F2000ND", item.f_nd_2000 == "0.00" ? "--" : item.f_nd_2000);

                        dc.Add("Bp3B1", item.z_one_p3 == "0.00" ? "--" : item.z_one_p3);
                        dc.Add("Bp3B2", item.z_two_p3 == "0.00" ? "--" : item.z_two_p3);
                        dc.Add("Bp3B3", item.z_three_p3 == "0.00" ? "--" : item.z_three_p3);
                        dc.Add("Bp3ND", item.z_nd_p3 == "0.00" ? "--" : item.z_nd_p3);

                        dc.Add("B_p3B1", item.f_one_p3 == "0.00" ? "--" : item.f_one_p3);
                        dc.Add("B_p3B2", item.f_two_p3 == "0.00" ? "--" : item.f_two_p3);
                        dc.Add("B_p3B3", item.f_three_p3 == "0.00" ? "--" : item.f_three_p3);
                        dc.Add("B_p3ND", item.f_nd_p3 == "0.00" ? "--" : item.f_nd_p3);

                    }
                    else if (item.level == "C")
                    {
                        dc.Add("C_F250B1", item.f_one_250 == "0.00" ? "--" : item.f_one_250);
                        dc.Add("C_F250B2", item.f_two_250 == "0.00" ? "--" : item.f_two_250);
                        dc.Add("C_F250B3", item.f_three_250 == "0.00" ? "--" : item.f_three_250);
                        dc.Add("C_F250ND", item.f_nd_250 == "0.00" ? "--" : item.f_nd_250);

                        dc.Add("C_F500B1", item.f_one_500 == "0.00" ? "--" : item.f_one_500);
                        dc.Add("C_F500B2", item.f_two_500 == "0.00" ? "--" : item.f_two_500);
                        dc.Add("C_F500B3", item.f_three_500 == "0.00" ? "--" : item.f_three_500);
                        dc.Add("C_F500ND", item.f_nd_500 == "0.00" ? "--" : item.f_nd_500);

                        dc.Add("C_F750B1", item.f_one_750 == "0.00" ? "--" : item.f_one_750);
                        dc.Add("C_F750B2", item.f_two_750 == "0.00" ? "--" : item.f_two_750);
                        dc.Add("C_F750B3", item.f_three_750 == "0.00" ? "--" : item.f_three_750);
                        dc.Add("C_F750ND", item.f_nd_750 == "0.00" ? "--" : item.f_nd_750);

                        dc.Add("C_F1000B1", item.f_one_1000 == "0.00" ? "--" : item.f_one_1000);
                        dc.Add("C_F1000B2", item.f_two_1000 == "0.00" ? "--" : item.f_two_1000);
                        dc.Add("C_F1000B3", item.f_three_1000 == "0.00" ? "--" : item.f_three_1000);
                        dc.Add("C_F1000ND", item.f_nd_1000 == "0.00" ? "--" : item.f_nd_1000);

                        dc.Add("C_F1250B1", item.f_one_1250 == "0.00" ? "--" : item.f_one_1250);
                        dc.Add("C_F1250B2", item.f_two_1250 == "0.00" ? "--" : item.f_two_1250);
                        dc.Add("C_F1250B3", item.f_three_1250 == "0.00" ? "--" : item.f_three_1250);
                        dc.Add("C_F1250ND", item.f_nd_1250 == "0.00" ? "--" : item.f_nd_1250);

                        dc.Add("C_F1500B1", item.f_one_1500 == "0.00" ? "--" : item.f_one_1500);
                        dc.Add("C_F1500B2", item.f_two_1500 == "0.00" ? "--" : item.f_two_1500);
                        dc.Add("C_F1500B3", item.f_three_1500 == "0.00" ? "--" : item.f_three_1500);
                        dc.Add("C_F1500ND", item.f_nd_1500 == "0.00" ? "--" : item.f_nd_1500);

                        dc.Add("C_F1750B1", item.f_one_1750 == "0.00" ? "--" : item.f_one_1750);
                        dc.Add("C_F1750B2", item.f_two_1750 == "0.00" ? "--" : item.f_two_1750);
                        dc.Add("C_F1750B3", item.f_three_1750 == "0.00" ? "--" : item.f_three_1750);
                        dc.Add("C_F1750ND", item.f_nd_1750 == "0.00" ? "--" : item.f_nd_1750);

                        dc.Add("C_F2000B1", item.f_one_2000 == "0.00" ? "--" : item.f_one_2000);
                        dc.Add("C_F2000B2", item.f_two_2000 == "0.00" ? "--" : item.f_two_2000);
                        dc.Add("C_F2000B3", item.f_three_2000 == "0.00" ? "--" : item.f_three_2000);
                        dc.Add("C_F2000ND", item.f_nd_2000 == "0.00" ? "--" : item.f_nd_2000);

                        dc.Add("C_Z250C1", item.z_one_250 == "0.00" ? "--" : item.z_one_250);
                        dc.Add("C_Z250C2", item.z_two_250 == "0.00" ? "--" : item.z_two_250);
                        dc.Add("C_Z250C3", item.z_three_250 == "0.00" ? "--" : item.z_three_250);
                        dc.Add("C_Z250ND", item.z_nd_250 == "0.00" ? "--" : item.z_nd_250);

                        dc.Add("C_Z500C1", item.z_one_500 == "0.00" ? "--" : item.z_one_500);
                        dc.Add("C_Z500C2", item.z_two_500 == "0.00" ? "--" : item.z_two_500);
                        dc.Add("C_Z500C3", item.z_three_500 == "0.00" ? "--" : item.z_three_500);
                        dc.Add("C_Z500ND", item.z_nd_500 == "0.00" ? "--" : item.z_nd_500);

                        dc.Add("C_Z750C1", item.z_one_750 == "0.00" ? "--" : item.z_one_750);
                        dc.Add("C_Z750C2", item.z_two_750 == "0.00" ? "--" : item.z_two_750);
                        dc.Add("C_Z750C3", item.z_three_750 == "0.00" ? "--" : item.z_three_750);
                        dc.Add("C_Z750ND", item.z_nd_750 == "0.00" ? "--" : item.z_nd_750);

                        dc.Add("C_Z1000C1", item.z_one_1000 == "0.00" ? "--" : item.z_one_1000);
                        dc.Add("C_Z1000C2", item.z_two_1000 == "0.00" ? "--" : item.z_two_1000);
                        dc.Add("C_Z1000C3", item.z_three_1000 == "0.00" ? "--" : item.z_three_1000);
                        dc.Add("C_Z1000ND", item.z_nd_1000 == "0.00" ? "--" : item.z_nd_1000);

                        dc.Add("C_Z1250C1", item.z_one_1250 == "0.00" ? "--" : item.z_one_1250);
                        dc.Add("C_Z1250C2", item.z_two_1250 == "0.00" ? "--" : item.z_two_1250);
                        dc.Add("C_Z1250C3", item.z_three_1250 == "0.00" ? "--" : item.z_three_1250);
                        dc.Add("C_Z1250ND", item.z_nd_1250 == "0.00" ? "--" : item.z_nd_1250);

                        dc.Add("C_Z1500C1", item.z_one_1500 == "0.00" ? "--" : item.z_one_1500);
                        dc.Add("C_Z1500C2", item.z_two_1500 == "0.00" ? "--" : item.z_two_1500);
                        dc.Add("C_Z1500C3", item.z_three_1500 == "0.00" ? "--" : item.z_three_1500);
                        dc.Add("C_Z1500ND", item.z_nd_1500 == "0.00" ? "--" : item.z_nd_1500);

                        dc.Add("C_Z1750C1", item.z_one_1750 == "0.00" ? "--" : item.z_one_1750);
                        dc.Add("C_Z1750C2", item.z_two_1750 == "0.00" ? "--" : item.z_two_1750);
                        dc.Add("C_Z1750C3", item.z_three_1750 == "0.00" ? "--" : item.z_three_1750);
                        dc.Add("C_Z1750ND", item.z_nd_1750 == "0.00" ? "--" : item.z_nd_1750);

                        dc.Add("C_Z2000C1", item.z_one_2000 == "0.00" ? "--" : item.z_one_2000);
                        dc.Add("C_Z2000C2", item.z_two_2000 == "0.00" ? "--" : item.z_two_2000);
                        dc.Add("C_Z2000C3", item.z_three_2000 == "0.00" ? "--" : item.z_three_2000);
                        dc.Add("C_Z2000ND", item.z_nd_2000 == "0.00" ? "--" : item.z_nd_2000);

                        dc.Add("Cp3C1", item.z_one_p3 == "0.00" ? "--" : item.z_one_p3);
                        dc.Add("Cp3C2", item.z_two_p3 == "0.00" ? "--" : item.z_two_p3);
                        dc.Add("Cp3C3", item.z_three_p3 == "0.00" ? "--" : item.z_three_p3);
                        dc.Add("Cp3ND", item.z_nd_p3 == "0.00" ? "--" : item.z_nd_p3);

                        dc.Add("C_p3C1", item.f_one_p3 == "0.00" ? "--" : item.f_one_p3);
                        dc.Add("C_p3C2", item.f_two_p3 == "0.00" ? "--" : item.f_two_p3);
                        dc.Add("C_p3C3", item.f_three_p3 == "0.00" ? "--" : item.f_three_p3);
                        dc.Add("C_p3ND", item.f_nd_p3 == "0.00" ? "--" : item.f_nd_p3);
                    }
                }
            }

            var dt_kfy_res_Info = settings.dt_kfy_res_Info;
            if (dt_kfy_res_Info != null)
            {
                if (dt_kfy_res_Info.lx_a != "")
                    dc.Add("LXA", dt_kfy_res_Info.lx_a);
                else
                    dc.Add("LXA", "--");
                if (dt_kfy_res_Info.lx_a != "")
                    dc.Add("LXB", dt_kfy_res_Info.lx_b);
                else
                    dc.Add("LXB", "--");
                if (dt_kfy_res_Info.lx_a != "")
                    dc.Add("LXC", dt_kfy_res_Info.lx_c);
                else
                    dc.Add("LXC", "--");

                dc.Add("A_ZP1", dt_kfy_res_Info.p1);
                dc.Add("A_FP1", dt_kfy_res_Info._p1);
                dc.Add("B_ZP1", dt_kfy_res_Info.p1);
                dc.Add("B_FP1", dt_kfy_res_Info._p1);
                dc.Add("C_ZP1", dt_kfy_res_Info.p1);
                dc.Add("C_FP1", dt_kfy_res_Info._p1);


                dc.Add("A_ZP2", dt_kfy_res_Info.p2);
                dc.Add("A_FP2", dt_kfy_res_Info._p2);
                dc.Add("B_ZP2", dt_kfy_res_Info.p2);
                dc.Add("B_FP2", dt_kfy_res_Info._p2);
                dc.Add("C_ZP2", dt_kfy_res_Info.p2);
                dc.Add("C_FP2", dt_kfy_res_Info._p2);

                //计算
                dc.Add("A_ZP3", dt_kfy_res_Info.p3);
                dc.Add("A_FP3", dt_kfy_res_Info._p3);
                dc.Add("B_ZP3", dt_kfy_res_Info.p3);
                dc.Add("B_FP3", dt_kfy_res_Info._p3);
                dc.Add("C_ZP3", dt_kfy_res_Info.p3);
                dc.Add("C_FP3", dt_kfy_res_Info._p3);


                dc.Add("A_ZP3重复2", dt_kfy_res_Info.p3);
                dc.Add("A_ZP3重复3", dt_kfy_res_Info.p3);
                dc.Add("A_ZP3重复4", dt_kfy_res_Info.p3);
                dc.Add("A_FP3重复2", dt_kfy_res_Info._p3);
                dc.Add("A_FP3重复3", dt_kfy_res_Info._p3);
                dc.Add("A_FP3重复4", dt_kfy_res_Info._p3);



                dc.Add("A_ZPMAX", dt_kfy_res_Info.pMax);
                dc.Add("A_FPMAX", dt_kfy_res_Info._pMax);
                dc.Add("B_ZPMAX", dt_kfy_res_Info.pMax);
                dc.Add("B_FPMAX", dt_kfy_res_Info._pMax);
                dc.Add("C_ZPMAX", dt_kfy_res_Info.pMax);
                dc.Add("C_FPMAX", dt_kfy_res_Info._pMax);

                var jc = dt_kfy_res_Info.defJC;
                dc.Add("抗风压备注1", dt_kfy_res_Info.desc);
                dc.Add("抗风压备注2", dt_kfy_res_Info.desc);
                dc.Add("抗风压备注3", dt_kfy_res_Info.desc);


                if (jc > 0)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        var pa = jc * 250;
                        if (i == 1)
                        {
                            dc.Add("A250重复1", pa.ToString());
                            dc.Add("A250重复2", pa.ToString());
                            dc.Add("A250重复3", pa.ToString());
                            dc.Add("A250重复4", pa.ToString());
                            dc.Add("A250重复5", pa.ToString());
                            dc.Add("A250重复6", pa.ToString());
                        }
                        if (i == 2)
                        {
                            dc.Add("A500重复1", pa.ToString());
                            dc.Add("A500重复2", pa.ToString());
                            dc.Add("A500重复3", pa.ToString());
                            dc.Add("A500重复4", pa.ToString());
                            dc.Add("A500重复5", pa.ToString());
                            dc.Add("A500重复6", pa.ToString());
                        }
                        if (i == 3)
                        {
                            dc.Add("A750重复1", pa.ToString());
                            dc.Add("A750重复2", pa.ToString());
                            dc.Add("A750重复3", pa.ToString());
                            dc.Add("A750重复4", pa.ToString());
                            dc.Add("A750重复5", pa.ToString());
                            dc.Add("A750重复6", pa.ToString());
                        }
                        if (i == 4)
                        {
                            dc.Add("A1000重复1", pa.ToString());
                            dc.Add("A1000重复2", pa.ToString());
                            dc.Add("A1000重复3", pa.ToString());
                            dc.Add("A1000重复4", pa.ToString());
                            dc.Add("A1000重复5", pa.ToString());
                            dc.Add("A1000重复6", pa.ToString());
                        }
                        if (i == 5)
                        {
                            dc.Add("A1250重复1", pa.ToString());
                            dc.Add("A1250重复2", pa.ToString());
                            dc.Add("A1250重复3", pa.ToString());
                            dc.Add("A1250重复4", pa.ToString());
                            dc.Add("A1250重复5", pa.ToString());
                            dc.Add("A1250重复6", pa.ToString());
                        }
                        if (i == 6)
                        {
                            dc.Add("A1500重复1", pa.ToString());
                            dc.Add("A1500重复2", pa.ToString());
                            dc.Add("A1500重复3", pa.ToString());
                            dc.Add("A1500重复4", pa.ToString());
                            dc.Add("A1500重复5", pa.ToString());
                            dc.Add("A1500重复6", pa.ToString());
                        }
                        if (i == 7)
                        {
                            dc.Add("A1750重复1", pa.ToString());
                            dc.Add("A1750重复2", pa.ToString());
                            dc.Add("A1750重复3", pa.ToString());
                            dc.Add("A1750重复4", pa.ToString());
                            dc.Add("A1750重复5", pa.ToString());
                            dc.Add("A1750重复6", pa.ToString());
                        }
                        if (i == 8)
                        {
                            dc.Add("A2000重复1", pa.ToString());
                            dc.Add("A2000重复2", pa.ToString());
                            dc.Add("A2000重复3", pa.ToString());
                            dc.Add("A2000重复4", pa.ToString());
                            dc.Add("A2000重复5", pa.ToString());
                            dc.Add("A2000重复6", pa.ToString());
                        }
                    }
                }
            }

            List<SortInfo> sortList = GetSort();

            if (settings.dt_qm_Info != null && settings.dt_qm_Info.Count > 0)
            {
                int zhengtiLevel = 0;
                int kekaiLevel = 0;

                var qmInfo = settings.dt_qm_Info.OrderBy(t => t.PaType).ToList();
                if (settings.dt_qm_zb_Info.testtype == "1")
                {
                    double zZTFS = 0d; double fZTFS = 0d;
                    double zKKFS = 0d; double fKKFS = 0d;
                    double resKK = 0d; double resZT = 0d;

                    GetQMLevel(settings, ref zhengtiLevel, ref kekaiLevel,
                          ref zZTFS, ref fZTFS, ref zKKFS, ref fKKFS, ref resKK, ref resZT);

                    for (int i = 0; i < sortList.Count(); i++)
                    {
                        var qmOne = qmInfo.Find(t => t.Pa == sortList[i].val.ToString() && t.PaType == sortList[i].index);
                        if (qmOne == null)
                            continue;

                        if (i == 0)
                        {
                            dc.Add("S50FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S50GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S50ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S50ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S50KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 1)
                        {
                            dc.Add("S100FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S100GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S100ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S100ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S100KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 2)
                        {
                            dc.Add("S150FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S150GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S150ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S150ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S150KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 3)
                        {
                            dc.Add("J100FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("J100GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("J100ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("J100ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("J100KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 4)
                        {
                            dc.Add("J50FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("J50GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("J50ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("J50ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("J50KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 5)
                        {
                            dc.Add("S_50FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S_50GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S_50ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S_50ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S_50KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 6)
                        {
                            dc.Add("S_100FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S_100GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S_100ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S_100ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S_100KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 7)
                        {
                            dc.Add("S_150FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("S_150GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("S_150ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("S_150ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("S_150KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 8)
                        {
                            dc.Add("J_100FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("J_100GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("J_100ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("J_100ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("J_100KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                        if (i == 9)
                        {
                            dc.Add("J_50FJ", qmOne.FJST == "0.00" ? "--" : qmOne.FJST);
                            dc.Add("J_50GF", qmOne.GFZH == "0.00" ? "--" : qmOne.GFZH);
                            dc.Add("J_50ZD", qmOne.ZDST == "0.00" ? "--" : qmOne.ZDST);
                            dc.Add("J_50ZT", qmOne.MQZT == "0.00" ? "--" : qmOne.MQZT);
                            dc.Add("J_50KK", qmOne.KKST == "0.00" ? "--" : qmOne.KKST);
                        }
                    }

                    if (resZT > 0)
                    {
                        dc.Add("整体正压pa", "100");
                        dc.Add("整体负压pa", "100");
                        dc.Add("整体使用pa", "100");
                        dc.Add("水密整体正压风速", Math.Round(zZTFS, 2).ToString());
                        dc.Add("水密整体负压风速", Math.Round(fZTFS, 2).ToString());
                        if (zZTFS > fZTFS)
                            dc.Add("整体平均风速", Math.Round(zZTFS, 2).ToString());
                        else
                            dc.Add("整体平均风速", Math.Round(fZTFS, 2).ToString());
                        dc.Add("整体QA", Math.Round(resZT, 2).ToString());
                    }
                    else
                    {
                        dc.Add("整体正压pa", "--");
                        dc.Add("整体负压pa", "--");
                        dc.Add("整体使用pa", "--");
                        dc.Add("水密整体正压风速", "--");
                        dc.Add("水密整体负压风速", "--");
                        dc.Add("整体平均风速", "--");
                        dc.Add("整体QA", "--");
                    }


                    if (resKK > 0)
                    {
                        dc.Add("可开正压pa", "100");
                        dc.Add("可开负压pa", "100");
                        dc.Add("可开使用pa", "100");
                        dc.Add("可开正压风速", Math.Round(zKKFS, 2).ToString());
                        dc.Add("可开负压风速", Math.Round(fKKFS, 2).ToString());
                        if (zKKFS > fKKFS)
                            dc.Add("可开平均风速", Math.Round(zKKFS, 2).ToString());
                        else
                            dc.Add("可开平均风速", Math.Round(fKKFS, 2).ToString());
                        dc.Add("可开QL", Math.Round(resKK, 2).ToString());
                    }
                    else
                    {
                        dc.Add("可开正压pa", "--");
                        dc.Add("可开负压pa", "--");
                        dc.Add("可开使用pa", "--");
                        dc.Add("可开正压风速", "--");
                        dc.Add("可开负压风速", "--");
                        dc.Add("可开平均风速", "--");
                        dc.Add("可开QL", "--");
                    }
                }
                else
                {
                    #region  默认
                    dc.Add("S50FJ", "--");
                    dc.Add("S50GF", "--");
                    dc.Add("S50ZD", "--");
                    dc.Add("S50ZT", "--");
                    dc.Add("S50KK", "--");

                    dc.Add("S100FJ", "--");
                    dc.Add("S100GF", "--");
                    dc.Add("S100ZD", "--");
                    dc.Add("S100ZT", "--");
                    dc.Add("S100KK", "--");

                    dc.Add("S150FJ", "--");
                    dc.Add("S150GF", "--");
                    dc.Add("S150ZD", "--");
                    dc.Add("S150ZT", "--");
                    dc.Add("S150KK", "--");

                    dc.Add("J100FJ", "--");
                    dc.Add("J100GF", "--");
                    dc.Add("J100ZD", "--");
                    dc.Add("J100ZT", "--");
                    dc.Add("J100KK", "--");

                    dc.Add("J50FJ", "--");
                    dc.Add("J50GF", "--");
                    dc.Add("J50ZD", "--");
                    dc.Add("J50ZT", "--");
                    dc.Add("J50KK", "--");

                    dc.Add("S_50FJ", "--");
                    dc.Add("S_50GF", "--");
                    dc.Add("S_50ZD", "--");
                    dc.Add("S_50ZT", "--");
                    dc.Add("S_50KK", "--");

                    dc.Add("S_100FJ", "--");
                    dc.Add("S_100GF", "--");
                    dc.Add("S_100ZD", "--");
                    dc.Add("S_100ZT", "--");
                    dc.Add("S_100KK", "--");

                    dc.Add("S_150FJ", "--");
                    dc.Add("S_150GF", "--");
                    dc.Add("S_150ZD", "--");
                    dc.Add("S_150ZT", "--");
                    dc.Add("S_150KK", "--");

                    dc.Add("J_100FJ", "--");
                    dc.Add("J_100GF", "--");
                    dc.Add("J_100ZD", "--");
                    dc.Add("J_100ZT", "--");
                    dc.Add("J_100KK", "--");

                    dc.Add("J_50FJ", "--");
                    dc.Add("J_50GF", "--");
                    dc.Add("J_50ZD", "--");
                    dc.Add("J_50ZT", "--");
                    dc.Add("J_50KK", "--");
                    #endregion
                }


                if (settings.dt_qm_zb_Info.testtype == "2")
                {
                    double zZTFS = 0d; double fZTFS = 0d;
                    double zKKFS = 0d; double fKKFS = 0d;
                    double resFC = 0d; double resMJ = 0d;

                    GetQMLevel(settings, ref zhengtiLevel, ref kekaiLevel,
                          ref zZTFS, ref fZTFS, ref zKKFS, ref fKKFS, ref resFC, ref resMJ);

                    //工程
                    var qm = qmInfo.Find(t => t.PaType == 3);
                    if (qm != null)
                    {
                        dc.Add("正压工程值", qm.Pa);
                        dc.Add("正压附加值", qm.FJST);
                        dc.Add("正压固附值", qm.GFZH);
                        dc.Add("正压总的值", qm.ZDST);
                        dc.Add("正压整体值", qm.MQZT);
                        dc.Add("正压可开值", qm.KKST);
                    }
                    var qm1 = qmInfo.Find(t => t.PaType == 4);
                    if (qm1 != null)
                    {
                        dc.Add("负压工程值", qm1.Pa);
                        dc.Add("负压附加值", qm1.FJST);
                        dc.Add("负压固附值", qm1.GFZH);
                        dc.Add("负压总的值", qm1.ZDST);
                        dc.Add("负压整体值", qm1.MQZT);
                        dc.Add("负压可开值", qm1.KKST);
                    }


                    if (resMJ > 0)
                    {
                        dc.Add("整体正压pa", "100");
                        dc.Add("整体负压pa", "100");
                        dc.Add("整体使用pa", "100");
                        dc.Add("水密整体正压风速", Math.Round(zZTFS, 2).ToString());
                        dc.Add("水密整体负压风速", Math.Round(fZTFS, 2).ToString());
                        if (zZTFS > fZTFS)
                            dc.Add("整体平均风速", Math.Round(zZTFS, 2).ToString());
                        else
                            dc.Add("整体平均风速", Math.Round(fZTFS, 2).ToString());
                        dc.Add("整体QA", resMJ.ToString());
                    }
                    else
                    {
                        dc.Add("整体正压pa", "--");
                        dc.Add("整体负压pa", "--");
                        dc.Add("整体使用pa", "--");
                        dc.Add("水密整体正压风速", "--");
                        dc.Add("水密整体负压风速", "--");
                        dc.Add("整体平均风速", "--");
                        dc.Add("整体QA", "--");
                    }


                    if (resFC > 0)
                    {
                        dc.Add("可开正压pa", "100");
                        dc.Add("可开负压pa", "100");
                        dc.Add("可开使用pa", "100");
                        dc.Add("可开正压风速", Math.Round(zKKFS, 2).ToString());
                        dc.Add("可开负压风速", Math.Round(fKKFS, 2).ToString());
                        if (zKKFS > fKKFS)
                            dc.Add("可开平均风速", Math.Round(zKKFS, 2).ToString());
                        else
                            dc.Add("可开平均风速", Math.Round(fKKFS, 2).ToString());
                        dc.Add("可开QL", resFC.ToString());
                    }
                    else
                    {
                        dc.Add("可开正压pa", "--");
                        dc.Add("可开负压pa", "--");
                        dc.Add("可开使用pa", "--");
                        dc.Add("可开正压风速", "--");
                        dc.Add("可开负压风速", "--");
                        dc.Add("可开平均风速", "--");
                        dc.Add("可开QL", "--");
                    }

                }
                else
                {
                    dc.Add("正压工程值", "--");
                    dc.Add("正压附加值", "--");
                    dc.Add("正压固附值", "--");
                    dc.Add("正压总的值", "--");
                    dc.Add("正压整体值", "--");
                    dc.Add("正压可开值", "--");

                    dc.Add("负压工程值", "--");
                    dc.Add("负压附加值", "--");
                    dc.Add("负压固附值", "--");
                    dc.Add("负压总的值", "--");
                    dc.Add("负压整体值", "--");
                    dc.Add("负压可开值", "--");
                }

                if (kekaiLevel == 0)
                {
                    dc.Add("可开气密性能等级", "--");
                }
                else
                {
                    dc.Add("可开气密性能等级", kekaiLevel.ToString());
                }
                if (zhengtiLevel == 0)
                {
                    dc.Add("试件整体气密性能等级", "--");
                }
                else
                {
                    dc.Add("试件整体气密性能等级", zhengtiLevel.ToString());
                }

                if (settings?.dt_qm_zb_Info?.jlgzj != null)
                {
                    dc.Add("集流管直径", settings?.dt_qm_zb_Info?.jlgzj);

                    var jmj = Formula.MathJieArea(double.Parse(settings.dt_qm_zb_Info.jlgzj));

                    dc.Add("截留管直径", (jmj * 1000).ToString());
                }
                else
                {
                    dc.Add("集流管直径", "--");
                    dc.Add("截留管直径", "--");
                }
            }

            if (settings.dt_sm_Info != null)
            {
                dc.Add("检测方法", settings.dt_sm_Info.Method);

                var sm_Pa = 0;
                if (settings.dt_sm_Info.sm_Pa != null)
                {
                    sm_Pa = settings.dt_sm_Info.sm_Pa.Value;
                }
                if (settings.dt_sm_Info.sm_PaDesc.Contains("▲") ||
                    settings.dt_sm_Info.sm_PaDesc.Contains("●"))
                {
                    if (settings.dt_sm_Info.sm_Pa == 250)
                    {
                        sm_Pa = 250;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 350)
                    {
                        sm_Pa = 500;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 500)
                    {
                        sm_Pa = 700;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 700)
                    {
                        sm_Pa = 1000;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 1000)
                    {
                        sm_Pa = 1000;
                    }
                }
                var sm_Pa2 = 0;

                if (settings.dt_sm_Info.sm_Pa2 != null)
                {
                    sm_Pa2 = settings.dt_sm_Info.sm_Pa2.Value;
                }
                if (settings.dt_sm_Info.sm_PaDesc2.Contains("▲") ||
                    settings.dt_sm_Info.sm_PaDesc2.Contains("●"))
                {
                    if (settings.dt_sm_Info.sm_Pa2 == 500)
                    {
                        sm_Pa2 = 500;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 700)
                    {
                        sm_Pa2 = 1000;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 1000)
                    {
                        sm_Pa2 = 1500;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 1500)
                    {
                        sm_Pa2 = 2000;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 2000)
                    {
                        sm_Pa2 = 2000;
                    }
                }

                if (sm_Pa == 250)
                {
                    dc.Add("可开250", settings.dt_sm_Info.sm_PaDesc);
                }
                if (sm_Pa == 350)
                {
                    dc.Add("可开350", settings.dt_sm_Info.sm_PaDesc);
                }
                if (sm_Pa == 500)
                {
                    dc.Add("可开500", settings.dt_sm_Info.sm_PaDesc);
                }
                if (sm_Pa == 700)
                {
                    dc.Add("可开700", settings.dt_sm_Info.sm_PaDesc);
                }
                if (sm_Pa == 1000)
                {
                    dc.Add("可开1000", settings.dt_sm_Info.sm_PaDesc);
                }

                if (sm_Pa2 == 500)
                {
                    dc.Add("固定500", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (sm_Pa2 == 700)
                {
                    dc.Add("固定700", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (sm_Pa2 == 1000)
                {
                    dc.Add("固定1000", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (sm_Pa2 == 1500)
                {
                    dc.Add("固定1500", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (sm_Pa2 == 2000)
                {
                    dc.Add("固定2000", settings.dt_sm_Info.sm_PaDesc2);
                }

                if (settings.dt_sm_Info.sm_Pa != null)
                {
                    dc.Add("可开未渗漏风压", settings.dt_sm_Info.sm_Pa.ToString());

                    var level = Formula.GetWaterTightLevel_KeKaiQi(settings.dt_sm_Info.sm_Pa.Value);
                    dc.Add("可开水密性能等级", level.ToString());
                }
                else
                {
                    dc.Add("可开未渗漏风压", "--");
                    dc.Add("可开水密性能等级", "--");

                }
                if (settings.dt_sm_Info.sm_Pa2 != null)
                {
                    dc.Add("固定未渗漏风压", settings.dt_sm_Info.sm_Pa2.ToString());
                    var level = Formula.GetWaterTightLevel_GuDing(settings.dt_sm_Info.sm_Pa2.Value);

                    dc.Add("固定水密等级", level.ToString());
                }
                else
                {
                    dc.Add("固定未渗漏风压", "--");
                    dc.Add("固定水密等级", "--");
                }

                if (!string.IsNullOrWhiteSpace(settings.dt_sm_Info.sm_Remark))
                {
                    dc.Add("水密备注", settings.dt_sm_Info.sm_Remark);
                }
                else
                {
                    dc.Add("水密备注", "--");
                }
            }
            if (settings.dt_pd_Info != null)
            {
                var testDesc = string.IsNullOrWhiteSpace(settings.dt_pd_Info.test_desc) ? "试件完好" : settings.dt_pd_Info.test_desc;
                if (settings.dt_pd_Info.test_result == "第一级")
                {
                    dc.Add("平面等级1", testDesc);
                }
                if (settings.dt_pd_Info.test_result == "第二级")
                {
                    dc.Add("平面等级2", testDesc);
                }
                if (settings.dt_pd_Info.test_result == "第三级")
                {
                    dc.Add("平面等级3", testDesc);
                }
                if (settings.dt_pd_Info.test_result == "第四级")
                {
                    dc.Add("平面等级4", testDesc);
                }
                if (settings.dt_pd_Info.test_result == "第五级")
                {
                    dc.Add("平面等级5", testDesc);
                }

                dc.Add("变形等级", settings.dt_pd_Info.test_result);
                dc.Add("幕墙整体备注", "--");

            }
            else
            {
                dc.Add("平面等级1", "--");
                dc.Add("平面等级2", "--");
                dc.Add("平面等级3", "--");
                dc.Add("平面等级4", "--");
                dc.Add("平面等级5", "--");
            }
            dc.Add("检测方法2", "平行四边形法");


            GetP1(settings, ref dc);

            //dc.Add("气密性能等级", "");
            //dc.Add("淋水流量", "");
            return dc;
        }

        #region  计算抗风压p1

        private void GetP1(Model_dt_Settings settings, ref Dictionary<string, string> dc)
        {
            List<WindPressureDGV> windPressureDGV = new List<WindPressureDGV>();
            var kfyTable = new DAL_dt_kfy_Info().GetkfyListByCode(_tempCode);
            if (kfyTable != null && kfyTable.Rows.Count > 0)
            {
                int lengA = int.Parse(settings.ganAchang);
                int lengB = int.Parse(settings.ganBchang);
                int lengC = int.Parse(settings.ganCchang);
                foreach (DataRow dr in kfyTable.Rows)
                {
                    var level = dr["level"].ToString();
                    if (level == "A")
                    {
                        windPressureDGV = GetGroupData(dr, lengA, settings.dt_kfy_res_Info.defJC);
                        double lx = double.Parse(settings.dt_kfy_res_Info.lx_a);
                        List<double> list = new List<double>();
                        double p1 = 0d;
                        double _p1 = 0d;

                        GetP1(windPressureDGV, lengA, lx, ref p1, ref _p1);
                        dc.Add("P1PA", p1.ToString());
                        dc.Add("FP1PA", _p1.ToString());

                    }
                    else if (level == "B")
                    {
                        windPressureDGV = GetGroupData(dr, lengB, settings.dt_kfy_res_Info.defJC);
                        if (settings.dt_kfy_res_Info.lx_b != "")
                        {
                            double lx = double.Parse(settings.dt_kfy_res_Info.lx_b);
                            List<double> list = new List<double>();
                            double p1 = 0d;
                            double _p1 = 0d;
                            GetP1(windPressureDGV, lengB, lx, ref p1, ref _p1);
                            dc.Add("P1PB", p1.ToString());
                            dc.Add("FP1PB", _p1.ToString());
                        }
                        else
                            dc.Add("P1PB", "--");
                    }
                    else if (level == "C")
                    {
                        windPressureDGV = GetGroupData(dr, lengC, settings.dt_kfy_res_Info.defJC);
                        if (settings.dt_kfy_res_Info.lx_c != "")
                        {
                            double lx = double.Parse(settings.dt_kfy_res_Info.lx_c);
                            List<double> list = new List<double>();
                            double p1 = 0d;
                            double _p1 = 0d;
                            GetP1(windPressureDGV, lengC, lx, ref p1, ref _p1);

                            dc.Add("P1PC", p1.ToString());
                            dc.Add("FP1PC", _p1.ToString());

                        }
                        else { dc.Add("P1PC", "--"); }
                    }
                }
            }
        }

        /// <summary>
        /// 分组获取数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<WindPressureDGV> GetGroupData(DataRow dr, int ganJianChangDu, int jcValue)
        {
            List<WindPressureDGV> tempWindPressureDGV = new List<WindPressureDGV>();

            var defKFYPa = new List<DefKFYPa>();
            for (int i = 1; i < 9; i++)
            {
                defKFYPa.Add(new DefKFYPa() { Value = jcValue * i });
            }

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
                if (i == 0)
                {
                    name = "P3";
                    field = "p3";
                }
                else if (i == 1)
                {
                    name = "PMax";
                    field = "p3max";
                }
                tempWindPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = name,
                    PaValue = -1,
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

        private void GetP1(List<WindPressureDGV> data, int leng, double lx,
           ref double p1, ref double _p1)
        {
            var zdefPa = 2000;
            var fdefPa = 2000;
            int lengA = leng;

            var tempData_z = data.FindAll(t => t.zlx > 0 && t.Pa != "P3" && t.Pa != "PMax").ToList();
            if (tempData_z != null && tempData_z.Count() > 0)
            {
                if (tempData_z[tempData_z.Count - 1] != null)
                {
                    zdefPa = tempData_z[tempData_z.Count - 1].PaValue;
                }
            }
            var tempData_f = data.FindAll(t => t.flx > 0 && t.Pa != "P3" && t.Pa != "PMax").ToList();
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
            if (zone != null && ztwo != null && zone?.zzd > 0 && ztwo.zzd > 0)
            {
                var x1 = float.Parse(zone.zzd.ToString());
                var x2 = float.Parse(ztwo.zzd.ToString());
                var y1 = zdefPa - 250;
                var y2 = zdefPa;

                var p = Calculate(x1, x2, y1, y2, lengA, lx);

                p1 = Math.Round(p, 0);
            }

            var fone = new WindPressureDGV();
            var ftwo = new WindPressureDGV();
            fone = data.Find(t => t.Pa == (fdefPa - 250) + "Pa");
            ftwo = data.Find(t => t.Pa == fdefPa + "Pa");
            if (fone != null && ftwo != null && fone?.fzd > 0 && ftwo.fzd > 0)
            {
                var _x1 = float.Parse(fone.fzd.ToString());
                var _x2 = float.Parse(ftwo.fzd.ToString());
                var y1 = fdefPa - 250;
                var y2 = fdefPa;
                var _p = Calculate(_x1, _x2, y1, y2, lengA, lx);
                _p1 = Math.Round(_p, 0);
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
                return Math.Round(x, 2);

            return Math.Round(k * x + b, 2);
        }
        #endregion
        /// <summary>
        /// 气密正常 等级
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="zhengtiLevel"></param>
        /// <param name="kekaiLevel"></param>
        private void GetQMLevel(Model_dt_Settings settings, ref int zhengtiLevel, ref int kekaiLevel,
            ref double zZTFS, ref double fZTFS,
            ref double zKKFS, ref double fKKFS,
            ref double resKK, ref double resZT
            )
        {
            double kekaifengchang = 0d;
            double shijianmianji = 0d;
            double daqiyali = 0d;
            double dangqianwendu = 0d;

            if (settings != null)
            {
                kekaifengchang = double.Parse(settings.kekaifengchang);
                shijianmianji = double.Parse(settings.shijianmianji);
                daqiyali = double.Parse(settings.DaQiYaLi);
                dangqianwendu = double.Parse(settings.DangQianWenDu);
            }
            if (settings.dt_qm_Info != null && settings.dt_qm_Info.Count > 0)
            {
                var zheng_s = settings.dt_qm_Info.Find(t => t.Pa == "100" && t.PaType == 1);
                var zheng_j = settings.dt_qm_Info.Find(t => t.Pa == "100" && t.PaType == 2);
                var fu_s = settings.dt_qm_Info.Find(t => t.Pa == "-100" && t.PaType == 1);
                var fu_j = settings.dt_qm_Info.Find(t => t.Pa == "-100" && t.PaType == 2);

                //逢长
                var kkValue_z = (double.Parse(zheng_s.KKST) + double.Parse(zheng_j.KKST)) / 2;
                var kkValue_f = (double.Parse(fu_s.KKST) + double.Parse(fu_j.KKST)) / 2;

                //面积
                var ztValue_z = (double.Parse(zheng_s.MQZT) + double.Parse(zheng_j.MQZT)) / 2;
                var ztValue_f = (double.Parse(fu_s.MQZT) + double.Parse(fu_j.MQZT)) / 2;


                var kkValue = 0d;
                var ztValue = 0d;

                if (kkValue_z > kkValue_f)
                    kkValue = Formula.GetIndexStichLength(kkValue_z, kkValue_z, daqiyali, kekaifengchang, dangqianwendu);
                else
                    kkValue = Formula.GetIndexStichLength(kkValue_f, kkValue_f, daqiyali, kekaifengchang, dangqianwendu);

                if (ztValue_z > ztValue_f)
                    ztValue = Formula.GetIndexStitchArea(ztValue_z, ztValue_z, daqiyali, shijianmianji, dangqianwendu);
                else
                    ztValue = Formula.GetIndexStitchArea(ztValue_f, ztValue_f, daqiyali, shijianmianji, dangqianwendu);

                zKKFS = kkValue_z;
                fKKFS = kkValue_f;

                zZTFS = ztValue_z;
                fZTFS = ztValue_f;


                resZT = ztValue;
                resKK = kkValue;

                kekaiLevel = Formula.GetStitchLengthLevel(kkValue);

                zhengtiLevel = Formula.GetAreaLevel(ztValue);
            }
        }


        private List<SortInfo> GetSort()
        {
            List<SortInfo> sortInfo = new List<SortInfo>();
            sortInfo.Add(new SortInfo() { index = 1, val = 50 });
            sortInfo.Add(new SortInfo() { index = 1, val = 100 });
            sortInfo.Add(new SortInfo() { index = 1, val = 150 });
            sortInfo.Add(new SortInfo() { index = 2, val = 100 });
            sortInfo.Add(new SortInfo() { index = 2, val = 50 });

            sortInfo.Add(new SortInfo() { index = 1, val = -50 });
            sortInfo.Add(new SortInfo() { index = 1, val = -100 });
            sortInfo.Add(new SortInfo() { index = 1, val = -150 });
            sortInfo.Add(new SortInfo() { index = 2, val = -100 });
            sortInfo.Add(new SortInfo() { index = 2, val = -50 });

            return sortInfo;
        }

        private void DrawLine(string saveFile, string code, List<Model_dt_kfy_Info> kfyList)
        {
            var index = 0;
            List<ImgInfo> imgList = new List<ImgInfo>();
            foreach (var item in kfyList)
            {
                index++;
                var zitem = new List<double>() { 0d };
                var fitem = new List<double>() { 0d };
                zitem.Add(double.Parse(item.z_nd_250));
                zitem.Add(double.Parse(item.z_nd_500));
                zitem.Add(double.Parse(item.z_nd_750));
                zitem.Add(double.Parse(item.z_nd_1000));
                zitem.Add(double.Parse(item.z_nd_1250));
                zitem.Add(double.Parse(item.z_nd_1500));
                zitem.Add(double.Parse(item.z_nd_1750));
                zitem.Add(double.Parse(item.z_nd_2000));

                fitem.Add(double.Parse(item.f_nd_250));
                fitem.Add(double.Parse(item.f_nd_500));
                fitem.Add(double.Parse(item.f_nd_750));
                fitem.Add(double.Parse(item.f_nd_1000));
                fitem.Add(double.Parse(item.f_nd_1250));
                fitem.Add(double.Parse(item.f_nd_1500));
                fitem.Add(double.Parse(item.f_nd_1750));
                fitem.Add(double.Parse(item.f_nd_2000));

                var imgUrl = System.Windows.Forms.Application.StartupPath + ($"\\tempImage\\{code}\\");

                //判断文件的存在
                if (!System.IO.Directory.Exists(imgUrl))
                {
                    System.IO.Directory.CreateDirectory(imgUrl);
                }
                var imgPath = imgUrl + $"{item.level}-{DateTime.Now.ToString("hhmmddfff")}.jpg";
                ImageLine(imgPath, zitem, fitem);
                imgList.Add(new ImgInfo() { url = imgPath, key = item.level, tag = item.level + "组挠度" });
            }
            InsertPtctureToWord(saveFile, imgList);
        }


        private void ImageLine(string file, List<double> zitem, List<double> fitem)
        {
            int height = 304, width = 510;

            var xCount = 30;
            var yCount = 16;

            var xSpacing = width / xCount;
            var ySpacing = height / yCount;

            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //清空图片背景色
                g.Clear(Color.White);
                System.Drawing.Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                System.Drawing.Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Regular);
                System.Drawing.Font font2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                LinearGradientBrush brush = new LinearGradientBrush(
                new System.Drawing.Rectangle(0, 0, image.Width, image.Height), Color.Black, Color.Black, 1.2f, true);

                g.FillRectangle(Brushes.White, 0, 0, width, height);
                Brush brush1 = new SolidBrush(Color.Black);
                Brush brush2 = new SolidBrush(Color.SaddleBrown);

                LinearGradientBrush brush3 = new LinearGradientBrush(
       new System.Drawing.Rectangle(0, 0, image.Width, image.Height), Color.Gray, Color.Gray, 1.2f, true);

                CreateBackground(g, image.Width, image.Height, xCount, yCount, xSpacing, ySpacing);

                #region  绘制折线
                //显示折线效果
                System.Drawing.Font font3 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                SolidBrush mybrush = new SolidBrush(Color.Red);


                var zSize = zitem.FindAll(t => t > 0).Count + 1;
                var fSize = fitem.FindAll(t => t > 0).Count + 1;

                //正压
                System.Drawing.Point[] points1 = new System.Drawing.Point[zSize];
                Pen brokenLinePan_Z = new Pen(Color.Black, 2);
                double initialx = width / 2;
                double initialy = height / 2;

                for (int i = 0; i < zSize; i++)
                {
                    points1[i].X = Convert.ToInt32(initialx + zitem[i] * xSpacing);
                    points1[i].Y = (int)initialy - i * ySpacing;
                    g.DrawRectangle(brokenLinePan_Z, points1[i].X - 1, points1[i].Y - 1, 2, 2);
                }
                g.DrawLines(brokenLinePan_Z, points1); //绘制折线


                //负压
                System.Drawing.Point[] points2 = new System.Drawing.Point[fSize];
                Pen brokenLinePan_F = new Pen(Color.Black, 2);

                for (int i = 0; i < fSize; i++)
                {
                    points2[i].X = Convert.ToInt32(initialx - fitem[i] * ySpacing);
                    points2[i].Y = (int)initialy + i * xSpacing;
                    g.DrawRectangle(brokenLinePan_F, points2[i].X - 1, points2[i].Y - 1, 2, 2);
                }
                g.DrawLines(brokenLinePan_F, points2); //绘制折线
                #endregion

                image.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }


        /// <summary>
        /// 背景
        /// </summary>
        /// <param name="g"></param>
        private void CreateBackground(Graphics g, int width, int height, int xCount, int yCount, int xSpacing, int ySpacing)
        {
            LinearGradientBrush brush = new LinearGradientBrush(
    new System.Drawing.Rectangle(0, 0, width, height), Color.Gray, Color.Gray, 1.2f, true);

            LinearGradientBrush brush1 = new LinearGradientBrush(
new System.Drawing.Rectangle(0, 0, width, height), Color.Black, Color.Black, 1.2f, true);

            System.Drawing.Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);

            //四周矩形
            g.DrawRectangle(new Pen(Color.Black), 0, 0, width - 1, height - 1);

            Pen mypen = new Pen(brush, 1);


            #region  背景表格
            String[] yStr = { "-15", "-14", "-13", "-12", "-11", "-10", "-9", "-8", "-7", "-6", "-5", "-4", "-3", "-2", "-1", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
            String[] xStr = { "2000", "1750", "1500", "1250", "1000", "750", "500", "250", "0", "-250", "-500", "-750", "-1000", "-1250", "-1500", "-1750", "-2000" };

            //绘制纵向线条
            int xWidth = xSpacing;
            for (int x = 1; x <= xCount; x++)
            {
                //标点
                // g.DrawString(yStr[x].ToString(), font, Brushes.Black, width / 2 + 10, height / 2); //设置文字内容及输出位置
                if (x == xCount / 2)
                {
                    mypen = new Pen(brush1, 2);
                    g.DrawLine(mypen, xWidth, 1, xWidth, height);

                    xWidth = xWidth + xSpacing;
                }
                else
                {
                    mypen = new Pen(brush, 1);
                    g.DrawLine(mypen, xWidth, 1, xWidth, height);
                    xWidth = xWidth + xSpacing;
                }
            }

            //绘制横向线条
            int yHeight = ySpacing;
            for (int y = 1; y <= yCount; y++)
            {
                //标点
                // g.DrawString(xStr[x].ToString(), font, Brushes.Black, width / 2 + 10, 205); //设置文字内容及输出位置

                if (y == yCount / 2)
                {
                    mypen = new Pen(brush1, 2);
                    g.DrawLine(mypen, 1, yHeight, width, yHeight);
                    yHeight = yHeight + ySpacing;
                }
                else
                {
                    mypen = new Pen(brush, 1);
                    g.DrawLine(mypen, 1, yHeight, width, yHeight);
                    yHeight = yHeight + ySpacing;
                }
            }
            #endregion
            #region xy描述
            System.Drawing.Font font1 = new System.Drawing.Font("宋体", 10, FontStyle.Regular);
            Brush brush2 = new SolidBrush(Color.Black);
             g.DrawString("Pa", font1, brush2, new PointF(width / 2, 0 + 10));
            //g.DrawString("2000Pa", font1, brush2, new PointF(width / 2, 0 + 10));
            // g.DrawString("-2000Pa", font1, brush2, new PointF(width / 2, height - 20));

            g.DrawString("mm", font1, brush2, new PointF(width - 40, height / 2 - 20));
            //g.DrawString("-15mm", font1, brush2, new PointF(0 + 10, height / 2 - 20));
            //g.DrawString("15mm", font1, brush2, new PointF(width - 40, height / 2 - 20));
            #endregion
        }


        /// <summary>
        /// 导入图片到word
        /// </summary>
        protected void InsertPtctureToWord(string file, List<ImgInfo> imgs)
        {
            object Nothing = System.Reflection.Missing.Value;
            //创建一个名为wordApp的组件对象
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();

            //word文档位置
            object filename = file;

            //定义该插入图片是否为外部链接
            object linkToFile = true;

            //定义插入图片是否随word文档一起保存
            object saveWithDocument = true;

            //打开word文档
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref filename, ref Nothing, ref Nothing, ref Nothing,
               ref Nothing, ref Nothing, ref Nothing, ref Nothing,
               ref Nothing, ref Nothing, ref Nothing, ref Nothing,
               ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            try
            {
                foreach (var item in imgs)
                {
                    //标签
                    object bookMark = item.tag;
                    //图片
                    string replacePic = item.url;

                    if (doc.Bookmarks.Exists(Convert.ToString(bookMark)) == true)
                    {
                        //查找书签
                        doc.Bookmarks.get_Item(ref bookMark).Select();
                        //设置图片位置
                        wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                        //在书签的位置添加图片
                        InlineShape inlineShape = wordApp.Selection.InlineShapes.AddPicture(replacePic, ref linkToFile, ref saveWithDocument, ref Nothing);
                        inlineShape.ConvertToShape().WrapFormat.Type = WdWrapType.wdWrapFront;
                        //设置图片大小
                        //if (tag == "图片")
                        //{
                        //    inlineShape.Width = 500;
                        //    inlineShape.Height = 300;
                        //}
                        //else
                        //{
                        //    inlineShape.Width = 250;
                        //    inlineShape.Height = 215;
                        //}
                        //inlineShape.Width = 300;
                        //inlineShape.Height = 200;
                        doc.Save();
                    }
                    else
                    {
                        doc.Close(ref Nothing, ref Nothing, ref Nothing);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //word文档中不存在该书签，关闭文档
                doc.Close(ref Nothing, ref Nothing, ref Nothing);
            }
        }
    }

    public class SortInfo
    {
        public int val { get; set; }
        public int index { get; set; }
    }
    public class ImgInfo
    {

        public string url { get; set; }
        public string tag { get; set; }
        public string key { get; set; }
    }
}
