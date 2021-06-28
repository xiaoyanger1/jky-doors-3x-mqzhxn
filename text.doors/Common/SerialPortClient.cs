using Modbus.Device;
using Modbus.Utility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Default;
using static text.doors.Default.PublicEnum;

namespace text.doors.Common
{
    public class SerialPortClient
    {
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();

        public SerialPort sp = new SerialPort();

        public static string _protName = System.Configuration.ConfigurationSettings.AppSettings["ProtName"].ToString();
        public static string _baudRate = System.Configuration.ConfigurationSettings.AppSettings["BaudRate"].ToString();
        public static string _dataBits = System.Configuration.ConfigurationSettings.AppSettings["DataBits"].ToString();
        public static string _stopBits = System.Configuration.ConfigurationSettings.AppSettings["StopBits"].ToString();
        public static string _parity = System.Configuration.ConfigurationSettings.AppSettings["Parity"].ToString();
        public static string _slaveID = System.Configuration.ConfigurationSettings.AppSettings["SlaveID"].ToString();

        public ModbusSerialMaster _MASTER;
        private static readonly Object syncLock = new Object();

        private ushort _StartAddress = 0;
        private ushort _NumOfPoints = 1;
        private byte _SlaveID = byte.Parse(_slaveID);

        public SerialPortClient()
        {
            sp.PortName = _protName;//串口号
            sp.BaudRate = int.Parse(_baudRate);//波特率
            sp.DataBits = int.Parse(_dataBits);//数据位
            sp.StopBits = (StopBits)int.Parse(_stopBits);//停止位
            if (_parity == "None")
            {
                sp.Parity = Parity.None;//校验位
            }
            else if (_parity == "Even")
            {
                sp.Parity = Parity.Even;//校验位
            }
            sp.ReadTimeout = 1500;//读取数据的超时时间，引发ReadExisting异常
        }


        public void Init()
        {
            string[] strPortNames = SerialPort.GetPortNames();

            if (strPortNames == null || strPortNames.Length == 0)
            {
                MessageBox.Show("未找到串口！", "串口", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
        public void SerialPortOpen()
        {
            if (_MASTER != null)
                _MASTER.Dispose();
            try
            {
                if (sp.IsOpen)
                {
                    sp.Close();
                    sp.Open();//打开串口
                }
                else
                {
                    sp.Open();//打开串口
                }
                //由客户端创建Modbus TCP的主
                _MASTER = ModbusSerialMaster.CreateAscii(sp);
                _MASTER.Transport.Retries = 0;   //不必调试
                _MASTER.Transport.ReadTimeout = 1500;//读取超时
            }
            catch (Exception ex)
            {
                sp.Close();
            }
        }


        #region 气密

        /// <summary>
        /// 发送按钮
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SendBtnSingleCoil(string command)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public int ReadEndState(string command)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        /// <summary>
        /// 获取正压是否开始计时
        /// </summary>
        public bool ReadQMTimeStart(string key)
        {
            if (sp.IsOpen)
            {
                lock (syncLock)
                {
                    _StartAddress = BFMCommand.GetCommandDict(key);
                    ushort[] t = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (Convert.ToInt32(t[0]) > 5)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }


        #endregion

        #region 水密

        /// <summary>
        /// 读取水密预备设定压力-稳定加压
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetSMYBSDYL(ref bool IsSuccess, string type)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (type == "SMYB")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密预备_设定值);
                        }
                        else if (type == "SMKS")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密开始_设定值);
                        }
                        else if (type == "YCJY")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密依次加压_设定值);
                        }
                        else if (type == "XYJ")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密开始_设定值);
                        }

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                    }
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 设置水密依次加压
        /// </summary>
        public bool SendSMYCJY(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.依次加压数值);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.依次加压);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 开关控制
        /// </summary>
        public bool SendSingleCoilControl(string command)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        #endregion


        #region 波动加压

        /// <summary>
        /// 设置水密-工程检测波动开始波动
        /// </summary>
        public bool SendBoDongksjy(double maxValue, double minValue)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.上限压力设定);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(maxValue));

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.下限压力设定);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(minValue));


                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.工程检测水密性波动开始);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 切换波动
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool qiehuanTab(bool type)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.国标检测波动加压开始);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, type);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        #endregion


        #region 位移



        /// <summary>
        /// 获取十个位移
        /// </summary>
        /// <returns></returns>
        public List<double> ReadHoldingRegisters(string choiceDisplace = "")
        {
            List<double> res = new List<double>();
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        var command = BFMCommand.位移1;
                        ushort numOfPoints = 20;

                        _StartAddress = BFMCommand.GetCommandDict(command);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, numOfPoints);
                        if (holding_register.Length > 0 && holding_register != null)
                        {
                            res.Add(ChangeValue(DemarcateType.位移传感器A1, holding_register[0], holding_register[1]));
                            res.Add(ChangeValue(DemarcateType.位移传感器A2, holding_register[2], holding_register[3]));
                            res.Add(ChangeValue(DemarcateType.位移传感器A3, holding_register[4], holding_register[5]));

                            res.Add(ChangeValue(DemarcateType.位移传感器B1, holding_register[6], holding_register[7]));
                            res.Add(ChangeValue(DemarcateType.位移传感器B2, holding_register[8], holding_register[9]));
                            res.Add(ChangeValue(DemarcateType.位移传感器B3, holding_register[10], holding_register[11]));

                            res.Add(ChangeValue(DemarcateType.位移传感器C1, holding_register[12], holding_register[13]));
                            res.Add(ChangeValue(DemarcateType.位移传感器C2, holding_register[14], holding_register[14]));
                            res.Add(ChangeValue(DemarcateType.位移传感器C3, holding_register[16], holding_register[17]));

                            res.Add(ChangeValue(DemarcateType.位移传感器PM, holding_register[18], holding_register[19]));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return res;
        }


        private double ChangeValue(DemarcateType enumDemarcate, ushort highOrderValue, ushort lowOrderValue)
        {
            var data = highOrderValue;
            var value = double.Parse(data.ToString());
            if (int.Parse(data.ToString()) > 35000)
            {
                value = -(65535 - int.Parse(data.ToString()));
            }
            else
            {
                value = int.Parse(data.ToString());
            }
            value = value / 100;

            var res = Formula.GetValues(enumDemarcate, float.Parse(value.ToString()));
            if (enumDemarcate == DemarcateType.位移传感器PM)
            {
                return double.Parse(Math.Round(res, 2).ToString());
            }
            else
            {
                return double.Parse(Math.Abs(Math.Round(res, 2)).ToString());

            }
        }


        /// <summary>
        /// 设置位移归零
        /// </summary>
        public bool SendDisplaceZero(string command)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils[0])
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置风压安全反复数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_FY_Value(string commandValue, string commandStr, double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandValue);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                        
                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);                        
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }


        /// <summary>
        /// 获取风压是否开始计时
        /// </summary>
        public bool Read_FY_Static_IsStart(string commandStr)
        {
            try
            {
                Logger.Info("获取风压计时开始commandStr:" + commandStr);
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        ushort[] t = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (commandStr == BFMCommand.风压安全_负压是否计时 || commandStr == BFMCommand.风压安全_正压是否计时)
                        {
                            if (Convert.ToInt32(t[0]) >= 15)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            if (Convert.ToInt32(t[0]) >= 40)
                                return true;
                            else
                                return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Info(ex);
                return false;
            }

        }


        /// <summary>
        /// 读取风压按钮状态
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int ReadHoldingRegistersBtnType(string command, ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        #endregion

        #region 首页


        /// <summary>
        /// 设置高压标0
        /// </summary>
        public bool SendGYBD(ref bool _GaoYaGuiLing, bool logon = false)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.高压标0_交替型按钮);
                        bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils[0])
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _GaoYaGuiLing = false;
                        }
                        else
                        {
                            if (logon == false)
                                _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _GaoYaGuiLing = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置低压标0
        /// </summary>
        public bool SendDYBD(ref bool _DiyaGuiLing, bool logon = false)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.低压标0_交替型按钮);
                        bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils[0])
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _DiyaGuiLing = false;
                        }
                        else
                        {
                            if (logon == false)
                                _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _DiyaGuiLing = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取温度显示
        /// </summary>
        //public double GetWDXS()
        //{
        //    double res = 0;
        //    if (sp.IsOpen)
        //    {
        //        try
        //        {
        //            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.温度显示);

        //            ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
        //            if (holding_register.Length > 0)
        //            {
        //                res = double.Parse((double.Parse(holding_register[0].ToString()) / 10).ToString());
        //                res = Formula.GetValues(PublicEnum.DemarcateType.温度传感器, float.Parse(res.ToString()));
        //            }
        //        }
        //        catch (Exception ex)
        //        { }

        //    }
        //    return res;
        //}

        /// <summary>
        /// 获取大气压力显示
        /// </summary>
        //public double GetDQYLXS()
        //{
        //    double res = 0;

        //    if (sp.IsOpen)
        //    {
        //        try
        //        {
        //            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.大气压力显示);
        //            ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
        //            if (holding_register.Length > 0)
        //            {
        //                res = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
        //                res = Formula.GetValues(PublicEnum.DemarcateType.大气压力传感器, float.Parse(res.ToString()));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return res;
        //}

        /// <summary>
        /// 差压显示
        /// </summary>
        /// <returns></returns>
        public List<double> ReadHoldingRegisters_Show()
        {
            List<double> result = new List<double>(); ;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        var command = BFMCommand.风速显示;
                        ushort numOfPoints = 44;

                        //public const string 风速显示 = "D90";
                        //public const string 差压高显示 = "D130";
                        //public const string 温度显示 = "D132";
                        //public const string 大气压力显示 = "D134";
                        _StartAddress = BFMCommand.GetCommandDict(command);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, numOfPoints);
                        if (holding_register.Length > 0 && holding_register != null)
                        {
                            double res = 0;
                            var f = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                            res = Formula.GetValues(PublicEnum.DemarcateType.风速传感器, float.Parse(f.ToString()));
                            result.Add(res);

                            var f1 = double.Parse((double.Parse(holding_register[32].ToString()) / 100).ToString());
                            res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器高, float.Parse(f1.ToString()));
                            result.Add(res);

                            var f2 = double.Parse((double.Parse(holding_register[34].ToString()) / 10).ToString());
                            res = Formula.GetValues(PublicEnum.DemarcateType.温度传感器, float.Parse(f2.ToString()));
                            result.Add(res);

                            var f3 = double.Parse((double.Parse(holding_register[36].ToString()) / 100).ToString());
                            res = Formula.GetValues(PublicEnum.DemarcateType.大气压力传感器, float.Parse(f3.ToString()));
                            result.Add(res);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 获取风速显示
        /// </summary>
        //public double GetFSXS()
        //{
        //    double res = 0;
        //    if (sp.IsOpen)
        //    {
        //        try
        //        {
        //            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风速显示);
        //            ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
        //            if (holding_register.Length > 0)
        //            {
        //                var f = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
        //                res = Formula.GetValues(PublicEnum.DemarcateType.风速传感器, float.Parse(f.ToString()));
        //            }
        //        }
        //        catch (Exception ex) { }
        //    }
        //    return res;
        //}


        /// <summary>
        /// 读取差压高显示
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetCY_High()
        {
            double res = 0;

            if (sp.IsOpen)
            {
                try
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.差压高显示);

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (holding_register.Length > 0)
                        {
                            var f = double.Parse(holding_register[0].ToString());

                            if (int.Parse(holding_register[0].ToString()) > 10000)
                                f = -(65535 - int.Parse(holding_register[0].ToString()));
                            else
                                f = int.Parse(holding_register[0].ToString());

                            res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器高, float.Parse(f.ToString()));

                            return int.Parse(Math.Round(res, 0).ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return 0;
        }


        /// <summary>
        /// 读取差压低传感器显示
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetCY_Low()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.差压低显示);

                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        var f = double.Parse(holding_register[0].ToString());

                        if (int.Parse(holding_register[0].ToString()) > 10000)
                            f = -(65535 - int.Parse(holding_register[0].ToString()));
                        else
                            f = int.Parse(holding_register[0].ToString());

                        f = double.Parse((f / 10).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器低, float.Parse(f.ToString()));
                        return int.Parse(Math.Round(res, 0).ToString());
                    }
                }
                catch (Exception ex) { }
            }
            return 0;
        }



        /// <summary>
        /// 读取抗风压极差
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetKFYjC()
        {
            int res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.改变级差);

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (holding_register.Length > 0)
                        {
                            res = int.Parse(holding_register[0].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return res;
        }

        /// <summary>
        /// 设置风机控制
        /// </summary>
        public bool SendFJKZ(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机控制);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置漏气阀控制
        /// </summary>
        public bool SendLQFKZ(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.漏气阀控制);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// 获取风机显示
        /// </summary>
        public double ReadFJSD(ref bool IsSuccess)
        {
            double res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机设定值);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return res;
        }

        /// <summary>
        /// 获取漏气阀显示
        /// </summary>
        public double ReadLQFShow(ref bool IsSuccess)
        {
            double res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.漏气阀设定值);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 获取漏气阀显示
        /// </summary>
        public double ReadPMStopShow(ref bool IsSuccess)
        {
            double res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.平面停止);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        public bool SendFengJiQiDong(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机启动);

                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 水泵启动
        /// </summary>
        public bool SendShuiBengQiDong(ref bool _ShuiBengQiDong)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水泵启动);
                        if (_ShuiBengQiDong)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _ShuiBengQiDong = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _ShuiBengQiDong = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SendShuiBengQiDong(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水泵启动);
                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);

                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 保护阀通
        /// </summary>
        public bool SendBaoHuFaTong(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.保护阀通);
                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 气密阀
        /// </summary>
        public bool SendQiMiFaKai(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.气密阀);
                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 四通阀开
        /// </summary>
        public bool SendSiTongFaKai(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.四通阀开);
                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 点动开
        /// </summary>
        public bool SendDianDongKai(bool isdown)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (isdown)
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动开);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动开);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// 点动关
        /// </summary>
        public bool SendDianDongGuan(bool isdown)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (isdown)
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动关);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动关);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 写入PID
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SendPid(string type, double value)
        {
            try
            {
                lock (syncLock)
                {
                    if (sp.IsOpen)
                    {
                        if (type == "P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.P);
                        else if (type == "I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.I);
                        else if (type == "D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.D);
                        else if (type == "_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._P);
                        else if (type == "_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._I);
                        else if (type == "_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._D);
                        else if (type == "B_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_P);
                        else if (type == "B_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_I);
                        else if (type == "B_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_D);

                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取PID
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetPID(string type, ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (type == "P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.P);
                        else if (type == "I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.I);
                        else if (type == "D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.D);

                        else if (type == "_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._P);
                        else if (type == "_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._I);
                        else if (type == "_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._D);

                        else if (type == "B_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_P);
                        else if (type == "B_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_I);
                        else if (type == "B_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_D);


                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return res;
        }

        #endregion



        /// <summary>
        /// 设置改变级差
        /// </summary>
        public bool SendGBJC(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.改变级差);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                    }
                }
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 级别按钮
        /// </summary>
        public bool SendLevelValueBtn(int commonNum, double v1, double v2)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        string btn_ZF_Num = "";
                        string btn_XZ_Num = "";
                        if (commonNum == 1)
                        {
                            btn_ZF_Num = BFMCommand.第一级振幅;
                            btn_XZ_Num = BFMCommand.第一级修正;
                        }
                        else if (commonNum == 2)
                        {
                            btn_ZF_Num = BFMCommand.第二级振幅;
                            btn_XZ_Num = BFMCommand.第二级修正;
                        }
                        else if (commonNum == 3)
                        {
                            btn_ZF_Num = BFMCommand.第三级振幅;
                            btn_XZ_Num = BFMCommand.第三级修正;
                        }
                        else if (commonNum == 4)
                        {
                            btn_ZF_Num = BFMCommand.第四级振幅;
                            btn_XZ_Num = BFMCommand.第四级修正;
                        }
                        else if (commonNum == 5)
                        {
                            btn_ZF_Num = BFMCommand.第五级振幅;
                            btn_XZ_Num = BFMCommand.第五级修正;
                        }


                        _StartAddress = BFMCommand.GetCommandDict(btn_ZF_Num);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(v1));

                        _StartAddress = BFMCommand.GetCommandDict(btn_XZ_Num);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(v2));
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 级别按钮
        /// </summary>
        public bool SendLevelBtn(int commonNum)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        string btn_Num = "";
                        if (commonNum == 1)
                        {
                            btn_Num = BFMCommand.第一级;
                        }
                        else if (commonNum == 2)
                        {
                            btn_Num = BFMCommand.第二级;
                        }
                        else if (commonNum == 3)
                        {
                            btn_Num = BFMCommand.第三级;
                        }
                        else if (commonNum == 4)
                        {
                            btn_Num = BFMCommand.第四级;
                        }
                        else if (commonNum == 5)
                        {
                            btn_Num = BFMCommand.第五级;
                        }

                        _StartAddress = BFMCommand.GetCommandDict(btn_Num);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SendTuiGanQiDong(int stat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.推杆启动);

                        if (stat == 1)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
    }
}
