using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Principles;

/// <summary>
/// SOLID — five design principles named by Robert C. Martin, widely used
/// in C# interviews and code reviews. They are guidelines, not laws.
/// </summary>
public sealed class SolidLesson : ILesson
{
    public string Key => "solid";
    public string Title => "SOLID principles";
    public string Category => "Principles";
    public string Summary => "SRP, OCP, LSP, ISP, DIP with a bad example and a better C# example for each.";

    public void Run()
    {
        LessonIo.Why(
            "Large C# codebases rot when every change touches the same god class, " +
            "when new features require editing switch statements in ten files, or " +
            "when tests need a database because a class new-s its dependencies. " +
            "SOLID is the vocabulary reviewers use to talk about those failures.");

        LessonIo.Principle("S — Single Responsibility",
            "A class should have one reason to change. UserService that validates, " +
            "saves, and sends email will change for three unrelated reasons.");

        LessonIo.Example("SRP — split orchestration from side effects", () =>
        {
            IUserRepository repo = new InMemoryUsers();
            IEmailSender mail = new ConsoleEmail();
            var service = new UserRegistration(repo, mail);
            service.Register("ada@example.com");
        });

        LessonIo.Principle("O — Open/Closed",
            "Open for extension, closed for modification. Add a new notifier by " +
            "adding a type, not by editing a switch inside a shipped class.");

        LessonIo.Example("OCP — new channel = new class", () =>
        {
            IEnumerable<INotifier> channels = [new LogNotifier(), new ConsoleNotifier()];
            foreach (var channel in channels)
            {
                channel.Send("deploy complete");
            }
        });

        LessonIo.Principle("L — Liskov Substitution",
            "A subtype must honor the base contract. Square inheriting Rectangle " +
            "and changing SetWidth to also set height breaks callers who expect " +
            "independent sides. Prefer composition when the math does not fit.");

        LessonIo.Example("LSP — both shapes honor Area()", () =>
        {
            Shape[] shapes = [new Rectangle(3, 4), new Circle(2)];
            foreach (var shape in shapes)
            {
                LessonIo.Result(shape.GetType().Name, shape.Area());
            }
        });

        LessonIo.Principle("I — Interface Segregation",
            "Do not force implementers to throw NotImplementedException. Split " +
            "fat IMachine (Print+Fax+Staple) into the methods a client actually needs.");

        LessonIo.Example("ISP — a printer only prints", () =>
        {
            IPrinter printer = new CheapPrinter();
            printer.Print("invoice");
        });

        LessonIo.Principle("D — Dependency Inversion",
            "Depend on abstractions. High-level UserRegistration depends on " +
            "IUserRepository, not SqlUserRepository. The composition root " +
            "(Program / DI container) wires the concrete type.");

        LessonIo.Interview(
            "How do you apply DIP in ASP.NET Core?",
            "Register services in Program.cs (builder.Services.AddScoped<IRepo, Repo>()) " +
            "and take IRepo in the constructor. Prefer interfaces owned by the " +
            "application (not by the infrastructure project) so the domain does not " +
            "reference SQL Server types. That is Clean Architecture / Ports and Adapters.");
    }

    private interface IUserRepository
    {
        void Add(string email);
    }

    private interface IEmailSender
    {
        void Send(string email, string body);
    }

    private sealed class InMemoryUsers : IUserRepository
    {
        public void Add(string email) => LessonIo.Result("saved", email);
    }

    private sealed class ConsoleEmail : IEmailSender
    {
        public void Send(string email, string body) => LessonIo.Result($"email to {email}", body);
    }

    private sealed class UserRegistration(IUserRepository repo, IEmailSender mail)
    {
        public void Register(string email)
        {
            repo.Add(email);
            mail.Send(email, "Welcome");
        }
    }

    private interface INotifier
    {
        void Send(string message);
    }

    private sealed class LogNotifier : INotifier
    {
        public void Send(string message) => LessonIo.Result("log", message);
    }

    private sealed class ConsoleNotifier : INotifier
    {
        public void Send(string message) => LessonIo.Result("console", message);
    }

    private abstract class Shape
    {
        public abstract double Area();
    }

    private sealed class Rectangle(double width, double height) : Shape
    {
        public override double Area() => width * height;
    }

    private sealed class Circle(double radius) : Shape
    {
        public override double Area() => Math.PI * radius * radius;
    }

    private interface IPrinter
    {
        void Print(string document);
    }

    private sealed class CheapPrinter : IPrinter
    {
        public void Print(string document) => LessonIo.Result("printed", document);
    }
}
