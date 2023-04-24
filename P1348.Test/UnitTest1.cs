namespace P1348.Test;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        Assert.That(solver.Solve(), Is.EqualTo(6));
    }
    
    [Test]
    public void Test02()
    {
        var solver = new Solver(new TestReader02());
        Assert.That(solver.Solve(), Is.EqualTo(16));
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
        Assert.That(solver.Solve(), Is.EqualTo(4));
    }
    
    [Test]
    public void Test05()
    {
        var solver = new Solver(new TestReader05());
        Assert.That(solver.Solve(), Is.EqualTo(-1));
    }
    
    [Test]
    public void Test06()
    {
        var solver = new Solver(new TestReader06());
        Assert.That(solver.Solve(), Is.EqualTo(-1));
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
        yield return "3 7";
        yield return "C.....P";
        yield return "C.....P";
        yield return "C.....P";
    }
}

public class TestReader02 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "4 8";
        yield return "C.X.....";
        yield return "..X..X..";
        yield return "..X..X..";
        yield return ".....X.P";
    }
}

public class TestReader03 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "6 11";
        yield return "XXXXXXXXXXX";
        yield return "X......XPPX";
        yield return "XC...P.XPPX";
        yield return "X......X..X";
        yield return "X....C....X";
        yield return "XXXXXXXXXXX";
    }
}

public class TestReader04 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "5 3";
        yield return ".C.";
        yield return "...";
        yield return "C.C";
        yield return "X.X";
        yield return "PPP";
    }
}

public class TestReader05 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "3 5";
        yield return "CCCCC";
        yield return ".....";
        yield return "PXPXP";
    }
}

public class TestReader06 : BaseTestReader
{
    protected override IEnumerator<string> GetEnumerator()
    {
        yield return "3 5";
        yield return "..X..";
        yield return "C.X.P";
        yield return "..X..";
    }
}