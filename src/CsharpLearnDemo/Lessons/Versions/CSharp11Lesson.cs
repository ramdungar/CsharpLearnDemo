using CsharpLearnDemo.Infrastructure;
using System.Numerics;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 11 (2022) with .NET 7 — raw strings, required members, generic math.
/// </summary>
public sealed class CSharp11Lesson : ILesson
{
    public string Key => "csharp11";
    public string Title => "C# 11 (2022) — raw strings, required, generic math";
    public string Category => "Versions";
    public string Summary => "Why raw string literals, required members, list patterns, and INumber<T> arrived.";

    public void Run()
    {
        LessonIo.Why(
            "Three themes. (1) People paste JSON, SQL, and HTML into C# strings " +
            "and drown in escapes — raw string literals (\"\"\"...\"\"\") fix that. " +
            "(2) Object initializers were popular but easy to forget a property; " +
            "required members make the compiler enforce initialization. " +
            "(3) .NET wanted one math library for int/double/generic tensors " +
            "(ML, games, finance) — generic math via static abstract interface " +
            "members (INumber<T>). List patterns extend pattern matching to sequences.");

        LessonIo.Principle(
            "required over optional constructors that lie",
            "If a property must be set for the object to be valid, mark it required " +
            "or put it in the constructor. Do not ship a parameterless constructor " +
            "plus 'remember to set X' comments.");

        LessonIo.Example("Raw string literals and UTF-8 strings", () =>
        {
            var json = """
                {
                  "name": "Ada",
                  "ok": true
                }
                """;
            LessonIo.Result("raw JSON first line", json.Split('\n')[0].Trim());

            ReadOnlySpan<byte> utf8 = "hi"u8; // UTF-8 string literal
            LessonIo.Result("u8 byte count", utf8.Length);
        });

        LessonIo.Example("required members + list patterns + generic math", () =>
        {
            var user = new User { Email = "ada@example.com", Name = "Ada" };
            LessonIo.Result("user", $"{user.Name} <{user.Email}>");

            int[] data = { 1, 2, 3, 4 };
            LessonIo.Result("list pattern", Describe(data));
            LessonIo.Result("generic sum int", Sum(1, 2, 3));
            LessonIo.Result("generic sum double", Sum(1.5, 2.5));
        });

        LessonIo.Interview(
            "What is a static abstract member on an interface?",
            "C# 11 lets interfaces declare static abstract operators/methods " +
            "(e.g. T.Zero, T + T). Generic math uses this so Sum<T> works for " +
            "any INumber<T>. Implementers must provide those static members. " +
            "This is how .NET avoided a copy-pasted Math class per numeric type.");
    }

    private static string Describe(int[] xs) =>
        xs switch
        {
            [] => "empty",
            [var only] => $"single {only}",
            [1, 2, .. var rest] => $"starts 1,2 then {rest.Length} more",
            _ => "other"
        };

    private static T Sum<T>(params T[] values) where T : INumber<T>
    {
        var total = T.Zero;
        foreach (var v in values)
        {
            total += v;
        }

        return total;
    }

    private sealed class User
    {
        public required string Email { get; init; }
        public required string Name { get; init; }
    }
}
