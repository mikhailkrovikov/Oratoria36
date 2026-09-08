using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RecipeValueConfiguration))]
    public class RecipeValueEntity
    {
        public Guid ValueId { get; set; } = Guid.NewGuid();
        public Guid ParameterId { get; set; }
        public double Value { get; set; }
    }
}
