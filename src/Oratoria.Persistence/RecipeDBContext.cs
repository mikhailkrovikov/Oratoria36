using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence
{
    public class RecipeDBContext : DbContext
    {
        public DbSet<RecipeValueEntity> Values { get; set; }

        public DbSet<RecipeParameterEntity> Parameters { get; set; }

        public DbSet<RecipeStepEntity> Steps { get; set; }

        public DbSet<RecipeEntity> Recipes { get; set; }

        public RecipeDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipies.db");
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
