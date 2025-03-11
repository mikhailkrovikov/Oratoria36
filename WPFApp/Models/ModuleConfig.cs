using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Modbus.Device;
using NLog;

namespace Oratoria36.Models
{
    public class ModuleConfig : INotifyPropertyChanged
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        TcpClient? TcpClient;
        ModbusIpMaster? Master;

        bool _isConnected;
        string? _ip;

        public string IP
        {
            get => _ip;
            private set
            {
                if (_ip != value)
                {
                    Logger.Info($"IP изменен с {_ip} на {value}");
                    _ip = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    Logger.Info($"Состояние сети изменено: {(value ? "Подключено" : "Отключено")}");
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }


        public void SetIP(string newIp)
        {
            if (IP != newIp)
            {
                CloseConnection();
                IP = newIp;
                Task.Run(() => InitializeModbusAsync(newIp));
                OnPropertyChanged(nameof(IP));
            }
        }

        public async Task InitializeModbusAsync(string ip)
        {
            try
            {
                TcpClient = new TcpClient();
                await TcpClient.ConnectAsync(ip, 502);
                Master = ModbusIpMaster.CreateIp(TcpClient);
                IsConnected = true;
                Logger.Info($"Успешное подключение к IP: {ip}");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Logger.Error(ex, $"Не удалось подключиться к IP: {ip}");
            }
        }

        public void CloseConnection()
        {
            if (IsConnected)
            {
                try
                {
                    Master?.Dispose();
                    TcpClient?.Close();
                    IsConnected = false;
                    Logger.Info("Соединение закрыто");
                }
                catch (Exception ex)
                {
                    Logger.Info("Соединение не удалось закрыть");
                }   
            }
            else
            {
                Logger.Info("Соединение не удалось закрыть");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Modbus команды

        public void WriteSingleRegister(ushort address, ushort value)
        {
            if (IsConnected)
            {
                Master.WriteSingleRegister(address, value);
            }
        }

        public void WriteMultipleRegisters(ushort startAddress, ushort[] values)
        {
            if (IsConnected)
            {
                Master.WriteMultipleRegisters(startAddress, values);
            }
        }

        public ushort ReadHoldingRegister(ushort address)
        {
            if (IsConnected)
            {
                return Master.ReadHoldingRegisters(address, 1)[0];
            }
            else
            {
                return (ushort)0;
            }
        }

        public ushort[] ReadHoldingRegisters(ushort startAddress, ushort count)
        {
            if (IsConnected)
            {
                return Master.ReadHoldingRegisters(startAddress, count);
            }
            else
            {
                return new ushort[count];
            }
        }

        public ushort ReadInputRegister(ushort address)
        {
            if (IsConnected)
            {
                return Master.ReadInputRegisters(address, 1)[0];
            }
            else
            {
                return (ushort)0;
            }
        }

        public ushort[] ReadInputRegisters(ushort startAddress, ushort count)
        {
            if (IsConnected)
            {
                return Master.ReadInputRegisters(startAddress, count);
            }
            else
            {
                return new ushort[count];
            }
        }

        public void WriteSingleCoil(ushort address, bool value)
        {
            if (IsConnected)
            {
                Master.WriteSingleCoil(address, value);
            }
        }

        public void WriteMultipleCoils(ushort startAddress, bool[] values)
        {
            if (IsConnected)
            {
                Master.WriteMultipleCoils(startAddress, values);
            }
        }

        public bool ReadInput(ushort address)
        {
            return IsConnected && Master.ReadInputs(address, 1)[0];
        }

        public bool[] ReadInputs(ushort startAddress, ushort count)
        {
            if (IsConnected)
            {
                return Master.ReadInputs(startAddress, count);
            }
            else
            {
                return new bool[count];
            }
        }

       
        public bool ReadCoil(ushort address)
        {
            if (IsConnected)
            {
                return Master.ReadCoils(address, 1)[0];
            }
            else
            {
                return false;
            }
        }

        
        public bool[] ReadCoils(ushort startAddress, ushort count)
        {
            if (IsConnected)
            {
                return Master.ReadCoils(startAddress, count);
            }
            else
            {
                return new bool[count];
            }
        }
        #endregion Modbus команды

    }
}
