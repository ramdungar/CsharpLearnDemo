using CsharpLearnDemo.Lessons.Interview;

namespace CsharpLearnDemo.Tests;

/// <summary>
/// Characterization tests for the interview solutions.
/// Pattern: Arrange / Act / Assert (AAA) — say this out loud in interviews.
/// </summary>
public sealed class InterviewAlgorithmTests
{
    [Theory]
    [InlineData("CSharp", "prahSC")]
    [InlineData("", "")]
    [InlineData("a", "a")]
    public void Reverse_returns_reversed_characters(string input, string expected)
    {
        Assert.Equal(expected, InterviewAlgorithms.Reverse(input));
    }

    [Fact]
    public void Reverse_null_becomes_empty()
    {
        Assert.Equal(string.Empty, InterviewAlgorithms.Reverse(null));
    }

    [Theory]
    [InlineData("A man, a plan, a canal: Panama", true)]
    [InlineData("race a car", false)]
    [InlineData("", true)]
    [InlineData("ab", false)]
    public void IsPalindrome_ignores_punctuation_and_case(string input, bool expected)
    {
        Assert.Equal(expected, InterviewAlgorithms.IsPalindrome(input));
    }

    [Fact]
    public void FizzBuzz_15_hits_all_branches()
    {
        var actual = InterviewAlgorithms.FizzBuzz(15);
        Assert.Equal("1", actual[0]);
        Assert.Equal("Fizz", actual[2]);
        Assert.Equal("Buzz", actual[4]);
        Assert.Equal("FizzBuzz", actual[14]);
        Assert.Equal(15, actual.Count);
    }

    [Fact]
    public void TwoSum_finds_pair()
    {
        var hit = InterviewAlgorithms.TwoSum([2, 7, 11, 15], 9);
        Assert.NotNull(hit);
        Assert.Equal((0, 1), hit);
    }

    [Fact]
    public void TwoSum_returns_null_when_none()
    {
        Assert.Null(InterviewAlgorithms.TwoSum([1, 2, 3], 100));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(10, 55)]
    public void Fibonacci_matches_known_values(int n, long expected)
    {
        Assert.Equal(expected, InterviewAlgorithms.Fibonacci(n));
    }

    [Fact]
    public void GroupAnagrams_clusters_words()
    {
        var groups = InterviewAlgorithms.GroupAnagrams(["eat", "tea", "tan", "ate", "nat", "bat"]);
        Assert.Equal(3, groups.Count);
        Assert.Equal(3, groups[Normalize("eat")].Count);
        Assert.Equal(2, groups[Normalize("tan")].Count);
        Assert.Single(groups[Normalize("bat")]);

        static string Normalize(string word)
        {
            var chars = word.ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }

    [Fact]
    public void TopPerDepartment_picks_highest_score()
    {
        var tops = InterviewAlgorithms.TopPerDepartment(
        [
            new("Ada", "Eng", 90),
            new("Grace", "Eng", 99),
            new("Alan", "Research", 95)
        ]);

        Assert.Equal(2, tops.Count);
        Assert.Equal("Grace", tops.Single(p => p.Department == "Eng").Name);
        Assert.Equal("Alan", tops.Single(p => p.Department == "Research").Name);
    }
}
