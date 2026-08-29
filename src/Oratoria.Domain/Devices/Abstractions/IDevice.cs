namespace Oratoria.Domain.Devices.Abstractions
{
    public interface IDevice<TStatus, TError>
        where TStatus : Enum
        where TError : Enum
    {    
        delegate void StateChangeHandler();
        delegate void ErrorStateChangedHandler();
        event StateChangeHandler? StateChanged;

        Enum DeviceId { get; set; }

        DeviceError<TError> DeviceErrors { get; set; }

        TStatus? State { get; }
    }
}
