using text.doors.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Default;
using Young.Core.SQLite;

namespace text.doors.Detection
{
    public partial class SensorSet : Form
    {
        private SerialPortClient _serialPortClient;
        public SensorSet(SerialPortClient serialPortLink)
        {
            InitializeComponent();
            CreateViewItem();
            _serialPortClient = serialPortLink;
        }


        public void CreateViewItem()
        {
            this.lv_cjz.Columns.Add(new ColumnHeader() { Text = "采集值", Width = 120, TextAlign = HorizontalAlignment.Left });
            this.lv_cjz.Columns.Add(new ColumnHeader() { Text = "采集时间", Width = 135, TextAlign = HorizontalAlignment.Left });
            this.lv_cjz.GridLines = true;

            this.lv_list.Columns.Add(new ColumnHeader() { Text = "ID", Width = 0, TextAlign = HorizontalAlignment.Left });
            this.lv_list.Columns.Add(new ColumnHeader() { Text = "采样标准量", Width = 120, TextAlign = HorizontalAlignment.Left });
            this.lv_list.Columns.Add(new ColumnHeader() { Text = "标准物理量", Width = 135, TextAlign = HorizontalAlignment.Left });
            this.lv_list.GridLines = true;

        }

        private void btn_collection_Click(object sender, EventArgs e)
        {
            var name = cbb_type.Text;
            if (string.IsNullOrWhiteSpace(cbb_type.Text))
            {
                MessageBox.Show("警告", "请选择传感器！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double res = 0;
            ListViewItem lvi = new ListViewItem();

            if (name == "风速传感器 (米/秒)")
            {
                //todo:改
                //res = _serialPortClient.GetFSXS();
                res = RegisterData.WindSpeed_Value;
            }
            if (name == "差压传感器高(Pa)")
            {
                //todo:改
                //res = _serialPortClient.GetCY_High();
                res = RegisterData.CY_High_Value;
            }
            if (name == "差压传感器低(Pa)")
            {
                //todo:改
                // res = _serialPortClient.GetCY_Low();
                res = RegisterData.CY_Low_Value;
            }
            if (name == "温度传感器(℃)")
            {
                //todo:改
                // res = _serialPortClient.GetWDXS();
                res = RegisterData.Temperature_Value;
            }
            if (name == "大气压力传感器(KPa)")
            {
                //todo:改
                //res = _serialPortClient.GetDQYLXS();
                res = RegisterData.AtmospherePa_Value;
            }
            if (name == "位移传感器1(mm)")
            {
                res = RegisterData.DisplaceA1;
            }
            if (name == "位移传感器2(mm)")
            {
                res = RegisterData.DisplaceA2;
            }
            if (name == "位移传感器3(mm)")
            {
                res = RegisterData.DisplaceA3;
            }
            if (name == "位移传感器4(mm)")
            {
                res = RegisterData.DisplaceB1;
            }
            if (name == "位移传感器5(mm)")
            {
                res = RegisterData.DisplaceB2;
            }
            if (name == "位移传感器6(mm)")
            {
                res = RegisterData.DisplaceB3;
            }
            if (name == "位移传感器7(mm)")
            {
                res = RegisterData.DisplaceC1;
            }
            if (name == "位移传感器8(mm)")
            {
                res = RegisterData.DisplaceC2;
            }
            if (name == "位移传感器9(mm)")
            {
                res = RegisterData.DisplaceC3;
            }
            lvi.Text = res.ToString();
            lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss"));

            this.lv_cjz.Items.Add(lvi);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            this.lv_cjz.Items.Clear();
        }

        private void btn_Compute_Click(object sender, EventArgs e)
        {
            if (this.lv_cjz.Items.Count > 0)
            {
                List<double> list = new List<double>();

                foreach (ListViewItem lv in this.lv_cjz.Items)
                {
                    list.Add(double.Parse((lv.Text)));
                }
                txt_ave.Text = Math.Round(list.Average(), 2).ToString();
            }
        }

        /// <summary>
        /// 添加标定区间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbb_type.Text))
            {
                MessageBox.Show("警告", "请选择传感器！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PublicEnum.DemarcateType? enum_Demarcate = GetEnum(cbb_type.Text);

            string sql = string.Format("insert into Demarcate_Dict(Enum,D_Key,D_Value) values('{0}','{1}','{2}')",
                enum_Demarcate.ToString(), txt_Key.Text, txt_ave.Text);

            bool isOk = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            if (isOk)
            {
                MessageBox.Show("添加成功！");
            }

        }

        private void BindLvList()
        {
            if (string.IsNullOrWhiteSpace(cbb_type.Text))
            {
                MessageBox.Show("警告", "请选择传感器！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void cbb_cgq_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbb_cgq.Text))
            {
                MessageBox.Show("警告", "请选择传感器！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            updateLvList();
        }

        /// <summary>
        /// 重新绑定列表
        /// </summary>
        private void updateLvList()
        {
            this.lv_list.Items.Clear();

            PublicEnum.DemarcateType? enum_Demarcate = GetEnum(cbb_cgq.Text);

            string sql = string.Format("select * from Demarcate_Dict where enum ='{0}'", enum_Demarcate.ToString());

            DataTable dt = SQLiteHelper.ExecuteDataset(sql).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();

                    lvi.Text = dt.Rows[i]["ID"].ToString();

                    lvi.SubItems.Add(dt.Rows[i]["D_Value"].ToString());
                    lvi.SubItems.Add(dt.Rows[i]["D_Key"].ToString());
                    this.lv_list.Items.Add(lvi);
                }
            }
        }

        private PublicEnum.DemarcateType? GetEnum(string name)
        {
            PublicEnum.DemarcateType? enum_Demarcate = null;

            if (name == "风速传感器 (米/秒)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.风速传感器;
            }
            if (name == "差压传感器高(Pa)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.差压传感器高;
            }

            if (name == "差压传感器低(Pa)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.差压传感器低;
            }
            if (name == "温度传感器(℃)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.温度传感器;
            }
            if (name == "大气压力传感器(KPa)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.大气压力传感器;
            }
            if (name == "位移传感器A1(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器A1;
            }
            if (name == "位移传感器A2(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器A2;
            }
            if (name == "位移传感器A3(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器A3;
            }
            if (name == "位移传感器B1(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器B1;
            }
            if (name == "位移传感器B2(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器B2;
            }
            if (name == "位移传感器B3(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器B3;
            }
            if (name == "位移传感器C1(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器C1;
            }
            if (name == "位移传感器C2(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器C2;
            }
            if (name == "位移传感器C3(mm)")
            {
                enum_Demarcate = PublicEnum.DemarcateType.位移传感器C3;
            }

            return enum_Demarcate;
        }

        private void lv_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            int length = lv_list.SelectedItems.Count;
            for (int i = 0; i < length; i++)
            {
                lbl_ID.Text = lv_list.SelectedItems[i].SubItems[0].Text;
                txt_sKey.Text = lv_list.SelectedItems[i].SubItems[1].Text;
                txt_sValue.Text = lv_list.SelectedItems[i].SubItems[2].Text;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearCon();
        }

        private void clearCon()
        {
            lbl_ID.Text = "";
            txt_sKey.Text = "";
            txt_sValue.Text = "";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lbl_ID.Text))
            {
                string sql = string.Format("update Demarcate_Dict set D_Key='{0}' , D_Value='{1}'  where ID='{2}'", txt_sValue.Text, txt_sKey.Text, lbl_ID.Text);
                if (SQLiteHelper.ExecuteNonQuery(sql) > 0)
                {
                    clearCon();
                    updateLvList();
                    MessageBox.Show("修改成功！");
                }
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lbl_ID.Text))
            {
                string sql = string.Format("delete  from Demarcate_Dict where ID='{0}'", lbl_ID.Text);
                if (SQLiteHelper.ExecuteNonQuery(sql) > 0)
                {
                    clearCon();
                    updateLvList();
                    MessageBox.Show("删除！");
                }
            }
        }

        private void cbb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lv_cjz.Items.Clear();
        }
    }

}
