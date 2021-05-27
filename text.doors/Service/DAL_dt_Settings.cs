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

            #region 添加实验樘号记录
            if (res)
            {
                var table = SQLiteHelper.ExecuteDataRow("select * from dt_Info where  dt_Code = '" + model.dt_Code + "'")?.Table;
                if (table == null || table.Rows.Count == 0)
                {
                    sql = string.Format("insert into dt_Info (info_Create,dt_Code,Airtight,Watertight,WindPressure,PlaneDeformation) values('{0}',datetime('now'),0,0,0,0)", model.dt_Code);
                    return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
                }
            }
            #endregion

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
                sql = @"select * from dt_Settings order by  dt_Create desc  LIMIT(1)";
            else
                sql = @"select * from dt_Settings  t where t.dt_Code ='" + code + "'";
            DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr == null)
                return null;
            return dr.Table;
        }


        /// <summary>
        /// 根据编号获取本次检测信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Model_dt_Settings GetInfoByCode(string code)
        {
            Model_dt_Settings settings = GetDTSettings(code);


            //获取检测项目
            var dtInfoList = new DAL_dt_Info().GetDTInfo(code);
            if (dtInfoList != null && dtInfoList.Count > 0)
                settings.dt_InfoList = dtInfoList;

            //获取气密
            var qmList = new DAL_dt_qm_Info().GetQMListByCode(code);
            if (qmList != null && qmList.Count > 0)
                settings.dt_qm_Info = qmList;

            //获取指标
            settings.dt_qm_zb_Info = new DAL_dt_qm_Info().GetQMZB(code);

            //获取水密
            settings.dt_sm_Info = new DAL_dt_sm_Info().GetSMListByCode(code);

            //获取幕墙水平
            settings.dt_pd_Info = new DAL_dt_pd_Info().GetPDListByCode(code);

            //抗风压
            var dt_kfy_InfoList = new DAL_dt_kfy_Info().GetKFYList(code);
            if (dt_kfy_InfoList != null && dt_kfy_InfoList.Count > 0)
                settings.dt_kfy_Info = dt_kfy_InfoList;

            //抗风压结果
            var dt_kfy_res_InfoList = new DAL_dt_kfy_Info().GetKFYResInfo(code);
            if (dt_kfy_res_InfoList != null )
                settings.dt_kfy_res_Info = dt_kfy_res_InfoList;

            return settings;
        }

        public Model_dt_Settings GetDTSettings(string code)
        {
            Model_dt_Settings settings = new Model_dt_Settings();
            var dt_settings = SQLiteHelper.ExecuteDataRow("select * from dt_settings where dt_Code='" + code + "'")?.Table;
            if (dt_settings != null && dt_settings.Rows.Count > 0)
            {
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
