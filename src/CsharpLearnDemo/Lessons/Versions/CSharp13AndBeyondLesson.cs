using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 13 (.NET 9, 2024) and a look at C# 14 (.NET 10).
/// This project targets .NET 8 / C# 12, so some 13+ features are shown as
/// documented examples rather than compiled syntax. That is itself a lesson:
/// language version is tied to the SDK / LangVersion you chose.
/// </summary>
public sealed class CSharp13AndBeyondLesson : ILesson
{
    public string Key => "csharp13";
    public string Title => "C# 13–14 (2024–) — finishing collections and extensions";
    public string Category => "Versions";
    public string Summary => "Why params collections, Lock, partial properties, and extension members exist.";

    public void Run()
    {
        LessonIo.Why(
            "C# 12 left a few sharp edges. params still meant 'array only', so " +
            "high-perf APIs could not take params ReadOnlySpan<T> without a hidden " +
            "allocation. lock (object) allocated a boxed monitor for some scenarios; " +
            "System.Threading.Lock is a dedicated type. Teams wanted partial " +
            "properties (source generators already had partial methods). C# 14 " +
            "continues the same story: extension members make LINQ-style APIs feel " +
            "like they belong on the type, and the field keyword reduces boilerplate " +
            "in properties. Each of these exists because real .NET 8/9 code reviews " +
            "kept hitting the same papercuts — not because the language was incomplete " +
            "in a theoretical sense.");

        LessonIo.Principle(
            "Adopt a language version as a team",
            "Set <LangVersion> and <TargetFramework> in Directory.Build.props. " +
            "Do not use preview features in a library your customers compile against " +
            "unless you control their SDK. Features that only change syntax (collection " +
            "expressions) are safer to adopt than features that need a newer runtime " +
            "(Lock, new BCL types).");

        LessonIo.Example("params today (array) — C# 13 allows params on collections/spans", () =>
        {
            // C# 1–12: params always creates an array (allocation).
            LessonIo.Result("JoinWords", JoinWords("one", "two", "three"));
            LessonIo.Note("On .NET 9 / C# 13 you can write: void M(params ReadOnlySpan<string> words)");
            LessonIo.Note("or params IEnumerable<T> / List<T> — no forced array allocation.");
        });

        LessonIo.Example("Locking: classic monitor vs System.Threading.Lock (C# 13)", () =>
        {
            var counter = new ThreadSafeCounter();
            Parallel.For(0, 200, _ => counter.Increment());
            LessonIo.Result("safe count", counter.Value);
            LessonIo.Note("C# 13: lock (myLock) where myLock is System.Threading.Lock uses Lock.EnterScope().");
        });

        LessonIo.Example("Other C# 13 headlines (syntax requires .NET 9)", () =>
        {
            LessonIo.Note(@"Escape \e for ESC (U+001B) — useful for ANSI terminals.");
            LessonIo.Note("Implicit index from end in initializers: new Buffer { [^1] = 42 }.");
            LessonIo.Note("ref / unsafe locals are allowed inside async and iterator methods.");
            LessonIo.Note("partial properties and indexers — source generators can fill them in.");
            LessonIo.Note("[OverloadResolutionPriority] lets APIs prefer a Span overload over T[].");
        });

        LessonIo.Example("C# 14 direction (preview on .NET 10)", () =>
        {
            LessonIo.Note("Extension members: extend a type with properties/operators, not only methods.");
            LessonIo.Note("field keyword: public int Age { get; set => field = value < 0 ? 0 : value; }");
            LessonIo.Note("Null-conditional assignment: customer?.Order = new Order();");
            LessonIo.Note("nameof(List<>) works on unbound generics.");
        });

        LessonIo.Interview(
            "How do you pick a C# / .NET version for a new project in 2026?",
            "Default to the current LTS (for this course's SDK that is .NET 8; " +
            "newer machines may use .NET 10 LTS). Use the matching C# version " +
            "(LangVersion latest). Bump to STS (.NET 9) only if you need a runtime " +
            "API (Lock, new ASP.NET features) and you accept a shorter support window. " +
            "Libraries should target the lowest TFM your users need and use multi-targeting " +
            "plus #if only when a feature truly requires a newer runtime.");
    }

    private static string JoinWords(params string[] words) => string.Join('-', words);

    /// <summary>
    /// Classic monitor lock. Still correct and required knowledge.
    /// </summary>
    private sealed class ThreadSafeCounter
    {
        private readonly object _gate = new();
        private int _value;

        public int Value
        {
            get
            {
                lock (_gate)
                {
                    return _value;
                }
            }
        }

        public void Increment()
        {
            lock (_gate)
            {
                _value++;
            }
        }
    }
}
