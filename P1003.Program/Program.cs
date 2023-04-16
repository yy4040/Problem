var solver = new Solver(new ConsoleReader());
var answer = solver.Solve();
foreach (var (zero, one) in answer)
    Console.WriteLine($"{zero} {one}");

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
    private readonly int[] _numbers;
    private readonly FibonacciResult[] _results;
    private readonly bool[] _cacheFlag;
    private const int _maxSize = 50;
    
    public Solver(IReader reader)
    {
        var count = int.Parse(reader.ReadLine());
        _numbers = Enumerable
            .Range(0, count)
            .Select(_ => reader.ReadLine())
            .Select(int.Parse)
            .ToArray();

        _results = new FibonacciResult[_maxSize];
        _results[0] = FibonacciResult.Zero;
        _results[1] = FibonacciResult.One;

        _cacheFlag = new bool[_maxSize];
        _cacheFlag[0] = _cacheFlag[1] = true;
    }

    public IEnumerable<(int zero, int one)> Solve() => _numbers
        .Select(Fibonacci)
        .Select(fr => (fr.ZeroCount, fr.OneCount));

    private FibonacciResult Fibonacci(int n)
    {
        if (n == 0) { return FibonacciResult.Zero; }
        if (n == 1) { return FibonacciResult.One; }

        if (_cacheFlag[n])
            return _results[n];

        var result = Fibonacci(n - 1) + Fibonacci(n - 2);
        _results[n] = result;
        _cacheFlag[n] = true;
        return result;
    }
}

public struct FibonacciResult
{
    public int ZeroCount;
    public int OneCount;

    public static readonly FibonacciResult Zero = new()
    {
        ZeroCount = 1
    };
    
    public static readonly FibonacciResult One = new()
    {
        OneCount = 1
    };
    
    public static FibonacciResult operator +(FibonacciResult a, FibonacciResult b) => new()
    {
        ZeroCount = a.ZeroCount + b.ZeroCount,
        OneCount = a.OneCount + b.OneCount
    };
}