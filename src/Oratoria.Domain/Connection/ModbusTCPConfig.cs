using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Modbus.Device;

namespace Oratoria.Domain.Connection
{
    public class ModbusTCPConfig : IConnectionConfig
    {
        private readonly ILogger _logger;
        private TcpClient? _tcpClient;
        private readonly object _locker;

        public ModbusIpMaster? Master { get; set; }

        public string? IP
        {
            get => field;
            set
            {
                if (field != value)
                {
                    _logger.LogInformation($"Адрес изменен с {field} на {value}");
                    field = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Port
        {
            get => field;
            set
            {
                if (field != value)
                {
                    _logger.LogInformation($"Порт изменен с {field} на {value}");
                    field = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsConnected
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        }

        public ModbusTCPConfig(object locker, ILogger logger)
        {
            _locker = locker;
            _logger = logger;
        }

        public async Task<bool> Connect()
        {
            try
            {
                if(IP == null)
                {
                    _logger.LogWarning("IP не задан, сбой подключения");
                    return false;
                }

                _logger.LogInformation($"Подключение к {IP}:{Port}");
                _tcpClient = new TcpClient();

                await _tcpClient
                    .ConnectAsync(IP, Port)
                    .WaitAsync(TimeSpan.FromSeconds(3));

                Master = ModbusIpMaster.CreateIp(_tcpClient);
                IsConnected = true;
                _logger.LogInformation($"Подключено к {IP}:{Port}");

                return true;
            }
            catch (TimeoutException)
            {
                IsConnected = false;
                _logger.LogError($"Не удалось подключиться к {IP}:{Port}");
                return false;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                _logger.LogError($"{ex}");
                return false;
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
