using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Principles;

/// <summary>
/// A few GoF / .NET-native patterns you will be asked about. Prefer the
/// ones the BCL already implements over rolling your own framework.
/// </summary>
public sealed class DesignPatternsLesson : ILesson
{
    public string Key => "patterns";
    public string Title => "Design patterns you will actually use";
    public string Category => "Principles";
    public string Summary => "Strategy, Observer, Factory, Decorator, Repository — and where the BCL already has them.";

    public void Run()
    {
        LessonIo.Why(
            "Patterns are named solutions to recurring design problems. They " +
            "exist so teams can say 'this is a Strategy' instead of redrawing " +
            "the same UML every sprint. In modern C#, many patterns are language " +
            "or library features: events are Observer, LINQ is Iterator + Strategy, " +
            "IEnumerable is Iterator, Dependency Injection replaces Service Locator, " +
            "and `using` is a constrained Dispose pattern.");

        LessonIo.Principle(
            "Prefer language features over pattern theater",
            "A lambda passed into List.Sort is a Strategy. A record with a " +
            "switch expression often beats a Visitor. Use a full class structure " +
            "when you have multiple implementations that will grow independently.");

        LessonIo.Example("Strategy — swap an algorithm", () =>
        {
            IPricing regular = new RegularPricing();
            IPricing sale = new PercentOffPricing(0.10m);
            LessonIo.Result("regular", regular.Price(100m));
            LessonIo.Result("sale", sale.Price(100m));
        });

        LessonIo.Example("Decorator — wrap without changing the core type", () =>
        {
            IPricing priced = new LoggingPricing(new PercentOffPricing(0.25m));
            LessonIo.Result("logged sale", priced.Price(80m));
        });

        LessonIo.Example("Simple factory + repository (persistence abstraction)", () =>
        {
            IClock clock = ClockFactory.Utc();
            INoteRepository notes = new InMemoryNotes();
            notes.Add(new Note("learn C#", clock.UtcNow));
            LessonIo.Result("stored", notes.All().Single().Title);
        });

        LessonIo.Interview(
            "Which patterns does the .NET BCL already give you?",
            "Observer: event / IObservable. Iterator: IEnumerable / yield. " +
            "Decorator: Stream wrappers (GZipStream, BufferedStream). " +
            "Adapter: HttpClient handlers. Strategy: IComparer, IEqualityComparer, " +
            "middleware. Dispose: IDisposable. Builder: UriBuilder, DbContext options. " +
            "Name the BCL type in an interview — it shows you ship, not just UML.");
    }

    private interface IPricing
    {
        decimal Price(decimal amount);
    }

    private sealed class RegularPricing : IPricing
    {
        public decimal Price(decimal amount) => amount;
    }

    private sealed class PercentOffPricing(decimal off) : IPricing
    {
        public decimal Price(decimal amount) => amount * (1 - off);
    }

    private sealed class LoggingPricing(IPricing inner) : IPricing
    {
        public decimal Price(decimal amount)
        {
            var result = inner.Price(amount);
            LessonIo.Note($"priced {amount} -> {result}");
            return result;
        }
    }

    private interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }

    private sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }

    private static class ClockFactory
    {
        public static IClock Utc() => new SystemClock();
    }

    private sealed record Note(string Title, DateTimeOffset CreatedUtc);

    private interface INoteRepository
    {
        void Add(Note note);
        IReadOnlyList<Note> All();
    }

    private sealed class InMemoryNotes : INoteRepository
    {
        private readonly List<Note> _items = [];
        public void Add(Note note) => _items.Add(note);
        public IReadOnlyList<Note> All() => _items;
    }
}
