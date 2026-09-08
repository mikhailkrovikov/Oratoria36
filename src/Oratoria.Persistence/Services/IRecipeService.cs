using Oratoria.Persistence.DTOs;

namespace Oratoria.Persistence.Services
{
    public interface IRecipeService
    {
        public Task<bool> CreateRecipe(RecipeDTO recipe);
        public Task<RecipeDTO> ReadRecipe(Guid id);
        public Task<bool> Update(RecipeDTO recipe);
        public Task DeleteRecipe(Guid id);

    }
}
