namespace P2293.Test;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test1()
    {
        var solver = new Solver(new TestReader01());
        Assert.That(solver.Solve(), Is.EqualTo(10));
    }
    
    [Test]
    public void Test2()
    {
        var solver = new Solver(new TestReader02());
        Assert.That(solver.Solve(), Is.EqualTo(33602993115));
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
        yield return "3 10";
        yield return "1";
        yield return "2";
        yield return "5";
    }
}

public class TestReader02 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "20 1000";
        for (var i = 0; i < 20; ++i)
            yield return (i * 7 + 1).ToString();
    }
}