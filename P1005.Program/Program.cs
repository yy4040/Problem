var solver = new Solver(new ConsoleReader());
foreach (var answer in solver.Solve())
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
    private readonly CaseData[] _caseDataList;

    private readonly int[] _minimumBuildOrderTime = new int[1000];
    private readonly Stack<int> _taskStack = new Stack<int>();
    
    public Solver(IReader reader)
    {
        var caseCount = reader.ReadLine().ToInt();
        _caseDataList = Enumerable
            .Range(0, caseCount)
            .Select(_ => new CaseData(reader))
            .ToArray();
    }

    public IEnumerable<int> Solve()
    {
        foreach (var caseData in _caseDataList)
            yield return Solve(caseData);
    }

    private int Solve(CaseData data)
    {
        Array.Fill(_minimumBuildOrderTime, -1);
        _taskStack.Clear();
        
        _taskStack.Push(data.TargetIndex);
        while (_taskStack.Count > 0)
        {
            var targetIndex = _taskStack.Pop();
            var buildOrderTime = _minimumBuildOrderTime[targetIndex];
            
            if (buildOrderTime >= 0)
                continue;

            var preBuildingList = data.BuildingRuleReverse[targetIndex];
            if (preBuildingList.Count <= 0)
            {
                _minimumBuildOrderTime[targetIndex] = data.ConstructionTime[targetIndex];
                continue;
            }
            
            _taskStack.Push(targetIndex);
            var maxBuildOrder = -1;
            var aborted = false;
            foreach (var preBuildingIndex in preBuildingList)
            {
                var temp = _minimumBuildOrderTime[preBuildingIndex];
                if (temp < 0)
                {
                    _taskStack.Push(preBuildingIndex);
                    aborted = true;
                    break;
                }
                maxBuildOrder = Math.Max(maxBuildOrder, temp);
            }
            
            if (aborted)
                continue;
            
            var constructionTime = data.ConstructionTime[targetIndex];
            _minimumBuildOrderTime[targetIndex] = constructionTime + maxBuildOrder;
        }
        
        return _minimumBuildOrderTime[data.TargetIndex];
    }
}

public class CaseData
{
    public readonly int[] ConstructionTime; // 0 ≤ Di ≤ 100,000, Di는 정수
    
    public readonly List<int>[] BuildingRuleReverse; // 1 ≤ X, Y ≤ N
    public readonly int TargetIndex; // 1 ≤ W ≤ N

    public CaseData(IReader reader)
    {
        var param = reader.ReadLine().ToIntArray();
        var buildingNumber = param[0];
        var buildingRuleNumber = param[1];
        
        ConstructionTime = reader.ReadLine().ToIntArray();

        BuildingRuleReverse = Enumerable.Range(0, buildingNumber).Select(_ => new List<int>()).ToArray();
        
        for (var i = 0; i < buildingRuleNumber; ++i)
        {
            var temp = reader.ReadLine().ToIntArray();
            BuildingRuleReverse[temp[1] - 1].Add(temp[0] - 1);
        }

        TargetIndex = reader.ReadLine().ToInt() - 1;
    }
}