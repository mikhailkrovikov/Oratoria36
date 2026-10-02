using Oratoria.Domain.Recipes;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class TechRecipeAlgorithm : AlgorithmBase
    {
        private readonly Recipe _recipe;
        private readonly TechnologyModuleContext _context;
        private MagnetronSystemAlgorithm _magnetronSystem;
        public TechRecipeAlgorithm(
            Recipe recipe,
            TechnologyModuleContext context,
            MagnetronSystemAlgorithm magnetronSystem)
        {
            _recipe = recipe;
            _context = context;
            _magnetronSystem = magnetronSystem;
        }

        public bool CanStartRecipe()
        {
            return _recipe.Stages.All(stage =>
                IsValid(stage.HeatingTime) &&
                IsValid(stage.SputteringTime) &&
                IsValid(stage.PreSputteringTime) &&
                IsValid(stage.Pressure) &&
                IsValid(stage.HeatingTemp, 400) &&
                IsValid(stage.HeatingPower, 4000) &&
                IsValid(stage.Magn1Power, 4000) &&
                IsValid(stage.Magn2Power, 4000) &&
                IsValid(stage.Magn3Power, 4000) &&
                IsValid(stage.Consumption, _context.RRG.MaxFlowRate.Value));
        }


        private static bool IsValid(double? value, double max = double.MaxValue)
        {
            return !value.HasValue || (double.IsFinite(value.Value) && value >= 0 && value <= max);
        }

        public Task<AlgorithmResult> StartRecipe(CancellationToken cancellationToken = default)
        {
            var context = _context;
            var stages = _recipe.Stages.OrderBy(stage => stage.Number).Select(stage => stage.Copy()).ToList();
            return Execute(CanStartRecipe, body =>
            {
                body.DoTask(context.Throttle.Throttling);
                foreach (var stage in stages)
                {
                    body
                        .DoTask(ct => context.Heater.TurnOn((stage.HeatingPower ?? 0), ct))
                        .DoTask(ct => context.RRG.SetValue((stage.Consumption ?? 0), ct))
                        .DoAlgorithm(ct =>
                        {
                            var s1 = stage.Magn1Power ?? 0;
                            var s2 = stage.Magn2Power ?? 0;
                            var s3 = stage.Magn3Power ?? 0;
                            return _magnetronSystem.StartAllMagnetrons(s1, s2, s3, ct);
                        });
                }
                body.DoTask(context.Throttle.Open);
                return body;
            },
            cancellationToken);
        }
    }
}
