using System.Collections;

namespace Y2024.Solvers;

public class Day_One : Solver
{
   public static int Part_One()
   {
      var lines = File.ReadLines(Input("Day_01"));
      var leftNums = new List<int>();
      var rightNums = new List<int>();
      foreach (var line in lines)
      {
         var nums = line.Split(" ").Where(s => s != "").ToArray();
         leftNums.Add(int.Parse(nums[0]));
         rightNums.Add(int.Parse(nums[1]));
      }

      leftNums.Sort();
      rightNums.Sort();

      var result = 0;
      for (var i = 0; i < leftNums.Count; ++i)
      {
         var diff = Math.Abs(leftNums[i] - rightNums[i]);
         result += diff;
      }

      return result;
   }

   public static int Part_Two()
   {
      var lines = File.ReadLines(Input("Day_01"));
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

      var result = 0;
      foreach (var num in leftNums)
      {
         rightNums.TryGetValue(num, out var count);
         result += num * count;
      }
      return result;
   }
}