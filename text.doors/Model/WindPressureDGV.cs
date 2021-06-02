using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;

namespace text.doors.Model
{
    public class WindPressureDGV
    {

        public string Pa { get; set; }


        public double zwy1 { get; set; }
        public double zwy2 { get; set; }
        public double zwy3 { get; set; }

        public double zzd
        {
            get
            {
                return Math.Round(this.zwy2 - ((this.zwy1 + this.zwy3) / 2), 2);

                //return (double)Math.Round(decimal.Parse((this.zwy2 - (this.zwy1 + this.zwy3) / 2).ToString()), 2, MidpointRounding.AwayFromZero);
            }
        }
        public double zlx
        {
            get
            {
                return this.zzd == 0 ? 0d : Convert.ToInt32(GanJianChangDu / this.zzd);
            }
        }



        public double fwy1 { get; set; }
        public double fwy2 { get; set; }
        public double fwy3 { get; set; }


        public double fzd
        {
            get
            {
                return System.Math.Abs(Math.Round(this.fwy2 - (this.fwy1 + this.fwy3) / 2, 2));
            }
        }

        public double flx
        {
            get
            {
                return this.fzd == 0 ? 0d : Convert.ToInt32(GanJianChangDu / this.fzd);
            }
        }
        public int PaValue { get; set; }
        public int GanJianChangDu { get; set; }
    }
}
