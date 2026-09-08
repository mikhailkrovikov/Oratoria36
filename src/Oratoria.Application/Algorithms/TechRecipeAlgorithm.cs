using Microsoft.Extensions.Logging;
using Oratoria.Application.Recipe;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class TechRecipeAlgorithm : AlgorithmBase
    {
        private Recipe<TechnologyModuleContext, Stage> _recipe;
        private MagnetronSystemAlgorithm _magnetronSystem;
        public TechRecipeAlgorithm(
            TechnologyModuleContext context,
            Recipe<TechnologyModuleContext, Stage> recipe,
            ILoggerFactory loggerFactory,
            MagnetronSystemAlgorithm magnetronSystem)
            : base(loggerFactory.CreateLogger("Рабочий цикл"))
        {
            _recipe = recipe;
            _magnetronSystem = magnetronSystem;
        }

        public bool CanStartRecipe() => true;
        public bool CanStopRecipe() => true;

        public Task<AlgorithmResult> StartRecipe()
        {
            var context = _recipe.ModuleContext;
            return Execute(CanStartRecipe, body =>
            {
                body.DoTask(() => context.Throttle.Throttling());
                foreach (var stage in _recipe.Stages)
                {      
                    body
                        .DoTask(() => context.Heater.TurnOn(stage.HeatingPower))
                        .DoTask(() => context.RRG.SetValue(stage.Consumption))
                        .DoAlgorithm(_magnetronSystem, () =>
                        {
                            var s1 = stage.Magn1Power;
                            var s2 = stage.Magn2Power;
                            var s3 = stage.Magn3Power;
                            return _magnetronSystem.StartAllMagnetrons(s1, s2, s3);
                        });
                }
                body.DoTask(() => context.Throttle.Open());
                return body;
            });
        }
    }
}
