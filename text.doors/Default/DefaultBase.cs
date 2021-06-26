using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Service;

namespace text.doors.Default
{
    /// <summary>
    /// 默认配置
    /// </summary>
    public static class DefaultBase
    {

        #region --系统默认

        public static bool isRelease = false;
        /// <summary>
        /// 是否打开审核页面
        /// </summary>
        public static bool IsOpenComplexAssessment { get; set; }

        /// <summary>
        /// 当前设置测试的项目
        /// </summary>
        public static string base_TestItem = "";
        /// <summary>
        /// 确定是否设置樘号
        /// </summary>
        public static bool IsSetTong = false;


        /// <summary>
        /// 正压系数
        /// </summary>
        public static string Z_Factor = System.Configuration.ConfigurationSettings.AppSettings["Z_Factor"].ToString();
        /// <summary>
        /// 负压系数
        /// </summary>
        public static string F_Factor = System.Configuration.ConfigurationSettings.AppSettings["F_Factor"].ToString();


        /// <summary>
        /// 气密、水密等级字典
        /// </summary>
        public static Dictionary<int, int> AirtightLevel = new Dictionary<int, int>()
        {
            {1,0 },{2,100},{3,150},{4,200},{5,250},{6,300},{7,350},{8,400},{9,500},{10,600},{11,700}
        };
        #endregion

        /// <summary>
        /// 导入图片名称
        /// </summary>
        public static string ImagesName = "";
    }


    public static class RegisterData
    {
        public static double DisplaceA1 = 0;
        public static double DisplaceA2 = 0;
        public static double DisplaceA3 = 0;

        public static double DisplaceB1 = 0;
        public static double DisplaceB2 = 0;
        public static double DisplaceB3 = 0;

        public static double DisplaceC1 = 0;
        public static double DisplaceC2 = 0;
        public static double DisplaceC3 = 0;
        public static double Displace10 = 0;
        //差压高
        public static int CY_High_Value = 0;
        //差压低        
        public static int CY_Low_Value = 0;
        //风速
        public static double WindSpeed_Value = 0;
        //大气压力
        public static double AtmospherePa_Value = 0;
        //温度
        public static double Temperature_Value = 0;
    }
}
