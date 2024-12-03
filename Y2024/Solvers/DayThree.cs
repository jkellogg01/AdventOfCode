using System.Runtime.CompilerServices;
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
            Console.WriteLine($"range {from}:{until}");
            sum += SumInstructions(data[from..until]);
            try {
                from = enables.First(e => e > until);
            } catch (InvalidOperationException e) {
                return sum;
            }
        }
        sum += SumInstructions(data[from..]);
        // while (true)
        // {
        //     var until = disables.Count == 0 ? data.Length : disables.First();
        //     while (until < from)
        //     {
        //         if (disables.Count <= 1) return sum;
        //         disables.RemoveAt(0);
        //         until = disables.First();
        //     }
        //     // Console.WriteLine($"range {from} - {until}");
        //     sum += SumInstructions(data[from..until]);
        //     if (disables.Count == 0) break;
        //     disables.RemoveAt(0);
        //     if (enables.Count == 0) break;
        //     from = enables.First();
        //     enables.RemoveAt(0);
        // }
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