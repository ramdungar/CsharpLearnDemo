using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 1.0 (2002) with .NET Framework 1.0 — the original language.
/// C# 1.2 (2003) was a small refresh with .NET 1.1.
/// </summary>
public sealed class CSharp1Lesson : ILesson
{
    public string Key => "csharp1";
    public string Title => "C# 1.0 / 1.2 (2002–2003) — the foundation";
    public string Category => "Versions";
    public string Summary => "Why C# was created: classes, properties, events, delegates, structs, enums.";

    public void Run()
    {
        LessonIo.Why(
            "Microsoft needed a language purpose-built for the new .NET CLR: " +
            "garbage collected, type-safe, and easy for C++/Java/VB developers. " +
            "Java had no properties or events (awkward for WinForms designers). " +
            "C++ had no GC and was unsafe by default. VB6 could not scale. " +
            "C# 1.0 therefore shipped component-oriented OOP: properties for " +
            "designers, events + delegates for callbacks, structs for lightweight " +
            "values, and attributes for metadata (the basis of later ASP.NET/WCF).");

        LessonIo.Principle(
            ".NET Framework Design Guidelines (already in 1.0)",
            "PascalCase for public members, interfaces start with I, one class " +
            "per concept, exceptions not error codes, and properties instead of " +
            "GetX/SetX methods unless there is real work or a failure.");

        LessonIo.Example("Properties instead of Java-style getters", () =>
        {
            var account = new BankAccount("Ada", 100m);
            account.Deposit(25m);
            LessonIo.Result("owner", account.Owner);
            LessonIo.Result("balance", account.Balance);
        });

        LessonIo.Example("Delegates and events — the observer pattern in the language", () =>
        {
            // A delegate is a type-safe function pointer (multicast).
            // Events wrap delegates so outsiders can only += / -=, not invoke
            // or overwrite the list (encapsulation of the observer list).
            var button = new FakeButton();
            button.Clicked += (_, e) => LessonIo.Result("clicked", e.When);
            button.Click();
        });

        LessonIo.Example("struct vs class, enum, and attributes", () =>
        {
            var point = new Point(3, 4);
            LessonIo.Result("Point struct", $"{point.X},{point.Y}");
            LessonIo.Result("enum", Status.Active);
            LessonIo.Note("See [Flags] and [Obsolete] in the source — metadata the CLR and tools read.");
        });

        LessonIo.Interview(
            "What did C# 1.0 add that Java 1.4 did not have?",
            "First-class properties, events, delegates, value types (struct), " +
            "decimal, foreach over IEnumerable, using/IDisposable, and attributes. " +
            "Those choices are why WinForms/WPF data-binding and later ASP.NET " +
            "model binding feel native.");
    }

    private sealed class BankAccount
    {
        // Backing field is private — encapsulation from day one.
        private decimal _balance;

        public BankAccount(string owner, decimal opening)
        {
            Owner = owner;
            _balance = opening;
        }

        public string Owner { get; } // later syntax; in 1.0 this was get { return owner; }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            _balance += amount;
        }
    }

    private sealed class FakeButton
    {
        public event EventHandler<ClickEventArgs>? Clicked;

        public void Click() =>
            Clicked?.Invoke(this, new ClickEventArgs(DateTimeOffset.UtcNow));
    }

    private sealed class ClickEventArgs : EventArgs
    {
        public ClickEventArgs(DateTimeOffset when) => When = when;
        public DateTimeOffset When { get; }
    }

    private readonly struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    private enum Status
    {
        Draft = 0,
        Active = 1,
        Closed = 2
    }
}
