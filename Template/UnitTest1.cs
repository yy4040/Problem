namespace Template;

public class UnitTest1
{
    public void Test01()
    {
        var solver = new Solver(new TestReader01());
        //Assert.That(solver.Solve(), Is.EqualTo(1));
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
        yield return "Template";
    }
}