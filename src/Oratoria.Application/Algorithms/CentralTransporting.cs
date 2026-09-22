using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Application.TransportModule;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.Application.Algorithms
{
    public class CentralTransporting : AlgorithmBase
    {
        private readonly TransportContext _context;


        public CentralTransporting(TransportContext context, ILoggerFactory loggerFactory) 
            : base(loggerFactory.CreateLogger("Транспортировка"))
        {
            _context = context;
        }

        public bool CanLoadPlate() => true;
        public bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate(Plate plate)
        {
            // Implementation for loading plate
            throw new NotImplementedException();
        }

        public Task<AlgorithmResult> UnloadPlate(Plate plate)
        {
            // Implementation for unloading plate
            throw new NotImplementedException();
        }
    }
}
