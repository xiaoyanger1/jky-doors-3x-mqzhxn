using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;
using text.doors.Model;
using Young.Core.SQLite;

namespace text.doors.Service
{

    public class DAL_Demarcate_Dict
    {
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();

        private static List<Calibrating_Dict> DemarcateList = new List<Calibrating_Dict>();


        public DAL_Demarcate_Dict()
        {
            // DemarcateList = GetCalibrating_Dict();
        }


        //<summary>
        //温度传感器
        //</summary>
        private static List<Calibrating_Dict> _temperatureDict = null;
        public static List<Calibrating_Dict> temperatureDict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_temperatureDict == null)
                    temperatureDict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.温度传感器.ToString()).OrderBy(t => t.x).ToList();
                return _temperatureDict;
            }
            set
            {
                _temperatureDict = value;
            }
        }

        //<summary>
        //差压传感器高
        //</summary>
        private static List<Calibrating_Dict> _differentialPressureDictHige = null;
        public static List<Calibrating_Dict> differentialPressureDictHige
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_differentialPressureDictHige == null)
                    differentialPressureDictHige = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.差压传感器高.ToString()).OrderBy(t => t.x).ToList();
                return _differentialPressureDictHige;
            }
            set
            {
                _differentialPressureDictHige = value;
            }
        }
        //<summary>
        //差压传感器低
        //</summary>
        private static List<Calibrating_Dict> _differentialPressureDictLow = null;
        public static List<Calibrating_Dict> differentialPressureDictLow
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_differentialPressureDictLow == null)
                    differentialPressureDictLow = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.差压传感器低.ToString()).OrderBy(t => t.x).ToList();
                return _differentialPressureDictLow;
            }
            set
            {
                _differentialPressureDictLow = value;
            }
        }

        //<summary>
        //风速传感器
        //</summary>
        private static List<Calibrating_Dict> _windSpeedDict = null;
        public static List<Calibrating_Dict> windSpeedDict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_windSpeedDict == null)
                    windSpeedDict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.风速传感器.ToString()).OrderBy(t => t.x).ToList();
                return _windSpeedDict;
            }
            set
            {
                _windSpeedDict = value;
            }
        }

        //<summary>
        //大气压力传感器
        //</summary>
        private static List<Calibrating_Dict> _kPaDict = null;
        public static List<Calibrating_Dict> kPaDict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_kPaDict == null)
                    kPaDict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.大气压力传感器.ToString()).OrderBy(t => t.x).ToList();
                return _kPaDict;
            }
            set
            {
                _kPaDict = value;
            }
        }

        //<summary>
        //位移传感器1
        //</summary>
        private static List<Calibrating_Dict> _displacement1Dict = null;
        public static List<Calibrating_Dict> displacement1Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement1Dict == null)
                    displacement1Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器A1.ToString()).OrderBy(t => t.x).ToList();
                return _displacement1Dict;
            }
            set
            {
                _displacement1Dict = value;
            }
        }
        //<summary>
        //位移传感器2
        //</summary>
        private static List<Calibrating_Dict> _displacement2Dict = null;
        public static List<Calibrating_Dict> displacement2Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement2Dict == null)
                    displacement2Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器A2.ToString()).OrderBy(t => t.x).ToList();
                return _displacement2Dict;
            }
            set
            {
                _displacement2Dict = value;
            }
        }

        //<summary>
        //位移传感器3
        //</summary>
        private static List<Calibrating_Dict> _displacement3Dict = null;
        public static List<Calibrating_Dict> displacement3Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement3Dict == null)
                    displacement3Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器A3.ToString()).OrderBy(t => t.x).ToList();
                return _displacement3Dict;
            }
            set
            {
                _displacement3Dict = value;
            }
        }

        //<summary>
        //位移传感器3
        //</summary>
        private static List<Calibrating_Dict> _displacement4Dict = null;
        public static List<Calibrating_Dict> displacement4Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement4Dict == null)
                    displacement4Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器B1.ToString()).OrderBy(t => t.x).ToList();
                return _displacement4Dict;
            }
            set
            {
                _displacement4Dict = value;
            }
        }


        //<summary>
        //位移传感器3
        //</summary>
        private static List<Calibrating_Dict> _displacement5Dict = null;
        public static List<Calibrating_Dict> displacement5Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement5Dict == null)
                    displacement5Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器B2.ToString()).OrderBy(t => t.x).ToList();
                return _displacement5Dict;
            }
            set
            {
                _displacement5Dict = value;
            }
        }


        //<summary>
        //位移传感器B3
        //</summary>
        private static List<Calibrating_Dict> _displacement6Dict = null;
        public static List<Calibrating_Dict> displacement6Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement6Dict == null)
                    displacement6Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器B3.ToString()).OrderBy(t => t.x).ToList();
                return _displacement6Dict;
            }
            set
            {
                _displacement6Dict = value;
            }
        }


        //<summary>
        //位移传感器c1
        //</summary>
        private static List<Calibrating_Dict> _displacement7Dict = null;
        public static List<Calibrating_Dict> displacement7Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement7Dict == null)
                    displacement7Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器C1.ToString()).OrderBy(t => t.x).ToList();
                return _displacement7Dict;
            }
            set
            {
                _displacement7Dict = value;
            }
        }


        //<summary>
        //位移传感器C2
        //</summary>
        private static List<Calibrating_Dict> _displacement8Dict = null;
        public static List<Calibrating_Dict> displacement8Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement8Dict == null)
                    displacement8Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器C2.ToString()).OrderBy(t => t.x).ToList();
                return _displacement8Dict;
            }
            set
            {
                _displacement8Dict = value;
            }
        }


        //<summary>
        //位移传感器C3
        //</summary>
        private static List<Calibrating_Dict> _displacement9Dict = null;
        public static List<Calibrating_Dict> displacement9Dict
        {
            get
            {
                if (DemarcateList.Count == 0)
                {
                    DemarcateList = GetCalibrating_Dict();
                }
                if (_displacement9Dict == null)
                    displacement9Dict = DemarcateList.FindAll(t => t.Enum == PublicEnum.DemarcateType.位移传感器C3.ToString()).OrderBy(t => t.x).ToList();
                return _displacement9Dict;
            }
            set
            {
                _displacement9Dict = value;
            }
        }



        private static List<Calibrating_Dict> GetCalibrating_Dict()
        {
            List<Calibrating_Dict> list = new List<Calibrating_Dict>();
            try
            {
                string sql = string.Format("select * from Demarcate_Dict");

                DataTable dt = SQLiteHelper.ExecuteDataset(sql).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    Calibrating_Dict model = new Calibrating_Dict();
                    if (!string.IsNullOrWhiteSpace(dr["Enum"].ToString()))
                    {
                        model.Enum = dr["Enum"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(dr["D_Key"].ToString()))
                    {
                        model.x = float.Parse(dr["D_Key"].ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(dr["D_Value"].ToString()))
                    {
                        model.y = float.Parse(dr["D_Value"].ToString());
                    }
                    list.Add(model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return list;
        }

    }
}
