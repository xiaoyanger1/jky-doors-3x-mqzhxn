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

        /// <summary>
        /// 1.监控 2.工程检测
        /// </summary>
        public string testtype { get; set; }

        public int z_sjz_value { get; set; }
        public int f_sjz_value { get; set; }
        /// <summary>
        /// 设计值正附加
        /// </summary>
        public string sjz_z_fj { get; set; }
        /// <summary>
        /// 设计值正总的
        /// </summary>
        public string sjz_z_zd { get; set; }
        /// <summary>
        /// 设计值负附加
        /// </summary>
        public string sjz_f_fj { get; set; }
        /// <summary>
        /// 设计值负总的
        /// </summary>
        public string sjz_f_zd { get; set; }

    }
}
