using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 8.0 (2019) with .NET Core 3.0 — nullable reference types and modern slicing.
/// </summary>
public sealed class CSharp8Lesson : ILesson
{
    public string Key => "csharp8";
    public string Title => "C# 8.0 (2019) — nullable refs, ranges, async streams";
    public string Category => "Versions";
    public string Summary => "Why nullable reference types, IAsyncEnumerable, indices/ranges, and DIM arrived.";

    public void Run()
    {
        LessonIo.Why(
            "Tony Hoare called null references his billion-dollar mistake. " +
            "C# 8 added nullable reference types so the compiler tracks " +
            "'this string might be null' the way it already tracked int?. " +
            "Meanwhile, cloud APIs needed async streaming (IAsyncEnumerable) " +
            "instead of buffering a whole result set, and Span-based code needed " +
            "Python-like slicing (ranges). Default interface methods (from Java 8) " +
            "let Microsoft add members to widely implemented interfaces without " +
            "breaking every implementer. using declarations and switch expressions " +
            "continued the C# 6–7 'less ceremony' theme.");

        LessonIo.Principle(
            "Enable nullable and treat warnings as errors in new projects",
            "<Nullable>enable</Nullable> is now the default for new SDK templates. " +
            "Fix warnings; do not sprinkle ! (null-forgiving) to silence them.");

        LessonIo.Example("Nullable reference types and null-coalescing assignment", () =>
        {
            string? maybe = GetOptionalName(include: false);
            maybe ??= "anonymous"; // assign only if null (C# 8 ??=)
            LessonIo.Result("name", maybe);
            LessonIo.Result("length of required", Require(maybe).Length);
        });

        LessonIo.Example("Indices, ranges, and switch expressions", () =>
        {
            var letters = new[] { "a", "b", "c", "d", "e" };
            LessonIo.Result("last (^1)", letters[^1]);
            LessonIo.Result("slice [1..4]", string.Join(",", letters[1..4]));
            LessonIo.Result("switch expr", Mood(3));
        });

        LessonIo.Example("using declaration + default interface method", () =>
        {
            IClock clock = new UtcClock();
            LessonIo.Result("Stamp (DIM default)", clock.Stamp());
        });

        // Async stream demo (await foreach)
        ConsumeAsyncStream().GetAwaiter().GetResult();

        LessonIo.Interview(
            "What does a compiler warning 'CS8602 dereference of a possibly null' mean?",
            "You enabled nullable reference types and used a string? (or a " +
            "reference the compiler cannot prove is non-null) without checking. " +
            "Fix it with a real check, ??, or by changing the API to return string. " +
            "The ! operator tells the compiler 'I know better' — use it only when " +
            "you truly have a proof the analyzer cannot see.");
    }

    private static string? GetOptionalName(bool include) => include ? "Ada" : null;

    private static string Require(string? value) =>
        value ?? throw new ArgumentNullException(nameof(value));

    private static string Mood(int stars) =>
        stars switch
        {
            <= 1 => "sad",
            2 or 3 => "ok",
            >= 4 => "happy"
        };

    private static async Task ConsumeAsyncStream()
    {
        LessonIo.Subheading("await foreach (async streams)");
        await foreach (var n in CountAsync(3))
        {
            LessonIo.Result("tick", n);
        }
    }

    private static async IAsyncEnumerable<int> CountAsync(int times)
    {
        for (var i = 1; i <= times; i++)
        {
            await Task.Delay(20);
            yield return i;
        }
    }

    private interface IClock
    {
        DateTimeOffset Now { get; }

        // Default interface method (C# 8): existing implementers get this for free.
        string Stamp() => Now.ToString("O");
    }

    private sealed class UtcClock : IClock
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
