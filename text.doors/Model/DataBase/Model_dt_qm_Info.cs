using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_qm_Info
    {
        public string dt_Code { get; set; }
       
        ///// <summary>
        ///// 正压缝长
        ///// </summary>
        //public string qm_Z_FC { get; set; }
        ///// <summary>
        ///// 正压缝长
        ///// </summary>
        //public string qm_F_FC { get; set; }
        ///// <summary>
        ///// 正压面积
        ///// </summary>
        //public string qm_Z_MJ { get; set; }
        ///// <summary>
        ///// 负压面积
        ///// </summary>
        //public string qm_F_MJ { get; set; }

        public string Pa { get; set; }

        /// <summary>
        /// 升 1，降2
        /// </summary>
        public int PaType { get; set; }

        public string FJST { get; set; }
        public string GFZH { get; set; }
        public string ZDST { get; set; }
        public string MQZT { get; set; }
        public string KKST { get; set; }
    }
}
