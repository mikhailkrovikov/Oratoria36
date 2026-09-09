namespace Oratoria.Domain.Carrier
{
    public class RouteNode
    {
        public string NodeId { get; }
        public bool IsEmpty { get; set; } = true;

        public RouteOperation Load { get; }
        public RouteOperation Unload { get; }

        public RouteNode(string NodeId, RouteOperation Load, RouteOperation Unload)
        {
            this.NodeId = NodeId;
            this.Load = Load;
            this.Unload = Unload;
        }
    }
}