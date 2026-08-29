using System.ComponentModel;

namespace Oratoria.Domain.Connection
{
    public interface IConnectionConfig : INotifyPropertyChanged, IDisposable
    {
        bool IsConnected { get; set; }
        Task<bool> Connect();
        void CloseConnection();
    }
}
