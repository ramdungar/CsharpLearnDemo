using CsharpLearnDemo.Infrastructure;

// C# 12: alias any type — not only namespaces. Handy for verbose generics.
using Number = int;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 12 (2023) with .NET 8 LTS — primary constructors and collection expressions.
/// This is the language version this project compiles with.
/// </summary>
public sealed class CSharp12Lesson : ILesson
{
    public string Key => "csharp12";
    public string Title => "C# 12 (2023) — primary constructors, collection expressions";
    public string Category => "Versions";
    public string Summary => "Why primary constructors, collection expressions, and inline arrays were added.";

    public void Run()
    {
        LessonIo.Why(
            "DI-heavy ASP.NET apps repeat the same constructor: accept " +
            "dependencies, assign to readonly fields, 15 lines of zero logic. " +
            "Primary constructors (already on records since C# 9) now work on " +
            "classes and structs. Collection expressions ([1, 2, 3]) unify array, " +
            "list, span, and spread syntax so you stop memorizing new[] / new List. " +
            "Inline arrays help interop and high-perf buffers without unsafe code. " +
            "Optional lambda parameters and 'using alias = any type' finish small " +
            "papercuts reported after C# 11.");

        LessonIo.Principle(
            "Primary constructors are for capture, not for hiding invariants",
            "If you need validation, keep an explicit constructor body (or a " +
            "factory). Dumping ten dependencies into a primary constructor can " +
            "also be a smell — the class may be doing too much (SRP).");

        LessonIo.Example("Primary constructor on a class", () =>
        {
            var greeter = new Greeter("Hello");
            LessonIo.Result("greet", greeter.Greet("world"));
        });

        LessonIo.Example("Collection expressions and spread", () =>
        {
            // One syntax for arrays, lists, spans. The target type decides.
            int[] row = [1, 2, 3];
            List<int> grown = [.. row, 4, 5];
            ReadOnlySpan<int> slice = [10, 20, 30];
            LessonIo.Result("array", string.Join(",", row));
            LessonIo.Result("list spread", string.Join(",", grown));
            LessonIo.Result("span[1]", slice[1]);
        });

        LessonIo.Example("Default lambda parameters + alias any type", () =>
        {
            var increment = (int x, int by = 1) => x + by;
            LessonIo.Result("lambda default", increment(10));
            LessonIo.Result("lambda by 5", increment(10, 5));

            Number id = 42;
            LessonIo.Result("int alias demo", id);
        });

        LessonIo.Interview(
            "Collection expressions vs new List<T> { }?",
            "Collection expressions are target-typed and can create arrays, " +
            "List<T>, Span<T>, and user types with a collection builder. The spread " +
            "..xs copies elements. They avoid the 'new List<int> { 1, 2 }' vs " +
            "'new[] { 1, 2 }' inconsistency. Prefer them in C# 12+ codebases.");
    }

    // Primary constructor: 'prefix' is in scope for the whole type.
    // The compiler generates a stored field if you use the parameter in members.
    private sealed class Greeter(string prefix)
    {
        public string Greet(string name) => $"{prefix}, {name}";
    }
}
