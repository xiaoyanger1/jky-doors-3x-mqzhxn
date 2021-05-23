using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_qm_zb_Info
    {
        public string dt_Code { get; set; }

        /// <summary>
        /// 正压缝长
        /// </summary>
        public string Z_FC { get; set; }
        /// <summary>
        /// 正压缝长
        /// </summary>
        public string F_FC { get; set; }
        /// <summary>
        /// 正压面积
        /// </summary>
        public string Z_MJ { get; set; }
        /// <summary>
        /// 负压面积
        /// </summary>
        public string F_MJ { get; set; }

    }
}
