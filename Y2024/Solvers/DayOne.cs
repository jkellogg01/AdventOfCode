using System.Collections;

namespace Y2024.Solvers;

public abstract class DayOne : Solver
{
   public static int Part_One()
   {
      var lines = InputLines(1, false);
      var leftNums = new List<int>();
      var rightNums = new List<int>();
      foreach (var line in lines)
      {
         var nums = line.Split(" ").Where(s => s != "").Select(int.Parse).ToArray();
         leftNums.Add(nums[0]);
         rightNums.Add(nums[1]);
      }

      leftNums.Sort();
      rightNums.Sort();

      return leftNums.Select((val, idx) => Math.Abs(val - rightNums[idx])).Sum();
   }

   public static int Part_Two()
   {
      var lines = InputLines(1, false);
      var leftNums = new List<int>();
      var rightNums = new Dictionary<int, int>();
      foreach (var line in lines)
      {
         var nums = line.Split(" ").Where(s => s != "").ToArray();
         leftNums.Add(int.Parse(nums[0]));
         var rightNum = int.Parse(nums[1]);
         if (!rightNums.TryGetValue(rightNum, out var prevCount))
         {
            rightNums.Add(rightNum, 1);
         }

         rightNums[rightNum] = prevCount + 1;
      }

      return leftNums.Select(num =>
      {
         rightNums.TryGetValue(num, out var count);
         return num * count;
      }).Sum();
   }
}