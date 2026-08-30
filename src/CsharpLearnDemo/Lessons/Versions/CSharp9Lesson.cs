using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 9.0 (2020) with .NET 5 — records, init, and "minimal" programs.
/// .NET 5 unified Framework + Core into one platform; the language followed
/// with features for immutable data and small APIs.
/// </summary>
public sealed class CSharp9Lesson : ILesson
{
    public string Key => "csharp9";
    public string Title => "C# 9.0 (2020) — records and top-level programs";
    public string Category => "Versions";
    public string Summary => "Why records, init-only setters, target-typed new, and relational patterns.";

    public void Run()
    {
        LessonIo.Why(
            ".NET 5 was the first 'one .NET'. Microservices and Minimal APIs " +
            "wanted less boilerplate (top-level statements, target-typed new). " +
            "DDD and functional teams wanted immutable data with value equality " +
            "without writing Equals/GetHashCode/clone by hand — records + with. " +
            "init-only properties let you use object initializers without making " +
            "setters public forever. Pattern matching grew relational and logical " +
            "patterns so validation reads like a spec.");

        LessonIo.Principle(
            "Records for data, classes for behavior",
            "A record is a great DTO, event, or value-like model. A class with " +
            "identity (an Entity with an Id) should usually stay a class — two " +
            "customers with the same name are not the same customer.");

        LessonIo.Example("record, with-expression, value equality", () =>
        {
            var ada = new Person("Ada", "Lovelace");
            var ada2 = new Person("Ada", "Lovelace");
            var grace = ada with { First = "Grace" }; // nondestructive mutation

            LessonIo.Result("ada == ada2 (value equality)", ada == ada2);
            LessonIo.Result("with copy", grace);
            LessonIo.Result("ToString", ada.ToString());
        });

        LessonIo.Example("init-only + target-typed new + relational patterns", () =>
        {
            Point p = new() { X = 3, Y = 4 }; // target-typed new (C# 9)
            LessonIo.Result("point", $"{p.X},{p.Y}");
            LessonIo.Result("tax band", TaxBand(48_000m));
        });

        LessonIo.Interview(
            "record vs class vs struct vs record struct?",
            "class: reference type, identity equality by default. " +
            "record class: reference type, value equality + with + Deconstruct. " +
            "struct: value type, no identity, beware mutation and copies. " +
            "record struct (C# 10): value type with value equality. " +
            "Choose record when the type IS its data; class when it DOES things " +
            "or has a persistent identity.");
    }

    private sealed record Person(string First, string Last);

    private sealed class Point
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    private static string TaxBand(decimal income) =>
        income switch
        {
            < 12_000m => "none",
            < 50_000m => "basic",
            < 125_000m => "higher",
            _ => "additional"
        };
}
