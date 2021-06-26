using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using text.doors.Default;
using text.doors.Model;
using text.doors.Model.DataBase;
using text.doors.Service;

namespace text.doors.Common
{
    public class Formula
    {

        /// <summary>
        /// 根据枚举获取字典数据
        /// </summary>
        /// <param name="enum_Demarcate"></param>
        /// <returns></returns>
        private static List<Calibrating_Dict> GetListByEnum(PublicEnum.DemarcateType enum_Demarcate)
        {
            if (enum_Demarcate == PublicEnum.DemarcateType.差压传感器高)
            {
                return DAL_Demarcate_Dict.differentialPressureDictHige;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.差压传感器低)
            {
                return DAL_Demarcate_Dict.differentialPressureDictLow;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.大气压力传感器)
            {
                return DAL_Demarcate_Dict.kPaDict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.风速传感器)
            {
                return DAL_Demarcate_Dict.windSpeedDict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.温度传感器)
            {
                return DAL_Demarcate_Dict.temperatureDict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器A1)
            {
                return DAL_Demarcate_Dict.displacement1Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器A2)
            {
                return DAL_Demarcate_Dict.displacement2Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器A3)
            {
                return DAL_Demarcate_Dict.displacement3Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器B1)
            {
                return DAL_Demarcate_Dict.displacement4Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器B2)
            {
                return DAL_Demarcate_Dict.displacement5Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器B3)
            {
                return DAL_Demarcate_Dict.displacement6Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器C1)
            {
                return DAL_Demarcate_Dict.displacement7Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器C2)
            {
                return DAL_Demarcate_Dict.displacement8Dict;
            }
            if (enum_Demarcate == PublicEnum.DemarcateType.位移传感器C3)
            {
                return DAL_Demarcate_Dict.displacement9Dict;
            }
            return new List<Calibrating_Dict>();
        }


        #region   y=kx+b
        /// <summary>
        /// 获取标定后值
        /// </summary>
        /// <returns></returns>
        public static double GetValues(PublicEnum.DemarcateType enum_Demarcate, float x)
        {
            if (x == 0)
            {
                return x;
            }
            List<Calibrating_Dict> dict = GetListByEnum(enum_Demarcate);

            if (dict == null || dict.Count == 0)
            {
                return Math.Round(x, 2);
            }

            if (dict.Find(t => t.y == x) != null)
            {
                return dict.Find(t => t.y == x).x;
            }

            float k = 0, b = 0;

            Compute_KB(dict, x, ref k, ref b);

            if (k == 0 && b == 0)
                return Math.Round(x, 2);
            return Math.Round(k * x + b, 2);
        }

        private static readonly int _decimaldigits = 2;//小数位数保留2位
      

        /// <summary>
        /// 获取KB
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="x"></param>
        /// <param name="k"></param>
        /// <param name="b"></param>
        private static void Compute_KB(List<Calibrating_Dict> dictList, float x, ref float k, ref float b)
        {
            // 对数据合计
            for (int i = 0; i < dictList.Count; i++)
            {
                if (dictList.Count < i + 1)
                {
                    break;
                }

                if (dictList[i].x > x && i == 0)
                {
                    break;
                }

                if (dictList[i].y > x && dictList[i - 1].y < x)
                {
                    Calculate(dictList[i - 1].y, dictList[i].y, dictList[i - 1].x, dictList[i].x, ref k, ref b);
                }
            }
        }
        /// <summary>
        /// 计算斜率k及纵截距b值
        /// </summary>
        /// <param name="x1">坐标点x1</param>
        /// <param name="x2">坐标点x2</param>
        /// <param name="y1">坐标点y1</param>
        /// <param name="y2">坐标点y2</param>
        /// <param name="kvalue">斜率k值</param>
        /// <param name="bvalue">纵截距b值</param>
        public static void Calculate(float x1, float x2, float y1, float y2, ref float kvalue, ref float bvalue)//求方程y=kx+b 系数 k ,b
        {
            float coefficient = 1;//系数值
            try
            {
                if ((x1 == 0) || (x2 == 0) || (x1 == x2)) return; //排除为零的情况以及x1，x2相等时无法运算的情况
                //if (y1 == y2) return; //根据具体情况而定，如何这两个值相等，得到的就是一条直线
                float temp = 0;
                if (x1 >= x2)
                {
                    coefficient = (float)Math.Round((x1 / x2), _decimaldigits);
                    temp = y2 * coefficient; //将对应的函数乘以系数
                    bvalue = (float)((temp - y1) / (coefficient - 1));
                    kvalue = (float)((y1 - bvalue) / x1); //求出k值
                }
                else
                {
                    coefficient = x2 / x1;
                    temp = y1 * coefficient;
                    bvalue = (float)((temp - y2) / (coefficient - 1));//求出b值
                    kvalue = (float)((y2 - bvalue) / x2); //求出k值
                }
            }
            catch
            {
                bvalue = 0;
                kvalue = 0;
            }

        }





        #endregion


        #region  截面积
        /// <summary>
        /// 计算截面积
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal MathJieArea(double? pipeDiameter = null)
        {
            double _D = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["PipeDiameter"].ToString());
            if (pipeDiameter != null)
            {
                _D = pipeDiameter.Value;
            }
            var res = Math.Pow((_D / 2), 2) * 3.1415;

            if (res.ToString().Contains("E"))
            {
                Decimal dData = 0.0M;
                dData = Decimal.Parse(res.ToString(), System.Globalization.NumberStyles.Float);
                return dData;
            }
            return decimal.Parse(res.ToString());
        }
        #endregion

        #region 获取分级指标缝长和面积

        /// <summary>
        /// 获取分级指标[面积]
        /// </summary>
        ///  <param name="p1">生100</param>
        ///  <param name="p2">降100</param>
        ///  <param name="daqiyali">大气压力</param>
        ///  <param name="wendu">当前温度</param>
        ///  <param name="shijianmianji">试件面级</param>
        public static double GetIndexStitchArea(double p1, double p2, double daqiyali, double shijianmianji, double wendu)
        {
            var area = MathJieArea();
            //风速
            double windSpeed = (p1 + p2) / 2;
            double qt = windSpeed * double.Parse(area.ToString());

            var q1 = (293 * qt * daqiyali) / (101.3 * (wendu + 273)) * 3600;

            var qa = Math.Round(q1 / (shijianmianji * 4.65), 2);
            return qa;
        }

        /// <summary>
        /// 获取分级指标[逢长]
        /// </summary>
        ///  <param name="p1">生100</param>
        ///  <param name="p2">降100</param>
        ///  <param name="daqiyali">大气压力</param>
        ///  <param name="wendu">当前温度</param>
        ///  <param name="kekaifengchang">可开逢长</param>
        public static double GetIndexStichLength(double p1, double p2, double daqiyali, double kekaifengchang, double wendu)
        {
            //风速
            var area = MathJieArea();
            var windSpeed = (p1 + p2) / 2;
            var qk = windSpeed * double.Parse(area.ToString());

            var q2 = (293 * qk * daqiyali) / (101.3 * (wendu + 273)) * 3600;
            if (q2 == 0)
            {
                return 0;
            }
            var ql = Math.Round(q2 / (kekaifengchang * 4.65), 2);
            return ql;
        }
        #endregion


        #region 等级划分

        /// <summary>
        /// 获取逢长等级【可开】
        /// </summary>
        /// <returns></returns>
        public static int GetStitchLengthLevel(double value)
        {
            int res = 0;
            if (4 >= value && value > 2.5)
            {
                res = 1;
            }
            else if (2.5 >= value && value > 1.5)
            {
                res = 2;
            }
            else if (1.5 >= value && value > 0.5)
            {
                res = 3;
            }
            else if (value > 0 && value <= 0.5)
            {
                res = 4;
            }
            return res;
        }

        /// <summary>
        /// 获取面积分级【整体】
        /// </summary>
        /// <returns></returns>
        public static int GetAreaLevel(double value)
        {
            int res = 0;
            if (4 >= value && value > 2)
            {
                res = 1;
            }
            else if (2 >= value && value > 1.2)
            {
                res = 2;
            }
            else if (1.2 >= value && value > 0.5)
            {
                res = 3;
            }

            else if (value > 0 && value <= 0.5)
            {
                res = 4;
            }
            return res;
        }

        /// <summary>
        /// 获取水密分级固定
        /// </summary>
        /// <returns></returns>
        public static int GetWaterTightLevel_GuDing(int value)
        {
            int res = 0;
            if (value >= 500 && value < 700)
            {
                res = 1;
            }
            else if (value >= 700 && value < 1000)
            {
                res = 2;
            }
            else if (value >= 1000 && value < 1500)
            {
                res = 3;
            }
            else if (value >= 1500 && value < 2000)
            {
                res = 4;
            }

            else if (value >= 2000)
            {
                res = 5;
            }
            return res;
        }

        /// <summary>
        /// 获取水密分级可开启
        /// </summary>
        /// <returns></returns>
        public static int GetWaterTightLevel_KeKaiQi(int value)
        {
            int res = 0;
            if (value >= 250 && value < 350)
            {
                res = 1;
            }
            else if (value >= 350 && value < 500)
            {
                res = 2;
            }
            else if (value >= 500 && value < 700)
            {
                res = 3;
            }
            else if (value >= 700 && value < 1000)
            {
                res = 4;
            }
            else if (value >= 1000)
            {
                res = 5;
            }
            return res;
        }

        /// <summary>
        /// 获取风压分级
        /// </summary>
        /// <returns></returns>
        public static int GetWindPressureLevel(int value)
        {
            int res = 0;
            if (1500 > value && value >= 1000)
            {
                res = 1;
            }
            else if (2000 > value && value >= 1500)
            {
                res = 2;
            }
            else if (2500 > value && value >= 2000)
            {
                res = 3;
            }
            else if (3000 > value && value >= 2500)
            {
                res = 4;
            }
            else if (3500 > value && value >= 3000)
            {
                res = 5;
            }
            else if (4000 > value && value >= 3500)
            {
                res = 6;
            }
            else if (4500 > value && value >= 4000)
            {
                res = 7;
            }
            else if (5000 > value && value >= 4500)
            {
                res = 8;
            }
            else if (value >= 5000)
            {
                res = 9;
            }
            return res;
        }

        #endregion
    }
}
