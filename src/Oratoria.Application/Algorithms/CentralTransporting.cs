using Oratoria.Application.TransportModule;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class CentralTransporting : AlgorithmBase
    {
        private readonly TransportContext _context;

        public CentralTransporting(TransportContext context)
        {
            _context = context;
        }

        public bool CanLoadPlate() => true;
        public bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate(Plate plate, int targetModule)
        {
            return Execute(CanLoadPlate, 
                body => body
                .DoTask((cts) => _context.Carriage.MoveCarriage(targetModule, cts)));
        }

        public Task<AlgorithmResult> UnloadPlate(Plate plate, int targetModule)
        {
            return Execute(CanLoadPlate,
                body => body
                .DoTask((cts) => _context.Carriage.MoveCarriage(targetModule, cts)));
        }
    }
}
