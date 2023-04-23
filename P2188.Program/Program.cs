var solver = new Solver(new ConsoleReader());
var answer = solver.Solve();
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
    public static IEnumerable<int> ToIntEnumerable(this string s) => s
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse);
    public static int[] ToIntArray(this string s) => s
        .ToIntEnumerable()
        .ToArray();
}

public class Solver
{
    private readonly int[][] _preferredShedIdMap;
    private readonly bool[] _visit;
    private readonly int[] _shedToAssignedCowId;
    
    public Solver(IReader reader)
    {
        var param = reader.ReadLine().ToIntArray();
        var cowNumber = param[0];
        var shedNumber = param[1];
        
        _preferredShedIdMap = Enumerable
            .Range(0, cowNumber)
            .Select(_ => reader.ReadLine().ToIntEnumerable().Skip(1).Select(i => i - 1).ToArray())
            .ToArray();

        _visit = new bool[cowNumber];
        _shedToAssignedCowId = new int[shedNumber];
    }

    public int Solve()
    {
        var matchingCount = 0;
        
        Array.Fill(_shedToAssignedCowId, -1);
        for (var i = 0; i < _preferredShedIdMap.Length; ++i)
        {
            Array.Fill(_visit, false);
            if (Matching(i))
                ++matchingCount;
        }

        return matchingCount;
    }

    private bool Matching(int cowId)
    {
        if (_visit[cowId]) { return false; }
        _visit[cowId] = true;

        var preferredShedIdList = _preferredShedIdMap[cowId];
        foreach (var shedId in preferredShedIdList)
        {
            var assignedCowId = _shedToAssignedCowId[shedId];
            if (assignedCowId < 0 || Matching(assignedCowId))
            {
                _shedToAssignedCowId[shedId] = cowId;
                return true;
            }
        }

        return false;
    }
}