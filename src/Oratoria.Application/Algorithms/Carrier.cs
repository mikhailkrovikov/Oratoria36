using Oratoria.Domain.Carrier;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oratoria.Application.Algorithms
{
    public class Carrier
    {
        public RouteNode Gateway1 { get; }
        public RouteNode Gateway2 { get; }
        public RouteNode Transport { get; }
        public RouteNode Module1 { get; }
        public RouteNode Module2 { get; }
        public RouteNode Module3 { get; }
        public RouteNode Module4 { get; }
        public RouteGraph Graph { get; }
    }
}
