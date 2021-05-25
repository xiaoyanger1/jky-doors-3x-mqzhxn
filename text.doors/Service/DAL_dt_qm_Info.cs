using text.doors.Common;
using text.doors.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;
using Young.Core.SQLite;
using text.doors.Service;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Microsoft;

namespace text.doors.dal
{
    public class DAL_dt_qm_Info
    {
        /// <summary>
        /// 添加气密信息
        /// </summary>
        /// <param name="mode"></param>
        public bool AddQM(List<Model_dt_qm_Info> list, Model_dt_qm_zb_Info model_dt_qm_zb_Info)
        {
            //删除结果
            SQLiteHelper.ExecuteNonQuery("delete from dt_qm_Info where  dt_Code='" + model_dt_qm_zb_Info.dt_Code + "' ");


            var sql = "";
            foreach (var model in list)
            {
                sql += string.Format(@"insert into dt_qm_Info (dt_Code,Pa,PaType,FJST,GFZH,ZDST,MQZT,KKST,testtype) 
                values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}');",
             model.dt_Code, model.Pa, model.PaType, model.FJST, model.GFZH, model.ZDST, model.MQZT, model.KKST, model.testtype);
            }
            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            if (!res)
                return false;

            AddFJZB(model_dt_qm_zb_Info);
            new DAL_dt_Info().UpdateTestType(model_dt_qm_zb_Info.dt_Code, PublicEnum.SystemItem.Airtight, 1);

            return true;
        }

        /// <summary>
        /// 分级指标
        /// </summary>
        /// <returns></returns>
        private bool AddFJZB(Model_dt_qm_zb_Info zb)
        {
            //删除结果
            SQLiteHelper.ExecuteNonQuery("delete from dt_qm_zb_info where  dt_Code='" + zb.dt_Code + "' ");

            var sql = string.Format(@"insert into dt_qm_zb_info (dt_Code,Z_MJ,F_MJ,Z_FC,F_FC,sjz_value) 
                values('{0}','{1}',{2},'{3}','{4}','{5}')",
                 zb.dt_Code, zb.Z_MJ, zb.F_MJ, zb.Z_FC, zb.F_FC, zb.sjz_value
                 );
            return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }


        public List<Model_dt_qm_Info> GetQMListByCode(string code)
        {
            List<Model_dt_qm_Info> list = new List<Model_dt_qm_Info>();

            var dt_qm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_qm_Info where dt_Code='" + code + "' order by  Pa,PaType")?.Table;
            if (dt_qm_Info != null)
            {
                foreach (DataRow item in dt_qm_Info.Rows)
                {
                    #region
                    Model_dt_qm_Info model = new Model_dt_qm_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.Pa = item["Pa"].ToString();
                    model.PaType = int.Parse(item["PaType"].ToString());
                    model.FJST = item["FJST"].ToString();
                    model.GFZH = item["GFZH"].ToString();
                    model.ZDST = item["ZDST"].ToString();
                    model.MQZT = item["MQZT"].ToString();
                    model.KKST = item["KKST"].ToString();
                    model.testtype = item["KKST"].ToString();
                    list.Add(model);
                    #endregion
                }
            }
            return list;
        }

        /// <summary>
        /// 获取气密指标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Model_dt_qm_zb_Info GetQMZB(string code)
        {
            Model_dt_qm_zb_Info dt_qm_zb_Info = null;
            var dt_qm_zb_info = SQLiteHelper.ExecuteDataRow("select * from dt_qm_zb_info where dt_Code='" + code + "'")?.Table;

            if (dt_qm_zb_info != null && dt_qm_zb_info.Rows.Count > 0)
            {
                dt_qm_zb_Info = new Model_dt_qm_zb_Info();

                dt_qm_zb_Info.dt_Code = dt_qm_zb_info.Rows[0]["dt_Code"].ToString();
                dt_qm_zb_Info.Z_FC = dt_qm_zb_info.Rows[0]["Z_FC"].ToString();
                dt_qm_zb_Info.F_FC = dt_qm_zb_info.Rows[0]["F_FC"].ToString();
                dt_qm_zb_Info.Z_MJ = dt_qm_zb_info.Rows[0]["Z_MJ"].ToString();
                dt_qm_zb_Info.F_MJ = dt_qm_zb_info.Rows[0]["F_MJ"].ToString();
                dt_qm_zb_Info.sjz_value = int.Parse(dt_qm_zb_info.Rows[0]["sjz_value"].ToString());
            }
            return dt_qm_zb_Info;
        }
    }
}
