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
    public Solver(IReader reader)
    {
    }

    public int Solve()
    {
        return 0;
    }
}