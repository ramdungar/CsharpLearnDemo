namespace CsharpLearnDemo.Lessons.Interview;

/// <summary>
/// Small problems that show up on C# screens. Implementations are written
/// the way you should talk through them: clear names, guards, LINQ when it
/// helps, explicit loops when the algorithm is the point.
///
/// Tests in CsharpLearnDemo.Tests lock the behavior so you can refactor safely
/// (TDD / characterization tests — a professional practice).
/// </summary>
public static class InterviewAlgorithms
{
    /// <summary>
    /// Reverse the characters of a string. string is immutable, so we allocate
    /// a new one. In-place reverse needs a char[] or Span&lt;char&gt;.
    /// Interview follow-up: reverse words, not characters.
    /// </summary>
    public static string Reverse(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text ?? string.Empty;
        }

        var chars = text.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    /// <summary>
    /// Two-pointer palindrome, ignoring non-letters and case (classic "valid palindrome").
    /// </summary>
    public static bool IsPalindrome(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return true;
        }

        var left = 0;
        var right = text.Length - 1;
        while (left < right)
        {
            if (!char.IsLetterOrDigit(text[left]))
            {
                left++;
                continue;
            }

            if (!char.IsLetterOrDigit(text[right]))
            {
                right--;
                continue;
            }

            if (char.ToLowerInvariant(text[left]) != char.ToLowerInvariant(text[right]))
            {
                return false;
            }

            left++;
            right--;
        }

        return true;
    }

    /// <summary>
    /// FizzBuzz as a pure function so tests do not scrape the console.
    /// </summary>
    public static IReadOnlyList<string> FizzBuzz(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);
        var result = new string[n];
        for (var i = 1; i <= n; i++)
        {
            var fizz = i % 3 == 0;
            var buzz = i % 5 == 0;
            result[i - 1] = (fizz, buzz) switch
            {
                (true, true) => "FizzBuzz",
                (true, false) => "Fizz",
                (false, true) => "Buzz",
                _ => i.ToString()
            };
        }

        return result;
    }

    /// <summary>
    /// Return indices of two numbers that add to <paramref name="target"/>.
    /// O(n) with a dictionary — mention the O(n^2) brute force first, then optimize.
    /// </summary>
    public static (int i, int j)? TwoSum(IReadOnlyList<int> nums, int target)
    {
        ArgumentNullException.ThrowIfNull(nums);
        var seen = new Dictionary<int, int>();
        for (var i = 0; i < nums.Count; i++)
        {
            var need = target - nums[i];
            if (seen.TryGetValue(need, out var j))
            {
                return (j, i);
            }

            seen[nums[i]] = i;
        }

        return null;
    }

    /// <summary>
    /// Iterative Fibonacci. Interviewers watch for the naive exponential recursion.
    /// </summary>
    public static long Fibonacci(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        if (n < 2)
        {
            return n;
        }

        long prev = 0, curr = 1;
        for (var i = 2; i <= n; i++)
        {
            var next = prev + curr;
            prev = curr;
            curr = next;
        }

        return curr;
    }

    /// <summary>
    /// Group words that are anagrams. Sort each word as the key (O(n * k log k)).
    /// Follow-up: count letters in an int[26] key for lowercase English.
    /// </summary>
    public static IReadOnlyDictionary<string, List<string>> GroupAnagrams(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);
        return words
            .GroupBy(Normalize)
            .ToDictionary(g => g.Key, g => g.ToList());

        static string Normalize(string word)
        {
            var chars = word.ToLowerInvariant().ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }

    /// <summary>
    /// LINQ: people with the highest score per department — a very common
    /// "can you actually use LINQ" screen.
    /// </summary>
    public static IReadOnlyList<PersonScore> TopPerDepartment(IEnumerable<PersonScore> people)
    {
        ArgumentNullException.ThrowIfNull(people);
        return people
            .GroupBy(p => p.Department)
            .Select(g => g.OrderByDescending(p => p.Score).First())
            .OrderBy(p => p.Department)
            .ToList();
    }
}

/// <summary>Tiny DTO for the LINQ interview problem.</summary>
public sealed record PersonScore(string Name, string Department, int Score);
