using GraphLib;

namespace GraphConsole;

internal class Program
{
    static void Main(string[] args)
    {
        Graph coursesGraph = new();
        Guid cs101 = coursesGraph.AddVertex("CS101", new ComputerScienceCourse { CourseName = "Computer Science 101" });
        Guid cs201 = coursesGraph.AddVertex("CS201", new ComputerScienceCourse { CourseName = "Computer Science 201" });
        Guid ds101 = coursesGraph.AddVertex("DS101", new ComputerScienceCourse { CourseName = "Data Structures" });
        Guid al101 = coursesGraph.AddVertex("AL101", new ComputerScienceCourse { CourseName = "Assembly Language" });
        Guid os101 = coursesGraph.AddVertex("OS101", new ComputerScienceCourse { CourseName = "Operating Systems" });
        Guid alg101 = coursesGraph.AddVertex("ALG101", new ComputerScienceCourse { CourseName = "Algorithms" });

        coursesGraph.AddEdge(coursesGraph.VerticesList.Where(x => x.Id == cs101).First(),
            coursesGraph.VerticesList.Where(x => x.Id == cs201).First(), 0);
        coursesGraph.AddEdge(coursesGraph.VerticesList.Where(x => x.Id == cs201).First(),
            coursesGraph.VerticesList.Where(x => x.Id == al101).First(), 0);
        coursesGraph.AddEdge(coursesGraph.VerticesList.Where(x => x.Id == cs201).First(),
            coursesGraph.VerticesList.Where(x => x.Id == ds101).First(), 0);
        coursesGraph.AddEdge(coursesGraph.VerticesList.Where(x => x.Id == ds101).First(),
            coursesGraph.VerticesList.Where(x => x.Id == os101).First(), 0);
        coursesGraph.AddEdge(coursesGraph.VerticesList.Where(x => x.Id == ds101).First(),
            coursesGraph.VerticesList.Where(x => x.Id == alg101).First(), 0);

        // Topological sort
        Console.WriteLine("Topological Sort");
        coursesGraph.TopologicalSort();
        Console.WriteLine("Depth First Search");
        coursesGraph.DepthFirstSearch();
        Console.WriteLine("Breadth First Search");
        coursesGraph.BreadthFirstSearch();
    }
}

public class ComputerScienceCourse
{
    public string? CourseName { get; set; }
}
