namespace Oratoria36.Models.Signals
{
    public class OutputSignal : Signal<bool>
    {
        public OutputSignal(string name, ushort channel, ModuleConfig module, string description = "")
            : base(name, channel, module, description) { }


        public override async Task<bool> ReadValueAsync(ModuleConfig module)
        {
            if (module == null || !module.IsConnected)
            {
                IsValid = false;
                OnPropertyChanged(nameof(IsValid));
                return false;
            }

            try
            {
                ushort address = 0;
                byte bitOffset = (byte)(Channel % 8);
                ushort byteOffset = (ushort)(Channel / 8);

                bool[] outputs = await Task.Run(() =>
                    module.Master.ReadCoils(1, (ushort)(address + byteOffset), 8));

                if (outputs != null && outputs.Length > bitOffset)
                {
                    if (Value != outputs[bitOffset])
                    {
                        Value = outputs[bitOffset];
                    }

                    IsValid = true;
                    OnPropertyChanged(nameof(IsValid));
                    return true;
                }
                else
                {
                    IsValid = false;
                    OnPropertyChanged(nameof(IsValid));
                    return false;
                }
            }
            catch
            {
                IsValid = false;
                OnPropertyChanged(nameof(IsValid));
                return false;
            }
        }

        public override async Task<bool> WriteValueAsync(ModuleConfig module)
        {
            if (module == null || !module.IsConnected)
            {
                return false;
            }

            try
            {
                ushort address = 0;
                byte bitOffset = (byte)(Channel % 8);
                ushort byteOffset = (ushort)(Channel / 8);

                await Task.Run(() =>
                    module.Master.WriteSingleCoil((ushort)(address + byteOffset), Value));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
