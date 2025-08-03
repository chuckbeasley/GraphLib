namespace GraphLib;

public class Vertex
{
    public Guid Id { get; init; }
    public bool Visited { get; set; }
    public bool IsInTree { get; set; }
    public bool IsBoundary { get; set; }
    public int Depth { get; set; }
    public string? Label { get; set; }
    public object? Data { get; init; }

    public Vertex()
    {
    }

    internal Vertex(string label, object data)
    {
        Id = Guid.NewGuid();
        Label = label;
        IsInTree = false;
        Visited = false;
        IsBoundary = true;
        Depth = 0;
        Data = data;
    }

    public Type? GetDataType()
    {
        return Data?.GetType();
    }

    public override string? ToString()
    {
        return Label;
    }
}