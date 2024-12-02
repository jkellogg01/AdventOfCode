using System.Text;

namespace Y2024.Solvers;

public static class DayTwo
{
    private static readonly SolutionInput input = new SolutionInput(2);

    public static int PartOne()
    {
        return input
            .Lines()
            .Select(line => line
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .Count(Safe);
    }

    public static int PartTwo()
    {
        return input
            .Lines()
            .Select(line => line
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .Count(DampenedSafe);
    }

    private static bool Safe(int[] nums) => DeltasInsideBounds(nums, 1, 3) && ConstantChangeDirection(nums);

    private static bool DampenedSafe(int[] nums)
    {
        for (var i = 0; i < nums.Length; i++)
        {
            var numsSkipping = nums.Where((_, idx) => idx != i).ToArray();
            var safe = DeltasInsideBounds(numsSkipping, 1, 3) && ConstantChangeDirection(numsSkipping);
            if (!safe) continue;
            return true;
        }

        return false;
    }

    private static bool DeltasInsideBounds(int[] nums, int min, int max)
    {
        var prev = nums[0];
        return nums.Skip(1).All(num =>
        {
            var delta = Math.Abs(num - prev);
            var result = delta >= min && delta <= max;
            prev = num;
            return result;
        });
    }
    
    private static bool ConstantChangeDirection(int[] nums)
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

        return result;
    }
    
    private static string NumString(int[] nums)
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