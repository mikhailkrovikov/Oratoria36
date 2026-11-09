namespace Oratoria.Persistence.Services
{
    public interface IRecipeService
    {
        public Task<bool> CreateRecipe(Recipe recipe);
        public Task<Recipe?> ReadRecipe(Guid id);
        public Task<List<Recipe>> ReadRecipes(int moduleId);
        public Task<Recipe?> ReadRecipe(string name);
        public Task<bool> Update(Recipe recipe);
        public Task DeleteRecipe(Guid id);

    }
}
