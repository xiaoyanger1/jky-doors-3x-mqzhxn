using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.dal;
using text.doors.Default;
using text.doors.Model.DataBase;
using Young.Core.SQLite;

namespace text.doors.Service
{
    public class DAL_dt_sm_Info
    {
        /// <summary>
        /// 添加水密信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model_dt_sm_Info model)
        {
            //删除
            SQLiteHelper.ExecuteNonQuery("delete from dt_sm_Info where  dt_Code='" + model.dt_Code + "' ");

            var sql = string.Format("insert into dt_sm_Info (dt_Code,sm_PaDesc,sm_PaDesc2,sm_Pa,sm_Pa2,sm_Remark,Method,sxyl,xxyl,gongchengjiance) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                model.dt_Code, model.sm_PaDesc, model.sm_PaDesc2, model.sm_Pa, model.sm_Pa2, model.sm_Remark, model.Method, model.sxyl, model.xxyl, model.gongchengjiance);
            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            if (res)
            {
                res = new DAL_dt_Info().UpdateTestType(model.dt_Code, PublicEnum.SystemItem.Watertight, 1);
            }
            return res;
        }
        public Model_dt_sm_Info GetSMListByCode(string code)
        {
            Model_dt_sm_Info model = null;
            var dt_sm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_sm_Info where dt_Code='" + code + "'")?.Table;
            if (dt_sm_Info != null)
            {
                model = new Model_dt_sm_Info();
                model.dt_Code = dt_sm_Info.Rows[0]["dt_Code"].ToString();
                model.sm_PaDesc = dt_sm_Info.Rows[0]["sm_PaDesc"].ToString();
                model.sm_PaDesc2 = dt_sm_Info.Rows[0]["sm_PaDesc2"].ToString();
                model.sm_Pa = int.Parse(dt_sm_Info.Rows[0]["sm_Pa"].ToString());
                model.sm_Pa2 = int.Parse(dt_sm_Info.Rows[0]["sm_Pa2"].ToString());
                model.sm_Remark = dt_sm_Info.Rows[0]["sm_Remark"].ToString();
                model.Method = dt_sm_Info.Rows[0]["Method"].ToString();
                model.sxyl = dt_sm_Info.Rows[0]["sxyl"].ToString();
                model.xxyl = dt_sm_Info.Rows[0]["xxyl"].ToString();
                model.gongchengjiance = dt_sm_Info.Rows[0]["gongchengjiance"].ToString();
            }
            return model;
        }
    }
}
