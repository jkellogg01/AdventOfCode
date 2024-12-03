using System.Text.RegularExpressions;

namespace Y2024.Solvers;

public abstract class DayThree
{
    private static readonly SolutionInput input = new(3);

    public static int PartOne()
    {
        var data = input.AllText(false);

        return SumInstructions(data);
    }

    public static int PartTwo()
    {
        var data = input.AllText(false);

        var enable = new Regex(@"do\(\)");

        var disable = new Regex(@"don't\(\)");

        var enables = enable.Matches(data).Select(e => e.Index).ToList();

        var disables = disable.Matches(data).Select(e => e.Index).ToList();

        var sum = 0;
        var from = 0;
        foreach(var until in disables)
        {
            if (until <= from) continue;
            sum += SumInstructions(data[from..until]);
            try {
                from = enables.First(e => e > until);
            } catch (InvalidOperationException) {
                return sum;
            }
        }
        sum += SumInstructions(data[from..]);
        return sum;
    }

    public static int SumInstructions(string input)
    {
        var instruction = new Regex(@"mul\((\d+),(\d+)\)");

        return instruction 
            .Matches(input)
            .Select(ins => ins.Groups.Values
                .Skip(1)
                .Select(e => int.Parse(e.ToString()))
                .Aggregate((e, acc) => e * acc))
            .Sum();
    }
}