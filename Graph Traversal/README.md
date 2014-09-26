Graph Traversal
===============

The code in this area was written as a part of a MS Visio Add-in that I created called "Pathfinder".

Pathfinder was created to assist team members and myself quickly and reliably determine all pathways through generally complex business decision trees.

Decision trees would be created in MS Visio as a flow chart. The add-in then 
  - analyzed the chart
  - translated it into a digraph (this object here: https://github.com/jecolasurdo/CodeSamples/blob/master/Graph%20Traversal/GraphObjects/ImmutableGraph.cs), 
  - and then determined all pathways through the graph (including loops, which were each permitted to be traversed once per unique outcome).

This process all hinges on an algorythm for traversing the graph, which can be viewed here: https://github.com/jecolasurdo/CodeSamples/blob/master/Graph%20Traversal/GraphObjects/EdgeCollection.cs#L18

Initially, I looked into using someone else's graph library to handle this task, but I couldn't find any libraries that were super lightweight and simple. The requirements of the project weren't such that I needed a super heavy weight graph library, so I concluded that writing it myself would be sufficient (and it has been, so far).

Some features/notes:
 - Non-Recursive Implementation: Since C# doesn't support tail call optimization I try to avoid recursive algorithms in this language. If I had it to do again, I would probably write this using recursion in F# rather than in C#.
 - Unit tests can be found here: https://github.com/jecolasurdo/CodeSamples/blob/master/Graph%20Traversal/UseCases.cs
 - Immutibility: One of the primary graph objects is immutible. I did this to help with some threading I was working on earlier in the project. However, I think this I poorly named this object as its members are not necessesarily guaranteed to satisfy the mutation requirement. This isn't a big deal, but next time I work on this project, I will clean that up.
