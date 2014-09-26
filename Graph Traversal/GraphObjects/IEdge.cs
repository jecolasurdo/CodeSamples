namespace SomeCompany.Pathfinder.GraphObjects
{
    public interface IEdge
    {
        int Id { get; }
        Node FromNode { get; }
        Node ToNode { get; }
        string EdgeName { get; }
    }
}

