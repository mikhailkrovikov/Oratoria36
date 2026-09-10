using Microsoft.Extensions.Logging;
using Oratoria.Application.Gateway1;
using Oratoria.Application.TransportModule;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.Application.Algorithms
{
    public class GatewayTransporting : AlgorithmBase
    {
        private readonly GatewayContext _context;
        public Plate Plate;

        public GatewayTransporting(GatewayContext context, Plate plate, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger("Транспортировка"))
        {
            _context = context;
            Plate = plate;
        }

        public bool CanLoadPlate() => true;
        public bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate()
        {
            // Implementation for loading plate
            throw new NotImplementedException();
        }

        public Task<AlgorithmResult> UnloadPlate()
        {
            // Implementation for unloading plate
            throw new NotImplementedException();
        }
    }
}
