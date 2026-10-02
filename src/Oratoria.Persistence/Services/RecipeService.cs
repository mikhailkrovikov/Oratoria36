using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Persistence.Entities;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDBContext _dbContext;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(RecipeDBContext dbContext, ILogger<RecipeService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateRecipe(Recipe recipe)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipe.Name))
                    return false;

                var exists = await _dbContext.Recipes.AnyAsync(r => r.Name == recipe.Name);
                if (exists)
                    return false;

                var entity = ToEntity(recipe);
                await _dbContext.Recipes.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                recipe.Id = entity.RecipeId;
                recipe.CreatedAt = entity.CreatedAt;
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                _logger.LogWarning(ex, "Не удалось создать рецепт");
                return false;
            }
        }


        public async Task DeleteRecipe(Guid id)
        {
            var rec = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id);
            if (rec != null)
            {
                _dbContext.Remove(rec);
                await _dbContext.SaveChangesAsync();
            }
            else _logger.LogDebug("Не найден рецепт для удаления");
        }

        public async Task<List<Recipe>> ReadRecipes(int moduleId)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Where(r => r.ModuleId == moduleId)
                .OrderBy(r => r.Name)
                .Select(r => new Recipe
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    ModuleId = r.ModuleId,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<Recipe?> ReadRecipe(string name)
        {
            var id = await _dbContext.Recipes
                .Where(r => r.Name == name)
                .Select(r => (Guid?)r.RecipeId)
                .FirstOrDefaultAsync();

            return id.HasValue ? await ReadRecipe(id.Value) : null;
        }

        public async Task<Recipe?> ReadRecipe(Guid id)
        {
            var entity = await _dbContext.Recipes
                .AsNoTracking()
                .AsSingleQuery()
                .Include(r => r.Steps)
                .ThenInclude(s => s.Parameters)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (entity != null)
                return ToRecipe(entity);
            return null;
        }

        public async Task<bool> Update(Recipe recipe)
        {
            try
            {
                if (recipe.Id == null || string.IsNullOrWhiteSpace(recipe.Name))
                    return false;

                if (await _dbContext.Recipes.AnyAsync(r => r.Name == recipe.Name && r.RecipeId != recipe.Id))
                    return false;

                var existing = await _dbContext.Recipes
                    .AsSingleQuery()
                    .Include(r => r.Steps)
                    .ThenInclude(s => s.Parameters)
                        .FirstOrDefaultAsync(r => r.RecipeId == recipe.Id);

                if (existing is null)
                    return false;

                existing.Name = recipe.Name;
                existing.ModuleId = recipe.ModuleId;
                _dbContext.RemoveRange(existing.Steps);
                existing.Steps = ToEntity(recipe).Steps;
                _dbContext.Steps.AddRange(existing.Steps);
                await _dbContext.SaveChangesAsync();
                recipe.CreatedAt = existing.CreatedAt;
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                _logger.LogWarning(ex, "Не удалось обновить рецепт");
                return false;
            }
        }

        private static RecipeEntity ToEntity(Recipe recipe) => new()
        {
            Name = recipe.Name,
            ModuleId = recipe.ModuleId,
            Steps = recipe.Stages.Select(stage => new RecipeStepEntity
            {
                Number = stage.Number,
                Parameters = ToParameters(stage)
            }).ToList()
        };

        private static List<RecipeParameterEntity> ToParameters(Stage stage)
        {
            var parameters = new List<RecipeParameterEntity>();

            void Add(RecipeParameter parameter, double? value)
            {
                if (value.HasValue)
                    parameters.Add(new RecipeParameterEntity { Parameter = parameter, Value = value.Value });
            }

            Add(RecipeParameter.HeatingTime, stage.HeatingTime);
            Add(RecipeParameter.HeatingPower, stage.HeatingPower);
            Add(RecipeParameter.HeatingTemp, stage.HeatingTemp);
            Add(RecipeParameter.Pressure, stage.Pressure);
            Add(RecipeParameter.Consumption, stage.Consumption);
            Add(RecipeParameter.SputteringTime, stage.SputteringTime);
            Add(RecipeParameter.PreSputteringTime, stage.PreSputteringTime);
            Add(RecipeParameter.Magn1Power, stage.Magn1Power);
            Add(RecipeParameter.Magn2Power, stage.Magn2Power);
            Add(RecipeParameter.Magn3Power, stage.Magn3Power);
            return parameters;
        }

        private static Stage ToStage(RecipeStepEntity step)
        {
            var stage = new Stage { Number = step.Number };
            foreach (var parameter in step.Parameters)
            {
                switch (parameter.Parameter)
                {
                    case RecipeParameter.HeatingTime: stage.HeatingTime = parameter.Value; break;
                    case RecipeParameter.HeatingPower: stage.HeatingPower = parameter.Value; break;
                    case RecipeParameter.HeatingTemp: stage.HeatingTemp = parameter.Value; break;
                    case RecipeParameter.Pressure: stage.Pressure = parameter.Value; break;
                    case RecipeParameter.Consumption: stage.Consumption = parameter.Value; break;
                    case RecipeParameter.SputteringTime: stage.SputteringTime = parameter.Value; break;
                    case RecipeParameter.PreSputteringTime: stage.PreSputteringTime = parameter.Value; break;
                    case RecipeParameter.Magn1Power: stage.Magn1Power = parameter.Value; break;
                    case RecipeParameter.Magn2Power: stage.Magn2Power = parameter.Value; break;
                    case RecipeParameter.Magn3Power: stage.Magn3Power = parameter.Value; break;
                }
            }
            return stage;
        }

        private static Recipe ToRecipe(RecipeEntity entity) => new()
        {
            Id = entity.RecipeId,
            Name = entity.Name,
            ModuleId = entity.ModuleId,
            CreatedAt = DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc),
            Stages = entity.Steps.OrderBy(s => s.Number).Select(ToStage).ToList()
        };
    }
}
