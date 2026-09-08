using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Persistence.DTOs;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDBContext _dbContext;
        private readonly ILogger<UserService> _logger;

        public RecipeService(RecipeDBContext dbContext, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateRecipe(RecipeDTO recipe)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipe.Name) || recipe.Steps.Count == 0)
                    return false;

                var exists = await _dbContext.Recipes.AnyAsync(r => r.Name == recipe.Name);
                if (exists)
                    return false;

                await _dbContext.Recipes.AddAsync(ToEntity(recipe));
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
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
            else _logger.LogDebug("Не найден рецепт с для удаления");
        }

        public async Task<RecipeDTO?> ReadRecipe(Guid id)
        {
            var entity = await _dbContext.Recipes
                .Include(r => r.Steps)
                .ThenInclude(s => s.Parameters)
                .ThenInclude(p => p.Value)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (entity != null)
                return (RecipeDTO?)ToDto(entity);
            return null;
        }

        public async Task<bool> Update(RecipeDTO recipe)
        {
            try
            {
                if (recipe.Id == null)
                    return false;

                var existing = await _dbContext.Recipes
                    .Include(r => r.Steps)
                    .ThenInclude(s => s.Parameters)
                    .ThenInclude(p => p.Value)
                    .FirstOrDefaultAsync(r => r.RecipeId == recipe.Id);

                if (existing is null)
                    return false;

                existing.Name = recipe.Name;
                _dbContext.RemoveRange(existing.Steps);
                existing.Steps = ToEntity(recipe).Steps;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        private static RecipeEntity ToEntity(RecipeDTO dto) => new()
        {
            Name = dto.Name,
            Steps = dto.Steps.Select(step => new RecipeStepEntity
            {
                Number = step.Number,
                Parameters = step.Parameters.Select(pair => new RecipeParameterEntity
                {
                    Name = pair.Key,
                    Value = new RecipeValueEntity { Value = pair.Value }
                }).ToList()
            }).ToList()
        };

        private static RecipeDTO ToDto(RecipeEntity entity) => new()
        {
            Id = entity.RecipeId,
            Name = entity.Name,
            Steps = entity.Steps
                    .OrderBy(s => s.Number)
                    .Select(step => new RecipeStepDTO
                    {
                        Number = step.Number,
                        Parameters = step.Parameters.ToDictionary(
                            p => p.Name,
                            p => p.Value.Value)
                    })
                    .ToList()
        };
    }
}
