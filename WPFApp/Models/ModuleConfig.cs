using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Modbus.Device;
using NLog;
using Oratoria36.Models.Signals;


namespace Oratoria36.Models
{
    public partial class ModuleConfig : INotifyPropertyChanged
    {
        private Logger _logger = LogManager.GetLogger("Net");
        private TcpClient _tcpClient;
        private bool _isConnected;
        private string _ip;

        private Poller _poller;


        private readonly List<Signal> _signals = new List<Signal>();

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

        private int _port = 502;
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
                    _logger.Info($"Состояние сети изменено: {(value ? "Подключено" : "Отключено")}");
                    OnPropertyChanged(nameof(IsConnected));

                    if (value)
                        StartPolling();
                    else
                        StopPolling();
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


        internal void RegisterSignal(Signal signal)
        {
            if (signal != null && !_signals.Contains(signal))
            {
                _signals.Add(signal);

                _poller?.RegisterSignal(signal);
            }
        }

        private void StartPolling()
        {
            if (_poller == null)
            {
                _poller = new Poller(this);

                foreach (var signal in _signals)
                {
                    _poller.RegisterSignal(signal);
                }
            }
            else
            {
                _poller.Start();
            }
        }

        private void StopPolling()
        {
            _poller?.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
