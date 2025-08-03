namespace GraphLib
{
    public class Edge<T1, T2>
        where T1 : Vertex, new()
        where T2 : Vertex, new()
    {
        public int Weight { get; set; }
        public T1? Start { get; set; }
        public T2? End { get; set; }

        private Edge() { }

        internal Edge(T1 start, T2 end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
    }
}