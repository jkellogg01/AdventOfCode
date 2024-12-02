using System.Text;

namespace Y2024.Solvers;

public static class DayTwo
{
    private static readonly SolutionInput input = new SolutionInput(2);

    public static int Part_One()
    {
        return input
            .Lines()
            .Select(line => line
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .Count(nums => deltasInsideBounds(nums, 1, 3) && constantChangeDirection(nums));
    }

    public static int Part_Two()
    {
        return input
            .Lines()
            .Select(line => line
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .Count(safe_two);
    }

    private static bool safe_two(int[] nums)
    {
        for (var i = 0; i < nums.Length; i++)
        {
            var numsSkipping = nums.Where((_, idx) => idx != i).ToArray();
            var safe = deltasInsideBounds(numsSkipping, 1, 3) && constantChangeDirection(numsSkipping);
            if (!safe) continue;
            Console.WriteLine($"[{numString(numsSkipping)}] SAFE!");
            return true;
        }

        return false;
    }

    private static bool deltasInsideBounds(int[] nums, int min, int max)
    {
        var prev = nums[0];
        return nums.Skip(1).All(num =>
        {
            var delta = Math.Abs(num - prev);
            var result = delta >= min && delta <= max;
            prev = num;
            if (!result) Console.WriteLine($"[{numString(nums)}] unsafe: out-of-bounds-delta <{delta}>");
            return result;
        });
    }
    
    private static bool constantChangeDirection(int[] nums)
    {
        var result = true;
        var prev = 0;
        var prevDelta = 0;
        for (var i = 0; i < nums.Length; i++)
        {
            if (i == 0)
            {
                prev = nums[i];
                continue;
            }

            var curr = nums[i];
            if (prevDelta != 0 && curr - prev > 0 != prevDelta > 0)
            {
                result = false;
            }

            prevDelta = curr - prev;
            prev = curr;
        }

        if (!result) Console.WriteLine($"[{numString(nums)}] unsafe: non-constant direction");
        return result;
    }
    
    private static string numString(int[] nums)
    {
        var result = new StringBuilder();
        result.Append(' ');
        foreach (var num in nums)
        {
            result.Append(num + " ");
        }

        return result.ToString();
    }
}