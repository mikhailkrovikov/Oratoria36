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
        private Logger _logger = LogManager.GetLogger("Net");
        private TcpClient _tcpClient;
        private bool _isConnected;
        private string _ip;
        private string _lastLoggedIp;

        public string IP
        {
            get => _ip;
            set
            {
                if (_ip != value)
                {
                    if (!string.IsNullOrEmpty(value) && _lastLoggedIp != value)
                    {
                        _logger.Info($"IP изменен с {_ip ?? "не задан"} на {value}");
                        _lastLoggedIp = value;
                    }

                    _ip = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _port = 502;
        private int _lastLoggedPort = 0;

        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    if (value != 0 && _lastLoggedPort != value)
                    {
                        _logger.Info($"Порт изменен с {_port} на {value}");
                        _lastLoggedPort = value;
                    }

                    _port = value;
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

        public ModbusIpMaster Master { get; private set; }

        public async Task InitializeModbusAsync(string ip)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(ip, Port);
                Master = ModbusIpMaster.CreateIp(_tcpClient);
                IsConnected = true;
                _logger.Info($"Успешное подключение к IP: {ip}, порт: {Port}");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                _logger.Error(ex, $"Не удалось подключиться к IP: {ip}, порт: {Port}");
            }
        }

        public void CloseConnection()
        {
            if (IsConnected)
            {
                try
                {
                    Master?.Dispose();
                    _tcpClient?.Close();
                    _tcpClient?.Dispose();
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
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
