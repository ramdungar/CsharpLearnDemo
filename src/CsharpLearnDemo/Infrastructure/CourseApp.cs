namespace CsharpLearnDemo.Infrastructure;

/// <summary>
/// Entry-point logic extracted from Program so tests can drive the course
/// without starting a real interactive console (Separation of Concerns).
///
/// Usage:
///   dotnet run                  interactive menu
///   dotnet run -- list          print the catalog
///   dotnet run -- all           run every lesson
///   dotnet run -- csharp3       run one lesson by key
///   dotnet run -- versions      run a whole category
/// </summary>
public static class CourseApp
{
    public static int Run(string[] args)
    {
        var catalog = LessonCatalog.All();

        if (args.Length == 0)
        {
            return RunInteractive(catalog);
        }

        var command = args[0].Trim().ToLowerInvariant();
        return command switch
        {
            "list" or "--list" or "-l" => List(catalog),
            "all" or "--all" => RunMany(catalog),
            "help" or "--help" or "-h" => Help(),
            _ => RunByKeyOrCategory(catalog, command)
        };
    }

    private static int Help()
    {
        Console.WriteLine("CsharpLearnDemo — start-to-end C# course");
        Console.WriteLine();
        Console.WriteLine("  dotnet run                  Interactive numbered menu");
        Console.WriteLine("  dotnet run -- list          Show every lesson key");
        Console.WriteLine("  dotnet run -- all           Run every lesson");
        Console.WriteLine("  dotnet run -- <key>         Run one lesson (e.g. csharp3, solid)");
        Console.WriteLine("  dotnet run -- <category>    fundamentals | versions | principles | interview");
        return 0;
    }

    private static int List(IReadOnlyList<ILesson> catalog)
    {
        string? current = null;
        foreach (var lesson in catalog)
        {
            if (lesson.Category != current)
            {
                current = lesson.Category;
                Console.WriteLine();
                Console.WriteLine(current.ToUpperInvariant());
            }

            Console.WriteLine($"  {lesson.Key,-16} {lesson.Title}");
            Console.WriteLine($"                   {lesson.Summary}");
        }

        return 0;
    }

    private static int RunByKeyOrCategory(IReadOnlyList<ILesson> catalog, string token)
    {
        var matches = catalog
            .Where(l =>
                l.Key.Equals(token, StringComparison.OrdinalIgnoreCase) ||
                l.Category.Equals(token, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
        {
            Console.Error.WriteLine($"Unknown lesson or category '{token}'. Try: dotnet run -- list");
            return 1;
        }

        return RunMany(matches);
    }

    private static int RunMany(IEnumerable<ILesson> lessons)
    {
        foreach (var lesson in lessons)
        {
            LessonIo.Heading($"{lesson.Category} / {lesson.Title}  [{lesson.Key}]");
            Console.WriteLine(lesson.Summary);
            lesson.Run();
        }

        return 0;
    }

    private static int RunInteractive(IReadOnlyList<ILesson> catalog)
    {
        Help();
        List(catalog);
        Console.WriteLine();
        Console.WriteLine("Type a lesson key, a category, 'all', or 'q' to quit.");

        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line is null)
            {
                return 0;
            }

            var token = line.Trim().ToLowerInvariant();
            if (token is "q" or "quit" or "exit")
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (token is "list")
            {
                List(catalog);
                continue;
            }

            if (token is "all")
            {
                RunMany(catalog);
                continue;
            }

            RunByKeyOrCategory(catalog, token);
        }
    }
}
