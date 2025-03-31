using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Modbus.Device;
using NLog;
using Oratoria36.Service;

namespace Oratoria36.Models
{
    public class ModuleConfig : INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetLogger("ModuleConfig");
        public ModbusIpMaster Master { get; private set; }
        private TcpClient _tcpClient;

        private string _ip;
        private int _port = 502;
        private bool _isConnected;
        private int _moduleId;
        
        public int ModuleId
        {
            get => _moduleId;
            set
            {
                if (_moduleId != value)
                {
                    _moduleId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string IP
        {
            get => _ip;
            set
            {
                if (_ip != value)
                {
                    _logger.Info($"IP изменен с {_ip} на {value}");
                    _ip = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _logger.Info($"Порт изменен с {_port} на {value}");
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
                    _logger.Info($"Состояние подключения модуля {ModuleId} изменено: {(value ? "Подключено" : "Отключено")}");
                    OnPropertyChanged();
                }
            }
        }

        public async Task InitializeModbusAsync(string ip)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(ip, Port);
                Master = ModbusIpMaster.CreateIp(_tcpClient);
                IsConnected = true;
                _logger.Info($"Подключено к {ip}:{Port}");
                ModbusPoller.Instance.OnModuleConnected(this);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                _logger.Error(ex, $"Ошибка подключения к {ip}:{Port}");
            }
        }

        public void CloseConnection()
        {
            try
            {
                ModbusPoller.Instance.OnModuleDisconnected(this);
                Master?.Dispose();
                _tcpClient?.Close();
                _tcpClient?.Dispose();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при закрытии соединения");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
