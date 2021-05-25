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
    public class DAL_dt_kfy_res_Info
    {
        /// <summary>
        /// 添加抗风压信息
        /// </summary>
        /// <param name="mode"></param>
        public bool Add_kfy_Info(Model_dt_kfy_res_Info model)
        {
            SQLiteHelper.ExecuteNonQuery("delete from dt_kfy_res_Info where dt_Code = '" + model.dt_Code + "'");

            #region 拼接

            var sql = string.Format(
                              @"insert into dt_kfy_res_Info (
                            dt_Code ,
                            info_Level,
                            defJC,
                            CheckLock,
                            p1,
                            p2,
                            p3,
                            pMax,
                            _p1,
                            _p2,
                            _p3,
                            _pMax,
                            lx,
                            testtype,
                            desc
                            ) 
                            values(
                            '{0}',{1},{2},{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},'{14}')",
                              model.dt_Code,
                              model.info_Level,
                              model.defJC,
                              model.CheckLock,
                              model.p1,
                              model.p2,
                              model.p3,
                              model.pMax,
                              model._p1,
                              model._p2,
                              model._p3,
                              model._pMax,
                              model.lx,
                              model.testtype,
                              model.desc);
            #endregion
            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            if (res)
            {
                return new DAL_dt_Info().UpdateTestType(model.dt_Code, PublicEnum.SystemItem.AirPressure, 1);
            }
            return true;
        }

    }
}
