//#define DEBUG_PRINT

using System.Text;

// init
var solver = new Solver(new ConsoleReader());
var answer = solver.Solve();

Console.WriteLine(answer);

#if DEBUG_PRINT
// print solution
if (answer > 0)
{
    var solution = solver.GetSolution();
    Console.WriteLine(solution);
}
#endif

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
    private const int _tryLimit = 10;
    private int _bestTryCount = 999;

    private readonly Map[] _cachedMaps;
#if DEBUG_PRINT
    private readonly Map[] _historyMaps;
#endif
    
    public Solver(IReader reader)
    {
        var parameters = reader
            .ReadLine()!
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    
        var height = parameters[0];
        var width = parameters[1];

        _cachedMaps = new Map[_tryLimit + 1];
        var data = Enumerable
            .Range(0, height)
            .Select(_ => reader.ReadLine()!.Take(width).ToArray())
            .ToArray();
        _cachedMaps[0] = new Map(data);
        for (var i = 1; i < _cachedMaps.Length; ++i)
            _cachedMaps[i] = new Map(width, height);

#if DEBUG_PRINT
        _historyMaps = new Map[_tryLimit + 1]; // for debug
        _historyMaps[0] = new Map(data);
        for (var i = 1; i < _historyMaps.Length; ++i)
            _historyMaps[i] = new Map(width, height);
#endif
    }

    public int Solve()
    {
        // dfs
        foreach (var dir in Enum.GetValues<Direction>())
            Search(1, dir);

        // print answer
        return _bestTryCount <= _tryLimit ? _bestTryCount : -1;
    }

#if DEBUG_PRINT
    public string GetSolution()
    {
        var builder = new StringBuilder();
        for (var i = 1; i < _historyMaps.Length; ++i)
        {
            var map = _historyMaps[i];
            builder.AppendLine($"<<<< {i} : {map.LastMoveDirection.GetDirectionString()} >>>>");
            builder.AppendLine(map.ToString());
            if (map.Complete)
                break;
        }
        return builder.ToString();
    }
#endif

    private SearchResult Search(int tryCount, Direction searchDir)
    {
        if (tryCount < 1)
            return SearchResult.Error;
        if (tryCount >= _bestTryCount)
            return SearchResult.Error;

        var prevMap = _cachedMaps[tryCount - 1];
        var map = _cachedMaps[tryCount];
    
        prevMap.CopyTo(map);

        map.Move(searchDir switch
        {
            Direction.Right => new Vector2 { x = 1, y = 0 },
            Direction.Bottom => new Vector2 { x = 0, y = 1 },
            Direction.Left => new Vector2 { x = -1, y = 0 },
            Direction.Top => new Vector2 { x = 0, y = -1 }
        });

        if (map.Complete)
        {
#if DEBUG_PRINT
        map.CopyTo(_historyMaps[tryCount]);
#endif
            _bestTryCount = tryCount;
            return SearchResult.Found;
        }
        if (map.ExitBlue)
            return SearchResult.Error;
        if (tryCount >= _tryLimit)
            return SearchResult.Error;

        var resultList = Enum
            .GetValues<Direction>()
            .Where(dir => dir != searchDir)
            .Select(dir => Search(tryCount + 1, dir));

        var founded = false;
        foreach (var _ in resultList.Where(r => r == SearchResult.Found))
        {
#if DEBUG_PRINT
        map.CopyTo(_historyMaps[tryCount]);
#endif
            founded = true;
        }

        return founded ? SearchResult.Found : SearchResult.Error;
    }
}

public class Map
{
    private readonly char[][] _data;
    private readonly int _w;
    private readonly int _h;

    private Vector2 _red, _blue;

    public bool ExitRed { get; private set; }
    public bool ExitBlue { get; private set; }
    public bool Complete => ExitRed && !ExitBlue;
    
    // for debug
    public Vector2 LastMoveDirection { get; private set; }

    public Map(int w, int h)
    {
        _w = w;
        _h = h;
        
        _data = new char[h][];
        for (var y = 0; y < h; ++y)
            _data[y] = new char[w];
    }

    public Map(char[][] data)
    {
        _data = data;
        _w = data[0].Length;
        _h = data.Length;
        UpdateSymbolPosition();
    }

    public char[] this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    public void CopyTo(Map other)
    {
        for (var y = 0; y < _h; ++y)
        for (var x = 0; x < _w; ++x)
        {
            other._data[y][x] = _data[y][x];
        }

        other._red = _red;
        other._blue = _blue;
        other.ExitRed = ExitRed;
        other.ExitBlue = ExitBlue;
        other.LastMoveDirection = LastMoveDirection;
    }

    public void Move(Vector2 dir)
    {
        // move red ball
        var collision = false;
        {
            var result = MoveBall('R', dir);
            if (result == 'O')
                ExitRed = true;
            else if (result == 'B')
                collision = true;
        }
        
        // move blue ball
        {
            var result = MoveBall('B', dir);
            if (result == 'O')
                ExitBlue = true;
        }
        
        // move red ball, when collision
        if (collision)
        {
            var result = MoveBall('R', dir);
            if (result == 'O')
                ExitRed = true;
        }

        LastMoveDirection = dir;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var y = 0; y < _h; ++y)
        {
            for (var x = 0; x < _w; ++x)
                builder.Append(_data[y][x]);
            builder.AppendLine();
        }
        return builder.ToString();
    }
    
    private char MoveBall(char target, Vector2 dir)
    {
        var origin = target == 'R' ? _red : _blue;
        var pos = origin;
        var c = _data[pos.y][pos.x];
        while (c == target || c == '.')
        {
            pos += dir;
            c = _data[pos.y][pos.x];
        }

        _data[origin.y][origin.x] = '.';

        if (c != 'O')
        {
            pos -= dir;
            _data[pos.y][pos.x] = target;
            if (target == 'R') _red = pos;
            else _blue = pos;
        }
        
        return c;
    }

    private void UpdateSymbolPosition()
    {
        for (var y = 0; y < _h; ++y)
        {
            for (var x = 0; x < _w; ++x)
            {
                var c = _data[y][x];
                if (c == 'R')
                {
                    _red = new Vector2 { x = x, y = y };
                }
                else if (c == 'B')
                {
                    _blue = new Vector2 { x = x, y = y };
                }
            }
        }
    }
}

public struct Vector2
{
    public int x, y;

    public static Vector2 operator +(Vector2 a, Vector2 b)
        => new Vector2() { x = a.x + b.x, y = a.y + b.y };
    
    public static Vector2 operator -(Vector2 a, Vector2 b)
        => new Vector2() { x = a.x - b.x, y = a.y - b.y };

    public string GetDirectionString()
    {
        if (x > 0) return nameof(Direction.Right);
        if (y < 0) return nameof(Direction.Top);
        if (x < 0) return nameof(Direction.Left);
        if (y > 0) return nameof(Direction.Bottom);
        return "None";
    }
}

public enum Direction
{
    Right,
    Bottom,
    Left,
    Top
}

public enum SearchResult
{
    None,
    Found,
    Error
}