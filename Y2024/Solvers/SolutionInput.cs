namespace Y2024.Solvers;

public class SolutionInput(int day)
{
    private string Path(bool test) => $"Input/Day_{day:D2}{(test ? "t" : "")}.txt";

    public IEnumerable<string> Lines() => Lines(false);
    public IEnumerable<string> Lines(bool test)
    {
        return File.ReadLines(Path(test));
    }

    public string[] AllLines() => AllLines(false);
    public string[] AllLines(bool test)
    {
        return File.ReadAllLines(Path(test));
    }

    public string AllText() => AllText(false);
    public string AllText(bool test)
    {
        return File.ReadAllText(Path(test));
    }
}