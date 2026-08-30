using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 7.0–7.3 (2017–2018) — tuples, pattern matching, and Span-era performance.
/// </summary>
public sealed class CSharp7Lesson : ILesson
{
    public string Key => "csharp7";
    public string Title => "C# 7.0–7.3 (2017–2018) — tuples and patterns";
    public string Category => "Versions";
    public string Summary => "Why tuples, pattern matching, local functions, discards, and ref/span landed.";

    public void Run()
    {
        LessonIo.Why(
            "Two different audiences pulled C# 7. Application developers wanted " +
            "to stop inventing tiny DTO classes and out-parameters for multiple " +
            "return values (tuples) and wanted F#-style matching on types and " +
            "shapes (is/switch patterns). Systems/cloud developers wanted zero-copy " +
            "slicing of buffers (ref returns, in parameters, readonly struct, Span<T> " +
            "in 7.2) because UTF-8 parsing and Kestrel needed to allocate less. " +
            "7.1 added async Main and default literals; 7.3 improved generic " +
            "constraints (Enum, Delegate, unmanaged).");

        LessonIo.Principle(
            "Prefer tuples for internal returns, records for public models",
            "(int code, string name) is perfect inside a method. A public API " +
            "usually deserves a named type so you can version it and document it.");

        LessonIo.Example("Tuples, deconstruction, discards", () =>
        {
            var (ok, value) = TryParseAge("42");
            LessonIo.Result("TryParseAge", $"{ok}, {value}");

            // Discard _: you must acknowledge the slot but you do not need it.
            var (first, _, last) = ("Ada", "King", "Lovelace");
            LessonIo.Result("deconstructed", $"{first} {last}");
        });

        LessonIo.Example("Pattern matching (is / switch) and throw expressions", () =>
        {
            object payload = 15;
            if (payload is int n && n > 10)
            {
                LessonIo.Result("is-pattern int > 10", n);
            }

            string kind = Classify(payload);
            LessonIo.Result("Classify", kind);
            LessonIo.Result("Ensure", EnsurePositive(3));
        });

        LessonIo.Example("out var, binary literals, digit separators", () =>
        {
            if (int.TryParse("1010", out var parsed))
            {
                LessonIo.Result("out var", parsed);
            }

            int flags = 0b_0001_0100; // readable bit masks
            int million = 1_000_000;
            LessonIo.Result("bits / million", $"{flags} / {million}");
        });

        LessonIo.Interview(
            "ValueTuple vs Tuple<T>?",
            "System.Tuple is a reference type (heap, Item1 names). ValueTuple " +
            "(C# 7) is a struct with optional element names, supports deconstruction, " +
            "and is what the language uses for (int, string). Names are mostly " +
            "a compile-time illusion — over the wire you should still use a DTO.");
    }

    private static (bool ok, int age) TryParseAge(string text) =>
        int.TryParse(text, out var n) ? (true, n) : (false, 0);

    private static string Classify(object value) =>
        value switch
        {
            int n when n < 0 => "negative int",
            int => "non-negative int",
            string s => $"string len {s.Length}",
            null => "null",
            _ => "something else"
        };

    private static int EnsurePositive(int n) =>
        n > 0 ? n : throw new ArgumentOutOfRangeException(nameof(n));
}
