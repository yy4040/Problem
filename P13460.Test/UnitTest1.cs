namespace t13460;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        Assert.That(solver.Solve(), Is.EqualTo(1));
    }
    
    [Test]
    public void Test02()
    {
        var solver = new Solver(new TestReader02());
        Assert.That(solver.Solve(), Is.EqualTo(5));
    }
    
    [Test]
    public void Test03()
    {
        var solver = new Solver(new TestReader03());
        Assert.That(solver.Solve(), Is.EqualTo(5));
    }
    
    [Test]
    public void Test04()
    {
        var solver = new Solver(new TestReader04());
        Assert.That(solver.Solve(), Is.EqualTo(-1));
    }
    
    [Test]
    public void Test05()
    {
        var solver = new Solver(new TestReader05());
        Assert.That(solver.Solve(), Is.EqualTo(1));
    }
    
    [Test]
    public void Test06()
    {
        var solver = new Solver(new TestReader06());
        Assert.That(solver.Solve(), Is.EqualTo(7));
    }
    
    [Test]
    public void Test07()
    {
        var solver = new Solver(new TestReader07());
        Assert.That(solver.Solve(), Is.EqualTo(-1));
    }
    
    [Test]
    public void Test08()
    {
        var solver = new Solver(new TestReader08());
        Assert.That(solver.Solve(), Is.EqualTo(3));
    }

    [Test]
    public void Test09()
    {
        var solver = new Solver(new TestReader09());
        Assert.That(solver.Solve(), Is.EqualTo(3));
    }
}

public abstract class BaseTestReader : IReader
{
    private IEnumerator<string>? _enumerator;
    
    public string ReadLine()
    {
        _enumerator ??= GetEnumerator();
        return _enumerator.MoveNext() ? _enumerator.Current : string.Empty;
    }

    protected abstract IEnumerator<string> GetEnumerator();
}

public class TestReader01 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "5 5";
        yield return "#####";
        yield return "#..B#";
        yield return "#.#.#";
        yield return "#RO.#";
        yield return "#####";
    }
}

public class TestReader02 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "7 7";
        yield return "#######";
        yield return "#...RB#";
        yield return "#.#####";
        yield return "#.....#";
        yield return "#####.#";
        yield return "#O....#";
        yield return "#######";
    }
}

public class TestReader03 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "7 7";
        yield return "#######";
        yield return "#..R#B#";
        yield return "#.#####";
        yield return "#.....#";
        yield return "#####.#";
        yield return "#O....#";
        yield return "#######";
    }
}

public class TestReader04 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "10 10";
        yield return "##########";
        yield return "#R#...##B#";
        yield return "#...#.##.#";
        yield return "#####.##.#";
        yield return "#......#.#";
        yield return "#.######.#";
        yield return "#.#....#.#";
        yield return "#.#.#.#..#";
        yield return "#...#.O#.#";
        yield return "##########";
    }
}

public class TestReader05 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "3 7";
        yield return "#######";
        yield return "#R.O.B#";
        yield return "#######";
    }
}

public class TestReader06 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "10 10";
        yield return "##########";
        yield return "#R#...##B#";
        yield return "#...#.##.#";
        yield return "#####.##.#";
        yield return "#......#.#";
        yield return "#.######.#";
        yield return "#.#....#.#";
        yield return "#.#.##...#";
        yield return "#O..#....#";
        yield return "##########";
    }
}

public class TestReader07 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "3 10";
        yield return "##########";
        yield return "#.O....RB#";
        yield return "##########";
    }
}

public class TestReader08 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "10 10";
        yield return "##########";
        yield return "#R.......#";
        yield return "#B.......#";
        yield return "#........#";
        yield return "#........#";
        yield return "#........#";
        yield return "#........#";
        yield return "#O.......#";
        yield return "#........#";
        yield return "##########";
    }
}

public class TestReader09 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "7 6";
        yield return "######";
        yield return "######";
        yield return "####.#";
        yield return "#.R#.#";
        yield return "#.#.##";
        yield return "#O.B.#";
        yield return "######";
    }
}