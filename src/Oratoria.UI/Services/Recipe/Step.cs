using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.UI.Services.Recipe
{
    public class Step
    {
        public int Number { get; set; }
        public List<Parameter> Parameters { get; set; } = new();
    }
}
