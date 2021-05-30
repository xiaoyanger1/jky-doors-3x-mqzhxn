﻿using Microsoft.Office.Interop.Word;
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
            if (cm_Report.SelectedIndex == 0)
            {
                MessageBox.Show("请选择模板！", "请选择模板！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                    lbl_message.Visible = false;

                    MessageBox.Show("导出成功", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("数据出现问题，导出失败!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        #region old

        /// <summary>
        /// 导入图片到word
        /// </summary>
        protected void InsertPtctureToExcel(string file, string tag, string imageName)
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
                //标签
                object bookMark = tag;
                //图片
                string replacePic = imageName;

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
                    if (tag == "图片")
                    {
                        inlineShape.Width = 500;
                        inlineShape.Height = 300;
                    }
                    else
                    {
                        inlineShape.Width = 250;
                        inlineShape.Height = 215;
                    }
                    doc.Save();
                }
                else
                {
                    doc.Close(ref Nothing, ref Nothing, ref Nothing);
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


            dc.Add("集流管直径", "");
            dc.Add("气密性能等级", "");
            dc.Add("淋水流量", "");

            if (settings.dt_kfy_Info != null && settings.dt_kfy_Info.Count > 0)
            {
                var kfyInfo = settings.dt_kfy_Info.OrderBy(t => t.level);
                foreach (var item in settings.dt_kfy_Info)
                {
                    if (item.level == "A")
                    {
                        dc.Add("A_Z250A1", item.z_one_250);
                        dc.Add("A_Z250A2", item.z_two_250);
                        dc.Add("A_Z250A3", item.z_three_250);
                        dc.Add("A_Z250ND", item.z_nd_250);

                        dc.Add("A_Z500A1", item.z_one_500);
                        dc.Add("A_Z500A2", item.z_two_250);
                        dc.Add("A_Z500A3", item.z_three_250);
                        dc.Add("A_Z500ND", item.z_nd_500);

                        dc.Add("A_Z750A1", item.z_one_750);
                        dc.Add("A_Z750A2", item.z_two_750);
                        dc.Add("A_Z750A3", item.z_three_750);
                        dc.Add("A_Z750ND", item.z_nd_750);

                        dc.Add("A_Z1000A1", item.z_one_1000);
                        dc.Add("A_Z1000A2", item.z_two_1000);
                        dc.Add("A_Z1000A3", item.z_three_1000);
                        dc.Add("A_Z1000ND", item.z_nd_1000);

                        dc.Add("A_Z1250A1", item.z_one_1250);
                        dc.Add("A_Z1250A2", item.z_two_1250);
                        dc.Add("A_Z1250A3", item.z_three_1250);
                        dc.Add("A_Z1250ND", item.z_nd_1250);

                        dc.Add("A_Z1500A1", item.z_one_1500);
                        dc.Add("A_Z1500A2", item.z_two_1500);
                        dc.Add("A_Z1500A3", item.z_three_1500);
                        dc.Add("A_Z1500ND", item.z_nd_1500);

                        dc.Add("A_Z1750A1", item.z_one_1750);
                        dc.Add("A_Z1750A2", item.z_two_1750);
                        dc.Add("A_Z1750A3", item.z_three_1750);
                        dc.Add("A_Z1750ND", item.z_nd_1750);

                        dc.Add("A_Z2000A1", item.z_one_2000);
                        dc.Add("A_Z2000A2", item.z_two_2000);
                        dc.Add("A_Z2000A3", item.z_three_2000);
                        dc.Add("A_Z2000ND", item.z_nd_2000);

                        dc.Add("A_F250A1", item.f_one_250);
                        dc.Add("A_F250A2", item.f_two_250);
                        dc.Add("A_F250A3", item.f_three_250);
                        dc.Add("A_F250ND", item.f_nd_250);

                        dc.Add("A_F500A1", item.f_one_500);
                        dc.Add("A_F500A2", item.f_two_500);
                        dc.Add("A_F500A3", item.f_three_500);
                        dc.Add("A_F500ND", item.f_nd_500);

                        dc.Add("A_F750A1", item.f_one_750);
                        dc.Add("A_F750A2", item.f_two_750);
                        dc.Add("A_F750A3", item.f_three_750);
                        dc.Add("A_F750ND", item.f_nd_750);

                        dc.Add("A_F1000A1", item.f_one_1000);
                        dc.Add("A_F1000A2", item.f_two_1000);
                        dc.Add("A_F1000A3", item.f_three_1000);
                        dc.Add("A_F1000ND", item.f_nd_1000);

                        dc.Add("A_F1250A1", item.f_one_1250);
                        dc.Add("A_F1250A2", item.f_two_1250);
                        dc.Add("A_F1250A3", item.f_three_1250);
                        dc.Add("A_F1250ND", item.f_nd_1250);

                        dc.Add("A_F1500A1", item.f_one_1500);
                        dc.Add("A_F1500A2", item.f_two_1500);
                        dc.Add("A_F1500A3", item.f_three_1500);
                        dc.Add("A_F1500ND", item.f_nd_1500);

                        dc.Add("A_F1750A1", item.f_one_1750);
                        dc.Add("A_F1750A2", item.f_two_1750);
                        dc.Add("A_F1750A3", item.f_three_1750);
                        dc.Add("A_F1750ND", item.f_nd_1750);

                        dc.Add("A_F2000A1", item.f_one_2000);
                        dc.Add("A_F2000A2", item.f_two_2000);
                        dc.Add("A_F2000A3", item.f_three_2000);
                        dc.Add("A_F2000ND", item.f_nd_2000);

                        dc.Add("Ap3A1", item.z_one_p3);
                        dc.Add("Ap3A2", item.z_two_p3);
                        dc.Add("Ap3A3", item.z_three_p3);
                        dc.Add("Ap3ND", item.z_nd_p3);

                        dc.Add("A_p3A1", item.f_one_p3);
                        dc.Add("A_p3A2", item.f_two_p3);
                        dc.Add("A_p3A3", item.f_three_p3);
                        dc.Add("A_p3ND", item.f_nd_p3);
                    }
                    else if (item.level == "B")
                    {
                        dc.Add("B_Z250B1", item.z_one_250);
                        dc.Add("B_Z250B2", item.z_two_250);
                        dc.Add("B_Z250B3", item.z_three_250);
                        dc.Add("B_Z250ND", item.z_nd_250);

                        dc.Add("B_Z500B1", item.z_one_500);
                        dc.Add("B_Z500B2", item.z_two_250);
                        dc.Add("B_Z500B3", item.z_three_250);
                        dc.Add("B_Z500ND", item.z_nd_500);

                        dc.Add("B_Z750B1", item.z_one_750);
                        dc.Add("B_Z750B2", item.z_two_750);
                        dc.Add("B_Z750B3", item.z_three_750);
                        dc.Add("B_Z750ND", item.z_nd_750);

                        dc.Add("B_Z1000B1", item.z_one_1000);
                        dc.Add("B_Z1000B2", item.z_two_1000);
                        dc.Add("B_Z1000B3", item.z_three_1000);
                        dc.Add("B_Z1000ND", item.z_nd_1000);

                        dc.Add("B_Z1250B1", item.z_one_1250);
                        dc.Add("B_Z1250B2", item.z_two_1250);
                        dc.Add("B_Z1250B3", item.z_three_1250);
                        dc.Add("B_Z1250ND", item.z_nd_1250);

                        dc.Add("B_Z1500B1", item.z_one_1500);
                        dc.Add("B_Z1500B2", item.z_two_1500);
                        dc.Add("B_Z1500B3", item.z_three_1500);
                        dc.Add("B_Z1500ND", item.z_nd_1500);

                        dc.Add("B_Z1750B1", item.z_one_1750);
                        dc.Add("B_Z1750B2", item.z_two_1750);
                        dc.Add("B_Z1750B3", item.z_three_1750);
                        dc.Add("B_Z1750BND", item.z_nd_1750);

                        dc.Add("B_Z2000B1", item.z_one_2000);
                        dc.Add("B_Z2000B2", item.z_two_2000);
                        dc.Add("B_Z2000B3", item.z_three_2000);
                        dc.Add("B_Z2000ND", item.z_nd_2000);

                        dc.Add("B_F250B1", item.f_one_250);
                        dc.Add("B_F250B2", item.f_two_250);
                        dc.Add("B_F250B3", item.f_three_250);
                        dc.Add("B_F250ND", item.f_nd_250);

                        dc.Add("B_F500B1", item.f_one_500);
                        dc.Add("B_F500B2", item.f_two_500);
                        dc.Add("B_F500B3", item.f_three_500);
                        dc.Add("B_F500ND", item.f_nd_500);

                        dc.Add("B_F750B1", item.f_one_750);
                        dc.Add("B_F750B2", item.f_two_750);
                        dc.Add("B_F750B3", item.f_three_750);
                        dc.Add("B_F750ND", item.f_nd_750);

                        dc.Add("B_F1000B1", item.f_one_1000);
                        dc.Add("B_F1000B2", item.f_two_1000);
                        dc.Add("B_F1000B3", item.f_three_1000);
                        dc.Add("B_F1000ND", item.f_nd_1000);

                        dc.Add("B_F1250B1", item.f_one_1250);
                        dc.Add("B_F1250B2", item.f_two_1250);
                        dc.Add("B_F1250B3", item.f_three_1250);
                        dc.Add("B_F1250ND", item.f_nd_1250);

                        dc.Add("B_F1500B1", item.f_one_1500);
                        dc.Add("B_F1500B2", item.f_two_1500);
                        dc.Add("B_F1500B3", item.f_three_1500);
                        dc.Add("B_F1500ND", item.f_nd_1500);

                        dc.Add("B_F1750B1", item.f_one_1750);
                        dc.Add("B_F1750B2", item.f_two_1750);
                        dc.Add("B_F1750B3", item.f_three_1750);
                        dc.Add("B_F1750ND", item.f_nd_1750);

                        dc.Add("B_F2000B1", item.f_one_2000);
                        dc.Add("B_F2000B2", item.f_two_2000);
                        dc.Add("B_F2000B3", item.f_three_2000);
                        dc.Add("B_F2000ND", item.f_nd_2000);

                        dc.Add("Bp3B1", item.z_one_p3);
                        dc.Add("Bp3B2", item.z_two_p3);
                        dc.Add("Bp3B3", item.z_three_p3);
                        dc.Add("Bp3ND", item.z_nd_p3);

                        dc.Add("B_p3B1", item.f_one_p3);
                        dc.Add("B_p3B2", item.f_two_p3);
                        dc.Add("B_p3B3", item.f_three_p3);
                        dc.Add("B_p3ND", item.f_nd_p3);

                    }
                    else if (item.level == "C")
                    {
                        dc.Add("C_F250B1", item.z_one_250);
                        dc.Add("C_F250B2", item.z_two_250);
                        dc.Add("C_F250B3", item.z_three_250);
                        dc.Add("C_F250ND", item.z_nd_250);

                        dc.Add("C_F500B1", item.z_one_500);
                        dc.Add("C_F500B2", item.z_two_250);
                        dc.Add("C_F500B3", item.z_three_250);
                        dc.Add("C_F500ND", item.z_nd_500);

                        dc.Add("C_F750B1", item.z_one_750);
                        dc.Add("C_F750B2", item.z_two_750);
                        dc.Add("C_F750B3", item.z_three_750);
                        dc.Add("C_F750ND", item.z_nd_750);

                        dc.Add("C_F1000B1", item.z_one_1000);
                        dc.Add("C_F1000B2", item.z_two_1000);
                        dc.Add("C_F1000B3", item.z_three_1000);
                        dc.Add("C_F1000ND", item.z_nd_1000);

                        dc.Add("C_F1250B1", item.z_one_1250);
                        dc.Add("C_F1250B2", item.z_two_1250);
                        dc.Add("C_F1250B3", item.z_three_1250);
                        dc.Add("C_F1250ND", item.z_nd_1250);

                        dc.Add("C_F1500B1", item.z_one_1500);
                        dc.Add("C_F1500B2", item.z_two_1500);
                        dc.Add("C_F1500B3", item.z_three_1500);
                        dc.Add("C_F1500ND", item.z_nd_1500);

                        dc.Add("C_F1750B1", item.z_one_1750);
                        dc.Add("C_F1750B2", item.z_two_1750);
                        dc.Add("C_F1750B3", item.z_three_1750);
                        dc.Add("C_F1750ND", item.z_nd_1750);

                        dc.Add("C_F2000B1", item.z_one_2000);
                        dc.Add("C_F2000B2", item.z_two_2000);
                        dc.Add("C_F2000B3", item.z_three_2000);
                        dc.Add("C_F2000ND", item.z_nd_2000);

                        dc.Add("C_Z250C1", item.f_one_250);
                        dc.Add("C_Z250C2", item.f_two_250);
                        dc.Add("C_Z250C3", item.f_three_250);
                        dc.Add("C_Z250ND", item.f_nd_250);

                        dc.Add("C_Z500C1", item.f_one_500);
                        dc.Add("C_Z500C2", item.f_two_500);
                        dc.Add("C_Z500C3", item.f_three_500);
                        dc.Add("C_Z500ND", item.f_nd_500);

                        dc.Add("C_Z750C1", item.f_one_750);
                        dc.Add("C_Z750C2", item.f_two_750);
                        dc.Add("C_Z750C3", item.f_three_750);
                        dc.Add("C_Z750ND", item.f_nd_750);

                        dc.Add("C_Z1000C1", item.f_one_1000);
                        dc.Add("C_Z1000C2", item.f_two_1000);
                        dc.Add("C_Z1000C3", item.f_three_1000);
                        dc.Add("C_Z1000ND", item.f_nd_1000);

                        dc.Add("C_Z1250C1", item.f_one_1250);
                        dc.Add("C_Z1250C2", item.f_two_1250);
                        dc.Add("C_Z1250C3", item.f_three_1250);
                        dc.Add("C_Z1250ND", item.f_nd_1250);

                        dc.Add("C_Z1500C1", item.f_one_1500);
                        dc.Add("C_Z1500C2", item.f_two_1500);
                        dc.Add("C_Z1500C3", item.f_three_1500);
                        dc.Add("C_Z1500ND", item.f_nd_1500);

                        dc.Add("C_Z1750C1", item.f_one_1750);
                        dc.Add("C_Z1750C2", item.f_two_1750);
                        dc.Add("C_Z1750C3", item.f_three_1750);
                        dc.Add("C_Z1750ND", item.f_nd_1750);

                        dc.Add("C_Z2000C1", item.f_one_2000);
                        dc.Add("C_Z2000C2", item.f_two_2000);
                        dc.Add("C_Z2000C3", item.f_three_2000);
                        dc.Add("C_Z2000ND", item.f_nd_2000);

                        dc.Add("Cp3C1", item.z_one_p3);
                        dc.Add("Cp3C2", item.z_two_p3);
                        dc.Add("Cp3C3", item.z_three_p3);
                        dc.Add("Cp3ND", item.z_nd_p3);

                        dc.Add("C_p3C1", item.f_one_p3);
                        dc.Add("C_p3C2", item.f_two_p3);
                        dc.Add("C_p3C3", item.f_three_p3);
                        dc.Add("C_p3ND", item.f_nd_p3);
                    }
                }
            }

            var dt_kfy_res_Info = settings.dt_kfy_res_Info;
            if (dt_kfy_res_Info != null)
            {
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
                    GetQMLevel(settings, ref zhengtiLevel, ref kekaiLevel);

                    for (int i = 0; i < sortList.Count(); i++)
                    {
                        var qmOne = qmInfo.Find(t => t.Pa == sortList[i].val.ToString() && t.PaType == sortList[i].index);
                        if (qmOne == null)
                            continue;

                        if (i == 0)
                        {
                            dc.Add("S50FJ", qmOne.FJST);
                            dc.Add("S50GF", qmOne.GFZH);
                            dc.Add("S50ZD", qmOne.ZDST);
                            dc.Add("S50ZT", qmOne.MQZT);
                            dc.Add("S50KK", qmOne.KKST);
                        }
                        if (i == 1)
                        {
                            dc.Add("S100FJ", qmOne.FJST);
                            dc.Add("S100GF", qmOne.GFZH);
                            dc.Add("S100ZD", qmOne.ZDST);
                            dc.Add("S100ZT", qmOne.MQZT);
                            dc.Add("S100KK", qmOne.KKST);
                        }
                        if (i == 2)
                        {
                            dc.Add("S150FJ", qmOne.FJST);
                            dc.Add("S150GF", qmOne.GFZH);
                            dc.Add("S150ZD", qmOne.ZDST);
                            dc.Add("S150ZT", qmOne.MQZT);
                            dc.Add("S150KK", qmOne.KKST);
                        }
                        if (i == 3)
                        {
                            dc.Add("J100FJ", qmOne.FJST);
                            dc.Add("J100GF", qmOne.GFZH);
                            dc.Add("J100ZD", qmOne.ZDST);
                            dc.Add("J100ZT", qmOne.MQZT);
                            dc.Add("J100KK", qmOne.KKST);
                        }
                        if (i == 4)
                        {
                            dc.Add("J50FJ", qmOne.FJST);
                            dc.Add("J50GF", qmOne.GFZH);
                            dc.Add("J50ZD", qmOne.ZDST);
                            dc.Add("J50ZT", qmOne.MQZT);
                            dc.Add("J50KK", qmOne.KKST);
                        }
                        if (i == 5)
                        {
                            dc.Add("S_50FJ", qmOne.FJST);
                            dc.Add("S_50GF", qmOne.GFZH);
                            dc.Add("S_50ZD", qmOne.ZDST);
                            dc.Add("S_50ZT", qmOne.MQZT);
                            dc.Add("S_50KK", qmOne.KKST);
                        }
                        if (i == 6)
                        {
                            dc.Add("S_100FJ", qmOne.FJST);
                            dc.Add("S_100GF", qmOne.GFZH);
                            dc.Add("S_100ZD", qmOne.ZDST);
                            dc.Add("S_100ZT", qmOne.MQZT);
                            dc.Add("S_100KK", qmOne.KKST);
                        }
                        if (i == 7)
                        {
                            dc.Add("S_150FJ", qmOne.FJST);
                            dc.Add("S_150GF", qmOne.GFZH);
                            dc.Add("S_150ZD", qmOne.ZDST);
                            dc.Add("S_150ZT", qmOne.MQZT);
                            dc.Add("S_150KK", qmOne.KKST);
                        }
                        if (i == 8)
                        {
                            dc.Add("J_100FJ", qmOne.FJST);
                            dc.Add("J_100GF", qmOne.GFZH);
                            dc.Add("J_100ZD", qmOne.ZDST);
                            dc.Add("J_100ZT", qmOne.MQZT);
                            dc.Add("J_100KK", qmOne.KKST);
                        }
                        if (i == 9)
                        {
                            dc.Add("J_50FJ", qmOne.FJST);
                            dc.Add("J_50GF", qmOne.GFZH);
                            dc.Add("J_50ZD", qmOne.ZDST);
                            dc.Add("J_50ZT", qmOne.MQZT);
                            dc.Add("J_50KK", qmOne.KKST);
                        }
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
                    GetQM_GCLevel(settings, ref zhengtiLevel, ref kekaiLevel);
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
                        sm_Pa = 250;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 500)
                    {
                        sm_Pa = 350;
                    }
                    if (settings.dt_sm_Info.sm_Pa == 700)
                    {
                        sm_Pa = 500;
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
                        sm_Pa2 = 500;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 1000)
                    {
                        sm_Pa2 = 750;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 1500)
                    {
                        sm_Pa2 = 1000;
                    }
                    if (settings.dt_sm_Info.sm_Pa2 == 2000)
                    {
                        sm_Pa2 = 1500;
                    }
                }

                if (settings.dt_sm_Info.sm_Pa == 250)
                {
                    dc.Add("可开250", settings.dt_sm_Info.sm_PaDesc);
                }
                if (settings.dt_sm_Info.sm_Pa == 350)
                {
                    dc.Add("可开350", settings.dt_sm_Info.sm_PaDesc);
                }
                if (settings.dt_sm_Info.sm_Pa == 500)
                {
                    dc.Add("可开500", settings.dt_sm_Info.sm_PaDesc);
                }
                if (settings.dt_sm_Info.sm_Pa == 700)
                {
                    dc.Add("可开700", settings.dt_sm_Info.sm_PaDesc);
                }
                if (settings.dt_sm_Info.sm_Pa == 1000)
                {
                    dc.Add("可开1000", settings.dt_sm_Info.sm_PaDesc);
                }

                if (settings.dt_sm_Info.sm_Pa2 == 500)
                {
                    dc.Add("固定500", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (settings.dt_sm_Info.sm_Pa2 == 700)
                {
                    dc.Add("固定700", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (settings.dt_sm_Info.sm_Pa2 == 1000)
                {
                    dc.Add("固定1000", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (settings.dt_sm_Info.sm_Pa2 == 1500)
                {
                    dc.Add("固定1500", settings.dt_sm_Info.sm_PaDesc2);
                }
                if (settings.dt_sm_Info.sm_Pa2 == 2000)
                {
                    dc.Add("固定2000", settings.dt_sm_Info.sm_PaDesc2);
                }

                if (settings.dt_sm_Info.sm_Pa != null)
                {
                    dc.Add("可开未渗漏风压", sm_Pa.ToString());

                    var level = Formula.GetWaterTightLevel_KeKaiQi(sm_Pa);
                    dc.Add("可开水密性能等级", level.ToString());
                }
                else
                {
                    dc.Add("可开未渗漏风压", "--");
                    dc.Add("可开水密性能等级", "--");

                }
                if (settings.dt_sm_Info.sm_Pa2 != null)
                {
                    dc.Add("固定未渗漏风压", sm_Pa2.ToString());
                    var level = Formula.GetWaterTightLevel_GuDing(sm_Pa2);

                    dc.Add("固定水密等级", level.ToString());
                }
                else
                {
                    dc.Add("固定未渗漏风压", "--");
                    dc.Add("固定水密等级", "--");
                }
            }
            dc.Add("检测方法2", "");

            dc.Add("水密备注", "");
            dc.Add("幕墙整体备注", "");
            return dc;
        }


        /// <summary>
        /// 气密正常 等级
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="zhengtiLevel"></param>
        /// <param name="kekaiLevel"></param>
        private void GetQMLevel(Model_dt_Settings settings, ref int zhengtiLevel, ref int kekaiLevel)
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
                var fcValue_z = (double.Parse(zheng_s.KKST) + double.Parse(zheng_j.KKST)) / 2;
                var fcValue_f = (double.Parse(fu_s.KKST) + double.Parse(fu_j.KKST)) / 2;

                //面积
                var mjValue_z = (double.Parse(zheng_s.MQZT) + double.Parse(zheng_j.MQZT)) / 2;
                var mjValue_f = (double.Parse(fu_s.MQZT) + double.Parse(fu_j.MQZT)) / 2;

                var fcValue = 0d;
                var mjValue = 0d;

                if (fcValue_z > fcValue_f)
                    fcValue = Formula.GetIndexStichLength(fcValue_z, fcValue_z, daqiyali, kekaifengchang, dangqianwendu);
                else
                    fcValue = Formula.GetIndexStichLength(fcValue_f, fcValue_f, daqiyali, kekaifengchang, dangqianwendu);

                if (mjValue_z > mjValue_f)
                    mjValue = Formula.GetIndexStitchArea(mjValue_z, mjValue_z, daqiyali, shijianmianji, dangqianwendu);
                else
                    mjValue = Formula.GetIndexStitchArea(mjValue_f, mjValue_f, daqiyali, shijianmianji, dangqianwendu);


                kekaiLevel = Formula.GetStitchLengthLevel(fcValue);

                zhengtiLevel = Formula.GetAreaLevel(mjValue);
            }
        }

        /// <summary>
        /// 工程
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="zhengtiLevel"></param>
        /// <param name="kekaiLevel"></param>
        private void GetQM_GCLevel(Model_dt_Settings settings, ref int zhengtiLevel, ref int kekaiLevel)
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
                var zhengya = settings.dt_qm_Info.Find(t => t.PaType == 3);
                var fuya = settings.dt_qm_Info.Find(t => t.PaType == 4);


                //逢长
                var fcValue_z = double.Parse(zhengya.KKST);
                var fcValue_f = double.Parse(fuya.KKST);

                //面积
                var mjValue_z = double.Parse(zhengya.MQZT);
                var mjValue_f = double.Parse(fuya.MQZT);

                var fcValue = 0d;
                var mjValue = 0d;

                if (fcValue_z > fcValue_f)
                    fcValue = Formula.GetIndexStichLength(fcValue_z, fcValue_z, daqiyali, kekaifengchang, dangqianwendu);
                else
                    fcValue = Formula.GetIndexStichLength(fcValue_f, fcValue_f, daqiyali, kekaifengchang, dangqianwendu);

                if (mjValue_z > mjValue_f)
                    mjValue = Formula.GetIndexStitchArea(mjValue_z, mjValue_z, daqiyali, shijianmianji, dangqianwendu);
                else
                    mjValue = Formula.GetIndexStitchArea(mjValue_f, mjValue_f, daqiyali, shijianmianji, dangqianwendu);


                zhengtiLevel = Formula.GetStitchLengthLevel(fcValue);

                kekaiLevel = Formula.GetAreaLevel(mjValue);
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

        private void ImageLine(string file, string name, List<double> zitem, List<double> fitem)
        {
            int height = 350, width = 350;
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
                g.FillRectangle(Brushes.AliceBlue, 0, 0, width, height);
                Brush brush1 = new SolidBrush(Color.Black);
                Brush brush2 = new SolidBrush(Color.SaddleBrown);

                g.DrawString(name, font1, brush1, new PointF(85, 30));
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Black), 0, 0, image.Width - 1, image.Height - 1);

                Pen mypen = new Pen(brush, 1);

                //绘制线条
                //绘制纵向线条
                //int x = 15;
                //for (int i = 0; i < 21; i++)
                //{
                //    g.DrawLine(mypen, x, 80, x, 335);
                //    x = x + 15;
                //}

                int x = 15;
                for (int i = 0; i < 21; i++)
                {
                    g.DrawLine(mypen, x, 196, x, 204);
                    x = x + 15;
                }

                Pen mypen1 = new Pen(Color.Black, 3);
                x = 165;
                g.DrawLine(mypen1, x, 80, x, 335);

                //绘制横向线条
                //int y = 15;
                //for (int i = 0; i < 18; i++)
                //{
                //    if (i == 0)
                //    {
                //        g.DrawLine(mypen, 15, 80, 330, 80);
                //    }
                //    else
                //    {
                //        g.DrawLine(mypen, 15, 80 + y, 330, 80 + y);
                //        y = y + 15;
                //    }
                //}

                int y = 15;
                for (int i = 0; i < 18; i++)
                {
                    if (i == 0)
                    {
                        g.DrawLine(mypen, 167, 80, 173, 80);

                    }
                    else
                    {
                        g.DrawLine(mypen, 167, 80 + y, 173, 80 + y);
                        y = y + 15;
                    }
                }
                y = 200;
                g.DrawLine(mypen1, 15, y, 330, y);

                // x轴
                String[] n = { "-10", "-9", "-8", "-7", "-6", "-5", "-4", "-3", "-2", "-1", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                x = 11;
                for (int i = 0; i < 21; i++)
                {
                    if (i % 2 == 0)
                    {
                        g.DrawString(n[i].ToString(), font, Brushes.Black, x, 205); //设置文字内容及输出位置
                    }
                    x = x + 15;
                }

                //y轴
                String[] m = { "2000", "1750", "1500", "1250", "1000", "750", "500", "250", "0", "-250", "-500", "-750", "-1000", "-1250", "-1500", "-1750", "-2000" };
                y = 74;
                for (int i = 0; i < 17; i++)
                {
                    if (m[i] == "0")
                    { y = y + 15; continue; }

                    if (Convert.ToInt32(m[i]) > -1)
                    {
                        g.DrawString(m[i].ToString(), font, Brushes.Black, 130, y); //设置文字内容及输出位置
                    }
                    else
                    {
                        g.DrawString(m[i].ToString(), font, Brushes.Black, 173, y); //设置文字内容及输出位置
                    }
                    y = y + 15;
                }

                //double[] z_item1 = new double[9] { 0, 0.55, 1.19, 1.85, 2.40, 3.15, 3.65, 4.01, 4.80 };
                //double[] z_item2 = new double[9] { 0, 0.5, 1.0, 1.48, 2.01, 2.50, 2.99, 3.49, 4.88 };

                //显示折线效果
                System.Drawing.Font font3 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                SolidBrush mybrush = new SolidBrush(Color.Red);
                //正压
                System.Drawing.Point[] points1 = new System.Drawing.Point[9];
                Pen mypen2 = new Pen(Color.Black, 1);
                double initialx = 165;
                double initialy = 200;

                for (int i = 0; i < zitem.Count; i++)
                {
                    points1[i].X = Convert.ToInt32(initialx + zitem[i] * 10 * 1.5);
                    points1[i].Y = (int)initialy - i * 15;
                    g.DrawRectangle(mypen2, points1[i].X - 1, points1[i].Y - 1, 2, 2);
                }
                g.DrawLines(mypen2, points1); //绘制折线

                //绘制数字
                for (int i = 1; i < zitem.Count; i++)
                {
                    g.DrawString(fitem[i].ToString(), font3, Brushes.Red, 15, points1[i].Y - 20);
                }

                //负压
                System.Drawing.Point[] points2 = new System.Drawing.Point[9];
                Pen mypen3 = new Pen(Color.Black, 1);

                for (int i = 0; i < 9; i++)
                {
                    points2[i].X = Convert.ToInt32(initialx - fitem[i] * 10 * 1.5);
                    points2[i].Y = (int)initialy + i * 15;
                    g.DrawRectangle(mypen3, points2[i].X - 1, points2[i].Y - 1, 2, 2);
                }
                g.DrawLines(mypen3, points2); //绘制折线

                //绘制数字
                for (int i = 1; i < fitem.Count; i++)
                {
                    g.DrawString("-" + fitem[i].ToString(), font3, Brushes.Red, 15, 205 + 15 * i);
                }

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
        #endregion

    }

    public class SortInfo
    {
        public int val { get; set; }
        public int index { get; set; }
    }
}
