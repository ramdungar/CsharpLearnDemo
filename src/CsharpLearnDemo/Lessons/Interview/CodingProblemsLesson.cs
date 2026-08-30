using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Interview;

/// <summary>
/// Walk through the algorithms in <see cref="InterviewAlgorithms"/> the way
/// you should at a whiteboard: restated problem, brute force, then the better idea.
/// </summary>
public sealed class CodingProblemsLesson : ILesson
{
    public string Key => "problems";
    public string Title => "Interview coding problems (worked)";
    public string Category => "Interview";
    public string Summary => "Reverse, palindrome, FizzBuzz, TwoSum, Fibonacci, anagrams, LINQ grouping.";

    public void Run()
    {
        LessonIo.Why(
            "Screens test whether you can communicate a solution, pick a data " +
            "structure, and handle edges — not whether you memorized LeetCode #1. " +
            "Always: restate, example, brute force complexity, better approach, " +
            "then code with tests in mind.");

        LessonIo.Principle(
            "Talk complexity out loud",
            "O(n) time / O(n) extra memory for TwoSum with a dictionary. " +
            "Interviewers hire the explanation as much as the code.");

        LessonIo.Example("Reverse + palindrome", () =>
        {
            LessonIo.Result("Reverse(CSharp)", InterviewAlgorithms.Reverse("CSharp"));
            LessonIo.Result("A man, a plan, a canal: Panama", InterviewAlgorithms.IsPalindrome("A man, a plan, a canal: Panama"));
            LessonIo.Result("csharp", InterviewAlgorithms.IsPalindrome("csharp"));
        });

        LessonIo.Example("FizzBuzz 15", () =>
        {
            LessonIo.Result("15", string.Join(",", InterviewAlgorithms.FizzBuzz(15)));
        });

        LessonIo.Example("TwoSum", () =>
        {
            var hit = InterviewAlgorithms.TwoSum([2, 7, 11, 15], 9);
            LessonIo.Result("2+7=9 indices", hit is null ? "none" : $"{hit.Value.i},{hit.Value.j}");
        });

        LessonIo.Example("Fibonacci", () =>
        {
            LessonIo.Result("Fib(10)", InterviewAlgorithms.Fibonacci(10));
        });

        LessonIo.Example("Group anagrams + top score per department", () =>
        {
            var groups = InterviewAlgorithms.GroupAnagrams(["eat", "tea", "tan", "ate", "nat", "bat"]);
            LessonIo.Result("anagram keys", string.Join(" | ", groups.Select(kv => $"{kv.Key}:{kv.Value.Count}")));

            var tops = InterviewAlgorithms.TopPerDepartment(
            [
                new("Ada", "Eng", 90),
                new("Grace", "Eng", 99),
                new("Alan", "Research", 95)
            ]);
            LessonIo.Result("tops", string.Join(", ", tops.Select(p => $"{p.Department}:{p.Name}")));
        });

        LessonIo.Interview(
            "How do you practice for a C# coding interview this week?",
            "Implement the methods in InterviewAlgorithms from scratch on paper, " +
            "then run `dotnet test`. Add a failing test for an edge (empty, overflow, " +
            "unicode) before you change the code. Be ready to rewrite TwoSum without " +
            "LINQ and FizzBuzz without a tuple switch if they want 'simple C#'.");
    }
}
