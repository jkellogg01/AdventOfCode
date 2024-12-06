using System.Data;
using System.Text;

namespace Y2024.Solvers;

public abstract class DaySix
{
    private static readonly SolutionInput input = new(6);

    private const char guardChar = '^';
    private const char obstacleChar = '#';

    public static int PartOne()
    {
        var data = input.AllLines();

        var guard = new Position { row = -1, col = -1 };
        var obstacles = new List<Position>();

        for (int i = 0; i < data.Length; i++)
        {
            var line = data[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == obstacleChar) obstacles.Add(new Position { row = i, col = j });
                else if (line[j] == guardChar) guard = new Position { row = i, col = j };
            }
        }

        if (guard.row < 0 || guard.col < 0) throw new Exception("invalid guard location");
        // Console.WriteLine($"guard at {guard.row + 1}:{guard.col + 1}");

        return TracePath(guard, obstacles, data.Length, data[0].Length).Count;
    }

    public static int PartTwo()
    {
        var data = input.AllLines();

        var guardStart = new Position { row = -1, col = -1 };
        var obstacles = new List<Position>();

        for (int i = 0; i < data.Length; i++)
        {
            var line = data[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == obstacleChar) obstacles.Add(new Position { row = i, col = j });
                else if (line[j] == guardChar) guardStart = new Position { row = i, col = j };
            }
        }

        if (guardStart.row < 0 || guardStart.col < 0) throw new Exception("invalid guard location");
        // Console.WriteLine($"guard at {guardStart.row + 1}:{guardStart.col + 1}");

        var visits = TracePath(guardStart, obstacles, data.Length, data[0].Length);

        var sum = 0;

        foreach (var testPosition in visits)
        {
            if (testPosition.Equals(guardStart)) continue;
            // Console.WriteLine($"TEST: [{testPosition.row + 1:D3}:{testPosition.col + 1:D3}]");
            obstacles.Add(testPosition);
            var guardOne = guardStart;
            var guardOneFacing = Direction.Up;
            var guardTwo = guardStart;
            var guardTwoFacing = Direction.Up;
            var invalidPosition = new Position
            {
                row = -1,
                col = -1,
            };
            while (true)
            {
                guardOne = RayWalk(guardOne, guardOneFacing, obstacles);
                if (guardOne.Equals(invalidPosition)) break;
                guardOneFacing = Turn(guardOneFacing);
                guardTwo = RayWalk(guardTwo, guardTwoFacing, obstacles);
                if (guardTwo.Equals(invalidPosition)) break;
                guardTwoFacing = Turn(guardTwoFacing);
                guardTwo = RayWalk(guardTwo, guardTwoFacing, obstacles);
                if (guardTwo.Equals(invalidPosition)) break;
                guardTwoFacing = Turn(guardTwoFacing);
                // Console.WriteLine($"1: [{guardOne.row + 1:D3}:{guardOne.col + 1:D3}] 2: [{guardTwo.row + 1:D3}:{guardTwo.col + 1:D3}]");
                if (guardOne.Equals(guardTwo) && guardOneFacing.Equals(guardTwoFacing))
                {
                    // Console.WriteLine($"obstacle at [{testPosition.row + 1:D3}:{testPosition.col + 1:D3}] creates cycle");
                    sum += 1;
                    break;
                }
            }
            obstacles.Remove(testPosition);
        }

        return sum;
    }

    private static HashSet<Position> TracePath(Position guard, List<Position> obstacles, int rowBound, int colBound)
    {
        var positions = new HashSet<Position>();
        var guardFacing = Direction.Up;

        while (InsideBounds(guard, rowBound, colBound))
        {
            positions.Add(guard);
            guard = Walk(guard, ref guardFacing, obstacles);
        }

        return positions;
    }

    private static bool InsideBounds(Position position, int rowBound, int colBound)
    {
        return position.col >= 0 && position.col < colBound &&
               position.row >= 0 && position.row < rowBound;
    }

    private static bool RayCast(Position start, Direction direction, List<Position> obstacles)
    {
        return obstacles.Any(pos => direction switch
        {
            Direction.Up => pos.col == start.col && pos.row < start.row,
            Direction.Right => pos.row == start.row && pos.col > start.col,
            Direction.Down => pos.col == start.col && pos.row > start.row,
            Direction.Left => pos.row == start.row && pos.col < start.col,
            _ => throw new Exception("invalid direction!"),
        });
    }

    private static Position RayWalk(Position start, Direction direction, List<Position> obstacles)
    {
        if (!RayCast(start, direction, obstacles)) return new Position
        {
            row = -1,
            col = -1,
        };
        var hit = obstacles.Where(pos => direction switch
        {
            Direction.Up => pos.col == start.col && pos.row < start.row,
            Direction.Down => pos.col == start.col && pos.row > start.row,
            Direction.Left => pos.row == start.row && pos.col < start.col,
            Direction.Right => pos.row == start.row && pos.col > start.col,
            _ => throw new Exception("invalid direction!"),
        }).MinBy(pos =>
        {
            // Console.WriteLine($"[{start.row + 1:D3}:{start.col + 1:D3}] => {direction} HIT: [{pos.row + 1:D3}:{pos.col + 1:D3}]");
            return direction switch
            {
                Direction.Up => Math.Abs(pos.row - start.row),
                Direction.Down => Math.Abs(pos.row - start.row),
                Direction.Left => Math.Abs(pos.col - start.col),
                Direction.Right => Math.Abs(pos.col - start.col),
                _ => throw new Exception("invalid direction!"),
            };
        });
        return direction switch
        {
            Direction.Up => new Position { row = hit.row + 1, col = hit.col },
            Direction.Down => new Position { row = hit.row - 1, col = hit.col },
            Direction.Left => new Position { row = hit.row, col = hit.col + 1 },
            Direction.Right => new Position { row = hit.row, col = hit.col - 1 },
            _ => throw new Exception("invalid direction!"),
        };
    }

    private static Position Walk(Position start, Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Position { row = start.row - 1, col = start.col },
            Direction.Down => new Position { row = start.row + 1, col = start.col },
            Direction.Right => new Position { row = start.row, col = start.col + 1 },
            Direction.Left => new Position { row = start.row, col = start.col - 1 },
            _ => throw new Exception("invalid direction"),
        };
    }

    private static Position Walk(Position start, ref Direction direction, List<Position> obstacles)
    {
        var forward = Walk(start, direction);
        if (obstacles.Any(pos => pos.Equals(forward)))
        {
            direction = Turn(direction);
            forward = Walk(start, direction);
        }
        return forward;
    }

    private static Direction Turn(Direction start)
    {
        return start switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new Exception("invalid direction"),
        };
    }

    private struct Position
    {
        public int row;
        public int col;
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}