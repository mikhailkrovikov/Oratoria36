using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Oratoria.Persistence;
using Oratoria.Persistence.DTOs;
using Oratoria.Persistence.Entities;
using Oratoria.Persistence.Services;

namespace Oratoria.Tests
{
    public class RecipeServiceTests
    {
        private SqliteConnection _connection = null!;
        private RecipeDBContext _db = null!;
        private RecipeService _service = null!;

        [SetUp]
        public async Task SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            await _connection.OpenAsync();
            var options = new DbContextOptionsBuilder<RecipeDBContext>()
                .UseSqlite(_connection).Options;
            _db = new RecipeDBContext(options);
            await _db.Database.MigrateAsync();
            _service = new RecipeService(_db, NullLogger<RecipeService>.Instance);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _db.DisposeAsync();
            await _connection.DisposeAsync();
        }

        [Test]
        public async Task Recipe_can_be_created_loaded_updated_and_deleted()
        {
            var recipe = new RecipeDTO
            {
                Name = "Напыление",
                ModuleId = 2,
                Steps = new List<RecipeStepDTO>
                {
                    new()
                    {
                        Number = 1,
                        Parameters = new Dictionary<string, double> { { "Время нагрева, сек", 12.5 } }
                    }
                }
            };

            Assert.That(await _service.CreateRecipe(recipe), Is.True);
            var createdAt = recipe.CreatedAt;
            var loaded = await _service.ReadRecipe(recipe.Id!.Value);
            Assert.That(loaded!.Steps[0].Parameters["Время нагрева, сек"], Is.EqualTo(12.5));
            Assert.That((await _service.ReadRecipes(2)).Single().CreatedAt, Is.EqualTo(createdAt));
            Assert.That(await _service.ReadRecipes(3), Is.Empty);

            loaded.Steps[0].Parameters["Время нагрева, сек"] = 30;
            Assert.That(await _service.Update(loaded), Is.True);
            loaded = await _service.ReadRecipe(recipe.Name);
            Assert.That(loaded!.Steps[0].Parameters["Время нагрева, сек"], Is.EqualTo(30));
            Assert.That(loaded.CreatedAt, Is.EqualTo(createdAt));
            Assert.That(await _db.Values.CountAsync(), Is.EqualTo(1));

            // Повторная перезапись очищенным рецептом.
            loaded.Steps.Clear();
            Assert.That(await _service.Update(loaded), Is.True);
            Assert.That((await _service.ReadRecipe(recipe.Id.Value))!.Steps, Is.Empty);
            Assert.That(await _db.Values.CountAsync(), Is.Zero);

            await _service.DeleteRecipe(recipe.Id.Value);
            Assert.That(await _service.ReadRecipes(2), Is.Empty);
            Assert.That(await _db.Parameters.CountAsync(), Is.Zero);
        }

        [Test]
        public async Task Empty_recipe_is_allowed_but_duplicate_names_are_not()
        {
            var recipe = new RecipeDTO { Name = "Рецепт", ModuleId = 2 };
            Assert.That(await _service.CreateRecipe(recipe), Is.True);
            Assert.That(await _service.CreateRecipe(new RecipeDTO { Name = "Рецепт", ModuleId = 3 }), Is.False);

            var other = new RecipeDTO { Name = "Другой рецепт", ModuleId = 2 };
            Assert.That(await _service.CreateRecipe(other), Is.True);
            other.Name = recipe.Name;
            Assert.That(await _service.Update(other), Is.False);
            Assert.That((await _service.ReadRecipes(2)).Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Migration_enforces_unique_names_in_database()
        {
            _db.Recipes.Add(new RecipeEntity { Name = "Рецепт", ModuleId = 2 });
            await _db.SaveChangesAsync();
            _db.Recipes.Add(new RecipeEntity { Name = "Рецепт", ModuleId = 3 });
            Assert.ThrowsAsync<DbUpdateException>(async () => await _db.SaveChangesAsync());
        }
    }
}
