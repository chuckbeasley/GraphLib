namespace GraphLib;

public static class EdgeFactory<T1,T2> where T1 : Vertex, new() where T2 : Vertex, new()
{
    public static Edge<T1,T2> CreateEdge(T1 start, T2 end, int weight)
    {
        return new Edge<T1,T2>(start, end, weight);
    }
}