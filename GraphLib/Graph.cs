namespace GraphLib;

public class Graph
{
    public List<Vertex> VerticesList { get; }
    public List<Edge<Vertex, Vertex>> EdgeList { get; }

    private const int MAX_VERTICES = 20;
    private int infinity = int.MaxValue;
    private List<List<int>> adjacencyMatrix;
    private int numVertices;
    private int numTree;
    private List<DistantOriginal> sPath;
    private List<DistantOriginal> shortestPath;
    private int currentVertex;
    private int startToCurrent;

    public Graph()
    {
        VerticesList = new List<Vertex>();
        EdgeList = new List<Edge<Vertex, Vertex>>();
        numVertices = 0;
        numTree = 0;
        adjacencyMatrix = new List<List<int>>();
        sPath = new List<DistantOriginal>(MAX_VERTICES);
        shortestPath = new List<DistantOriginal>();
    }

    public Guid AddVertex(string label, object data)
    {
        Vertex vertex = VertexFactory.CreateVertex(label, data);
        VerticesList.Add(vertex);
        numVertices = VerticesList.Count;
        // Expand adjacencyMatrix for new vertex
        foreach (var row in adjacencyMatrix)
        {
            row.Add(0);
        }
        var newRow = new List<int>(new int[numVertices]);
        adjacencyMatrix.Add(newRow);
        return vertex.Id;
    }

    public void AddEdge(int start, int end, int weight)
    {
        if (start < 0 || start >= VerticesList.Count || end < 0 || end >= VerticesList.Count)
            throw new ArgumentOutOfRangeException("Vertex index out of range.");

        var startVertex = VerticesList[start];
        var endVertex = VerticesList[end];

        startVertex.IsBoundary = false;
        endVertex.Depth = startVertex.Depth + 1;

        EdgeList.Add(EdgeFactory<Vertex, Vertex>.CreateEdge(startVertex, endVertex, weight));
        adjacencyMatrix[start][end] = weight;
    }

    public void AddEdge(Vertex start, Vertex end, int weight)
    {
        int startIdx = VerticesList.IndexOf(start);
        int endIdx = VerticesList.IndexOf(end);
        if (startIdx == -1 || endIdx == -1)
            throw new ArgumentException("Vertex not found in VerticesList.");
        start.IsBoundary = false;
        end.Depth = start.Depth + 1;
        EdgeList.Add(EdgeFactory<Vertex, Vertex>.CreateEdge(start, end, weight));
        adjacencyMatrix[startIdx][endIdx] = weight;
    }

    public float CalculateBetweenness(Vertex vertex)
    {
        return 0;
    }

    public float CalculateEigenvectorCentrality(Vertex vertex)
    {
        return 0;
    }

    public int CalculateDegree(Vertex vertex)
    {
        return EdgeList.Count(e => e.Start == vertex || e.End == vertex);
    }

    public int CalculateDegree(Guid id)
    {
        return EdgeList.Count(e => e?.Start?.Id == id || e?.End?.Id == id);
    }

    public float CalculateDegreeCentrality(Vertex vertex)
    {
        return (float)CalculateDegree(vertex) / (VerticesList.Count - 1);
    }

    public float CalculateDegreeCentrality(Guid id)
    {
        return (float)CalculateDegree(id) / (VerticesList.Count - 1);
    }

    public float CalculateDensity()
    {
        int n = VerticesList.Count;
        if (n <= 1) return 0f;
        return (2f * EdgeList.Count) / (n * (n - 1));
    }
    public float CalculateClosenessCentrality(Vertex vertex)
    {
        if (VerticesList.Count == 0) return 0f;
        var distances = new Dictionary<Vertex, int>();
        foreach (var v in VerticesList)
        {
            distances[v] = int.MaxValue;
        }
        distances[vertex] = 0;
        var queue = new Queue<Vertex>();
        queue.Enqueue(vertex);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var edge in EdgeList.Where(e => e.Start == current))
            {
                var neighbor = edge.End;
                if (distances[neighbor] == int.MaxValue)
                {
                    distances[neighbor] = distances[current] + edge.Weight;
                    queue.Enqueue(neighbor);
                }
            }
        }
        var totalDistance = distances.Values.Sum();
        return totalDistance > 0 ? (float)(VerticesList.Count - 1) / totalDistance : 0f;
    }

    public void DeleteVertex(int vertex)
    {
        if (vertex < 0 || vertex >= VerticesList.Count)
            throw new ArgumentOutOfRangeException("Vertex index out of range.");
        VerticesList.RemoveAt(vertex);
        numVertices = VerticesList.Count;
        adjacencyMatrix.RemoveAt(vertex);
        foreach (var row in adjacencyMatrix)
        {
            row.RemoveAt(vertex);
        }
    }

    public void DeleteVertex(Guid id)
    {
        var vertex = VerticesList.FirstOrDefault(v => v.Id == id);
        if (vertex != null)
        {
            VerticesList.Remove(vertex);
            EdgeList.RemoveAll(e => e.Start?.Id == id || e.End?.Id == id);
        }
    }

    public void DeleteEdge(int start, int end)
    {
        if (start < 0 || start >= VerticesList.Count || end < 0 || end >= VerticesList.Count)
            throw new ArgumentOutOfRangeException("Vertex index out of range.");

        var startVertex = VerticesList[start];
        var endVertex = VerticesList[end];

        var edge = EdgeList.FirstOrDefault(e => e.Start == startVertex && e.End == endVertex);
        if (edge != null)
        {
            EdgeList.Remove(edge);
        }
    }

    public void DeleteEdge(Guid start, Guid end)
    {
        EdgeList.Remove(EdgeList.First(e => e.Start.Id == start && e.End.Id == end));
    }

    public void Clear()
    {
        VerticesList.Clear();
        EdgeList.Clear();
        numVertices = 0;
        numTree = 0;
        shortestPath.Clear();
    }

    public void ShowVertex(int v)
    {
        Console.WriteLine(VerticesList[v].Label);
    }

    public void ShowVertex(Guid id)
    {
        Console.WriteLine(VerticesList.First(v => v.Id == id).Label);
    }

    public int NoSuccessors()
    {
        for (int row = 0; row < numVertices; row++)
        {
            bool isEdge = false;
            for (int col = 0; col < numVertices; col++)
            {
                if (adjacencyMatrix[row][col] > 0)
                {
                    isEdge = true;
                    break;
                }
            }
            if (!isEdge)
            {
                return row;
            }
        }
        return -1;
    }

    public void TopologicalSort()
    {
        var boundaries = EdgeList.OrderBy(x => x.End?.Depth).ThenBy(x => x.End?.IsBoundary).ToList();
        HashSet<Vertex> vertices = new HashSet<Vertex>();
        foreach (var vertex in boundaries)
        {
            vertices.Add(vertex.Start!);
            vertices.Add(vertex.End!);
        }

        foreach (var vertex in vertices)
        {
            Console.WriteLine(vertex.Label);
        }
    }

    public void DepthFirstSearch()
    {
        if (EdgeList.First().Start == null)
            return;

        HashSet<Vertex> visited = new HashSet<Vertex>();

        DepthFirstSearchInternal(EdgeList.First().Start!, visited);
    }

    public void BreadthFirstSearch()
    {
        if (EdgeList.First().Start! == null)
            return;

        HashSet<Vertex> visited = new HashSet<Vertex>();
        Queue<Vertex> queue = new Queue<Vertex>();

        visited.Add(EdgeList.First().Start!);
        queue.Enqueue(EdgeList.First().Start!);

        // Enqueue all adjacent vertices that haven't been visited yet
        foreach (Edge<Vertex, Vertex> edge in EdgeList.OrderBy(x => x.End?.Depth))
        {
            Vertex? adjacent = edge.End;
            if (adjacent != null && !visited.Contains(adjacent))
            {
                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }

        while (queue.Count > 0)
        {
            Vertex vertex = queue.Dequeue();
            Console.WriteLine(vertex.Label);
        }
    }

    public void MinimumSpanningTree()
    {
        if (VerticesList.Count == 0)
            return;

        var visited = new HashSet<Vertex>();
        var stack = new Stack<Vertex>();
        var startVertex = VerticesList[0];
        visited.Add(startVertex);
        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            var currentVertex = stack.Peek();
            // Find an adjacent unvisited vertex
            var adjacent = EdgeList
                .Where(e => e.Start == currentVertex && !visited.Contains(e.End))
                .Select(e => e.End)
                .FirstOrDefault();

            if (adjacent == null)
            {
                stack.Pop();
            }
            else
            {
                visited.Add(adjacent);
                stack.Push(adjacent);
                Console.WriteLine($"{currentVertex.Label} - {adjacent.Label}");
            }
        }
    }

    public void Path()
    {
        int startTree = 0;
        VerticesList[startTree].IsInTree = true;
        numTree = 0;

        for (int i = 0; i < numVertices; i++)
        {
            int tempDist = adjacencyMatrix[startTree][i];
            if (i < sPath.Count)
                sPath[i] = new DistantOriginal(startTree, tempDist);
            else
                sPath.Add(new DistantOriginal(startTree, tempDist));
        }

        while (numTree < numVertices)
        {
            int indexMin = GetMin();
            int minDist = sPath[indexMin].Distance;
            currentVertex = indexMin;
            startToCurrent = sPath[indexMin].Distance;
            VerticesList[currentVertex].IsInTree = true;
            numTree++;
            AdjustShortPath();
        }

        DisplayPaths();
        numTree = 0;
        for (int i = 0; i < numVertices; i++)
        {
            VerticesList[i].IsInTree = false;
        }
    }

    public int GetMin()
    {
        int minDist = infinity;
        int indexMin = 0;
        for (int i = 0; i < numVertices; i++)
        {
            if (!VerticesList[i].IsInTree && sPath[i].Distance < minDist)
            {
                minDist = sPath[i].Distance;
                indexMin = i;
            }
        }
        return indexMin;
    }

    public void AdjustShortPath()
    {
        int column = 1;
        while (column < numVertices)
        {
            if (VerticesList[column].IsInTree)
            {
                column++;
                continue;
            }

            int currentToFringe = adjacencyMatrix[currentVertex][column];
            int startToFringe = startToCurrent + currentToFringe;
            int sPathDist = sPath[column].Distance;

            if (startToFringe < sPathDist)
            {
                sPath[column].ParentVertex = currentVertex;
                sPath[column].Distance = startToFringe;
            }

            column++;
        }
    }

    public void DisplayPaths()
    {
        for (int i = 0; i < numVertices; i++)
        {
            Console.Write(VerticesList[i].Label + "=");
            if (sPath[i].Distance == infinity)
            {
                Console.Write("inf");
            }
            else
            {
                Console.Write(sPath[i].Distance);
            }

            string parent = VerticesList[sPath[i].ParentVertex].Label;
            Console.Write("(" + parent + ") ");
        }
    }

    public bool AreConnected(Vertex v1, Vertex v2)
    {
        return EdgeList.Any(e => e.Start == v1 && e.End == v2);
    }

    public void Path(Guid startId)
    {
        var startVertex = VerticesList.FirstOrDefault(v => v.Id == startId);
        if (startVertex == null)
            return;

        var distances = VerticesList.ToDictionary(v => v, v => v == startVertex ? 0 : int.MaxValue);
        var previous = VerticesList.ToDictionary(v => v, v => (Vertex?)null);
        var unvisited = new HashSet<Vertex>(VerticesList);

        while (unvisited.Count > 0)
        {
            var current = unvisited.OrderBy(v => distances[v]).First();
            unvisited.Remove(current);

            foreach (var edge in EdgeList.Where(e => e.Start == current))
            {
                var neighbor = edge.End;
                if (!unvisited.Contains(neighbor)) continue;

                int alt = distances[current] + edge.Weight;
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                }
            }
        }

        // Display paths
        foreach (var vertex in VerticesList)
        {
            Console.Write(vertex.Label + "=");
            if (distances[vertex] == int.MaxValue)
            {
                Console.Write("inf");
            }
            else
            {
                Console.Write(distances[vertex]);
            }

            var path = new Stack<string>();
            var v = vertex;
            while (previous[v] != null)
            {
                path.Push(previous[v]!.Label!);
                v = previous[v]!;
            }
            if (path.Count > 0)
            {
                Console.Write("(" + string.Join("->", path) + ") ");
            }
            else
            {
                Console.Write(" ");
            }
        }
        Console.WriteLine();
    }

    public bool IsEmpty()
    {
        return VerticesList.Count == 0;
    }

    private void DepthFirstSearchInternal(Vertex vertex, HashSet<Vertex> visited)
    {
        // Mark the current node as visited
        visited.Add(vertex);
        Console.WriteLine(vertex.Label);

        // Recur for all the vertices adjacent to this vertex
        foreach (Edge<Vertex, Vertex> edge in EdgeList.Where(x => x.Start == vertex || x.End == vertex))
        {
            Vertex? adjacent = edge.End;
            if (adjacent != null && !visited.Contains(adjacent))
            {
                DepthFirstSearchInternal(adjacent, visited);
            }
        }
    }
}