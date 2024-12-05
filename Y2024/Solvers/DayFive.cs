namespace Y2024.Solvers;

public abstract class DayFive
{
    private static readonly SolutionInput input = new(5);
    public static int PartOne()
    {
        var data = input.Lines();

        var rules = ParseRules(data.TakeWhile(line => line != "").Where(line => line != "").ToArray());
        var updates = ParseUpdates(data.SkipWhile(line => line != "").Where(line => line != "").ToArray());

        var sum = 0;
        foreach (var update in UpdatesFollowRules(updates, rules))
        {
            Console.WriteLine(update.Select(e => e.ToString()).Aggregate((acc, e) => acc + " " + e));
            var middle = update.Length / 2;
            Console.WriteLine($"+ {update[middle]}");
            sum += update[middle];
        }

        return sum;
    }

    public static int PartTwo()
    {
        var data = input.Lines();

        var rules = ParseRules(data.TakeWhile(line => line != "").Where(line => line != "").ToArray());
        var updates = ParseUpdates(data.SkipWhile(line => line != "").Where(line => line != "").ToArray());

        var sum = 0;
        foreach (var update in UpdatesDontFollowRules(updates, rules))
        {
            var updateFixed = ConformUpdateToRules(update, rules);
            var middle = updateFixed.Length / 2;
            sum += updateFixed[middle];
        }

        return sum;
    }

    // key = after, value = list of befores
    private static Dictionary<int, List<int>> ParseRules(string[] lines)
    {
        var rules = new Dictionary<int, List<int>>();
        foreach (var line in lines.Select(line => line.Split('|').Where(line => line != "").Select(int.Parse).ToArray()))
        {
            var exists = rules.TryGetValue(line[1], out var ints);
            ints ??= new List<int>(1);
            ints.Add(line[0]);
            if (exists) rules[line[1]] = ints;
            else rules.Add(line[1], ints);
        }
        return rules;
    }

    private static List<int[]> ParseUpdates(string[] lines)
    {
        var updates = new List<int[]>();
        foreach(var line in lines.Select(line => line.Split(',').Where(line => line != "").Select(int.Parse).ToArray()))
        {
            updates.Add(line);
        }
        return updates;
    }

    private static List<int[]> UpdatesFollowRules(List<int[]> updates, Dictionary<int, List<int>> rules)
    {
        return updates.Where(update => UpdateFollowsRules(update, rules)).ToList();
    }

    private static List<int[]> UpdatesDontFollowRules(List<int[]> updates, Dictionary<int, List<int>> rules)
    {
        return updates.Where(update => !UpdateFollowsRules(update, rules)).ToList();
    }

    private static bool UpdateFollowsRules(int[] update, Dictionary<int, List<int>> rules)
    {
            // fuck it we do n squared and then try to optimize later
            for (int i = 0; i < update.Length - 1; i++)
            {
                for (int j = i + 1; j < update.Length; j++)
                {
                    var before = update[i];
                    var after = update[j];
                    if (!rules.ContainsKey(before)) continue;
                    if (rules[before].Contains(after)) return false;
                }
            }
            return true;
    }

    private static int[] ConformUpdateToRules(int[] update, Dictionary<int, List<int>> rules)
    {
        Array.Sort(update, (a, b) => {
            if (rules.ContainsKey(a) && rules[a].Contains(b))
            {
                return 1;
            } 
            else if (rules.ContainsKey(b) && rules[b].Contains(a))
            {
                return -1;
            }
            else return 0;
        });
        return update;
    }


}