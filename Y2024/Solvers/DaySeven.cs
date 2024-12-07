namespace Y2024.Solvers;

public abstract class DaySeven
{
    private static readonly SolutionInput input = new(7);

    public static int PartOne()
    {
        var data = input.Lines();

        ulong sum = 0;
        int tested = 0;
        int success = 0;
        foreach (var line in data)
        {
            tested += 1;
            Console.WriteLine($"line {tested}");
            var splitLine = line.Split(": ");
            try
            {
                var test = ulong.Parse(splitLine[0]);
                var nums = splitLine[1].Split(" ").Select(ulong.Parse).ToArray();
                if (Reachable(0, nums, test))
                {
                    Console.WriteLine($"+ {test}");
                    success += 1;
                    sum += test;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"exception for line {tested}: {e.Message}");
                continue;
            }
        }
        Console.WriteLine($"{success}/{tested} rows succeeded");
        Console.WriteLine($"actual value: {sum}");
        return 0;
    }

    public static int PartTwo()
    {
        throw new NotImplementedException();
    }

    private static bool Reachable(ulong initial, ulong[] components, ulong target)
    {
        if (initial == target && components.Length == 0) return true;
        else if (initial >= target || components.Length == 0) return false;
        else if (components.Length == 0) return false;
        // Console.WriteLine($"{target:D12} <- {initial}");

        if (initial == 0)
        {
            return Reachable(components[0], components[1..], target);
        }

        return Reachable(initial * components[0], components[1..], target) ||
               Reachable(initial + components[0], components[1..], target);
    }
}