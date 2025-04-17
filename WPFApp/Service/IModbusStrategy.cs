using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;
using NLog;
using Oratoria36.Models;

namespace Oratoria36.Models.Signals
{
    public interface IModbusStrategy
    {
        public bool GetDigitalInput(ushort pinNumber, ModbusIpMaster master)
        {
            try
            {
                return master.ReadInputs(pinNumber, 1)[0];
            }
            catch
            {
                return false;
            }
            
        }
        public void SetDigitalOutput(ushort pinNumber, bool value, ModbusIpMaster master)
        {
            try
            {
                master.WriteSingleCoil(pinNumber, value);
            }
            catch { }
        }
        public ushort GetAnalogInput(ushort channel, ModbusIpMaster master)
        {
            try
            {
                return master.ReadHoldingRegisters(channel, 1)[0];
            }
            catch
            {
                return 0;
            }
        }
        public void SetAnalogOutput(ushort channel, ushort value, ModbusIpMaster master)
        {
            try
            {
                master.WriteSingleRegister(channel, value);
            }
            catch { }
        }
    }   
}
