using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oratoria.Domain
{
    public enum PlateErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("пластина нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пластина не взята из каретки")]
        NotTakenFromTransport,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пластина не взята из ложемента")]
        NotTakenFromModule,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пластина не поставлена в ложемент")]
        NotPlacedInModule,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пластина в манипуляторе")]
        NotInManipulator,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пластина не поставлена в каретку")]
        NotPlacedInTransport
    }

    public enum PlateStatus
    {
        [Description("Не инициализована")]
        None,

        [Description("В ложементе")]
        Module,

        [Description("В транспорте")]
        Transport,

        [Description("В манипуляторе")]
        Manipulator,
    }

    public class Plate : Device<PlateStatus, PlateErrors>
    {
        public InputSignal<bool> Position1In { get; }
        public OutputSignal<bool> PlatePrivod3 { get; }

        public void SetState(PlateStatus value)
        {
            if (_state.Equals(value)) return;
            _state = value;
            OnStateChanged();
        }

        public Plate(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
        }

        private PlateStatus _state;
        public override PlateStatus State => _state;

        public Task<bool> GetState(
            bool expected,
            CancellationToken cancellationToken = default)
        {
            return RunOperation(cancellationToken, async token =>
            {
                PlatePrivod3.Value = true;

                try
                {
                    bool res = await EventWaiter.WaitEvent(nameof(Position1In.OnSignalChanged),
                        Position1In,
                        (bool x) => Position1In.Value == expected,
                        2000, token);
                    if (expected != res)
                        Logger.LogError(DeviceErrors.Last().GetDescription());
                    return res;
                }
                finally
                {
                    PlatePrivod3.Value = false;
                }
            });
        }
    }
}
