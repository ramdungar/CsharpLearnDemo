namespace CsharpLearnDemo;

/// <summary>
/// Classic Program.Main entry point (the C# 1–8 style).
/// C# 9 also allows "top-level statements" (no Program class) — see csharp9.
/// We keep Main here so beginners can see where the process actually starts.
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        // CourseApp owns the menu so Program stays a thin composition root
        // (a common production practice: Main should not contain business logic).
        return Infrastructure.CourseApp.Run(args);
    }
}
