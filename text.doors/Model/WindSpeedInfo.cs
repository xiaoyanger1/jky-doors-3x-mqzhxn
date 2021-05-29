using text.doors.Common;
using text.doors.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;

namespace text.doors.Model
{
    public class WindSpeedInfo
    {
        public WindSpeedInfo()
        {

            FJST_Z_S_50 = new List<double>();
            FJST_Z_S_100 = new List<double>();
            FJST_Z_S_150 = new List<double>();
            FJST_Z_J_100 = new List<double>();
            FJST_Z_J_50 = new List<double>();
            FJST_Z_YCJY = new List<double>();

            FJST_F_S_50 = new List<double>();
            FJST_F_S_100 = new List<double>();
            FJST_F_S_150 = new List<double>();
            FJST_F_J_100 = new List<double>();
            FJST_F_J_50 = new List<double>();
            FJST_F_YCJY = new List<double>();


            GFZH_Z_S_50 = new List<double>();
            GFZH_Z_S_100 = new List<double>();
            GFZH_Z_S_150 = new List<double>();
            GFZH_Z_J_100 = new List<double>();
            GFZH_Z_J_50 = new List<double>();
            GFZH_Z_YCJY = new List<double>();

            GFZH_F_S_50 = new List<double>();
            GFZH_F_S_100 = new List<double>();
            GFZH_F_S_150 = new List<double>();
            GFZH_F_J_100 = new List<double>();
            GFZH_F_J_50 = new List<double>();
            GFZH_F_YCJY = new List<double>();


            ZDST_Z_S_50 = new List<double>();
            ZDST_Z_S_100 = new List<double>();
            ZDST_Z_S_150 = new List<double>();
            ZDST_Z_J_100 = new List<double>();
            ZDST_Z_J_50 = new List<double>();
            ZDST_Z_YCJY = new List<double>();

            ZDST_F_S_50 = new List<double>();
            ZDST_F_S_100 = new List<double>();
            ZDST_F_S_150 = new List<double>();
            ZDST_F_J_100 = new List<double>();
            ZDST_F_J_50 = new List<double>();
            ZDST_F_YCJY = new List<double>();



            Pa = "0";
            FJST = 0.00;
            GFZH = 0.00;
            ZDST = 0.00;
            //MQZT = 0.00;
            //KKST = 0.00;
        }

        //压力pa
        public string Pa { get; set; }
        public int PaType { get; set; }
        //附加渗透
        public double FJST { get; set; }
        //固附之和
        public double GFZH { get; set; }
        //总的渗透量
        public double ZDST { get; set; }
        //幕墙整体
        public double MQZT
        {
            get
            {
                if (ZDST == 0 || FJST == 0)
                    return 0.00;
                return ZDST - FJST;
            }
        }
        //可开渗透
        public double KKST
        {
            get
            {
                if (ZDST == 0 || GFZH == 0)
                    return 0.00;
                return ZDST - GFZH;
            }
        }

        /// <summary>
        /// 附加渗透
        /// </summary>

        private List<double> FJST_Z_S_50 = new List<double>();
        private List<double> FJST_Z_S_100 = new List<double>();
        private List<double> FJST_Z_S_150 = new List<double>();
        private List<double> FJST_Z_J_100 = new List<double>();
        private List<double> FJST_Z_J_50 = new List<double>();
        private List<double> FJST_Z_YCJY = new List<double>();

        private List<double> FJST_F_S_50 = new List<double>();
        private List<double> FJST_F_S_100 = new List<double>();
        private List<double> FJST_F_S_150 = new List<double>();
        private List<double> FJST_F_J_100 = new List<double>();
        private List<double> FJST_F_J_50 = new List<double>();
        private List<double> FJST_F_YCJY = new List<double>();


        /// <summary>
        /// 固附之和
        /// </summary>

        private List<double> GFZH_Z_S_50 = new List<double>();
        private List<double> GFZH_Z_S_100 = new List<double>();
        private List<double> GFZH_Z_S_150 = new List<double>();
        private List<double> GFZH_Z_J_100 = new List<double>();
        private List<double> GFZH_Z_J_50 = new List<double>();
        private List<double> GFZH_Z_YCJY = new List<double>();

        private List<double> GFZH_F_S_50 = new List<double>();
        private List<double> GFZH_F_S_100 = new List<double>();
        private List<double> GFZH_F_S_150 = new List<double>();
        private List<double> GFZH_F_J_100 = new List<double>();
        private List<double> GFZH_F_J_50 = new List<double>();
        private List<double> GFZH_F_YCJY = new List<double>();

        /// <summary>
        /// 总的渗透
        /// </summary>

        private List<double> ZDST_Z_S_50 = new List<double>();
        private List<double> ZDST_Z_S_100 = new List<double>();
        private List<double> ZDST_Z_S_150 = new List<double>();
        private List<double> ZDST_Z_J_100 = new List<double>();
        private List<double> ZDST_Z_J_50 = new List<double>();
        private List<double> ZDST_Z_YCJY = new List<double>();


        private List<double> ZDST_F_S_50 = new List<double>();
        private List<double> ZDST_F_S_100 = new List<double>();
        private List<double> ZDST_F_S_150 = new List<double>();
        private List<double> ZDST_F_J_100 = new List<double>();
        private List<double> ZDST_F_J_50 = new List<double>();
        private List<double> ZDST_F_YCJY = new List<double>();

        /// <summary>
        /// 获取风速数据
        /// </summary>
        /// <returns></returns>
        public List<WindSpeedInfo> GetWindSpeed()
        {
            List<WindSpeedInfo> list = new List<WindSpeedInfo>();

            AddYL_S50(list);
            AddYL_S100(list);
            AddYL_S150(list);
            AddYL_J100(list);
            AddYL_J50(list);


            AddYL_S_50(list);
            AddYL_S_100(list);
            AddYL_S_150(list);
            AddYL_J_100(list);
            AddYL_J_50(list);

            AddYL_Z_YCJA(list);
            AddYL_F_YCJA(list);

            return list;
        }

        /// <summary>
        /// 增加风速数据(正压附加渗透)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddZY_FJST(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.Z_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter50)
                FJST_Z_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                FJST_Z_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                FJST_Z_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                FJST_Z_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                FJST_Z_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(负压附加渗透)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddFY_FJST(double data, PublicEnum.Kpa_Level fs)
        {
            if (fs == PublicEnum.Kpa_Level.liter50)
                FJST_F_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                FJST_F_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                FJST_F_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                FJST_F_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                FJST_F_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }


        /// <summary>
        /// 增加风速数据(正压固附之和)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddZY_GFZH(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.F_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter50)
                GFZH_Z_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                GFZH_Z_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                GFZH_Z_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                GFZH_Z_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                GFZH_Z_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(负压压固附之和)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void Add_FY_GFZH(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.F_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter50)
                GFZH_F_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                GFZH_F_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                GFZH_F_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                GFZH_F_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                GFZH_F_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(正压固附之和)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddZY_ZDST(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.F_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter50)
                ZDST_Z_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                ZDST_Z_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                ZDST_Z_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                ZDST_Z_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                ZDST_Z_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(负压固附之和)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void Add_FY_ZDST(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.F_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter50)
                ZDST_F_S_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                ZDST_F_S_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                ZDST_F_S_150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                ZDST_F_J_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                ZDST_F_J_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.Z_YCJY)
                GFZH_Z_YCJY.Add(data);
            if (fs == PublicEnum.Kpa_Level.F_YCJY)
                GFZH_F_YCJY.Add(data);
        }


        /// <summary>
        /// 增加升压50
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S50(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "50";
            model.PaType = 1;
            model.FJST = FJST_Z_S_50.Count() == 0 ? 0d : Math.Round(FJST_Z_S_50.Sum(t => t) / FJST_Z_S_50.Count(), 2);
            model.GFZH = GFZH_Z_S_50.Count() == 0 ? 0d : Math.Round(GFZH_Z_S_50.Sum(t => t) / GFZH_Z_S_50.Count(), 2);
            model.ZDST = ZDST_Z_S_50.Count() == 0 ? 0d : Math.Round(ZDST_Z_S_50.Sum(t => t) / ZDST_Z_S_50.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加升压100
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S100(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "100";
            model.PaType = 1;
            model.FJST = FJST_Z_S_100.Count() == 0 ? 0d : Math.Round(FJST_Z_S_100.Sum(t => t) / FJST_Z_S_100.Count(), 2);
            model.GFZH = GFZH_Z_S_100.Count() == 0 ? 0d : Math.Round(GFZH_Z_S_100.Sum(t => t) / GFZH_Z_S_100.Count(), 2);
            model.ZDST = ZDST_Z_S_100.Count() == 0 ? 0d : Math.Round(ZDST_Z_S_100.Sum(t => t) / ZDST_Z_S_100.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加升压150
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S150(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "150";
            model.PaType = 1;
            model.FJST = FJST_Z_S_150.Count() == 0 ? 0d : Math.Round(FJST_Z_S_150.Sum(t => t) / FJST_Z_S_150.Count(), 2);
            model.GFZH = GFZH_Z_S_150.Count() == 0 ? 0d : Math.Round(GFZH_Z_S_150.Sum(t => t) / GFZH_Z_S_150.Count(), 2);
            model.ZDST = ZDST_Z_S_150.Count() == 0 ? 0d : Math.Round(ZDST_Z_S_150.Sum(t => t) / ZDST_Z_S_150.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加降压100
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_J100(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "100";
            model.PaType = 2;
            model.FJST = FJST_Z_J_100.Count() == 0 ? 0d : Math.Round(FJST_Z_J_100.Sum(t => t) / FJST_Z_J_100.Count(), 2);
            model.GFZH = GFZH_Z_J_100.Count() == 0 ? 0d : Math.Round(GFZH_Z_J_100.Sum(t => t) / GFZH_Z_J_100.Count(), 2);
            model.ZDST = ZDST_Z_J_100.Count() == 0 ? 0d : Math.Round(ZDST_Z_J_100.Sum(t => t) / ZDST_Z_J_100.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加降压50
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_J50(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "50";
            model.PaType = 2;
            model.FJST = FJST_Z_J_50.Count() == 0 ? 0d : Math.Round(FJST_Z_J_50.Sum(t => t) / FJST_Z_J_50.Count(), 2);
            model.GFZH = GFZH_Z_J_50.Count() == 0 ? 0d : Math.Round(GFZH_Z_J_50.Sum(t => t) / GFZH_Z_J_50.Count(), 2);
            model.ZDST = ZDST_Z_J_50.Count() == 0 ? 0d : Math.Round(ZDST_Z_J_50.Sum(t => t) / ZDST_Z_J_50.Count(), 2);
            list.Add(model);
        }



        /// <summary>
        /// 增加升压-50
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S_50(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "-50";
            model.PaType = 1;
            model.FJST = FJST_F_S_50.Count() == 0 ? 0d : Math.Round(FJST_F_S_50.Sum(t => t) / FJST_F_S_50.Count(), 2);
            model.GFZH = GFZH_F_S_50.Count() == 0 ? 0d : Math.Round(GFZH_F_S_50.Sum(t => t) / GFZH_F_S_50.Count(), 2);
            model.ZDST = ZDST_F_S_50.Count() == 0 ? 0d : Math.Round(ZDST_F_S_50.Sum(t => t) / ZDST_F_S_50.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加升压-100
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S_100(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "-100";
            model.PaType = 1;
            model.FJST = FJST_F_S_100.Count() == 0 ? 0d : Math.Round(FJST_F_S_100.Sum(t => t) / FJST_F_S_100.Count(), 2);
            model.GFZH = GFZH_F_S_100.Count() == 0 ? 0d : Math.Round(GFZH_F_S_100.Sum(t => t) / GFZH_F_S_100.Count(), 2);
            model.ZDST = ZDST_F_S_100.Count() == 0 ? 0d : Math.Round(ZDST_F_S_100.Sum(t => t) / ZDST_F_S_100.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加升压-150
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_S_150(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "-150";
            model.PaType = 1;
            model.FJST = FJST_F_S_150.Count() == 0 ? 0d : Math.Round(FJST_F_S_150.Sum(t => t) / FJST_F_S_150.Count(), 2);
            model.GFZH = GFZH_F_S_150.Count() == 0 ? 0d : Math.Round(GFZH_F_S_150.Sum(t => t) / GFZH_F_S_150.Count(), 2);
            model.ZDST = ZDST_F_S_150.Count() == 0 ? 0d : Math.Round(ZDST_F_S_150.Sum(t => t) / ZDST_F_S_150.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加降压-100
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_J_100(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "-100";
            model.PaType = 2;
            model.FJST = FJST_F_J_100.Count() == 0 ? 0d : Math.Round(FJST_F_J_100.Sum(t => t) / FJST_F_J_100.Count(), 2);
            model.GFZH = GFZH_F_J_100.Count() == 0 ? 0d : Math.Round(GFZH_F_J_100.Sum(t => t) / GFZH_F_J_100.Count(), 2);
            model.ZDST = ZDST_F_J_100.Count() == 0 ? 0d : Math.Round(ZDST_F_J_100.Sum(t => t) / ZDST_F_J_100.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加降压-50
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_J_50(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "-50";
            model.PaType = 2;
            model.FJST = FJST_F_J_50.Count() == 0 ? 0d : Math.Round(FJST_F_J_50.Sum(t => t) / FJST_F_J_50.Count(), 2);
            model.GFZH = GFZH_F_J_50.Count() == 0 ? 0d : Math.Round(GFZH_F_J_50.Sum(t => t) / GFZH_F_J_50.Count(), 2);
            model.ZDST = ZDST_F_J_50.Count() == 0 ? 0d : Math.Round(ZDST_F_J_50.Sum(t => t) / ZDST_F_J_50.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 依次加压正
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_Z_YCJA(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "正设计值";
            model.PaType = 1;
            model.FJST = FJST_Z_YCJY.Count() == 0 ? 0d : Math.Round(FJST_F_J_50.Sum(t => t) / FJST_F_J_50.Count(), 2);
            model.GFZH = GFZH_Z_YCJY.Count() == 0 ? 0d : Math.Round(GFZH_F_J_50.Sum(t => t) / GFZH_F_J_50.Count(), 2);
            model.ZDST = ZDST_Z_YCJY.Count() == 0 ? 0d : Math.Round(ZDST_F_J_50.Sum(t => t) / ZDST_F_J_50.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 依次加压负
        /// </summary>
        /// <param name="list"></param>
        public void AddYL_F_YCJA(List<WindSpeedInfo> list)
        {
            WindSpeedInfo model = new WindSpeedInfo();
            model.Pa = "负设计值";
            model.PaType = 2;
            model.FJST = FJST_F_YCJY.Count() == 0 ? 0d : Math.Round(FJST_F_J_50.Sum(t => t) / FJST_F_J_50.Count(), 2);
            model.GFZH = GFZH_F_YCJY.Count() == 0 ? 0d : Math.Round(GFZH_F_J_50.Sum(t => t) / GFZH_F_J_50.Count(), 2);
            model.ZDST = ZDST_F_YCJY.Count() == 0 ? 0d : Math.Round(ZDST_F_J_50.Sum(t => t) / ZDST_F_J_50.Count(), 2);
            list.Add(model);
        }

    }
}
