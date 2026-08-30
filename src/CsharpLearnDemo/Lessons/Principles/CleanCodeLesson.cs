using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Principles;

/// <summary>
/// Everyday standards used in professional C# teams: naming, DRY/KISS/YAGNI,
/// errors, async, and Microsoft's Framework Design Guidelines.
/// </summary>
public sealed class CleanCodeLesson : ILesson
{
    public string Key => "cleancode";
    public string Title => "Clean code, standards, and practices";
    public string Category => "Principles";
    public string Summary => "DRY, KISS, YAGNI, naming, exceptions, async rules, and .editorconfig habits.";

    public void Run()
    {
        LessonIo.Why(
            "A language feature is useless if the team cannot read last year's " +
            "code. Microsoft published Framework Design Guidelines so the BCL " +
            "itself is consistent; EditorConfig and analyzers (CA/IDE/Roslyn) " +
            "enforce the same rules in your repo. These practices exist because " +
            "code is read far more often than it is written.");

        LessonIo.Principle("DRY — Don't Repeat Yourself",
            "Duplication of knowledge (the same business rule in two methods) is " +
            "the problem — not every similar-looking loop. Extract when a rule " +
            "must stay in sync.");

        LessonIo.Principle("KISS — Keep It Simple",
            "The simplest design that passes the tests and matches the domain. " +
            "Do not introduce a mediator/factory/generic wizardry for one call site.");

        LessonIo.Principle("YAGNI — You Aren't Gonna Need It",
            "Do not add an IAbstractWidgetFactory because a future tenant 'might' " +
            "need it. Branch when you have the second real case.");

        LessonIo.Example("Naming — Framework Design Guidelines", () =>
        {
            // Types and public members: PascalCase
            // Parameters and locals: camelCase
            // Interfaces: I + adjective/noun (IDisposable, IUserRepository)
            // Async methods: suffix Async
            // Booleans: Is/Has/Can
            var cart = new ShoppingCart();
            cart.Add(new Sku("BOOK-1"), quantity: 2);
            LessonIo.Result("total items", cart.ItemCount);
            LessonIo.Result("HasItems", cart.HasItems);
        });

        LessonIo.Example("Guard clauses — fail fast, keep the happy path unindented", () =>
        {
            try
            {
                PrintLabel(null!);
            }
            catch (ArgumentException ex)
            {
                LessonIo.Result("guard", ex.Message);
            }
        });

        LessonIo.Example("Async practice: return the Task, do not block", () =>
        {
            var text = ReadMottoAsync().GetAwaiter().GetResult();
            LessonIo.Result("motto", text);
            LessonIo.Note("In real apps the caller awaits. Avoid async void except event handlers.");
            LessonIo.Note("Do not mix sync-over-async (.Result) on ASP.NET — deadlock risk.");
        });

        LessonIo.Interview(
            "What coding standards would you set up on a new C# repo?",
            "SDK-style project, Nullable enable, TreatWarningsAsErrors on CI, " +
            "EditorConfig (indent, usings, naming), .NET analyzers + StyleCop or " +
            "Meziantou as a team choice, xUnit/NUnit + coverlet, dependabot, " +
            "and a short CONTRIBUTING.md. Format with `dotnet format`. Follow " +
            "Microsoft naming and the IDisposable/async guidelines already in this lesson.");
    }

    private static void PrintLabel(string? sku)
    {
        // Guard clause: reject bad input immediately (Design by Contract-ish).
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        LessonIo.Result("label", sku.ToUpperInvariant());
    }

    private static Task<string> ReadMottoAsync() =>
        Task.FromResult("Make the change easy, then make the easy change.");

    private readonly record struct Sku(string Value);

    private sealed class ShoppingCart
    {
        private readonly Dictionary<string, int> _lines = new();

        public int ItemCount => _lines.Values.Sum();
        public bool HasItems => ItemCount > 0;

        public void Add(Sku sku, int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            _lines[sku.Value] = _lines.GetValueOrDefault(sku.Value) + quantity;
        }
    }
}
