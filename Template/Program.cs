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
    public Solver(IReader reader)
    {
    }

    public int Solve()
    {
        return 0;
    }
}