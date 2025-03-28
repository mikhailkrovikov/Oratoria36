namespace Oratoria36.Models.Signals
{
    public class InputSignal : Signal<bool>
    {
        public InputSignal(string name, ushort channel, ModuleConfig module, string description = "")
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

                bool[] inputs = await Task.Run(() =>
                    module.Master.ReadInputs(1, (ushort)(address + byteOffset), 8));

                if (inputs != null && inputs.Length > bitOffset)
                {
                    if (Value != inputs[bitOffset])
                    {
                        Value = inputs[bitOffset];
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
            return await Task.FromResult(false);
        }
    }
}
