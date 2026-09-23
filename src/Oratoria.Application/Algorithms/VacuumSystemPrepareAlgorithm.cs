using Microsoft.Extensions.Logging;
using Oratoria.Application.TransportModule;
using Oratoria.Application.VacuumModule;
using Oratoria.Domain.Algorithms;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Algorithms
{
    public class VacuumSystemPrepareAlgorithm : AlgorithmBase
    {
        private readonly VacuumContext vacuumContext;
        private ILogger logger;

        public VacuumSystemPrepareAlgorithm(VacuumContext vacuumContext, ILoggerFactory loggerFactory)
        {
            this.vacuumContext = vacuumContext;
            logger = loggerFactory.CreateLogger("Подготовка вакуумной системы");
        }

        // TODO:
        // Работают криогенники, все модули напущены до атмосферы
        public bool CanStart()
        {
            return vacuumContext.KN1_TM.State == PumpStatus.On &&
                vacuumContext.KN2_Shl.State == PumpStatus.On;
        }

        public Task<AlgorithmResult> Start(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Запуск подготовки вакуумной системы");
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
                    logger.LogInformation("Ожидание давления в шлюзах ниже 1000 Па");
                    var tasks = new List<Task<bool>>
                    {
                        WaitForPressure(vacuumContext.Gateway1LowVacuum, 1000, c),
                        WaitForPressure(vacuumContext.Gateway2LowVacuum, 1000, c)
                    };
                    return (await Task.WhenAll(tasks)).All(result => result);
                })
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))

                // Откачка шлюзов до 3 Па
                .DoTask(c => vacuumContext.FK_AVR.OpenValve(c))
                .DoTask(c => vacuumContext.FK_AP.OpenValve(c))
                .DoTask(c => vacuumContext.AP1.TurnOn(c))
                .DoTask(async c =>
                {
                    logger.LogInformation("Ожидание давления в шлюзах ниже 3 Па");
                    var tasks = new List<Task<bool>>
                    {
                        WaitForPressure(vacuumContext.Gateway1LowVacuum, 3, c),
                        WaitForPressure(vacuumContext.Gateway2LowVacuum, 3, c)
                    };
                    return (await Task.WhenAll(tasks)).All(result => result);
                })
                .DoTask(c => vacuumContext.FK_AVR.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AP.CloseValve(c))
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))
                .DoTask(c => vacuumContext.FK_Trb.CloseValve(c))
                // TODO: по какому датчику и до скольки качать?
                // Откачка шлюзов через КН2      
                .DoTask(c => vacuumContext.KN2_Zatvor.OpenValve(c))
                .DoTask(async c =>
                {
                    logger.LogInformation("Ожидание высокого вакуума в шлюзах");
                    return await WaitForPressure(vacuumContext.KNGatewayHighVacuum, 1, c);
                })
                .DoTask(c => vacuumContext.KN2_Zatvor.CloseValve(c))

                // Откачка транспорта до 1000 па
                .DoTask(c => vacuumContext.FK_OK.OpenValve(c))
                .DoTask(c => vacuumContext.FK_TM.OpenValve(c))
                .DoTask(async c =>
                {
                    logger.LogInformation("Ожидание давления в транспортном модуле ниже 1000 Па вакуума в шлюзах");
                    return await WaitForPressure(vacuumContext.TransportLowVacuum, 1000, c);
                })
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))

                // Откачка транспорта до 3 па
                .DoTask(c => vacuumContext.FK_AVR.OpenValve(c))
                .DoTask(c => vacuumContext.FK_AP.OpenValve(c))
                .DoTask(async c =>
                {
                    logger.LogInformation("Ожидание давления в транспортном модуле ниже 3 Па вакуума в шлюзах");
                    return await WaitForPressure(vacuumContext.TransportLowVacuum, 3, c);
                })

                // Откачка транспорта до ВВ
                .DoTask(c => vacuumContext.FK_AVR.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AP.CloseValve(c))
                .DoTask(c => vacuumContext.FK_TM.CloseValve(c))
                .DoTask(c => vacuumContext.KN_Zatvor_TM.OpenValve(c))
                .DoTask(async c =>
                {
                    logger.LogInformation("Ожидание высокого вакуума в транспортном модуле");
                    return await WaitForPressure(vacuumContext.TransportHighVacuum, 1, c);
                })
                .DoAction(() => logger.LogInformation("Подготовка вакуумной системы завершена")),

                cancellationToken);
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
