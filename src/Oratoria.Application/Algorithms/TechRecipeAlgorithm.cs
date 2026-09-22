using Oratoria.Application.Recipe;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class TechRecipeAlgorithm : AlgorithmBase
    {
        private Recipe<TechnologyModuleContext, Stage> _recipe;
        private MagnetronSystemAlgorithm _magnetronSystem;
        public TechRecipeAlgorithm(
            Recipe<TechnologyModuleContext, Stage> recipe,
            MagnetronSystemAlgorithm magnetronSystem)
        {
            _recipe = recipe;
            _magnetronSystem = magnetronSystem;
        }

        public bool CanStartRecipe() => true;
        public Task<AlgorithmResult> StartRecipe(CancellationToken cancellationToken = default)
        {
            var context = _recipe.ModuleContext;
            return Execute(CanStartRecipe, body =>
            {
                body.DoTask(context.Throttle.Throttling);
                foreach (var stage in _recipe.Stages)
                {
                    body
                        .DoTask(ct => context.Heater.TurnOn(stage.HeatingPower, ct))
                        .DoTask(ct => context.RRG.SetValue(stage.Consumption, ct))
                        .DoAlgorithm(ct =>
                        {
                            var s1 = stage.Magn1Power;
                            var s2 = stage.Magn2Power;
                            var s3 = stage.Magn3Power;
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
