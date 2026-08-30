using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 2.0 (2005) with .NET Framework 2.0 — generics and the end of ArrayList pain.
/// </summary>
public sealed class CSharp2Lesson : ILesson
{
    public string Key => "csharp2";
    public string Title => "C# 2.0 (2005) — generics, nullable, iterators";
    public string Category => "Versions";
    public string Summary => "Why generics, Nullable<T>, yield return, anonymous methods, and partial types.";

    public void Run()
    {
        LessonIo.Why(
            "C# 1 collections stored object. Every int in an ArrayList was boxed; " +
            "every read needed a cast that could blow up at runtime. Java 5 and " +
            "the CLR team solved the same problem: parametric polymorphism " +
            "(generics) with reification on the CLR (List<int> is a real type at " +
            "runtime, unlike Java erasure). Databases and WinForms also needed " +
            "'int that can be NULL' — Nullable<T>. Iterators (yield) were added " +
            "because writing IEnumerator by hand was a state-machine nightmare. " +
            "Anonymous methods were the first step toward lambdas. Partial types " +
            "let WinForms designers generate one file while you edit another.");

        LessonIo.Principle(
            "Type safety over casts",
            "If the compiler can enforce the element type, do not use non-generic " +
            "collections. Boxing in hot paths is a perf bug, not a style issue.");

        LessonIo.Example("Generics: one algorithm, many types, zero boxing for int", () =>
        {
            var numbers = new List<int> { 1, 2, 3 };
            numbers.Add(4);
            LessonIo.Result("List<int>", string.Join(",", numbers));
            LessonIo.Result("Max", Max(numbers));
            LessonIo.Result("Max strings", Max(new[] { "aa", "z", "bbb" }));
        });

        LessonIo.Example("Nullable value types — T?", () =>
        {
            int? maybe = null;           // Nullable<int>
            int? also = 7;
            LessonIo.Result("HasValue on null", maybe.HasValue);
            LessonIo.Result("GetValueOrDefault", maybe.GetValueOrDefault(-1));
            LessonIo.Result("also + 1 (lifted operators)", also + 1);
        });

        LessonIo.Example("Iterators: yield return builds a state machine", () =>
        {
            // Deferred execution: nothing runs until someone foreach-es.
            var evens = TakeEven(new[] { 1, 2, 3, 4, 5, 6 });
            LessonIo.Result("evens", string.Join(",", evens));
        });

        LessonIo.Example("Anonymous method (precursor to lambdas)", () =>
        {
            // C# 2: delegate (int x) { return x * x; }
            // C# 3: x => x * x
            Converter<int, int> square = delegate (int x) { return x * x; };
            LessonIo.Result("anonymous method 5^2", square(5));
        });

        LessonIo.Interview(
            "Are C# generics the same as Java generics?",
            "No. CLR generics are reified: List<int> and List<string> are different " +
            "runtime types, and List<int> stores unboxed ints. Java erases generics " +
            "to object (plus bridges). That is why C# can do where T : struct, " +
            "new(), and why you can reflect on T at runtime.");
    }

    // Generic method with a constraint. Constraints exist so the compiler
    // knows which operations are legal on T — no guessing, no boxing.
    private static T Max<T>(IEnumerable<T> items) where T : IComparable<T>
    {
        using var e = items.GetEnumerator();
        if (!e.MoveNext())
        {
            throw new InvalidOperationException("Empty sequence.");
        }

        var best = e.Current;
        while (e.MoveNext())
        {
            if (e.Current.CompareTo(best) > 0)
            {
                best = e.Current;
            }
        }

        return best;
    }

    private static IEnumerable<int> TakeEven(IEnumerable<int> source)
    {
        foreach (var n in source)
        {
            if (n % 2 == 0)
            {
                yield return n; // pause here; resume on the next MoveNext()
            }
        }
    }
}
