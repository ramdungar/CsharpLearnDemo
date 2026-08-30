namespace CsharpLearnDemo.Infrastructure;

/// <summary>
/// One teachable unit. Every lesson implements this so the host can list
/// and run them the same way (Open/Closed principle: add a lesson, do not
/// rewrite the menu).
/// </summary>
public interface ILesson
{
    /// <summary>Short key used on the command line, e.g. "csharp3" or "solid".</summary>
    string Key { get; }

    /// <summary>Human title shown in the menu.</summary>
    string Title { get; }

    /// <summary>Group: Fundamentals, Versions, Principles, Interview.</summary>
    string Category { get; }

    /// <summary>One-line "what you will learn".</summary>
    string Summary { get; }

    /// <summary>Prints the explanation and runs live examples. Must not block on input.</summary>
    void Run();
}
