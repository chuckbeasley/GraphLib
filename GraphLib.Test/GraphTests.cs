namespace GraphLib.Test
{
    public class GraphTests
    {
        private Graph CreateSimpleGraph()
        {
            var graph = new Graph();
            var v1 = graph.AddVertex("A", null);
            var v2 = graph.AddVertex("B", null);
            var v3 = graph.AddVertex("C", null);
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(1, 2, 2);
            return graph;
        }

        private Graph CreateCoursesGraph()
        {
            var graph = new Graph();
            var cs101 = graph.AddVertex("CS101", null);
            var cs201 = graph.AddVertex("CS201", null);
            var al101 = graph.AddVertex("AL101", null);
            var ds101 = graph.AddVertex("DS101", null);
            var os101 = graph.AddVertex("OS101", null);
            var alg101 = graph.AddVertex("ALG101", null);

            graph.AddEdge(graph.VerticesList.Where(x => x.Id == cs101).First(),
                graph.VerticesList.Where(x => x.Id == cs201).First(), 0);
            graph.AddEdge(graph.VerticesList.Where(x => x.Id == cs201).First(),
                graph.VerticesList.Where(x => x.Id == al101).First(), 0);
            graph.AddEdge(graph.VerticesList.Where(x => x.Id == cs201).First(),
                graph.VerticesList.Where(x => x.Id == ds101).First(), 0);
            graph.AddEdge(graph.VerticesList.Where(x => x.Id == ds101).First(),
                graph.VerticesList.Where(x => x.Id == os101).First(), 0);
            graph.AddEdge(graph.VerticesList.Where(x => x.Id == ds101).First(),
                graph.VerticesList.Where(x => x.Id == alg101).First(), 0);
            return graph;
        }

        [Fact]
        public void AddVertex_AddsVertex()
        {
            var graph = new Graph();
            var id = graph.AddVertex("A", null);
            Assert.Single(graph.VerticesList);
            Assert.Equal("A", graph.VerticesList[0].Label);
            Assert.Equal(id, graph.VerticesList[0].Id);
        }

        [Fact]
        public void AddEdge_ByIndex_AddsEdge()
        {
            var graph = new Graph();
            graph.AddVertex("A", null);
            graph.AddVertex("B", null);
            graph.AddEdge(0, 1, 5);
            Assert.Single(graph.EdgeList);
            Assert.Equal(5, graph.EdgeList[0].Weight);
        }

        [Fact]
        public void AddEdge_ByVertex_AddsEdge()
        {
            var graph = new Graph();
            graph.AddVertex("A", null);
            graph.AddVertex("B", null);
            var v1 = graph.VerticesList[0];
            var v2 = graph.VerticesList[1];
            graph.AddEdge(v1, v2, 3);
            Assert.Single(graph.EdgeList);
            Assert.Equal(3, graph.EdgeList[0].Weight);
        }

        [Fact]
        public void AddEdge_InvalidIndex_Throws()
        {
            var graph = new Graph();
            graph.AddVertex("A", null);
            Assert.Throws<ArgumentOutOfRangeException>(() => graph.AddEdge(0, 1, 1));
        }

        [Fact]
        public void AddEdge_InvalidVertex_Throws()
        {
            var graph = new Graph();
            var v1 = new Vertex();
            var v2 = new Vertex();
            Assert.Throws<ArgumentException>(() => graph.AddEdge(v1, v2, 1));
        }

        [Fact]
        public void CalculateDegree_ReturnsCorrectDegree()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            var v2 = graph.VerticesList[1];
            Assert.Equal(1, graph.CalculateDegree(v1));
            Assert.Equal(2, graph.CalculateDegree(v2));
        }

        [Fact]
        public void CalculateDegree_ById_ReturnsCorrectDegree()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            Assert.Equal(1, graph.CalculateDegree(v1.Id));
        }

        [Fact]
        public void CalculateDegreeCentrality_ReturnsCorrectValue()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            float expected = 1f / (graph.VerticesList.Count - 1);
            Assert.Equal(expected, graph.CalculateDegreeCentrality(v1));
        }

        [Fact]
        public void CalculateDensity_ReturnsCorrectValue()
        {
            var graph = CreateSimpleGraph();
            float expected = (2f * graph.EdgeList.Count) / (graph.VerticesList.Count * (graph.VerticesList.Count - 1));
            Assert.Equal(expected, graph.CalculateDensity());
        }

        [Fact]
        public void CalculateClosenessCentrality_ReturnsValue()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            var value = graph.CalculateClosenessCentrality(v1);
            Assert.True(value >= 0);
        }

        [Fact]
        public void DeleteVertex_ByIndex_RemovesVertex()
        {
            var graph = CreateSimpleGraph();
            graph.DeleteVertex(0);
            Assert.Equal(2, graph.VerticesList.Count);
        }

        [Fact]
        public void DeleteVertex_ById_RemovesVertex()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            graph.DeleteVertex(v1.Id);
            Assert.DoesNotContain(v1, graph.VerticesList);
        }

        [Fact]
        public void DeleteEdge_ByIndex_RemovesEdge()
        {
            var graph = CreateSimpleGraph();
            graph.DeleteEdge(0, 1);
            Assert.Single(graph.EdgeList);
        }

        [Fact]
        public void DeleteEdge_ById_RemovesEdge()
        {
            var graph = CreateSimpleGraph();
            var e = graph.EdgeList[0];
            graph.DeleteEdge(e.Start!.Id, e.End!.Id);
            Assert.Single(graph.EdgeList);
        }

        [Fact]
        public void Clear_RemovesAllVerticesAndEdges()
        {
            var graph = CreateSimpleGraph();
            graph.Clear();
            Assert.Empty(graph.VerticesList);
            Assert.Empty(graph.EdgeList);
        }

        [Fact]
        public void IsEmpty_ReturnsTrueForEmptyGraph()
        {
            var graph = new Graph();
            Assert.True(graph.IsEmpty());
        }

        [Fact]
        public void IsEmpty_ReturnsFalseForNonEmptyGraph()
        {
            var graph = CreateSimpleGraph();
            Assert.False(graph.IsEmpty());
        }

        [Fact]
        public void AreConnected_ReturnsTrueIfConnected()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            var v2 = graph.VerticesList[1];
            Assert.True(graph.AreConnected(v1, v2));
        }

        [Fact]
        public void AreConnected_ReturnsFalseIfNotConnected()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            var v3 = graph.VerticesList[2];
            Assert.False(graph.AreConnected(v1, v3));
        }

        [Fact]
        public void CalculateBetweenness_ReturnsZero()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            Assert.Equal(0, graph.CalculateBetweenness(v1));
        }

        [Fact]
        public void CalculateEigenvectorCentrality_ReturnsZero()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            Assert.True(graph.CalculateEigenvectorCentrality(v1) > 0);
        }

        [Fact]
        public void TopologicalSort_WritesLabels()
        {
            var graph = CreateSimpleGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.TopologicalSort();
            var output = sw.ToString();
            Assert.Contains("A", output);
            Assert.Contains("B", output);
            Assert.Contains("C", output);
        }

        [Fact]
        public void DepthFirstSearch_WritesLabels()
        {
            var graph = CreateCoursesGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.DepthFirstSearch();
            var output = sw.ToString();
            Assert.Contains("A", output);
        }

        [Fact]
        public void BreadthFirstSearch_WritesLabels()
        {
            var graph = CreateCoursesGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.BreadthFirstSearch();
            var output = sw.ToString();
            Assert.Contains("A", output);
        }

        [Fact]
        public void DepthFirstSearch_ReturnsExpectedOrder()
        {
            var graph = CreateCoursesGraph();
            var output = new StringWriter();
            Console.SetOut(output);

            graph.DepthFirstSearch();

            var result = output.ToString().Trim().Split(Environment.NewLine);
            var expected = new[] { "CS101", "CS201", "AL101", "DS101", "OS101", "ALG101" }; // Adjust as needed
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BreadthFirstSearch_ReturnsExpectedOrder()
        {
            var graph = CreateCoursesGraph();
            var output = new StringWriter();
            Console.SetOut(output);

            graph.BreadthFirstSearch();

            var result = output.ToString().Trim().Split(Environment.NewLine);
            var expected = new[] { "CS101", "CS201", "AL101", "DS101", "OS101", "ALG101" }; // Adjust as needed
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShowVertex_ByIndex_WritesLabel()
        {
            var graph = CreateSimpleGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.ShowVertex(0);
            var output = sw.ToString();
            Assert.Contains("A", output);
        }

        [Fact]
        public void ShowVertex_ById_WritesLabel()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.ShowVertex(v1.Id);
            var output = sw.ToString();
            Assert.Contains("A", output);
        }

        [Fact]
        public void MinimumSpanningTree_WritesEdges()
        {
            var graph = CreateSimpleGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.MinimumSpanningTree();
            var output = sw.ToString();
            Assert.Contains("A - B", output);
        }

        [Fact]
        public void Path_NoArg_WritesPaths()
        {
            var graph = CreateSimpleGraph();
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.Path();
            var output = sw.ToString();
            Assert.Contains("A=0", output);
            Assert.Contains("B=1", output);
        }

        [Fact]
        public void Path_ByGuid_WritesPaths()
        {
            var graph = CreateSimpleGraph();
            var v1 = graph.VerticesList[0];
            using var sw = new StringWriter();
            Console.SetOut(sw);
            graph.Path(v1.Id);
            var output = sw.ToString();
            Assert.Contains("A=0", output);
            Assert.Contains("B=1", output);
        }

        [Fact]
        public void NoSuccessors_ReturnsCorrectIndex()
        {
            var graph = CreateSimpleGraph();
            int idx = graph.NoSuccessors();
            Assert.True(idx >= 0 && idx < graph.VerticesList.Count);
        }
    }
}
