var caseCount = Console.ReadLine()!.ToInt();
for (var i = 0; i < caseCount; ++i)
{
    var solver = new Solver(new ConsoleReader());
    var answer = solver.Solve();
    Console.WriteLine(answer);
}

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
    private readonly int[][] _preferredBookIdMap;
    private readonly bool[] _visit;
    private readonly int[] _bookToAssignedStudentId;
    
    public Solver(IReader reader)
    {
        var param = reader.ReadLine().ToIntArray();
        var bookNumber = param[0];
        var studentNumber = param[1];
        
        _preferredBookIdMap = Enumerable
            .Range(0, studentNumber)
            .Select(_ =>
            {
                var temp = reader.ReadLine().ToIntArray();
                return Enumerable.Range(temp[0] - 1, temp[1] - temp[0] + 1).ToArray();
            })
            .ToArray();

        _visit = new bool[studentNumber];
        _bookToAssignedStudentId = new int[bookNumber];
    }

    public int Solve()
    {
        var matchingCount = 0;
        
        Array.Fill(_bookToAssignedStudentId, -1);
        for (var i = 0; i < _preferredBookIdMap.Length; ++i)
        {
            Array.Fill(_visit, false);
            if (Matching(i))
                ++matchingCount;
        }

        return matchingCount;
    }

    private bool Matching(int studentId)
    {
        if (_visit[studentId]) { return false; }
        _visit[studentId] = true;

        var preferredBookIdList = _preferredBookIdMap[studentId];
        foreach (var bookId in preferredBookIdList)
        {
            var assignedStudentId = _bookToAssignedStudentId[bookId];
            if (assignedStudentId < 0 || Matching(assignedStudentId))
            {
                _bookToAssignedStudentId[bookId] = studentId;
                return true;
            }
        }

        return false;
    }
}