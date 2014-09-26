using System.Collections.Generic;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public interface IPathCollection : IList<IPath>
    {
        string DescribeAllPaths();
    }
}
