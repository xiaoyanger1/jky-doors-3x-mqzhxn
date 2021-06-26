using text.doors.Common;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Model.DataBase;
using Young.Core.SQLite;
using static text.doors.Default.PublicEnum;
using System.Data;

namespace text.doors.dal
{
    public class DAL_dt_Info
    {
        /// <summary>
        /// 更新实验状态
        /// type:0未完成，1完成
        /// </summary>
        /// <returns></returns>
        public bool UpdateTestType(string code, SystemItem systemItem, int type)
        {
            string sql = "update dt_Info  set";
            if (systemItem == SystemItem.Airtight)
            {
                sql += " Airtight=" + type + "";
            }
            else if (systemItem == SystemItem.Watertight)
            {
                sql += " Watertight=" + type + "";
            }
            else if (systemItem == SystemItem.AirPressure)
            {
                sql += " WindPressure=" + type + "";
            }
            else if (systemItem == SystemItem.PlaneDeformation)
            {
                sql += " PlaneDeformation=" + type + "";
            }
            
            sql += " where dt_Code='" + code + "'";
            return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }

        public List<Model_dt_Info> GetDTInfo(string code)
        {
            List<Model_dt_Info> list = new List<Model_dt_Info>();
            var dt_Info = SQLiteHelper.ExecuteDataRow("select * from dt_Info where dt_Code='" + code + "'")?.Table;
            if (dt_Info != null)
            {
                foreach (DataRow item in dt_Info.Rows)
                {
                    #region
                    Model_dt_Info model = new Model_dt_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_Create = item["info_Create"].ToString();
                    model.Watertight = Convert.ToInt32(item["Watertight"].ToString());
                    model.WindPressure = Convert.ToInt32(item["WindPressure"].ToString());
                    model.Airtight = Convert.ToInt32(item["Airtight"].ToString());
                    model.PlaneDeformation = Convert.ToInt32(item["PlaneDeformation"].ToString());
                    list.Add(model);
                    #endregion
                }
            }
            return list;
        }

        /// <summary>
        /// 删除当前编号数据
        /// </summary>
        /// <returns></returns>
        public bool delete_dt_Info(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("delete from dt_Settings where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_Info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_qm_Info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_sm_Info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_kfy_Info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_pd_Info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_qm_zb_info where dt_Code ='{0}';", code);
            sb.AppendFormat("delete from dt_kfy_res_Info where dt_Code ='{0}';", code);
            

            return SQLiteHelper.ExecuteNonQuery(sb.ToString()) > 0 ? true : false;
        }
    }
}
