using Microsoft.Extensions.Logging;
using Oratoria.Application.VacuumModule;
using Oratoria.Domain.Algorithms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.Application.Algorithms
{
    public class ToIdleAlgoritm : AlgorithmBase
    {
        private readonly VacuumContext vacuumContext;
        private ILogger logger;

        public ToIdleAlgoritm(VacuumContext vacuumContext, ILoggerFactory loggerFactory)
        {
            this.vacuumContext = vacuumContext;
            logger = loggerFactory.CreateLogger("В ожидание");
        }

        public bool CanToIdle() => true;

        public Task<AlgorithmResult> ToIdle(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Переход в ожидание");
            return Execute(CanToIdle,
                body => body

                .DoAction(() => logger.LogInformation("Переход в ожидание завершен"))
                .DoTask(c => vacuumContext.FK_OK.CloseValve(c))
                .DoTask(c => vacuumContext.FK_Shl1.CloseValve(c))
                .DoTask(c => vacuumContext.FK_Shl2.CloseValve(c))
                .DoTask(c => vacuumContext.FK_Trb.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AVR.CloseValve(c))
                .DoTask(c => vacuumContext.FK_AP.CloseValve(c))
                .DoTask(c => vacuumContext.FK_TM.CloseValve(c))
                .DoTask(c => vacuumContext.AP1.TurnOff(c))
                .DoTask(c => vacuumContext.AVR.TurnOff(c)),
                
                cancellationToken);

        }
    }
}
