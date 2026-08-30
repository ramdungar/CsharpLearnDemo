namespace CsharpLearnDemo.Infrastructure;

/// <summary>
/// Shared console formatting. One printer for every lesson follows DRY:
/// change the look here, not in 20 files.
/// </summary>
public static class LessonIo
{
    public static void Heading(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 78));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 78));
    }

    public static void Subheading(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {title} ---");
    }

    /// <summary>Explains the business/engineering reason a feature exists.</summary>
    public static void Why(string text)
    {
        Console.WriteLine();
        Console.WriteLine("WHY THIS EXISTS");
        Console.WriteLine(Wrap(text, 78));
    }

    public static void Principle(string name, string text)
    {
        Console.WriteLine();
        Console.WriteLine($"PRACTICE / PRINCIPLE: {name}");
        Console.WriteLine(Wrap(text, 78));
    }

    public static void Interview(string question, string answer)
    {
        Console.WriteLine();
        Console.WriteLine("INTERVIEW");
        Console.WriteLine($"Q: {question}");
        Console.WriteLine($"A: {Wrap(answer, 76)}");
    }

    public static void Note(string text) => Console.WriteLine($"  note: {text}");

    public static void Result(string label, object? value) =>
        Console.WriteLine($"  {label} => {value}");

    /// <summary>
    /// Runs a live snippet and still prints a caption so the learner can
    /// match the console output to the source they are reading.
    /// </summary>
    public static void Example(string name, Action body)
    {
        Subheading(name);
        body();
    }

    public static string Wrap(string text, int width)
    {
        // Simple word wrap so long teaching paragraphs stay readable in a terminal.
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lines = new List<string>();
        var current = "";
        foreach (var word in words)
        {
            var next = string.IsNullOrEmpty(current) ? word : current + " " + word;
            if (next.Length > width)
            {
                lines.Add(current);
                current = word;
            }
            else
            {
                current = next;
            }
        }

        if (!string.IsNullOrEmpty(current))
        {
            lines.Add(current);
        }

        return string.Join(Environment.NewLine, lines);
    }
}
