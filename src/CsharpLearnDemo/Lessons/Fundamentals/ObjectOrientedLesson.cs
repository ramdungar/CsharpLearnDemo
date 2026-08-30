using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Fundamentals;

/// <summary>
/// Lesson 4 — object-oriented building blocks. C# 1 shipped as an OOP
/// language first; functional features arrived later as additions, not replacements.
/// </summary>
public sealed class ObjectOrientedLesson : ILesson
{
    public string Key => "oop";
    public string Title => "Object-oriented programming";
    public string Category => "fundamentals";
    public string Summary => "Class, interface, inheritance, polymorphism, encapsulation, and composition.";

    public void Run()
    {
        LessonIo.Why(
            "C# 1.0 was designed for component-oriented Windows and enterprise " +
            "apps: properties for designers, events for UI, interfaces for COM-like " +
            "contracts. Inheritance models 'is-a'; interfaces model 'can-do'. " +
            "Teams later overused inheritance, which is why modern C# guidance " +
            "is 'prefer composition' and 'prefer records for data'.");

        LessonIo.Principle(
            "Encapsulation + composition over inheritance",
            "Keep fields private. Expose behavior, not storage. Inherit only when " +
            "you truly share identity and substitutability (Liskov). Otherwise " +
            "inject a dependency (has-a).");

        LessonIo.Example("Encapsulation, inheritance, and polymorphism", () =>
        {
            // The variable type is the abstraction (INotifier). The runtime
            // type decides which override runs — virtual dispatch.
            INotifier notifier = new ConsoleNotifier("ops");
            var service = new PasswordResetService(notifier);
            service.Reset("user@example.com");
        });

        LessonIo.Example("abstract vs virtual vs interface vs sealed", () =>
        {
            Shape circle = new Circle(2);
            Shape square = new Square(3);
            LessonIo.Result("circle area", circle.Area());
            LessonIo.Result("square area", square.Area());
            LessonIo.Result("Name (virtual)", circle.Name);
        });

        LessonIo.Interview(
            "abstract class vs interface?",
            "An abstract class is a partial implementation: it can have fields, " +
            "constructors, and shared code. A class has one base class. " +
            "An interface is a contract (can-do). A class can implement many. " +
            "C# 8 added default interface methods so published APIs can evolve " +
            "without breaking implementers. Prefer interfaces for dependencies; " +
            "abstract classes for a real family of types that share state.");
    }

    private interface INotifier
    {
        void Notify(string message);
    }

    /// <summary>
    /// Composition: PasswordResetService HAS an INotifier. We can swap
    /// email, SMS, or a fake in tests without changing this class (DIP).
    /// </summary>
    private sealed class PasswordResetService
    {
        private readonly INotifier _notifier;

        public PasswordResetService(INotifier notifier) => _notifier = notifier;

        public void Reset(string email)
        {
            // In real code this would persist a token. Here we only show the call.
            _notifier.Notify($"Reset link sent to {email}");
        }
    }

    private sealed class ConsoleNotifier : INotifier
    {
        private readonly string _channel;

        public ConsoleNotifier(string channel) => _channel = channel;

        public void Notify(string message) =>
            LessonIo.Result($"notify[{_channel}]", message);
    }

    private abstract class Shape
    {
        // virtual: derived types MAY override. Default implementation exists.
        public virtual string Name => GetType().Name;

        // abstract: derived types MUST implement. No body here.
        public abstract double Area();
    }

    // sealed: nobody can inherit further. Use when a type is not designed
    // as a base (default in many style guides — inheritance is a privilege).
    private sealed class Circle : Shape
    {
        private readonly double _radius;
        public Circle(double radius) => _radius = radius;
        public override double Area() => Math.PI * _radius * _radius;
    }

    private sealed class Square : Shape
    {
        private readonly double _side;
        public Square(double side) => _side = side;
        public override double Area() => _side * _side;
    }
}
