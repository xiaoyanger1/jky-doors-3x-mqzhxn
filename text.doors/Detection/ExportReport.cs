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

            dc.Add("OLE_LINK1", "1");
            dc.Add("OLE_LINK10", "2");
            dc.Add("OLE_LINK11", "3");
            dc.Add("OLE_LINK12", "4");
            dc.Add("OLE_LINK13", "5");
            dc.Add("OLE_LINK14", "6");
            dc.Add("OLE_LINK15", "7");
            dc.Add("OLE_LINK16", "8");
            dc.Add("OLE_LINK17", "9");
            dc.Add("OLE_LINK18", "10");
            dc.Add("OLE_LINK19", "11");
            dc.Add("OLE_LINK2", "12");
            dc.Add("OLE_LINK20", "13");
            dc.Add("OLE_LINK21", "14");
            dc.Add("OLE_LINK22", "15");
            dc.Add("OLE_LINK23", "16");
            dc.Add("OLE_LINK24", "17");
            dc.Add("OLE_LINK25", "18");
            dc.Add("OLE_LINK26", "19");
            dc.Add("OLE_LINK27", "20");
            dc.Add("OLE_LINK28", "21");
            dc.Add("OLE_LINK29", "22");
            dc.Add("OLE_LINK3", "23");
            dc.Add("OLE_LINK30", "24");
            dc.Add("OLE_LINK31", "25");
            dc.Add("OLE_LINK32", "26");
            dc.Add("OLE_LINK33", "27");
            dc.Add("OLE_LINK34", "28");
            dc.Add("OLE_LINK35", "29");
            dc.Add("OLE_LINK36", "30");
            dc.Add("OLE_LINK37", "31");
            dc.Add("OLE_LINK38", "32");
            dc.Add("OLE_LINK39", "33");
            dc.Add("OLE_LINK4", "34");
            dc.Add("OLE_LINK40", "35");
            dc.Add("OLE_LINK41", "36");
            dc.Add("OLE_LINK42", "37");
            dc.Add("OLE_LINK43", "38");
            dc.Add("OLE_LINK44", "39");
            dc.Add("OLE_LINK45", "40");
            dc.Add("OLE_LINK46", "41");
            dc.Add("OLE_LINK47", "42");
            dc.Add("OLE_LINK48", "43");
            dc.Add("OLE_LINK49", "44");
            dc.Add("OLE_LINK5", "45");
            dc.Add("OLE_LINK50", "46");
            dc.Add("OLE_LINK51", "47");
            dc.Add("OLE_LINK6", "48");
            dc.Add("OLE_LINK7", "49");
            dc.Add("OLE_LINK8", "50");
            dc.Add("OLE_LINK9", "51");
            dc.Add("幕墙平面变形平面变形级别", "52");
            dc.Add("幕墙平面变形平面变形记录", "53");
            dc.Add("幕墙检测条件变形压力级别1", "54");
            dc.Add("幕墙检测条件变形压力级别1重复2", "55");
            dc.Add("幕墙检测条件变形压力级别1重复4", "56");
            dc.Add("幕墙检测条件变形压力级别2", "57");
            dc.Add("幕墙检测条件变形压力级别2重复2", "58");
            dc.Add("幕墙检测条件变形压力级别2重复4", "59");
            dc.Add("幕墙检测条件变形压力级别3", "60");
            dc.Add("幕墙检测条件变形压力级别3重复2", "61");
            dc.Add("幕墙检测条件变形压力级别3重复4", "62");
            dc.Add("幕墙检测条件变形压力级别4", "63");
            dc.Add("幕墙检测条件变形压力级别4重复2", "64");
            dc.Add("幕墙检测条件变形压力级别4重复4", "65");
            dc.Add("幕墙检测条件变形压力级别5", "66");
            dc.Add("幕墙检测条件变形压力级别5重复2", "67");
            dc.Add("幕墙检测条件变形压力级别5重复4", "68");
            dc.Add("幕墙检测条件变形压力级别6", "69");
            dc.Add("幕墙检测条件变形压力级别6重复2", "70");
            dc.Add("幕墙检测条件变形压力级别6重复4", "71");
            dc.Add("幕墙检测条件变形压力级别7", "72");
            dc.Add("幕墙检测条件变形压力级别7重复2", "73");
            dc.Add("幕墙检测条件变形压力级别7重复4", "74");
            dc.Add("幕墙检测条件变形压力级别8", "75");
            dc.Add("幕墙检测条件变形压力级别8重复2", "76");
            dc.Add("幕墙检测条件变形压力级别8重复4", "77");
            dc.Add("幕墙检测条件可开水密保持风压", "78");
            dc.Add("幕墙检测条件可开综合单位缝长渗透量", "79");
            dc.Add("幕墙检测条件可开缝长", "80");
            dc.Add("幕墙检测条件强度记录", "81");
            dc.Add("幕墙检测条件强度记录重复1", "82");
            dc.Add("幕墙检测条件强度记录重复2", "83");
            dc.Add("幕墙检测条件当前温度", "84");
            dc.Add("幕墙检测条件整体综合单位面积渗透量", "85");
            dc.Add("幕墙检测条件样品编号重复1", "86");
            dc.Add("幕墙检测条件水密检测方法", "87");
            dc.Add("幕墙检测条件淋水量", "88");
            dc.Add("幕墙检测条件综合可开气密等级", "89");
            dc.Add("幕墙检测条件综合面积气密等级", "90");


            dc.Add("幕墙气密检测附加正降压100帕风速", "");
            dc.Add("幕墙气密检测附加负升压100帕风速", "");
            dc.Add("幕墙气密检测附加负降压100帕风速", "");
            dc.Add("幕墙气密检测附加负降压50帕风速", "");
            dc.Add("幕墙水密检测可开1000帕部位", "");
            dc.Add("幕墙水密检测可开250帕状态", "");
            dc.Add("幕墙水密检测可开250帕部位", "");
            dc.Add("幕墙水密检测固定2000帕部位", "");
            dc.Add("幕墙水密检测水密记录", "");
            dc.Add("强度检测杆A正1250帕位移A1", "");
            dc.Add("强度检测杆A正1500帕位移A1", "");
            dc.Add("强度检测杆A正1750帕位移A1", "");
            dc.Add("强度检测杆A正1750帕位移A2", "");
            dc.Add("强度检测杆A正1750帕位移A3", "");
            dc.Add("强度检测杆A正1750帕挠度", "");
            dc.Add("强度检测杆A正2000帕位移A1", "");
            dc.Add("强度检测杆A正2000帕位移A2", "");
            dc.Add("强度检测杆A正2000帕位移A3", "");
            dc.Add("强度检测杆A正2000帕挠度", "");
            dc.Add("强度检测杆A正压P1", "");
            dc.Add("强度检测杆A负1250帕位移A1", "");
            dc.Add("强度检测杆A负1500帕位移A1", "");
            dc.Add("强度检测杆A负1750帕位移A1", "");
            dc.Add("强度检测杆A负1750帕位移A2", "");
            dc.Add("强度检测杆A负1750帕位移A3", "");
            dc.Add("强度检测杆A负1750帕挠度", "");
            dc.Add("强度检测杆A负2000帕位移A1", "");
            dc.Add("强度检测杆A负2000帕位移A2", "");
            dc.Add("强度检测杆A负2000帕位移A3", "");
            dc.Add("强度检测杆A负2000帕挠度", "");
            dc.Add("强度检测杆B负1250帕位移B1", "");
            dc.Add("强度检测杆B负1500帕位移B1", "");
            dc.Add("强度检测杆B负1750帕位移B1", "");
            dc.Add("强度检测杆B负1750帕位移B2", "");
            dc.Add("强度检测杆B负1750帕位移B3", "");
            dc.Add("强度检测杆B负1750帕挠度", "");
            dc.Add("强度检测杆B负2000帕位移B1", "");
            dc.Add("强度检测杆B负2000帕位移B2", "");
            dc.Add("强度检测杆B负2000帕位移B3", "");
            dc.Add("强度检测杆B负2000帕挠度", "");
            dc.Add("强度检测杆C回归系数", "");
            dc.Add("强度检测杆C正1250帕位移C1", "");
            dc.Add("强度检测杆C正1500帕位移C1", "");
            dc.Add("强度检测杆C正1750帕位移C1", "");
            dc.Add("强度检测杆C正1750帕位移C2", "");
            dc.Add("强度检测杆C正1750帕位移C3", "");
            dc.Add("强度检测杆C正1750帕挠度", "");
            dc.Add("强度检测杆C正2000帕位移C1", "");
            dc.Add("强度检测杆C正2000帕位移C2", "");
            dc.Add("强度检测杆C正2000帕位移C3", "");
            dc.Add("强度检测杆C正2000帕挠度", "");
            dc.Add("强度检测杆C负1250帕位移C1", "");
            dc.Add("强度检测杆C负1500帕位移C1", "");
            dc.Add("强度检测杆C负1750帕位移C1", "");
            dc.Add("强度检测杆C负1750帕位移C2", "");
            dc.Add("强度检测杆C负1750帕位移C3", "");
            dc.Add("强度检测杆C负1750帕挠度", "");
            dc.Add("强度检测杆C负2000帕位移C1", "");
            dc.Add("强度检测杆C负2000帕位移C2", "");
            dc.Add("强度检测杆C负2000帕位移C3", "");
            dc.Add("强度检测杆C负2000帕挠度 ", "");


            dc.Add("挠度曲线杆A300，540，12，12", "92");
            dc.Add("挠度曲线杆B300，540，12，12", "93");
            dc.Add("挠度曲线杆C300，540，12，12", "94");







            //dc.Add("OLE_LINK1", "1");
            //dc.Add("OLE_LINK10", "2");
            //dc.Add("OLE_LINK11", "3");
            //dc.Add("OLE_LINK12", "4");
            //dc.Add("OLE_LINK13", "5");
            //dc.Add("OLE_LINK14", "6");
            //dc.Add("OLE_LINK15", "7");
            //dc.Add("OLE_LINK16", "8");
            //dc.Add("OLE_LINK17", "9");
            //dc.Add("OLE_LINK18", "10");
            //dc.Add("OLE_LINK19", "11");
            //dc.Add("OLE_LINK2", "12");
            //dc.Add("OLE_LINK20", "13");
            //dc.Add("OLE_LINK21", "14");
            //dc.Add("OLE_LINK22", "15");
            //dc.Add("OLE_LINK23", "16");
            //dc.Add("OLE_LINK24", "17");
            //dc.Add("OLE_LINK25", "18");
            //dc.Add("OLE_LINK26", "19");
            //dc.Add("OLE_LINK27", "20");
            //dc.Add("OLE_LINK28", "");
            //dc.Add("OLE_LINK29", "");
            //dc.Add("OLE_LINK3", "");
            //dc.Add("OLE_LINK30", "");
            //dc.Add("OLE_LINK31", "");
            //dc.Add("OLE_LINK32", "");
            //dc.Add("OLE_LINK33", "");
            //dc.Add("OLE_LINK34", "");
            //dc.Add("OLE_LINK35", "");
            //dc.Add("OLE_LINK36", "");
            //dc.Add("OLE_LINK37", "");
            //dc.Add("OLE_LINK38", "");
            //dc.Add("OLE_LINK39", "");
            //dc.Add("OLE_LINK4", "");
            //dc.Add("OLE_LINK40", "");
            //dc.Add("OLE_LINK41", "");
            //dc.Add("OLE_LINK42", "");
            //dc.Add("OLE_LINK43", "");
            //dc.Add("OLE_LINK44", "");
            //dc.Add("OLE_LINK45", "");
            //dc.Add("OLE_LINK46", "");
            //dc.Add("OLE_LINK47", "");
            //dc.Add("OLE_LINK48", "");
            //dc.Add("OLE_LINK49", "");
            //dc.Add("OLE_LINK5", "");
            //dc.Add("OLE_LINK50", "");
            //dc.Add("OLE_LINK51", "");
            //dc.Add("OLE_LINK6", "");
            //dc.Add("OLE_LINK7", "");
            //dc.Add("OLE_LINK8", "");
            //dc.Add("OLE_LINK9", "");
            //dc.Add("幕墙平面变形平面变形级别", "");
            //dc.Add("幕墙平面变形平面变形记录", "");
            //dc.Add("幕墙检测条件变形压力级别1", "222222222");
            //dc.Add("幕墙检测条件变形压力级别1重复2", "22222222");
            //dc.Add("幕墙检测条件变形压力级别1重复4", "");
            //dc.Add("幕墙检测条件变形压力级别2", "");
            //dc.Add("幕墙检测条件变形压力级别2重复2", "");
            //dc.Add("幕墙检测条件变形压力级别2重复4", "");
            //dc.Add("幕墙检测条件变形压力级别3", "");
            //dc.Add("幕墙检测条件变形压力级别3重复2", "");
            //dc.Add("幕墙检测条件变形压力级别3重复4", "");
            //dc.Add("幕墙检测条件变形压力级别4", "");
            //dc.Add("幕墙检测条件变形压力级别4重复2", "");
            //dc.Add("幕墙检测条件变形压力级别4重复4", "");
            //dc.Add("幕墙检测条件变形压力级别5", "");
            //dc.Add("幕墙检测条件变形压力级别5重复2", "");
            //dc.Add("幕墙检测条件变形压力级别5重复4", "");
            //dc.Add("幕墙检测条件变形压力级别6", "");
            //dc.Add("幕墙检测条件变形压力级别6重复2", "");
            //dc.Add("幕墙检测条件变形压力级别6重复4", "");
            //dc.Add("幕墙检测条件变形压力级别7", "");
            //dc.Add("幕墙检测条件变形压力级别7重复2", "");
            //dc.Add("幕墙检测条件变形压力级别7重复4", "");
            //dc.Add("幕墙检测条件变形压力级别8", "");
            //dc.Add("幕墙检测条件变形压力级别8重复2", "");
            //dc.Add("幕墙检测条件变形压力级别8重复4", "");
            //dc.Add("幕墙检测条件可开水密保持风压", "");
            //dc.Add("幕墙检测条件可开综合单位缝长渗透量", "");
            //dc.Add("幕墙检测条件可开缝长", "");
            //dc.Add("幕墙检测条件强度记录", "");
            //dc.Add("幕墙检测条件强度记录重复1", "");
            //dc.Add("幕墙检测条件强度记录重复2", "");
            //dc.Add("幕墙检测条件当前温度", "");
            //dc.Add("幕墙检测条件整体综合单位面积渗透量", "");
            //dc.Add("幕墙检测条件样品编号重复1", "");
            //dc.Add("幕墙检测条件水密检测方法", "");
            //dc.Add("幕墙检测条件淋水量", "");
            //dc.Add("幕墙检测条件综合可开气密等级", "");
            //dc.Add("幕墙检测条件综合面积气密等级", "");


            //dc.Add("幕墙气密检测附加正降压100帕风速", "");
            //dc.Add("幕墙气密检测附加负升压100帕风速", "");
            //dc.Add("幕墙气密检测附加负降压100帕风速", "");
            //dc.Add("幕墙气密检测附加负降压50帕风速", "");
            //dc.Add("幕墙水密检测可开1000帕部位", "");
            //dc.Add("幕墙水密检测可开250帕状态", "");
            //dc.Add("幕墙水密检测可开250帕部位", "");
            //dc.Add("幕墙水密检测固定2000帕部位", "");
            //dc.Add("幕墙水密检测水密记录", "");
            //dc.Add("强度检测杆A正1250帕位移A1", "");
            //dc.Add("强度检测杆A正1500帕位移A1", "");
            //dc.Add("强度检测杆A正1750帕位移A1", "");
            //dc.Add("强度检测杆A正1750帕位移A2", "");
            //dc.Add("强度检测杆A正1750帕位移A3", "");
            //dc.Add("强度检测杆A正1750帕挠度", "");
            //dc.Add("强度检测杆A正2000帕位移A1", "");
            //dc.Add("强度检测杆A正2000帕位移A2", "");
            //dc.Add("强度检测杆A正2000帕位移A3", "");
            //dc.Add("强度检测杆A正2000帕挠度", "");
            //dc.Add("强度检测杆A正压P1", "");
            //dc.Add("强度检测杆A负1250帕位移A1", "");
            //dc.Add("强度检测杆A负1500帕位移A1", "");
            //dc.Add("强度检测杆A负1750帕位移A1", "");
            //dc.Add("强度检测杆A负1750帕位移A2", "");
            //dc.Add("强度检测杆A负1750帕位移A3", "");
            //dc.Add("强度检测杆A负1750帕挠度", "");
            //dc.Add("强度检测杆A负2000帕位移A1", "");
            //dc.Add("强度检测杆A负2000帕位移A2", "");
            //dc.Add("强度检测杆A负2000帕位移A3", "");
            //dc.Add("强度检测杆A负2000帕挠度", "");
            //dc.Add("强度检测杆B负1250帕位移B1", "");
            //dc.Add("强度检测杆B负1500帕位移B1", "");
            //dc.Add("强度检测杆B负1750帕位移B1", "");
            //dc.Add("强度检测杆B负1750帕位移B2", "");
            //dc.Add("强度检测杆B负1750帕位移B3", "");
            //dc.Add("强度检测杆B负1750帕挠度", "");
            //dc.Add("强度检测杆B负2000帕位移B1", "");
            //dc.Add("强度检测杆B负2000帕位移B2", "");
            //dc.Add("强度检测杆B负2000帕位移B3", "");
            //dc.Add("强度检测杆B负2000帕挠度", "");
            //dc.Add("强度检测杆C回归系数", "");
            //dc.Add("强度检测杆C正1250帕位移C1", "");
            //dc.Add("强度检测杆C正1500帕位移C1", "");
            //dc.Add("强度检测杆C正1750帕位移C1", "");
            //dc.Add("强度检测杆C正1750帕位移C2", "");
            //dc.Add("强度检测杆C正1750帕位移C3", "");
            //dc.Add("强度检测杆C正1750帕挠度", "");
            //dc.Add("强度检测杆C正2000帕位移C1", "");
            //dc.Add("强度检测杆C正2000帕位移C2", "");
            //dc.Add("强度检测杆C正2000帕位移C3", "");
            //dc.Add("强度检测杆C正2000帕挠度", "");
            //dc.Add("强度检测杆C负1250帕位移C1", "");
            //dc.Add("强度检测杆C负1500帕位移C1", "");
            //dc.Add("强度检测杆C负1750帕位移C1", "");
            //dc.Add("强度检测杆C负1750帕位移C2", "");
            //dc.Add("强度检测杆C负1750帕位移C3", "");
            //dc.Add("强度检测杆C负1750帕挠度", "");
            //dc.Add("强度检测杆C负2000帕位移C1", "");
            //dc.Add("强度检测杆C负2000帕位移C2", "");
            //dc.Add("强度检测杆C负2000帕位移C3", "");
            //dc.Add("强度检测杆C负2000帕挠度 ", "");


            //dc.Add("挠度曲线杆A300，540，12，12", "");
            //dc.Add("挠度曲线杆B300，540，12，12", "");
            //dc.Add("挠度曲线杆C300，540，12，12", "");




            return dc;
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
}
