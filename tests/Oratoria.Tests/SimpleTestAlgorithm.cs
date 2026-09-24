using Oratoria.Domain.Algorithms;

namespace Oratoria.Tests
{
    public class SimpleTestAlgorithm : AlgorithmBase
    {
        public Task<AlgorithmResult> Start(
            bool canStart,
            bool operation1, 
            bool operation2, 
            bool operation3, 
            CancellationToken cancelationToken = default)
        {
            return Execute(() => canStart,
                body =>
                {
                    var result = body.DoTask(c => Task.FromResult(operation1));
                    result = body.DoTask(c => Task.FromResult(operation2));
                    result = body.DoTask(c => Task.FromResult(operation3));
                    return result;
                }, cancelationToken);
        }
    }
}
