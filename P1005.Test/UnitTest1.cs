namespace P1005.Test;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        var answer = solver.Solve().ToArray();
        
        Assert.That(answer[0], Is.EqualTo(120));
        Assert.That(answer[1], Is.EqualTo(39));
    }
    
    [Test]
    public void Test02()
    {
        var solver = new Solver(new TestReader02());
        var answer = solver.Solve().ToArray();
        
        Assert.That(answer[0], Is.EqualTo(6));
        Assert.That(answer[1], Is.EqualTo(5));
        Assert.That(answer[2], Is.EqualTo(399990));
        Assert.That(answer[3], Is.EqualTo(2));
        Assert.That(answer[4], Is.EqualTo(0));
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
        yield return "2";
        yield return "4 4";
        yield return "10 1 100 10";
        yield return "1 2";
        yield return "1 3";
        yield return "2 4";
        yield return "3 4";
        yield return "4";
        yield return "8 8";
        yield return "10 20 1 5 8 7 1 43";
        yield return "1 2";
        yield return "1 3";
        yield return "2 4";
        yield return "2 5";
        yield return "3 6";
        yield return "5 7";
        yield return "6 7";
        yield return "7 8";
        yield return "7";
    }
}

public class TestReader02 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "5";
        yield return "3 2";
        yield return "1 2 3";
        yield return "3 2";
        yield return "2 1";
        yield return "1";
        yield return "4 3";
        yield return "5 5 5 5";
        yield return "1 2";
        yield return "1 3";
        yield return "2 3";
        yield return "4";
        yield return "5 10";
        yield return "100000 99999 99997 99994 99990";
        yield return "4 5";
        yield return "3 5";
        yield return "3 4";
        yield return "2 5";
        yield return "2 4";
        yield return "2 3";
        yield return "1 5";
        yield return "1 4";
        yield return "1 3";
        yield return "1 2";
        yield return "4";
        yield return "4 3";
        yield return "1 1 1 1";
        yield return "1 2";
        yield return "3 2";
        yield return "1 4";
        yield return "4";
        yield return "7 8";
        yield return "0 0 0 0 0 0 0";
        yield return "1 2";
        yield return "1 3";
        yield return "2 4";
        yield return "3 4";
        yield return "4 5";
        yield return "4 6";
        yield return "5 7";
        yield return "6 7";
        yield return "7";
    }
}