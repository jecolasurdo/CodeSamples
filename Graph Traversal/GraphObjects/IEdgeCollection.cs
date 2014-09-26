using System.Collections.Generic;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public interface IEdgeCollection
    {
        PathCollection FindAllPaths();
        bool IsTerminal(Path pathToCheck);
    }
}
