namespace P2188.Test;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        Assert.That(solver.Solve(), Is.EqualTo(4));
    }
    
    [Test]
    public void Test02()
    {
        var solver = new Solver(new TestReader02());
        Assert.That(solver.Solve(), Is.EqualTo(3));
    }
    
    [Test]
    public void Test03()
    {
        var solver = new Solver(new TestReader02());
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
        yield return "2 2 5";
        yield return "3 2 3 4";
        yield return "2 1 5";
        yield return "3 1 2 5";
        yield return "1 2";
    }
}

public class TestReader02 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "4 7";
        yield return "3 5 6 7";
        yield return "3 5 6 7";
        yield return "3 5 6 7";
        yield return "3 5 6 7";
    }
}

public class TestReader03 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "7 5";
        yield return "2 1 2";
        yield return "1 1";
        yield return "2 1 2";
        yield return "0";
        yield return "0";
        yield return "0";
        yield return "5 1 2 3 4 5";
    }
}