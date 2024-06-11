namespace GraphLib;

public class VertexDistance<T> where T: Vertex
{
    public int Distance { get; set; }
    public T ParentVertex { get; set; }

    public VertexDistance(int distance, T vertex)
    {
        Distance = distance;
        ParentVertex = vertex;
    }
}