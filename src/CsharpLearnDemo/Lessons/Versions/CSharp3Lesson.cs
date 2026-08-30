using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 3.0 (2007) with .NET Framework 3.5 — LINQ and the functional turn.
/// This is the version that permanently changed how C# is written.
/// </summary>
public sealed class CSharp3Lesson : ILesson
{
    public string Key => "csharp3";
    public string Title => "C# 3.0 (2007) — LINQ, lambdas, var";
    public string Category => "Versions";
    public string Summary => "Why LINQ needed lambdas, extension methods, anonymous types, and expression trees.";

    public void Run()
    {
        LessonIo.Why(
            "Data lived in objects, XML, and SQL Server, and each needed a " +
            "different clumsy API. Anders Hejlsberg’s bet: if the language can " +
            "express queries, the same mental model works everywhere (LINQ to " +
            "Objects, XML, SQL). Query syntax required several features at once: " +
            "lambda expressions (predicates), extension methods (Where/Select on " +
            "any IEnumerable), anonymous types (shape a projection without a DTO), " +
            "var (because anonymous types have no name), object/collection " +
            "initializers, auto-properties, and expression trees (so LINQ to SQL " +
            "can turn a lambda into a SQL statement instead of running it in-process).");

        LessonIo.Principle(
            "Declarative over imperative (when the query is the point)",
            "Say what you want (filter/project/group), not how to loop. " +
            "But remember deferred execution: the query runs when you enumerate, " +
            "not when you build it. That is the #1 LINQ interview question.");

        var people = new[]
        {
            new Person("Ada", "Lovelace", 36),
            new Person("Grace", "Hopper", 85),
            new Person("Alan", "Turing", 41),
            new Person("Linus", "Torvalds", 54)
        };

        LessonIo.Example("Extension methods + lambdas + var", () =>
        {
            // Where/Select are extension methods on IEnumerable<T> in System.Linq.
            var names = people
                .Where(p => p.Age < 50)
                .Select(p => p.First)
                .ToList(); // ToList() forces execution now

            LessonIo.Result("under 50", string.Join(", ", names));
            LessonIo.Result("word count via our extension", "hello world".WordCount());
        });

        LessonIo.Example("Query syntax is sugar for the same methods", () =>
        {
            var query =
                from p in people
                where p.Age >= 40
                orderby p.Age
                select new { p.First, p.Age }; // anonymous type

            foreach (var row in query)
            {
                LessonIo.Result("row", $"{row.First} ({row.Age})");
            }
        });

        LessonIo.Example("Deferred execution trap", () =>
        {
            var multiplier = 1;
            var q = people.Select(p => p.Age * multiplier);
            multiplier = 10; // captured variable — seen at execution time
            LessonIo.Result("ages * 10 because execution is deferred", string.Join(",", q));
        });

        LessonIo.Interview(
            "IEnumerable vs IQueryable?",
            "IEnumerable LINQ runs in memory with delegates. IQueryable builds an " +
            "expression tree that a provider (EF Core) can translate to SQL. " +
            "Calling .ToList() too early pulls the whole table. Calling a C# method " +
            "inside an IQueryable lambda often cannot translate and throws or " +
            "evaluates client-side.");
    }

    private sealed class Person
    {
        // Auto-property (C# 3): compiler generates the backing field.
        public string First { get; }
        public string Last { get; }
        public int Age { get; }

        public Person(string first, string last, int age)
        {
            First = first;
            Last = last;
            Age = age;
        }
    }
}

/// <summary>
/// Extension methods must live in a static class. They are static methods
/// with a 'this' modifier on the first parameter — syntactic sugar, not
/// a change to the target type (you cannot add fields this way).
/// </summary>
internal static class StringExtensions
{
    public static int WordCount(this string text) =>
        string.IsNullOrWhiteSpace(text)
            ? 0
            : text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}
