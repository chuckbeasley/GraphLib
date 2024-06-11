using System.Security.Cryptography.X509Certificates;

namespace GraphLib;

public class Graph
{
    public List<Vertex> VerticesList { get; }
    public List<Edge<Vertex, Vertex>> EdgeList { get; }

    private const int MAX_VERTICES = 20;
    private int infinity = int.MaxValue;
    private Vertex[] vertices;
    private int[,] adjacencyMatrix;
    private int numVertices;
    private int numTree;
    private DistantOriginal[] sPath;
    private List<DistantOriginal> shortestPath;
    private int currentVertex;
    private int startToCurrent;

    public Graph()
    {
        VerticesList = new List<Vertex>();
        EdgeList = new List<Edge<Vertex,Vertex>>();
        numVertices = 0;
        numTree = 0;

        sPath = new DistantOriginal[MAX_VERTICES];
        shortestPath = new List<DistantOriginal>();
    }

    public Guid AddVertex(string label, object data)
    {
        Vertex vertex = VertexFactory.CreateVertex(label, data);
        VerticesList.Add(vertex);
        return vertex.Id;
    }

    public void AddEdge(int start, int end, int weight)
    {
    }

    public void AddEdge(Vertex start, Vertex end, int weight)
    {
        start.IsBoundary = false;
        end.Depth = start.Depth + 1;
        EdgeList.Add(EdgeFactory<Vertex, Vertex>.CreateEdge(start, end, weight));
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

    public float CalculateDeggreeCentrality(Vertex vertex)
    {
        return (float)CalculateDegree(vertex) / (VerticesList.Count - 1);
    }

    public float CalculateDeggreeCentrality(Guid id)
    {
        return (float)CalculateDegree(id) / (VerticesList.Count - 1);
    }

    public float CalculateDensity()
    {
        return (2 * EdgeList.Count) / (VerticesList.Count * (VerticesList.Count - 1));
    }

    public void DeleteVertex(int vertex)
    {
        if (vertex != numVertices - 1)
        {
            for (int i = vertex; i < numVertices - 1; i++)
            {
                vertices[i] = vertices[i + 1];
            }

            for (int row = vertex; row < numVertices - 1; row++)
            {
                MoveRow(row, numVertices);
            }

            for (int col = vertex; col < numVertices - 1; col++)
            {
                MoveCol(col, numVertices - 1);
            }
        }

        numVertices--;
    }

    public void DeleteVertex(Guid id)
    {
        VerticesList.Remove(VerticesList.First(v => v.Id == id));
    }

    public void DeleteEdge(int start, int end)
    {
        adjacencyMatrix[start, end] = 0;
    }

    public void DeleteEdge(Guid start, Guid end)
    {
        EdgeList.Remove(EdgeList.First(e => e.Start.Id == start && e.End.Id == end));
    }

    public void Clear()
    {
        numVertices = 0;
    }

    public void ShowVertex(int v)
    {
        Console.WriteLine(vertices[v].Label);
    }

    public void ShowVertex(Guid id)
    {
        Console.WriteLine(VerticesList.First(v => v.Id == id).Label);
    }

    public int NoSuccessors()
    {
        bool isEdge;
        for (int row = 0; row < numVertices; row++)
        {
            isEdge = false;
            for (int col = 0; col < numVertices; col++)
            {
                if (adjacencyMatrix[row, col] > 0)
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

    private void DepthFirstSearchInternal(Vertex vertex, HashSet<Vertex> visited)
    {
        // Mark the current node as visited
        visited.Add(vertex);
        Console.WriteLine(vertex.Label);

        // Recur for all the vertices adjacent to this vertex
        foreach (Edge<Vertex, Vertex> edge in EdgeList.Where(x => x.Start == vertex ||  x.End == vertex))
        {
            Vertex? adjacent = edge.End;
            if (adjacent != null && !visited.Contains(adjacent))
            {
                DepthFirstSearchInternal(adjacent, visited);
            }
        }
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
        Stack<int> gStack = new Stack<int>();
        vertices[0].Visited = true;
        gStack.Push(0);

        while (gStack.Count > 0)
        {
            int currentVertex = gStack.Peek();
            int v = GetAdjacentUnvisitedVertex(currentVertex);
            if (v == -1)
            {
                gStack.Pop();
            }
            else
            {
                vertices[v].Visited = true;
                gStack.Push(v);
                ShowVertex(currentVertex);
                ShowVertex(v);
                Console.WriteLine();
            }
        }

        for (int i = 0; i < numVertices; i++)
        {
            vertices[i].Visited = false;
        }
    }

    public void Path() 
    {
        int startTree = 0;
        vertices[startTree].IsInTree = true;
        numTree = 0;

        for (int i = 0; i < numVertices; i++)
        {
            int tempDist = adjacencyMatrix[startTree, i];
            sPath[i] = new DistantOriginal(startTree, tempDist);
        }

        while (numTree < numVertices)
        {
            int indexMin = GetMin();
            int minDist = sPath[indexMin].Distance;
            currentVertex = indexMin;
            startToCurrent = sPath[indexMin].Distance;
            vertices[currentVertex].IsInTree = true;
            numTree++;
            AdjustShortPath();
        }

        DisplayPaths();
        numTree = 0;
        for (int i = 0; i < numVertices; i++)
        {
            vertices[i].IsInTree = false;
        }
    }

    public int GetMin()
    {
        int minDist = infinity;
        int indexMin = 0;
        for (int i = 0; i < numVertices; i++)
        {
            if (!vertices[i].IsInTree && sPath[i].Distance < minDist)
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
            if (vertices[column].IsInTree)
            {
                column++;
                continue;
            }

            int currentToFringe = adjacencyMatrix[currentVertex, column];
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
            Console.Write(vertices[i].Label + "=");
            if (sPath[i].Distance == infinity)
            {
                Console.Write("inf");
            }
            else
            {
                Console.Write(sPath[i].Distance);
            }

            string parent = vertices[sPath[i].ParentVertex].Label;
            Console.Write("(" + parent + ") ");
        }
    }

    private int GetAdjacentUnvisitedVertex(int v)
    {
        for (int i = 0; i < numVertices; i++)
        {
            if (adjacencyMatrix[v, i] == 1 && !vertices[i].Visited)
            {
                return i;
            }
        }

        return -1;
    }

    private void MoveRow(int row, int length)
    {
        for (int col = 0; col < length; col++)
        {
            adjacencyMatrix[row, col] = adjacencyMatrix[row + 1, col];
        }
    }

    private void MoveCol(int col, int length)
    {
        for (int row = 0; row < length; row++)
        {
            adjacencyMatrix[row, col] = adjacencyMatrix[row, col + 1];
        }
    }
}