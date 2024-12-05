namespace Y2024.Solvers;

public abstract class DayFour
{
    private static readonly SolutionInput input = new(4);

    private static (int, int)[] directions = {(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)};

    public static int PartOne()
    {
        var data = input.AllLines();

        var startCoords = new List<(int, int)>();
        for (int i = 0; i < data.Length; i++)
        {
            var line = data[i];
            var lineOffset = 0;
            while (true)
            {
                var next = line.IndexOf('X');
                if (next == -1) break;
                startCoords.Add((i, next + lineOffset));
                line = line[(next + 1)..];
                lineOffset += next + 1;
            }
        }
        var sum = 0;
        foreach(var coord in startCoords)
        {
            var needle = "MAS";
            foreach(var direction in directions) {
                if (Search(needle, data, coord, direction)) sum += 1;
            }
        }
        return sum;
    }

    public static int PartTwo()
    {
        var data = input.AllLines();

        var mStartCoords = new List<(int, int)>();
        var sStartCoords = new List<(int, int)>();
        for (int i = 0; i < data.Length; i++)
        {
            var line = data[i];
            var mLineOffset = 0;
            while (true)
            {
                var next = line[mLineOffset..].IndexOf('M');
                if (next == -1) break;
                mStartCoords.Add((i, next + mLineOffset));
                mLineOffset += next + 1;
            }
            var sLineOffset = 0;
            while (true)
            {
                var next = line[sLineOffset..].IndexOf('S');
                if (next == -1) break;
                sStartCoords.Add((i, next + sLineOffset));
                sLineOffset += next + 1;
            }
        }
        var sum = 0;
        foreach (var coord in mStartCoords)
        {
            if (!Search("AS", data, coord, (1, 1))) continue;
            var nextCoord = (coord.Item1 + 3, coord.Item2 - 1);
            if (Search("MAS", data, nextCoord, (-1, 1)) || Search("SAM", data, nextCoord, (-1, 1))) sum += 1;
        }
        foreach (var coord in sStartCoords)
        {
            if (!Search("AM", data, coord, (1, 1))) continue;
            var nextCoord = (coord.Item1 + 3, coord.Item2 - 1);
            if (Search("MAS", data, nextCoord, (-1, 1)) || Search("SAM", data, nextCoord, (-1, 1))) sum += 1;
        }
        return sum;
    }

    private static bool Search(string needle, string[] haystack, (int, int) start, (int, int) direction)
    {
        if (needle == "") 
        {
            return true;
        }
        var next = (start.Item1 + direction.Item1, start.Item2 + direction.Item2);
        if (next.Item1 >= haystack.Length || next.Item1 < 0) return false;
        if (next.Item2 >= haystack[next.Item1].Length || next.Item2 < 0) return false;
        var atNext = haystack[next.Item1][next.Item2];
        if (atNext != needle[0]) return false;
        return Search(needle[1..], haystack, next, direction);
    }
}