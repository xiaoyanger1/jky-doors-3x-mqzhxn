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

            var sql = string.Format(@"insert into dt_pd_Info (dt_Code,test_result,test_desc,zf1,zf2,zf3,zf4,zf5,xz1,xz2,xz3,xz4,xz5) 
                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}');",
           model.dt_Code, model.test_result, model.test_desc, model.zf1, model.zf2, model.zf3, model.zf4, model.zf5, model.xz1, model.xz2, model.xz3, model.xz4, model.xz5);

            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;

            new DAL_dt_Info().UpdateTestType(model.dt_Code, PublicEnum.SystemItem.PlaneDeformation, 1);

            return res;
        }


        public Model_dt_pd_Info GetPDListByCode(string code)
        {
            Model_dt_pd_Info model =null;

            var item = SQLiteHelper.ExecuteDataRow("select * from dt_pd_Info where dt_Code='" + code + "'")?.Table;
            if (item != null)
            {
                model = new Model_dt_pd_Info();
                model.dt_Code = item.Rows[0]["dt_Code"].ToString();
                model.test_result = item.Rows[0]["test_result"].ToString();
                model.test_desc = item.Rows[0]["test_desc"].ToString();
                model.zf1 = item.Rows[0]["zf1"].ToString();
                model.zf2 = item.Rows[0]["zf2"].ToString();
                model.zf3 = item.Rows[0]["zf3"].ToString();
                model.zf4 = item.Rows[0]["zf4"].ToString();
                model.zf5 = item.Rows[0]["zf5"].ToString();
                model.xz1 = item.Rows[0]["xz1"].ToString();
                model.xz2 = item.Rows[0]["xz2"].ToString();
                model.xz3 = item.Rows[0]["xz3"].ToString();
                model.xz4 = item.Rows[0]["xz4"].ToString();
                model.xz5 = item.Rows[0]["xz5"].ToString();
            }
            return model;
        }
    }
}
