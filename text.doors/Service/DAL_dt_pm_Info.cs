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
    public class DAL_dt_pd_Info
    {
        /// <summary>
        /// 添加平面水平信息
        /// </summary>
        /// <param name="mode"></param>
        public bool AddPD(Model_dt_pd_Info model)
        {
            //删除结果
            SQLiteHelper.ExecuteNonQuery("delete from dt_pd_Info where  dt_Code='" + model.dt_Code + "' ");

            var sql = string.Format(@"insert into dt_pd_Info (dt_Code,test_result,test_desc) 
                values('{0}','{1}',{2});",
           model.dt_Code, model.test_result, model.test_desc);

            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;

            new DAL_dt_Info().UpdateTestType(model.dt_Code, PublicEnum.SystemItem.PlaneDeformation, 1);

            return res;
        }


        public Model_dt_pd_Info GetPDListByCode(string code)
        {
            Model_dt_pd_Info model = new Model_dt_pd_Info();

            var item = SQLiteHelper.ExecuteDataRow("select * from dt_pd_Info where dt_Code='" + code + "'")?.Table;
            if (item != null)
            {
                model.dt_Code = item.Rows[0]["dt_Code"].ToString();
                model.test_result = item.Rows[0]["test_result"].ToString();
                model.test_desc = item.Rows[0]["test_desc"].ToString();
            }
            return model;
        }
    }
}
