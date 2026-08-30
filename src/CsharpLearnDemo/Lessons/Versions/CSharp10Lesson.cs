using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 10 (2021) with .NET 6 LTS — less boilerplate in every file.
/// This file itself uses a file-scoped namespace (the line above).
/// </summary>
public sealed class CSharp10Lesson : ILesson
{
    public string Key => "csharp10";
    public string Title => "C# 10 (2021) — global usings and file-scoped namespaces";
    public string Category => "Versions";
    public string Summary => "Why global usings, file-scoped namespaces, record structs, and interpolations improved.";

    public void Run()
    {
        LessonIo.Why(
            ".NET 6 was an LTS release aimed at 'C# for everyone' and minimal " +
            "APIs. The most hated ceremony in C# was the 15 using lines and the " +
            "namespace { } indent on every file. Global usings and file-scoped " +
            "namespaces remove that noise so the first line of a file is the type. " +
            "Record structs completed the C# 9 record story for value types. " +
            "Interpolated string handlers let logging libraries skip allocating " +
            "a string when the log level is off — a real cloud-cost feature.");

        LessonIo.Principle(
            "Project-wide defaults, local exceptions",
            "Put common usings in a GlobalUsings.cs or ImplicitUsings. Add extra " +
            "usings only in files that need an unusual namespace. This is the same " +
            "idea as EditorConfig: conventions live in one place.");

        LessonIo.Example("record struct + const interpolated string", () =>
        {
            var a = new Size(2, 3);
            var b = new Size(2, 3);
            LessonIo.Result("record struct equality", a == b);
            LessonIo.Result("const interpolation", VersionLabel);
        });

        LessonIo.Example("Extended property patterns", () =>
        {
            var order = new Order(new Customer("Ada"), 120m);
            if (order is { Buyer.Name: "Ada", Total: > 100m })
            {
                LessonIo.Result("pattern", "Ada spent over 100");
            }
        });

        LessonIo.Interview(
            "What is a file-scoped namespace and why use it?",
            "namespace Foo; applies to the whole file and saves one indent level. " +
            "The guideline is one namespace per file (usually matching the folder), " +
            "so the extra braces added nothing. Use the block form only when a file " +
            "must declare two namespaces — which you should almost never do.");
    }

    // const interpolated strings (C# 10): the pieces must themselves be const.
    private const string VersionName = "C#";
    private const string VersionNumber = "10";
    private const string VersionLabel = $"{VersionName} {VersionNumber}";

    private readonly record struct Size(int Width, int Height);

    private sealed record Customer(string Name);

    private sealed record Order(Customer Buyer, decimal Total);
}
