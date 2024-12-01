using System.Runtime.CompilerServices;

namespace Y2024.Solvers;

public class Solver
{
    private static string Input(int day, bool test) => $"Input/Day_{day:D2}{(test ? "t" : "")}.txt";

    protected static IEnumerable<string> InputLines(int day, bool test)
    {
        return File.ReadLines(Input(day, test));
    }
    
    protected static string[] InputAllLines(int day, bool test)
    {
        return File.ReadAllLines(Input(day, test));
    }

    protected static string InputAllText(int day, bool test)
    {
        return File.ReadAllText(Input(day, test));
    }
}