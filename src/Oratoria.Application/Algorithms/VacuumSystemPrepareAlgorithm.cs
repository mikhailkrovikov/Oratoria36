using Oratoria.Application.TransportModule;
using Oratoria.Application.VacuumModule;
using Oratoria.Domain.Algorithms;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Algorithms
{
    public class VacuumSystemPrepareAlgorithm : AlgorithmBase
    {
        private readonly VacuumContext vacuumContext;

        public VacuumSystemPrepareAlgorithm(VacuumContext vacuumContext)
        {
            this.vacuumContext = vacuumContext;
        }

        // TODO:
        // Работают криогенники, все модули напущены до атмосферы
        public bool CanStart() => true;

        public Task<AlgorithmResult> Start()
        {
            return Execute(CanStart,
                body => body
                .DoTask(c => vacuumContext.FK_Trb.OpenValve(c))

                // Откачка шлюзов до 1000 Па
                .DoTask(c => vacuumContext.AVR.TurnOn(c))
                .DoTask(c => vacuumContext.FK_OK.OpenValve(c))
                .DoTask(c => vacuumContext.FK_Shl1.OpenValve(c))
                .DoTask(c => vacuumContext.FK_Shl2.OpenValve(c))
                .DoTask(async c =>
                {
                   var tasks = new List<Task<bool>>
                   {
                       WaitForPressure(vacuumContext.Gateway1LowVacuum, 1000, c),
                       WaitForPressure(vacuumContext.Gateway2LowVacuum, 1000, c)
                   };
                   return (await Task.WhenAll(tasks)).Any();
                })
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))

                // Откачка шлюзов до 3 Па
                .DoTask(c => vacuumContext.FK_AVR.OpenValve(c))
                .DoTask(c => vacuumContext.FK_AP.OpenValve())
                .DoTask(c => vacuumContext.AP1.TurnOn(c))
                .DoTask(async c =>
                {
                    var tasks = new List<Task<bool>>
                    {
                        WaitForPressure(vacuumContext.Gateway1LowVacuum, 3, c),
                        WaitForPressure(vacuumContext.Gateway2LowVacuum, 3, c)
                    };
                    return (await Task.WhenAll(tasks)).Any();
                })
                .DoTask(c => vacuumContext.FK_AVR.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AP.CloseValve(c))
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))

                // TODO: по какому датчику и до скольки качать?
                // Откачка шлюзов через КН2      
                .DoTask(c => vacuumContext.KN2_Zatvor.OpenValve(c))
                .DoTask(c => WaitForPressure(vacuumContext.KNGatewayHighVacuum, 0.00001, c))
                .DoTask(c => vacuumContext.KN2_Zatvor.CloseValve(c))

                // Откачка транспорта до 1000 па
                .DoTask(c => vacuumContext.FK_OK.OpenValve(c))
                .DoTask(c => vacuumContext.FK_TM.OpenValve(c))
                .DoTask(c => WaitForPressure(vacuumContext.TransportLowVacuum, 1000, c))
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))

                // Откачка транспорта до 3 па
                .DoTask(c => vacuumContext.FK_AVR.OpenValve(c))
                .DoTask(c => vacuumContext.FK_AP.OpenValve(c))
                .DoTask(c => WaitForPressure(vacuumContext.TransportLowVacuum, 3, c))

                // Откачка транспорта до ВВ
                .DoTask(c => vacuumContext.FK_AVR.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AP.CloseValve(c))
                .DoTask(c => vacuumContext.FK_TM.CloseValve(c))
                .DoTask(c => vacuumContext.KN_Zatvor_TM.OpenValve(c))
                .DoTask(c => WaitForPressure(vacuumContext.TransportHighVacuum, 0.00001, c))
                );
        }

        private static async Task<bool> WaitForPressure(PressureSensor sensor, double targetPressure, CancellationToken token)
        {
            return await EventWaiter.WaitEvent(
                nameof(sensor.PressureSignal.OnSignalChanged),
                sensor.PressureSignal,
                (double d) => sensor.CurrentPressure <= targetPressure,
                -1,
                token);
        }
    }
}
