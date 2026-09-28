using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.UI.Services.Recipe
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<Step> Steps { get; set; } = new();
    }
}
