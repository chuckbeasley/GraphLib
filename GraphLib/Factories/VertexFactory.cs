namespace GraphLib;

public static class VertexFactory
{
    public static Vertex CreateVertex(string label, object data)
    {
        return new Vertex(label, data);
    }
}