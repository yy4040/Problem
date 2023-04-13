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

public class Solver
{
    private readonly int _targetValue;
    private readonly int[] _valueList;
    private readonly long[] _cachedValue;

    public Solver(IReader reader)
    {
        var args = reader
            .ReadLine()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        var coinCount = int.Parse(args[0]);
        _targetValue = int.Parse(args[1]);

        _valueList = Enumerable
            .Range(0, coinCount)
            .Select(_ => int.Parse(reader.ReadLine()))
            .Where(v => v <= _targetValue)
            .OrderByDescending(v => v)
            .ToArray();

        _cachedValue = new long[_targetValue * 100 + 100];
        Array.Fill(_cachedValue, -1L);
    }

    public long Solve()
    {
        return LoopCount(0, _targetValue);
    }

    private long LoopCount(int startIndex, int targetValue)
    {
        if (TryGetCachedValue(startIndex, targetValue, out var cachedValue))
            return cachedValue;
        
        var value = _valueList[startIndex];
        var count = targetValue / value;
        var result = 0L;
            
        for (var i = count; i >= 0; --i)
        {
            var remain = targetValue - value * i;
            if (remain == 0)
            {
                ++result;
                continue;
            }

            if (startIndex + 1 >= _valueList.Length)
                break;
                
            result += LoopCount(startIndex + 1, remain);
        }

        CacheValue(startIndex, targetValue, result);
        return result;
    }

    private bool TryGetCachedValue(int startIndex, int targetValue, out long cachedValue)
    {
        var index = targetValue * 100 + startIndex;
        cachedValue = _cachedValue[index];
        return cachedValue >= 0;
    }

    private void CacheValue(int startIndex, int targetValue, long cachingValue)
    {
        var index = targetValue * 100 + startIndex;
        _cachedValue[index] = cachingValue;
    }
}