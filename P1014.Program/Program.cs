var solver = new Solver(new ConsoleReader());
var answerList = solver.Solve();
foreach (var answer in answerList)
    Console.WriteLine(answer);

public interface IReader
{
    string ReadLine();
}

public class ConsoleReader : IReader
{
    public string ReadLine() => Console.ReadLine()!;
}

public static class StringExtension
{
    public static int ToInt(this string s) => int.Parse(s);
    public static int[] ToIntArray(this string s) => s
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();
}

public class Solver
{
    private readonly CaseData[] _caseList;
    
    public Solver(IReader reader)
    {
        var n = reader.ReadLine().ToInt();
        _caseList = Enumerable
            .Range(0, n)
            .Select(_ => new CaseData(reader))
            .ToArray();
    }

    public IEnumerable<int> Solve()
    {
        foreach (var caseData in _caseList)
        {
            yield return Solve(caseData);
        }
    }

    private int Solve(CaseData caseData)
    {
        var graph = caseData.Graph;
        var match = new int[graph.Length];
        var visited = new bool[graph.Length];
        
        // FindMaximumMatching
        var matchCount = 0;
        Array.Fill(match, -1);
        foreach (var node in graph
                     .Where(n => n.AdjacencyList.Count > 0)
                     .Where(n => n.Id % caseData.W % 2 == 0))
        {
            Array.Fill(visited, false);
            if (Matching(caseData.Graph, node.Id, match, visited))
                ++matchCount;
        }
        
        return caseData.EmptyCount - matchCount;
    }

    private bool Matching(Node[] graph, int targetId, int[] match, bool[] visited)
    {
        if (visited[targetId]) { return false; }
        visited[targetId] = true;
        
        foreach (var id in graph[targetId].AdjacencyList)
        {
            if (match[id] < 0 || Matching(graph, match[id], match, visited))
            {
                match[id] = targetId;
                return true;
            }
        }
        return false;
    }
}

public struct Vector
{
    public readonly int X;
    public readonly int Y;

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Node
{
    public readonly int Id;
    public readonly List<int> AdjacencyList = new();

    public Node(int id) => Id = id;
    
    public void Add(int id)
    {
        if (AdjacencyList.Contains(id) == false)
            AdjacencyList.Add(id);
    }
}

public class CaseData
{
    public readonly int W;
    public readonly int H;
    public readonly int EmptyCount;

    public readonly Node[] Graph;

    private static readonly Vector[] _offsets = new Vector[]
    {
        new(-1, -1), new(1, -1),
        new(-1, 0), new(1, 0),
        new(-1, 1), new(1, 1)
    };

    public CaseData(IReader reader)
    {
        var param = reader.ReadLine().ToIntArray();
        H = param[0];
        W = param[1];

        var map = Enumerable
            .Range(0, H)
            .Select(_ => reader
                .ReadLine()
                .ToArray())
            .ToArray();

        Graph = Enumerable
            .Range(0, W * H)
            .Select(i => new Node(i))
            .ToArray();

        for (var y = 0; y < H; ++y)
        for (var x = 0; x < W; ++x)
        {
            var c = map[y][x];
            if (c == 'x')
                continue;

            ++EmptyCount;

            var currentId = GetId(x, y);
            foreach (var offset in _offsets)
            {
                var tx = x + offset.X;
                var ty = y + offset.Y;
                
                if (IsOutOfBoundary(tx, ty))
                    continue;

                var tc = map[ty][tx];
                if (tc == 'x')
                    continue;
                
                var adjacencyId = GetId(tx, ty);
                Graph[currentId].Add(adjacencyId);
            }
        }
    }

    private int GetId(int x, int y) => y * W + x;

    private bool IsOutOfBoundary(int x, int y)
        => x < 0 || x >= W || y < 0 || y >= H;
}