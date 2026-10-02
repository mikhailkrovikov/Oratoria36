using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Persistence.DTOs;
using Oratoria.Persistence.Entities;

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

        public async Task<bool> CreateRecipe(RecipeDTO recipe)
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

        public async Task<List<RecipeDTO>> ReadRecipes(int moduleId)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Where(r => r.ModuleId == moduleId)
                .OrderBy(r => r.Name)
                .Select(r => new RecipeDTO
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    ModuleId = r.ModuleId,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<RecipeDTO?> ReadRecipe(string name)
        {
            var id = await _dbContext.Recipes
                .Where(r => r.Name == name)
                .Select(r => (Guid?)r.RecipeId)
                .FirstOrDefaultAsync();

            return id.HasValue ? await ReadRecipe(id.Value) : null;
        }

        public async Task<RecipeDTO?> ReadRecipe(Guid id)
        {
            var entity = await _dbContext.Recipes
                .AsNoTracking()
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
                if (recipe.Id == null || string.IsNullOrWhiteSpace(recipe.Name))
                    return false;

                if (await _dbContext.Recipes.AnyAsync(r => r.Name == recipe.Name && r.RecipeId != recipe.Id))
                    return false;

                var existing = await _dbContext.Recipes
                    .Include(r => r.Steps)
                    .ThenInclude(s => s.Parameters)
                    .ThenInclude(p => p.Value)
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

        private static RecipeEntity ToEntity(RecipeDTO dto) => new()
        {
            Name = dto.Name,
            ModuleId = dto.ModuleId,
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
            ModuleId = entity.ModuleId,
            CreatedAt = DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc),
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
