using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_Settings
    {
        public Model_dt_Settings()
        {
            dt_InfoList = new List<Model_dt_Info>();
            dt_qm_Info = new List<Model_dt_qm_Info>();
            dt_sm_Info = new Model_dt_sm_Info();
            dt_kfy_Info = new List<Model_dt_kfy_Info>();
        }

        public string dt_Code { get; set; }//编号
        public DateTime dt_Create { get; set; }


        #region 
        public string weituodianhua { get; set; }
        public string songyangriqi { get; set; }
        public string naihoujiao { get; set; }
        public string litingxilie { get; set; }
        public string yangpinbianhao { get; set; }
        public string mianbancaizhi { get; set; }
        public string mianbanxiangqianfangshi { get; set; }
        public string mianbanxiangqiancailiao { get; set; }
        public string kuangshanmifengcailiao { get; set; }
        public string kekaibishijianmianji { get; set; }
        public string caiyangfangshi { get; set; }
        public string weituoren { get; set; }
        public string zuidamianban { get; set; }
        public string jianlidanwei { get; set; }
        public string jianshedanwei { get; set; }
        public string shejidanwei { get; set; }
        public string ganCchang { get; set; }
        public string ganBchang { get; set; }
        public string ganAchang { get; set; }
        public string kekaimianji { get; set; }
        public string KaiQiFangShi { get; set; }
        public string kekaifengchang { get; set; }
        public string jiegoujiao { get; set; }
        public string shijiancenggao { get; set; }
        public string gudingmianji { get; set; }
        public string gudingfengchang { get; set; }
        public string shijiangaodu { get; set; }
        public string gongchengmingcheng { get; set; }
        public string shijiankuandu { get; set; }
        public string GuiGeXingHao { get; set; }
        public string YangPinMingCheng { get; set; }
        public string jianceyiju { get; set; }
        public string shijianmianji { get; set; }
        public string DaQiYaLi { get; set; }
        public string DangQianWenDu { get; set; }
        public string shengchandanwei { get; set; }
        public string WeiTuoBianHao { get; set; }
        public string baogaoriqi { get; set; }
        public string JianCeXiangMu { get; set; }
        public string ganjianABC { get; set; }
        public string JianCeRiQi { get; set; }
        public string WeiTuoDanWei { get; set; }

        #endregion

        public List<Model_dt_Info> dt_InfoList { get; set; }
        public List<Model_dt_qm_Info> dt_qm_Info { get; set; }
        public Model_dt_qm_zb_Info dt_qm_zb_Info { get; set; }
        public Model_dt_sm_Info dt_sm_Info { get; set; }
        public Model_dt_pd_Info dt_pd_Info { get; set; }

        public List<Model_dt_kfy_Info> dt_kfy_Info { get; set; }

        public Model_dt_kfy_res_Info dt_kfy_res_Info { get; set; }

        
    }


}
