namespace GraphLib
{
    public class Vertex
    {
        public Guid Id { get; }
        public bool Visited { get; set; }
        public bool IsInTree { get; set; }
        public bool IsBoundary { get; set; }
        public int Depth { get; set; }
        public string? Label { get; set; }
        public object? Data { get; }

        public Vertex()
        {
            Id = Guid.NewGuid();
        }

        internal Vertex(string label, object data)
        {
            Id = Guid.NewGuid();
            Label = label;
            Data = data;
        }

        public Type? GetDataType() => Data?.GetType();

        public override string? ToString() => Label;
    }
}