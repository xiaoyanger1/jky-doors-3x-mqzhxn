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
        /// 抗风压获取正负压  线性回归到10的q'
        /// </summary>
        /// <param name="airtightCalculation"></param>
        /// <param name="zy_q10"></param>
        /// <param name="fy_q10"></param>
        /// <returns></returns>
        public static void GetKFY_P1(List<WindPressureDGV> windPressureDGV, int gjcd, double lx, ref double zy, ref double fy)
        {
            windPressureDGV = windPressureDGV.FindAll(t => t.Pa != "P3阶段" && t.Pa != "P3残余变形" && t.Pa != "PMax/残余变形").ToList();

            string errorMsg = "";

            //正压
            double _z_a = 0;
            double _z_b = 0;
            var z_point = windPressureDGV.FindAll(t => t.zzd > 0).Select(t => new text.doors.Model.Point() { X = t.PaValue, Y = t.zzd }).ToList();
            var z_isSuccess = Formula.LinearRegression(z_point, ref _z_a, ref _z_b, ref errorMsg);
            if (z_isSuccess)
            {
                var y = gjcd / lx;
                zy = Math.Round((y - _z_b) / _z_a, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                zy = -100;
            }

            //负压
            double _f_a = 0;
            double _f_b = 0;

            var f_point = windPressureDGV.FindAll(t => t.fzd > 0).Select(t => new text.doors.Model.Point() { X = t.PaValue, Y = t.fzd }).ToList();
            var f_isSuccess = Formula.LinearRegression(f_point, ref _f_a, ref _f_b, ref errorMsg);
            if (f_isSuccess)
            {
                var y = Math.Round(gjcd / lx, 2, MidpointRounding.AwayFromZero);
                fy = Math.Round((y - _f_b) / _f_a, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                fy = -100;
            }
        }


        /// <summary>
        /// 对一组点通过最小二乘法进行线性回归
        /// </summary>
        /// <param name="parray"></param>
        public static bool LinearRegression(List<text.doors.Model.Point> parray, ref double a, ref double b, ref string errorMsg)
        {
            //点数不能小于2
            if (parray.Count < 2)
            {
                errorMsg = "点的数量小于2，无法进行线性回归";
                return false;
            }

            //求出横纵坐标的平均值
            double averagex = 0, averagey = 0;
            foreach (text.doors.Model.Point p in parray)
            {
                if (double.IsNaN(p.Y))
                {
                    return false;
                }
                averagex += p.X;
                averagey += p.Y;
            }
            averagex /= parray.Count;
            averagey /= parray.Count;

            //经验回归系数的分子与分母
            double numerator = 0;
            double denominator = 0;

            foreach (text.doors.Model.Point p in parray)
            {
                numerator += (p.X - averagex) * (p.Y - averagey);
                denominator += (p.X - averagex) * (p.X - averagex);
            }

            //回归系数b（Regression Coefficient）
            double RCB = numerator / denominator;

            //回归系数a
            double RCA = averagey - RCB * averagex;

            //b = Math.Round(RCB, 4, MidpointRounding.AwayFromZero);
            //a = Math.Round(RCA, 4, MidpointRounding.AwayFromZero);
            a = RCB;
            b = RCA;
            return true;
        }



        #endregion

        #region  计算流量
        /// <summary>
        /// 计算流量
        /// 公式为 Q = 3.1415*D的平方（配置）/4*v(风速平均值)*3600
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double MathFlow(double value)
        {
            if (value == 0)
            {
                return 0;
            }
            //double _D = DefaultBase._D;
            double _D = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["PipeDiameter"].ToString());

            return Math.Round(3.1415 * _D * _D / 4 * value * 3600, 2);
        }
        #endregion


        #region  截面积
        /// <summary>
        /// 计算截面积
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal MathJieArea()
        {
            double _D = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["PipeDiameter"].ToString());
            var res = Math.Pow((_D / 2), 2) * 3.1415;

            if (res.ToString().Contains("E"))
            {
                Decimal dData = 0.0M;
                dData = Decimal.Parse(res.ToString(), System.Globalization.NumberStyles.Float);
                return dData;
            }
            return decimal.Parse(res.ToString());
        }

        private static decimal ChangeDataToD(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Decimal.Parse(strData, System.Globalization.NumberStyles.Float);
            }
            return dData;
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
            var windSpeed = (p1 + p2) / 2;
            var qk = 0;//windSpeed * MathJieArea();

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
        /// 获取水密等级
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetWaterTightLevel(List<Model_dt_sm_Info> waterTight)
        {
            int value = 0;
            if (waterTight == null || waterTight.Count == 0)
                return value;

            //if (waterTight.Count == 3)
            //{
            //    List<int> list = new List<int>();
            //    waterTight.ForEach(t => list.Add(Convert.ToInt32(t.sm_Pa)));
            //    list.Sort();

            //    int min = list[0];
            //int intermediate = list[1];
            //int max = list[2];

            //     int minlevel = DefaultBase.AirtightLevel.Where(t => t.Value == min).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == min).FirstOrDefault().Key : 0;

            //int intermediatelevel = DefaultBase.AirtightLevel.Where(t => t.Value == intermediate).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == intermediate).FirstOrDefault().Key : 0;
            //int maxlevel = DefaultBase.AirtightLevel.Where(t => t.Value == max).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == max).FirstOrDefault().Key : 0;

            //if ((maxlevel - intermediatelevel) > 2)
            //{
            //    foreach (var item in DefaultBase.AirtightLevel)
            //    {
            //        if (item.Key == (intermediatelevel + 2))
            //        {
            //            max = item.Value; break;
            //        }
            //    }
            //}
            //value = (min + intermediate + max) / 3;
            //}
            //else
            //{
            //    foreach (var item in waterTight)
            //        value += int.Parse(item.sm_Pa);

            //    value = value / waterTight.Count;
            //}
            // value = int.Parse(waterTight.Min(t => t.sm_Pa));

            //return Formula.GetWaterTightLevel(value);
            return 0;
        }

        /// <summary>
        /// 获取水密压力
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetWaterTightPressure(List<Model_dt_sm_Info> list)
        {
            int value = 0;

            if (list == null || list.Count == 0)
                return value;

            if (list.Count == 3)
            {
                List<int> pas = new List<int>();
                list.ForEach(t => pas.Add(t.sm_Pa.Value));
                pas.Sort();

                int min = pas[0];
                int intermediate = pas[1];
                int max = pas[2];

                int minlevel = DefaultBase.AirtightLevel.Where(t => t.Value == min).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == min).FirstOrDefault().Key : 0;
                int intermediatelevel = DefaultBase.AirtightLevel.Where(t => t.Value == intermediate).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == intermediate).FirstOrDefault().Key : 0;
                int maxlevel = DefaultBase.AirtightLevel.Where(t => t.Value == max).Count() > 0 ? DefaultBase.AirtightLevel.Where(t => t.Value == max).FirstOrDefault().Key : 0;

                if ((maxlevel - intermediatelevel) > 2)
                {
                    foreach (var item in DefaultBase.AirtightLevel)
                    {
                        if (item.Key == (intermediatelevel + 2))
                        {
                            max = item.Value; break;
                        }
                    }
                }
                value = (min + intermediate + max) / 3;
            }
            else
            {
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (string.IsNullOrWhiteSpace(list[i].sm_Pa))
                //    {
                //        value = 0;
                //        break;
                //    }
                //    value += int.Parse(list[i].sm_Pa.ToString());
                //}
                value = value / list.Count;
            }


            return value;
        }



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
