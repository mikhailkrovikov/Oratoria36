using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Modbus.Device;
using NLog;
using Oratoria36.Service;

namespace Oratoria36.Models.Connection
{
    public class NetConfig : INotifyPropertyChanged
    {
        private Logger _logger = LogManager.GetLogger("ModuleConfig");
        public ModbusIpMaster Master { get; set; }
        public TcpClient _tcpClient;

        private string _ip;
        private int _port = 502;
        private bool _isConnected;

        private object _locker;

        public string IP
        {
            get => _ip;
            set
            {
                if (_ip != value && _ip != "")
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
                    OnPropertyChanged();
                }
            }
        }

        public NetConfig(object locker)
        {
            _locker = locker;
        }

        public async Task Connect(string ip)
        {
            try
            {
                _tcpClient = new TcpClient();
                var ret = Task.WaitAll(
                    new[]
                    {
                        _tcpClient.ConnectAsync(ip, Port)
                    },
                    TimeSpan.FromSeconds(5)
                    );
                Master = ModbusIpMaster.CreateIp(_tcpClient);
                IsConnected = true;
                _logger.Info($"Подключено к {ip}:{Port}");
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
                lock (_locker)
                {
                    Master?.Dispose();
                    Master = null;
                }
                _tcpClient?.Close();
                _tcpClient?.Dispose();
                IsConnected = false;
            }
            catch
            {

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
