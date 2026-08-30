using CsharpLearnDemo.Infrastructure;
using static System.Math; // C# 6: using static — import static members

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 6.0 (2015) with Roslyn — lots of small features because the compiler
/// became a platform (open source, APIs for analyzers).
/// </summary>
public sealed class CSharp6Lesson : ILesson
{
    public string Key => "csharp6";
    public string Title => "C# 6.0 (2015) — Roslyn productivity";
    public string Category => "Versions";
    public string Summary => "Why interpolation, nameof, null-conditional, and expression-bodied members appeared.";

    public void Run()
    {
        LessonIo.Why(
            "The compiler was rewritten as Roslyn (open source, in C#). Once " +
            "the compiler is a library, shipping many small syntax features became " +
            "cheap. Teams asked for less ceremony: string.Format is noisy, " +
            "null-check chains are noisy, rename-safe 'nameof' beats magic strings, " +
            "and one-line properties do not need braces. None of these change the " +
            "type system — they exist to make everyday code shorter and safer to refactor.");

        LessonIo.Principle(
            "String interpolation is not SQL parameterization",
            "$\"...{value}\" is great for messages. Never build SQL or script this " +
            "way — that is injection. Use parameters (EF, Dapper, SqlParameter).");

        LessonIo.Example("Interpolation, nameof, null-conditional, null-coalescing", () =>
        {
            Person? person = new Person("Grace");
            LessonIo.Result("interpolation", $"{person.Name} is {person.Name.Length} chars");
            LessonIo.Result("nameof", nameof(Person.Name)); // stays correct after rename

            person = null;
            // ?. short-circuits to null instead of throwing NullReferenceException
            LessonIo.Result("person?.Name", person?.Name ?? "(no person)");
            LessonIo.Result("Sqrt via using static", Sqrt(16));
        });

        LessonIo.Example("Auto-property initializers and exception filters", () =>
        {
            var config = new AppOptions();
            LessonIo.Result("initialized property", config.Timeout);

            try
            {
                throw new InvalidOperationException("transient-demo");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("transient"))
            {
                LessonIo.Note("filter matched — we logged without a nested if inside catch.");
            }
        });

        LessonIo.Interview(
            "Why prefer nameof over a string?",
            "Refactor/rename updates nameof. A raw string becomes a silent runtime " +
            "bug (INotifyPropertyChanged, ArgumentException param names, logging). " +
            "nameof is evaluated at compile time and has zero runtime cost.");
    }

    private sealed class Person
    {
        public Person(string name) => Name = name;
        public string Name { get; }
    }

    private sealed class AppOptions
    {
        // Auto-property initializer (C# 6) — no constructor required for defaults.
        public TimeSpan Timeout { get; } = TimeSpan.FromSeconds(30);
    }
}
