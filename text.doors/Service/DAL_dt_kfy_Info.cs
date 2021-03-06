
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
    public class DAL_dt_kfy_Info
    {
        /// <summary>
        /// 添加抗风压信息
        /// </summary>
        /// <param name="mode"></param>
        public bool Add_kfy_Info(List<Model_dt_kfy_Info> list, string code)
        {
            var sql = "";
            if (list != null && list.Count > 0)
            {
                SQLiteHelper.ExecuteNonQuery("delete from dt_kfy_Info where dt_Code = '" + code + "'");
                foreach (var model in list)
                {
                    #region 拼接
                    sql += string.Format(
      @"insert into dt_kfy_Info (
dt_Code ,
z_one_250 ,
z_one_500 ,
z_one_750 ,
z_one_1000 ,
z_one_1250 ,
z_one_1500 ,
z_one_1750 ,
z_one_2000 ,
z_two_250 ,
z_two_500 ,
z_two_750 ,
z_two_1000 ,
z_two_1250 ,
z_two_1500 ,
z_two_1750 ,
z_two_2000 ,
z_three_250 ,
z_three_500 ,
z_three_750 ,
z_three_1000 ,
z_three_1250 ,
z_three_1500 ,
z_three_1750 ,
z_three_2000 ,
z_nd_250 ,
z_nd_500 ,
z_nd_750 ,
z_nd_1000 ,
z_nd_1250 ,
z_nd_1500 ,
z_nd_1750 ,
z_nd_2000 ,
z_ix_250 ,
z_ix_500 ,
z_ix_750 ,
z_ix_1000 ,
z_ix_1250 ,
z_ix_1500 ,
z_ix_1750 ,
z_ix_2000 ,
f_one_250 ,
f_one_500 ,
f_one_750 ,
f_one_1000 ,
f_one_1250 ,
f_one_1500 ,
f_one_1750 ,
f_one_2000 ,
f_two_250 ,
f_two_500 ,
f_two_750 ,
f_two_1000 ,
f_two_1250 ,
f_two_1500 ,
f_two_1750 ,
f_two_2000 ,
f_three_250 ,
f_three_500 ,
f_three_750 ,
f_three_1000 ,
f_three_1250 ,
f_three_1500 ,
f_three_1750 ,
f_three_2000 ,
f_nd_250 ,
f_nd_500 ,
f_nd_750 ,
f_nd_1000 ,
f_nd_1250 ,
f_nd_1500 ,
f_nd_1750 ,
f_nd_2000 ,
f_ix_250 ,
f_ix_500 ,
f_ix_750 ,
f_ix_1000 ,
f_ix_1250 ,
f_ix_1500 ,
f_ix_1750 ,
f_ix_2000,
z_one_p3 ,
z_one_p3max ,
z_two_p3 ,
z_two_p3max ,
z_three_p3 ,
z_three_p3max ,
z_nd_p3 ,
z_nd_p3max ,
z_ix_p3 ,
z_ix_p3max ,
f_one_p3 ,
f_one_p3max ,
f_two_p3 ,
f_two_p3max ,
f_three_p3 ,
f_three_p3max ,
f_nd_p3 ,
f_nd_p3max ,
f_ix_p3 ,
f_ix_p3max ,
level
) 
values(
'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}',
'{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}',
'{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}',
'{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}','{80}',
'{81}','{82}','{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}','{91}','{92}','{93}','{94}','{95}','{96}','{97}','{98}','{99}','{100}',
'{101}');",
      model.dt_Code,
      model.z_one_250,
      model.z_one_500,
      model.z_one_750,
      model.z_one_1000,
      model.z_one_1250,
      model.z_one_1500,
      model.z_one_1750,
      model.z_one_2000,
      model.z_two_250,
      model.z_two_500,
      model.z_two_750,
      model.z_two_1000,
      model.z_two_1250,
      model.z_two_1500,
      model.z_two_1750,
      model.z_two_2000,
      model.z_three_250,
      model.z_three_500,
      model.z_three_750,
      model.z_three_1000,
      model.z_three_1250,
      model.z_three_1500,
      model.z_three_1750,
      model.z_three_2000,
      model.z_nd_250,
      model.z_nd_500,
      model.z_nd_750,
      model.z_nd_1000,
      model.z_nd_1250,
      model.z_nd_1500,
      model.z_nd_1750,
      model.z_nd_2000,
      model.z_ix_250,
      model.z_ix_500,
      model.z_ix_750,
      model.z_ix_1000,
      model.z_ix_1250,
      model.z_ix_1500,
      model.z_ix_1750,
      model.z_ix_2000,
      model.f_one_250,
      model.f_one_500,
      model.f_one_750,
      model.f_one_1000,
      model.f_one_1250,
      model.f_one_1500,
      model.f_one_1750,
      model.f_one_2000,
      model.f_two_250,
      model.f_two_500,
      model.f_two_750,
      model.f_two_1000,
      model.f_two_1250,
      model.f_two_1500,
      model.f_two_1750,
      model.f_two_2000,
      model.f_three_250,
      model.f_three_500,
      model.f_three_750,
      model.f_three_1000,
      model.f_three_1250,
      model.f_three_1500,
      model.f_three_1750,
      model.f_three_2000,
      model.f_nd_250,
      model.f_nd_500,
      model.f_nd_750,
      model.f_nd_1000,
      model.f_nd_1250,
      model.f_nd_1500,
      model.f_nd_1750,
      model.f_nd_2000,
      model.f_ix_250,
      model.f_ix_500,
      model.f_ix_750,
      model.f_ix_1000,
      model.f_ix_1250,
      model.f_ix_1500,
      model.f_ix_1750,
      model.f_ix_2000,
      model.z_one_p3,
    model.z_one_p3max,
    model.z_two_p3,
    model.z_two_p3max,
    model.z_three_p3,
    model.z_three_p3max,
    model.z_nd_p3,
    model.z_nd_p3max,
    model.z_ix_p3,
    model.z_ix_p3max,
    model.f_one_p3,
    model.f_one_p3max,
    model.f_two_p3,
    model.f_two_p3max,
    model.f_three_p3,
    model.f_three_p3max,
    model.f_nd_p3,
    model.f_nd_p3max,
    model.f_ix_p3,
    model.f_ix_p3max,
    model.level);
                    #endregion
                }
            }

            return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }


        /// <summary>
        /// 获取单当数据
        /// </summary>
        /// <param name = "code" ></ param >
        /// < param name="tong"></param>
        /// <returns></returns>
        public DataTable GetkfyListByCode(string code)
        {
            string sql = "select * from  dt_kfy_Info  where dt_Code='" + code + "'";

            var res = SQLiteHelper.ExecuteDataRow(sql);
            if (res == null)
            {
                return null;
            }
            return res.Table;
        }

        public List<Model_dt_kfy_Info> GetKFYList(string code)
        {
            List<Model_dt_kfy_Info> list = new List<Model_dt_kfy_Info>();
            var dt_kfy_Info = SQLiteHelper.ExecuteDataRow("select * from dt_kfy_Info where dt_Code='" + code + "' order by  level")?.Table;
            if (dt_kfy_Info != null)
            {
                foreach (DataRow item in dt_kfy_Info.Rows)
                {
                    #region
                    Model_dt_kfy_Info model = new Model.DataBase.Model_dt_kfy_Info();
                    model.z_one_250 = item["z_one_250"].ToString();
                    model.z_two_250 = item["z_two_250"].ToString();
                    model.z_three_250 = item["z_three_250"].ToString();
                    model.z_nd_250 = item["z_nd_250"].ToString();
                    model.z_ix_250 = item["z_ix_250"].ToString();
                    model.f_one_250 = item["f_one_250"].ToString();
                    model.f_two_250 = item["f_two_250"].ToString();
                    model.f_three_250 = item["f_three_250"].ToString();
                    model.f_nd_250 = item["f_nd_250"].ToString();
                    model.f_ix_250 = item["f_ix_250"].ToString();
                    model.z_one_500 = item["z_one_500"].ToString();
                    model.z_two_500 = item["z_two_500"].ToString();
                    model.z_three_500 = item["z_three_500"].ToString();
                    model.z_nd_500 = item["z_nd_500"].ToString();
                    model.z_ix_500 = item["z_ix_500"].ToString();
                    model.f_one_500 = item["f_one_500"].ToString();
                    model.f_two_500 = item["f_two_500"].ToString();
                    model.f_three_500 = item["f_three_500"].ToString();
                    model.f_nd_500 = item["f_nd_500"].ToString();
                    model.f_ix_500 = item["f_ix_500"].ToString();
                    model.z_one_750 = item["z_one_750"].ToString();
                    model.z_two_750 = item["z_two_750"].ToString();
                    model.z_three_750 = item["z_three_750"].ToString();
                    model.z_nd_750 = item["z_nd_750"].ToString();
                    model.z_ix_750 = item["z_ix_750"].ToString();
                    model.f_one_750 = item["f_one_750"].ToString();
                    model.f_two_750 = item["f_two_750"].ToString();
                    model.f_three_750 = item["f_three_750"].ToString();
                    model.f_nd_750 = item["f_nd_750"].ToString();
                    model.f_ix_750 = item["f_ix_750"].ToString();
                    model.z_one_1000 = item["z_one_1000"].ToString();
                    model.z_two_1000 = item["z_two_1000"].ToString();
                    model.z_three_1000 = item["z_three_1000"].ToString();
                    model.z_nd_1000 = item["z_nd_1000"].ToString();
                    model.z_ix_1000 = item["z_ix_1000"].ToString();
                    model.f_one_1000 = item["f_one_1000"].ToString();
                    model.f_two_1000 = item["f_two_1000"].ToString();
                    model.f_three_1000 = item["f_three_1000"].ToString();
                    model.f_nd_1000 = item["f_nd_1000"].ToString();
                    model.f_ix_1000 = item["f_ix_1000"].ToString();
                    model.z_one_1250 = item["z_one_1250"].ToString();
                    model.z_two_1250 = item["z_two_1250"].ToString();
                    model.z_three_1250 = item["z_three_1250"].ToString();
                    model.z_nd_1250 = item["z_nd_1250"].ToString();
                    model.z_ix_1250 = item["z_ix_1250"].ToString();
                    model.f_one_1250 = item["f_one_1250"].ToString();
                    model.f_two_1250 = item["f_two_1250"].ToString();
                    model.f_three_1250 = item["f_three_1250"].ToString();
                    model.f_nd_1250 = item["f_nd_1250"].ToString();
                    model.f_ix_1250 = item["f_ix_1250"].ToString();
                    model.z_one_1500 = item["z_one_1500"].ToString();
                    model.z_two_1500 = item["z_two_1500"].ToString();
                    model.z_three_1500 = item["z_three_1500"].ToString();
                    model.z_nd_1500 = item["z_nd_1500"].ToString();
                    model.z_ix_1500 = item["z_ix_1500"].ToString();
                    model.f_one_1500 = item["f_one_1500"].ToString();
                    model.f_two_1500 = item["f_two_1500"].ToString();
                    model.f_three_1500 = item["f_three_1500"].ToString();
                    model.f_nd_1500 = item["f_nd_1500"].ToString();
                    model.f_ix_1500 = item["f_ix_1500"].ToString();
                    model.z_one_1750 = item["z_one_1750"].ToString();
                    model.z_two_1750 = item["z_two_1750"].ToString();
                    model.z_three_1750 = item["z_three_1750"].ToString();
                    model.z_nd_1750 = item["z_nd_1750"].ToString();
                    model.z_ix_1750 = item["z_ix_1750"].ToString();
                    model.f_one_1750 = item["f_one_1750"].ToString();
                    model.f_two_1750 = item["f_two_1750"].ToString();
                    model.f_three_1750 = item["f_three_1750"].ToString();
                    model.f_nd_1750 = item["f_nd_1750"].ToString();
                    model.f_ix_1750 = item["f_ix_1750"].ToString();
                    model.z_one_2000 = item["z_one_2000"].ToString();
                    model.z_two_2000 = item["z_two_2000"].ToString();
                    model.z_three_2000 = item["z_three_2000"].ToString();
                    model.z_nd_2000 = item["z_nd_2000"].ToString();
                    model.z_ix_2000 = item["z_ix_2000"].ToString();
                    model.f_one_2000 = item["f_one_2000"].ToString();
                    model.f_two_2000 = item["f_two_2000"].ToString();
                    model.f_three_2000 = item["f_three_2000"].ToString();
                    model.f_nd_2000 = item["f_nd_2000"].ToString();
                    model.f_ix_2000 = item["f_ix_2000"].ToString();

                    model.z_one_p3 = item["z_one_p3"].ToString();
                    model.z_two_p3 = item["z_two_p3"].ToString();
                    model.z_three_p3 = item["z_three_p3"].ToString();
                    model.z_nd_p3 = item["z_nd_p3"].ToString();

                    model.f_one_p3 = item["f_one_p3"].ToString();
                    model.f_two_p3 = item["f_two_p3"].ToString();
                    model.f_three_p3 = item["f_three_p3"].ToString();
                    model.f_nd_p3 = item["f_nd_p3"].ToString();

                    model.z_one_p3max = item["z_one_p3max"].ToString();
                    model.z_two_p3max = item["z_two_p3max"].ToString();
                    model.z_three_p3max = item["z_three_p3max"].ToString();
                    model.z_nd_p3max = item["z_nd_p3max"].ToString();

                    model.f_one_p3max = item["f_one_p3max"].ToString();
                    model.f_two_p3max = item["f_two_p3max"].ToString();
                    model.f_three_p3max = item["f_three_p3max"].ToString();
                    model.f_nd_p3max = item["f_nd_p3max"].ToString();


                    model.level = item["level"].ToString();
                    model.dt_Code = item["dt_Code"].ToString();
                    list.Add(model);
                    #endregion
                }
            }
            return list;
        }

        public Model_dt_kfy_res_Info GetKFYResInfo(string code)
        {
            Model_dt_kfy_res_Info modelRes = null;
            var item = SQLiteHelper.ExecuteDataRow("select * from dt_kfy_res_Info where dt_Code='" + code + "'")?.Table;
            if (item != null)
            {
                modelRes = new Model_dt_kfy_res_Info();

                modelRes.dt_Code = item.Rows[0]["dt_Code"].ToString();
                modelRes.testtype = int.Parse(item.Rows[0]["testtype"].ToString());
                modelRes.desc = item.Rows[0]["desc"].ToString();
                modelRes.p1 = item.Rows[0]["p1"].ToString();
                modelRes.p2 = item.Rows[0]["p2"].ToString();
                modelRes.p3 = item.Rows[0]["p3"].ToString();
                modelRes.pMax = item.Rows[0]["pMax"].ToString();
                modelRes._p1 = item.Rows[0]["_p1"].ToString();
                modelRes._p2 = item.Rows[0]["_p2"].ToString();
                modelRes._p3 = item.Rows[0]["_p3"].ToString();
                modelRes._pMax = item.Rows[0]["_pMax"].ToString();
                modelRes.info_Level = int.Parse(item.Rows[0]["info_Level"].ToString());
                modelRes.defJC = int.Parse(item.Rows[0]["defJC"].ToString());
                modelRes.lx_a = item.Rows[0]["lx_a"].ToString();
                modelRes.lx_b = item.Rows[0]["lx_b"].ToString();
                modelRes.lx_c = item.Rows[0]["lx_c"].ToString();

            }
            return modelRes;
        }
    }
}
