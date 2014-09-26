using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public interface IPath : IEnumerable
    {
        Path Clone();
        bool ContainsDuplicateCycles();
        int ContainsInstancesOfSubset(Path subsetPath);
        PathCollection FindCycles();
        INode FinalNode { get; }
        INode FirstNode { get; }
        string EdgeListString { get; }
        string NodeListString { get; }
        string PathDescription { get; }
    }
}
