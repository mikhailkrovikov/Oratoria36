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
            return master.ReadInputs(pinNumber, 1)[0];
        }
        public void SetDigitalOutput(ushort pinNumber, bool value, ModbusIpMaster master)
        {
            master.WriteSingleCoil(pinNumber, value);
        }
        public ushort GetAnalogInput(ushort channel, ModbusIpMaster master)
        {
            return master.ReadHoldingRegisters(channel, 1)[channel];
        }
        public void SetAnalogOutput(ushort channel, ushort value, ModbusIpMaster master)
        {
            master.WriteSingleRegister(channel, value);
        }
    }
}
