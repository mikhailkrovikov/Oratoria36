namespace DigitalTwin
{
    public interface IRegister
    {
        event Action<ushort, bool> BoolInputChanged;
        event Action<ushort, double> DoubleInputChanged;

        void RegisterDevice<T>(ushort outputPin, ushort inputPin, int timeout);

        void RegisterDevice<T>(ushort outputPin, ushort inputPin1, ushort inputPin2, int timeout);

        void RegisterMechanicDevice<T>(ushort[] outputs, ushort[] inputs, int timeout);

        void SetOutput<T>(ushort pinNumber, T value);
        
        bool GetInputBool(ushort pinNumber);

        double GetInputDouble(ushort pinNumber);
    }
}
