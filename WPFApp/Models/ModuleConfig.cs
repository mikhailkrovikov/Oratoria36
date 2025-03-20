using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Modbus.Device;
using NLog;

namespace Oratoria36.Models
{
    public class ModuleConfig : INotifyPropertyChanged
    {
        Logger _logger = LogManager.GetLogger("Net");
        TcpClient? TcpClient;
        public ModbusIpMaster? Master { get; private set; }

        bool _isConnected;
        string? _ip;

        public string IP
        {
            get => _ip;
            private set
            {
                if (_ip != value)
                {
                    _logger.Info($"IP изменен с {_ip} на {value}");
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
                    _logger.Info($"Состояние сети изменено: {(value ? "Подключено" : "Отключено")}");
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        public async void SetIP(string newIp)
        {
            if (IP != newIp)
            {
                CloseConnection();
                IP = newIp;
                await InitializeModbusAsync(newIp);
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
                _logger.Info($"Успешное подключение к IP: {ip}");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                _logger.Error(ex, $"Не удалось подключиться к IP: {ip}");
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
                    TcpClient?.Dispose();
                    IsConnected = false;
                    _logger.Info("Соединение закрыто");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Соединение не удалось закрыть");
                }
            }
            else
            {
                _logger.Info("Нет активного соединения для закрытия");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}