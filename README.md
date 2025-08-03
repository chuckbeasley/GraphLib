# GraphLib

GraphLib is a C# library for creating, manipulating, and analyzing directed graphs. It provides a flexible API for working with vertices and edges, and includes algorithms for traversal, sorting, shortest paths, spanning trees, and centrality calculations.

## Features

- Add and remove vertices (by index or Guid) and edges (by index, Guid, or Vertex)
- Support for custom vertex data
- Adjacency matrix representation
- Topological sort
- Depth-first search (DFS)
- Breadth-first search (BFS)
- Minimum spanning tree traversal
- Shortest path calculation (Dijkstra-like, by index or Guid)
- Centrality metrics: degree, degree centrality, closeness, betweenness, eigenvector
- Density calculation
- Utility methods: clear graph, check connectivity, show vertex label, check if graph is empty

## Getting Started

### Installation

Add GraphLib as a project reference in your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\GraphLib\GraphLib.csproj" />
</ItemGroup>
```

### Usage Example

```csharp
using GraphLib;

Graph graph = new();
Guid v1 = graph.AddVertex("A", new { Info = "Vertex A" });
Guid v2 = graph.AddVertex("B", new { Info = "Vertex B" });
graph.AddEdge(graph.VerticesList[0], graph.VerticesList[1], 5);

graph.TopologicalSort();
graph.DepthFirstSearch();
graph.BreadthFirstSearch();
graph.Path(v1); // Shortest paths from v1
float degree = graph.CalculateDegree(graph.VerticesList[0]);
float density = graph.CalculateDensity();
```

See `GraphConsole/Program.cs` for a more detailed example, including custom vertex data and graph traversal.

## API Overview

### Core Classes

- `Graph`: Main class for graph operations.
- `Vertex`: Represents a node in the graph.
- `Edge<TStart, TEnd>`: Represents a directed edge between two vertices.
- `VertexFactory`: Utility for creating vertices.
- `EdgeFactory`: Utility for creating edges.

### Key Methods

- `AddVertex(string label, object data)`: Add a vertex with a label and custom data.
- `AddEdge(int start, int end, int weight)`: Add a directed edge by vertex index.
- `AddEdge(Vertex start, Vertex end, int weight)`: Add a directed edge by vertex reference.
- `DeleteVertex(int index | Guid id)`: Remove a vertex by index or ID.
- `DeleteEdge(int start, int end | Guid start, Guid end)`: Remove an edge by index or ID.
- `TopologicalSort()`: Print vertices in topological order.
- `DepthFirstSearch()`: Print vertices in DFS order.
- `BreadthFirstSearch()`: Print vertices in BFS order.
- `MinimumSpanningTree()`: Print edges in a spanning tree traversal.
- `Path()`, `Path(Guid startId)`: Print shortest paths from the first vertex or a specified vertex.
- Centrality and density calculations:
  - `CalculateDegree(Vertex | Guid)`
  - `CalculateDegreeCentrality(Vertex | Guid)`
  - `CalculateClosenessCentrality(Vertex)`
  - `CalculateBetweenness(Vertex)`
  - `CalculateEigenvectorCentrality(Vertex)`
  - `CalculateDensity()`
- Utility methods:
  - `Clear()`: Remove all vertices and edges
  - `AreConnected(Vertex, Vertex)`: Check if two vertices are directly connected
  - `ShowVertex(int | Guid)`: Print vertex label
  - `IsEmpty()`: Check if graph is empty

## Testing

Unit tests are provided in the `GraphLib.Test` project, using xUnit.

## Dependencies

- .NET 8.0 or higher
- No external dependencies for the core library

## Contributing

Contributions are welcome! Please submit issues or pull requests via GitHub.

## License

Specify your license here (e.g., MIT, Apache 2.0).
