namespace P1014.Test;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        var answerList = solver.Solve().ToArray();
        
        Assert.That(answerList[0], Is.EqualTo(4));
        Assert.That(answerList[1], Is.EqualTo(1));
        Assert.That(answerList[2], Is.EqualTo(2));
        Assert.That(answerList[3], Is.EqualTo(46));
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
        yield return "4";
        yield return "2 3";
        yield return "...";
        yield return "...";
        yield return "2 3";
        yield return "x.x";
        yield return "xxx";
        yield return "2 3";
        yield return "x.x";
        yield return "x.x";
        yield return "10 10";
        yield return "....x.....";
        yield return "..........";
        yield return "..........";
        yield return "..x.......";
        yield return "..........";
        yield return "x...x.x...";
        yield return ".........x";
        yield return "...x......";
        yield return "........x.";
        yield return ".x...x....";
    }
}