using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Default
{
    public class PublicEnum
    {
        /// <summary>
        /// 压力级别枚举
        /// </summary>
        public enum Kpa_Level
        {
            liter50,//升50 
            liter100,//升100 
            liter150,//升150
            drop100,//降100
            drop50,//降50
            Z_YCJY,
            F_YCJY
        }

        /// <summary>
        /// 系统项
        /// </summary>
        public enum SystemItem
        {
            /// <summary>
            /// 水密
            /// </summary>
            Watertight,
            /// <summary>
            /// 气密
            /// </summary>
            Airtight,
            /// <summary>
            /// 风压
            /// </summary>
            AirPressure,
            /// <summary>
            /// 平面水平
            /// </summary>
            PlaneDeformation

        }

        /// <summary>
        /// 气密性能检测
        /// </summary>
        public enum AirtightPropertyTest
        {
            /// <summary>
            /// 正压预备
            /// </summary>
            ZReady,
            /// <summary>
            /// 正压开始
            /// </summary>
            ZStart,
            /// <summary>
            /// 负压预备
            /// </summary>
            FReady,
            /// <summary>
            /// 负压开始
            /// </summary>
            FStart,
            /// <summary>
            /// 停止
            /// </summary>
            Stop,
            /// <summary>
            /// 正依次加压
            /// </summary>
            ZYCJY,
            /// <summary>
            /// 负依次加压
            /// </summary>
            FYCJY
        }


        /// <summary>
        /// 水密性能检测
        /// </summary>
        public enum WaterTightPropertyTest
        {
            Ready,//预备
            Start,//开始
            Next,//下一级
            CycleLoading,//依次加压
            Stop, //停止
            SrartBD, //开始波动
            StopBD //结束波动
        }


        /// <summary>
        /// 风压性能检测
        /// </summary>
        public enum WindPressureTest
        {
            /// <summary>
            /// 正压预备
            /// </summary>
            ZReady,
            /// <summary>
            /// 正压开始
            /// </summary>
            ZStart,
            /// <summary>
            /// 负压预备
            /// </summary>
            FReady,
            /// <summary>
            /// 负压开始
            /// </summary>
            FStart,
            /// <summary>
            /// 正反复
            /// </summary>
            ZRepeatedly,
            /// <summary>   
            /// 负反复
            /// </summary>
            FRepeatedly,
            /// <summary>
            /// 正安全
            /// </summary>
            ZSafety,
            /// <summary>
            /// 负安全
            /// </summary>
            FSafety,
            /// <summary>
            /// 结束
            /// </summary>
            /// 
            /// <summary>
            /// 正pmax
            /// </summary>
            ZPmax,
            /// <summary>
            /// 负pmax
            /// </summary>
            FPmax,
            /// <summary>
            /// 结束
            /// </summary>
            Stop
        }

        /// <summary>
        /// 标定
        /// </summary>
        public enum DemarcateType
        {
            风速传感器,
            差压传感器高,
            差压传感器低,
            温度传感器,
            大气压力传感器,
            位移传感器A1,
            位移传感器A2,
            位移传感器A3,
            位移传感器B1,
            位移传感器B2,
            位移传感器B3,
            位移传感器C1,
            位移传感器C2,
            位移传感器C3,
            位移传感器PM
        }

        /// <summary>
        /// 检测项
        /// </summary>
        public enum DetectionItem
        {
            气密水密抗风压性能检测,
            气密性能检测,
            水密性能检测,
            抗风压性能检测,
            气密性能及水密性能检测,
            气密性能及抗风压性能检测,
            水密性能及抗风压性能检测
        }

        /// <summary>
        /// 位移
        /// </summary>
        public enum DisplaceEnum
        {
            位移A1,
            位移A2,
            位移A3,
            位移B1,
            位移B2,
            位移B3,
            位移C1,
            位移C2,
            位移C3
        }

        public enum QM_TestCount
        {
            第一次 = 1,
            第二次 = 2
        }


        public enum KFY_DGVENUM
        {
            DGV_A = 1,
            DGV_B = 2,
            DGV_C = 3
        }
    }
}