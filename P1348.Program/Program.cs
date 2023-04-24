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

public class Node
{
    public int Id;
    public int X;
    public int Y;
    public readonly List<AdjacencyNode> AdjacencyList = new();
}

public class AdjacencyNode
{
    public readonly Node Target;
    public readonly int Cost;
    public AdjacencyNode(Node target, int cost)
    {
        Target = target;
        Cost = cost;
    }
}

public struct Position
{
    public readonly int X;
    public readonly int Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Solver
{
    private readonly char[][] _symbolMap;
    private readonly Node[][] _nodeMap;
    private readonly int[][] _carToParkCostMap;
    private readonly List<Node> _carList = new(100);
    private readonly List<Node> _parkList = new(100);

    private const char _wallSymbol = 'X';
    private const char _carSymbol = 'C';
    private const char _parkSymbol = 'P';
    
    public Solver(IReader reader)
    {
        var param = reader.ReadLine().ToIntArray();
        var height = param[0];
        var width = param[1];
        
        _symbolMap = Enumerable
            .Range(0, height)
            .Select(_ => reader.ReadLine().Prepend(_wallSymbol).Append(_wallSymbol).ToArray())
            .Prepend(Enumerable.Repeat(_wallSymbol, width + 2).ToArray())
            .Append(Enumerable.Repeat(_wallSymbol, width + 2).ToArray())
            .ToArray();

        _nodeMap = Enumerable
            .Range(0, height + 2)
            .Select(_ => new Node[width + 2])
            .ToArray();

        _tempCostTable = Enumerable
            .Range(0, height + 2)
            .Select(_ => new int[width + 2])
            .ToArray();
        _tempVisitTable = Enumerable
            .Range(0, height + 2)
            .Select(_ => new bool[width + 2])
            .ToArray();

        for (var y = 1; y <= height; ++y)
        for (var x = 1; x <= width; ++x)
        {
            var symbol = _symbolMap[y][x];
            if (symbol == _carSymbol)
            {
                var id = _carList.Count;
                var node = new Node() { Id = id, X = x, Y = y };
                _nodeMap[y][x] = node;
                _carList.Add(node);
            }
            else if (symbol == _parkSymbol)
            {
                var id = _parkList.Count;
                var node = new Node() { Id = id, X = x, Y = y };
                _nodeMap[y][x] = node;
                _parkList.Add(node);
            }
        }

        _tempCarFlag = new bool[_carList.Count];
        _tempParkToCar = new int[_parkList.Count];
        _carToParkCostMap = Enumerable
            .Range(0, _carList.Count)
            .Select(_ => Enumerable.Repeat(0, _parkList.Count).ToArray())
            .ToArray();
    }

    public int Solve()
    {
        if (_carList.Count <= 0)
            return 0;
        
        // 모든 car에 대하여, 도달할 수 있는 park와 그 비용 목록 만들기
        foreach (var car in _carList)
            SearchPark(car);
        
        // 모든 car에 대하여 매칭 시도
        var minCost = int.MaxValue;
        while (true)
        {
            var cost = CalcMatchingCost(minCost);
            if (cost < 0)
                break;
            minCost = Math.Min(minCost, cost);
        }
        
        return minCost == int.MaxValue ? -1 : minCost;
    }

    private readonly int[][] _tempCostTable;
    private readonly bool[][] _tempVisitTable;
    private readonly Queue<Position> _searchTask = new(2500);
    private readonly Position[] _searchDelta = new[]
    {
        new Position(-1, 0), // 좌
        new Position(1, 0), // 우
        new Position(0, -1), // 상
        new Position(0, 1) // 하
    };
    
    private void SearchPark(Node car)
    {
        // 초기화
        foreach (var tempArray in _tempCostTable)
            Array.Fill(tempArray, 0);
        foreach (var tempArray in _tempVisitTable)
            Array.Fill(tempArray, false);
        _searchTask.Clear();
        
        // 시작 위치 설정
        _searchTask.Enqueue(new Position(car.X, car.Y));
        
        // BFS 탐색
        while (_searchTask.Count > 0)
        {
            var current = _searchTask.Dequeue();
            
            // 이미 방문했으면 스킵
            if (_tempVisitTable[current.Y][current.X]) { continue; }
            _tempVisitTable[current.Y][current.X] = true;

            var cost = _tempCostTable[current.Y][current.X];

            // 주차장이면 인접 리스트에 등록
            var currentSymbol = _symbolMap[current.Y][current.X];
            if (currentSymbol == _parkSymbol)
            {
                var park = _nodeMap[current.Y][current.X];
                car.AdjacencyList.Add(new AdjacencyNode(park, cost));
                park.AdjacencyList.Add(new AdjacencyNode(car, cost));
                _carToParkCostMap[car.Id][park.Id] = cost;
            }

            // 4방향 탐색
            foreach (var delta in _searchDelta)
            {
                var nextY = current.Y + delta.Y;
                var nextX = current.X + delta.X;
                
                // 이미 방문했으면 스킵
                if (_tempVisitTable[nextY][nextX])
                    continue;
                
                // 벽이면 스킵
                if (_symbolMap[nextY][nextX] == _wallSymbol)
                    continue;
                
                // 비용 갱신
                _tempCostTable[nextY][nextX] = cost + 1;
                
                // 탐색 작업 목록에 등록
                _searchTask.Enqueue(new Position(nextX, nextY));
            }
        }
    }

    private readonly bool[] _tempCarFlag;
    private readonly int[] _tempParkToCar;
    private int CalcMatchingCost(int costLimit)
    {
        Array.Fill(_tempParkToCar, -1);

        for (var carId = 0; carId < _carList.Count; ++carId)
        {
            Array.Fill(_tempCarFlag, false);

            if (Matching(carId, costLimit) == false)
                return -1;
        }

        return _tempParkToCar
            .Select((carId, parkId) => new { carId, parkId })
            .Where(_ => _.carId >= 0)
            .Max(_ => _carToParkCostMap[_.carId][_.parkId]);
    }
    
    private bool Matching(int carId, int costLimit)
    {
        if (_tempCarFlag[carId]) { return false;  }
        _tempCarFlag[carId] = true;

        var list = _carList[carId]
            .AdjacencyList
            .Where(an => an.Cost < costLimit)
            .Select(an => an.Target);
        
        foreach (var park in list)
        {
            var assignedCarId = _tempParkToCar[park.Id];
            if (assignedCarId == -1 || Matching(assignedCarId, costLimit))
            {
                _tempParkToCar[park.Id] = carId;
                return true;
            }
        }

        return false;
    }
}