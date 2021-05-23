using text.doors.Common;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Model.DataBase;
using Young.Core.SQLite;
using System.Runtime.InteropServices.WindowsRuntime;
using NPOI.SS.Formula.Functions;
using System.Windows.Forms;
using text.doors.Detection;
using text.doors.Service;

namespace text.doors.dal
{
    public class DAL_dt_Settings
    {
        /// <summary>
        /// 添加基本编号设置
        /// </summary>
        public bool Add(Model_dt_Settings model)
        {
            var res = false;
            string delsql = "delete from dt_Settings where dt_Code='" + model.dt_Code + "'";
            SQLiteHelper.ExecuteNonQuery(delsql);

            #region 添加主表
            string sql = string.Format(@"
insert into dt_Settings
(
weituodianhua           ,
songyangriqi            ,
naihoujiao              ,
litingxilie             ,
yangpinbianhao          ,
mianbancaizhi           ,
mianbanxiangqianfangshi ,
mianbanxiangqiancailiao ,
kuangshanmifengcailiao  ,
kekaibishijianmianji    ,
caiyangfangshi          ,
weituoren               ,
zuidamianban            ,
jianlidanwei            ,
jianshedanwei           ,
shejidanwei             ,
ganCchang               ,
ganBchang               ,
ganAchang               ,
kekaimianji             ,
KaiQiFangShi            ,
kekaifengchang          ,
jiegoujiao              ,
shijiancenggao          ,
gudingmianji            ,
gudingfengchang         ,
shijiangaodu            ,
gongchengmingcheng      ,
shijiankuandu           ,
GuiGeXingHao            ,
YangPinMingCheng        ,
jianceyiju              ,
shijianmianji           ,
DaQiYaLi                ,
DangQianWenDu           ,
shengchandanwei         ,
baogaoriqi              ,
JianCeXiangMu           ,
ganjianABC              ,
JianCeRiQi              ,
WeiTuoDanWei            ,
WeiTuoBianHao           ,
dt_Code,
dt_Create
)
VALUES
(
'{0}','{1}','{2}' ,'{3}','{4}' , '{5}' , '{6}','{7}' ,'{8}' ,'{9}' , '{10}','{11}'  ,  '{12}' ,'{13}' ,'{14}' ,'{15}'  ,'{16}' ,'{17}','{18}' , '{19}' ,
'{20}' , '{21}' ,'{22}' ,'{23}','{24}' ,'{25}' ,'{26}' ,'{27}','{28}' , '{29}' ,'{30}','{31}' ,'{32}','{33}','{34}' ,'{35}' ,'{36}' ,'{37}' ,'{38}' ,
'{39}' ,'{40}','{41}','{42}',datetime('now'))",
model.weituodianhua,
model.songyangriqi,
model.naihoujiao,
model.litingxilie,
model.yangpinbianhao,
model.mianbancaizhi,
model.mianbanxiangqianfangshi,
model.mianbanxiangqiancailiao,
model.kuangshanmifengcailiao,
model.kekaibishijianmianji,
model.caiyangfangshi,
model.weituoren,
model.zuidamianban,
model.jianlidanwei,
model.jianshedanwei,
model.shejidanwei,
model.ganCchang,
model.ganBchang,
model.ganAchang,
model.kekaimianji,
model.KaiQiFangShi,
model.kekaifengchang,
model.jiegoujiao,
model.shijiancenggao,
model.gudingmianji,
model.gudingfengchang,
model.shijiangaodu,
model.gongchengmingcheng,
model.shijiankuandu,
model.GuiGeXingHao,
model.YangPinMingCheng,
model.jianceyiju,
model.shijianmianji,
model.DaQiYaLi,
model.DangQianWenDu,
model.shengchandanwei,
model.baogaoriqi,
model.JianCeXiangMu,
model.ganjianABC,
model.JianCeRiQi,
model.WeiTuoDanWei,
model.WeiTuoBianHao,
model.dt_Code);
            res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            #endregion

            //#region 添加实验樘号记录
            //if (res)
            //{
            //    var table = SQLiteHelper.ExecuteDataRow("select * from dt_Info where info_DangH = '" + tong + "' and dt_Code = '" + model.dt_Code + "'")?.Table;
            //    if (table == null || table.Rows.Count == 0)
            //    {
            //        sql = string.Format("insert into dt_Info (info_DangH,info_Create,dt_Code,Airtight,Watertight,WindPressure) values('{0}',datetime('now'),'{1}',0,0,0)", tong, model.dt_Code);
            //        return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            //    }
            //}
            //#endregion

            return true;
        }


        public List<DictName> GetCodeList()
        {
            List<DictName> lis = new List<DictName>();
            string sql = "select distinct dt_code from  dt_settings order by dt_Create desc";

            var dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr != null)
            {
                var dt = dr.Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lis.Add(new DictName()
                    {
                        name = dt.Rows[i]["dt_code"].ToString(),
                        id = i
                    });
                }
            }
            return lis;

        }

        /// <summary>
        /// 查询编号，如编号等于null，则查询最近一次数据
        /// </summary>
        /// <param name="code">编号</param>
        /// <returns></returns>
        public DataTable Getdt_SettingsByCode(string code)
        {
            string sql = "";
            if (string.IsNullOrWhiteSpace(code))
            {
                sql = @"select * from dt_Settings order by  dt_Create desc  LIMIT(1)";
            }
            else
            {
                sql = @"select * from dt_Settings  t
                            where t.dt_Code ='" + code + "'";
            }
            DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr == null)
                return null;
            return dr.Table;
        }


        /// <summary>
        /// 获取气密指标
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Model_dt_qm_zb_Info GetQMZB(string code)
        {
            Model_dt_qm_zb_Info dt_qm_zb_Info = null;
            //获取指标
            var dt_qm_zb_info = SQLiteHelper.ExecuteDataRow("select * from dt_qm_zb_info where dt_Code='" + code + "'")?.Table;

            if (dt_qm_zb_info != null && dt_qm_zb_info.Rows.Count > 0)
            {
                dt_qm_zb_Info = new Model_dt_qm_zb_Info();
                dt_qm_zb_Info.dt_Code = dt_qm_zb_info.Rows[0]["dt_Code"].ToString();
                dt_qm_zb_Info.Z_FC = dt_qm_zb_info.Rows[0]["Z_FC"].ToString();
                dt_qm_zb_Info.F_FC = dt_qm_zb_info.Rows[0]["F_FC"].ToString();
                dt_qm_zb_Info.Z_MJ = dt_qm_zb_info.Rows[0]["Z_MJ"].ToString();
                dt_qm_zb_Info.F_MJ = dt_qm_zb_info.Rows[0]["F_MJ"].ToString();
            }
            return dt_qm_zb_Info;
        }



        /// <summary>
        /// 根据编号获取本次检测信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Model_dt_Settings GetInfoByCode(string code)
        {
            Model_dt_Settings settings = new Model_dt_Settings();

            var dt_settings = SQLiteHelper.ExecuteDataRow("select * from dt_settings where dt_Code='" + code + "'")?.Table;

            settings.weituodianhua = dt_settings.Rows[0]["weituodianhua"].ToString();
            settings.songyangriqi = dt_settings.Rows[0]["songyangriqi"].ToString();
            settings.naihoujiao = dt_settings.Rows[0]["naihoujiao"].ToString();
            settings.litingxilie = dt_settings.Rows[0]["litingxilie"].ToString();
            settings.yangpinbianhao = dt_settings.Rows[0]["yangpinbianhao"].ToString();
            settings.mianbancaizhi = dt_settings.Rows[0]["mianbancaizhi"].ToString();
            settings.mianbanxiangqianfangshi = dt_settings.Rows[0]["mianbanxiangqianfangshi"].ToString();
            settings.mianbanxiangqiancailiao = dt_settings.Rows[0]["mianbanxiangqiancailiao"].ToString();
            settings.kuangshanmifengcailiao = dt_settings.Rows[0]["kuangshanmifengcailiao"].ToString();
            settings.kekaibishijianmianji = dt_settings.Rows[0]["kekaibishijianmianji"].ToString();
            settings.caiyangfangshi = dt_settings.Rows[0]["caiyangfangshi"].ToString();
            settings.weituoren = dt_settings.Rows[0]["weituoren"].ToString();
            settings.zuidamianban = dt_settings.Rows[0]["zuidamianban"].ToString();
            settings.jianlidanwei = dt_settings.Rows[0]["jianlidanwei"].ToString();
            settings.jianshedanwei = dt_settings.Rows[0]["jianshedanwei"].ToString();
            settings.shejidanwei = dt_settings.Rows[0]["shejidanwei"].ToString();
            settings.ganCchang = dt_settings.Rows[0]["ganCchang"].ToString();
            settings.ganBchang = dt_settings.Rows[0]["ganBchang"].ToString();
            settings.ganAchang = dt_settings.Rows[0]["ganAchang"].ToString();
            settings.kekaimianji = dt_settings.Rows[0]["kekaimianji"].ToString();
            settings.KaiQiFangShi = dt_settings.Rows[0]["KaiQiFangShi"].ToString();
            settings.kekaifengchang = dt_settings.Rows[0]["kekaifengchang"].ToString();
            settings.jiegoujiao = dt_settings.Rows[0]["jiegoujiao"].ToString();
            settings.shijiancenggao = dt_settings.Rows[0]["shijiancenggao"].ToString();
            settings.gudingmianji = dt_settings.Rows[0]["gudingmianji"].ToString();
            settings.gudingfengchang = dt_settings.Rows[0]["gudingfengchang"].ToString();
            settings.shijiangaodu = dt_settings.Rows[0]["shijiangaodu"].ToString();
            settings.gongchengmingcheng = dt_settings.Rows[0]["gongchengmingcheng"].ToString();
            settings.shijiankuandu = dt_settings.Rows[0]["shijiankuandu"].ToString();
            settings.GuiGeXingHao = dt_settings.Rows[0]["GuiGeXingHao"].ToString();
            settings.YangPinMingCheng = dt_settings.Rows[0]["YangPinMingCheng"].ToString();
            settings.jianceyiju = dt_settings.Rows[0]["jianceyiju"].ToString();
            settings.shijianmianji = dt_settings.Rows[0]["shijianmianji"].ToString();
            settings.DaQiYaLi = dt_settings.Rows[0]["DaQiYaLi"].ToString();
            settings.DangQianWenDu = dt_settings.Rows[0]["DangQianWenDu"].ToString();
            settings.shengchandanwei = dt_settings.Rows[0]["shengchandanwei"].ToString();
            settings.WeiTuoBianHao = dt_settings.Rows[0]["WeiTuoBianHao"].ToString();
            settings.baogaoriqi = dt_settings.Rows[0]["baogaoriqi"].ToString();
            settings.JianCeXiangMu = dt_settings.Rows[0]["JianCeXiangMu"].ToString();
            settings.ganjianABC = dt_settings.Rows[0]["ganjianABC"].ToString();
            settings.JianCeRiQi = dt_settings.Rows[0]["JianCeRiQi"].ToString();
            settings.WeiTuoDanWei = dt_settings.Rows[0]["WeiTuoDanWei"].ToString();
            settings.dt_Code = dt_settings.Rows[0]["dt_Code"].ToString();

            var dt_Info = SQLiteHelper.ExecuteDataRow("select * from dt_Info where dt_Code='" + code + "'   order by  info_DangH")?.Table;
            if (dt_Info != null)
            {
                List<Model_dt_Info> list = new List<Model_dt_Info>();
                foreach (DataRow item in dt_Info.Rows)
                {
                    #region
                    Model_dt_Info model = new Model_dt_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_Create = item["info_Create"].ToString();
                    model.Watertight = Convert.ToInt32(item["Watertight"].ToString());
                    model.WindPressure = Convert.ToInt32(item["WindPressure"].ToString());
                    model.Airtight = Convert.ToInt32(item["Airtight"].ToString());
                    list.Add(model);
                    #endregion
                }

                if (list.Count > 0)
                    settings.dt_InfoList = list;
            }

            //获取气密
            var qmList = new DAL_dt_qm_Info().GetQMListByCode(code);
            if (qmList != null && qmList.Count > 0)
                settings.dt_qm_Info = qmList;

            //获取指标
            settings.dt_qm_zb_Info = new DAL_dt_qm_Info().GetQMZB(code);

            //获取水密
            settings.dt_sm_Info = new DAL_dt_sm_Info().GetSMListByCode(code);

            var dt_kfy_Info = SQLiteHelper.ExecuteDataRow("select * from dt_kfy_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_kfy_Info != null)
            {
                List<Model_dt_kfy_Info> list = new List<Model_dt_kfy_Info>();
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



                    model.p1 = item["p1"].ToString();
                    model.p2 = item["p2"].ToString();
                    model.p3 = item["p3"].ToString();
                    model.pMax = item["z_pMax"].ToString();
                    model._p1 = item["_p1"].ToString();
                    model._p2 = item["_p2"].ToString();
                    model._p3 = item["_p3"].ToString();
                    model._pMax = item["f_pMax"].ToString();
                    model.CheckLock = int.Parse(item["CheckLock"].ToString());
                    model.info_DangH = item["info_DangH"].ToString();
                    model.testtype = int.Parse(item["testtype"].ToString());

                    list.Add(model);
                    #endregion
                }
                if (list.Count > 0)
                    settings.dt_kfy_Info = list;
            }
            return settings;
        }


        /// <summary>
        /// 查询编号是否存在
        /// </summary>
        public bool Getdt_SettingsByCodeIsExist(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            string sql = "select * from dt_Settings where  dt_Code = '" + code + "'";
            DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr == null)
                return false;
            return true;
        }

    }
}
