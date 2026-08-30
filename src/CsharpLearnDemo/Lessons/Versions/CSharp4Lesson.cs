using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 4.0 (2010) with .NET Framework 4.0 — interop and the dynamic era.
/// </summary>
public sealed class CSharp4Lesson : ILesson
{
    public string Key => "csharp4";
    public string Title => "C# 4.0 (2010) — dynamic, named/optional, variance";
    public string Category => "Versions";
    public string Summary => "Why dynamic, optional/named arguments, and generic covariance landed.";

    public void Run()
    {
        LessonIo.Why(
            "Three pressures hit at once. (1) Office/COM APIs used optional " +
            "parameters and late binding — C# callers had to pass Type.Missing " +
            "dozens of times. (2) IronPython/IronRuby and JSON-like objects " +
            "needed a way to opt out of static typing at the boundary. " +
            "(3) IEnumerable<string> could not be passed as IEnumerable<object> " +
            "even though that is safe — Java already had wildcard variance. " +
            "C# 4 added named/optional args, the dynamic type (DLR), and " +
            "out/in generic variance.");

        LessonIo.Principle(
            "Keep dynamic at the boundary",
            "dynamic skips compile-time checking. Use it to talk to COM, " +
            "Python, or loosely shaped JSON — then map into real types. " +
            "A codebase full of dynamic is a codebase the compiler cannot help.");

        LessonIo.Example("Named and optional arguments", () =>
        {
            // Optional args are compiled as CLR optional parameters + [Optional].
            // Named args let you skip middles and document intent at the call site.
            LessonIo.Result("defaults", FormatOrder());
            LessonIo.Result("named", FormatOrder(qty: 2, sku: "BOOK-1"));
        });

        LessonIo.Example("dynamic — resolved at runtime by the DLR", () =>
        {
            dynamic value = 10;
            LessonIo.Result("dynamic int + 5", value + 5);
            value = "C#";
            LessonIo.Result("dynamic string + version", value + " 4");
            // value.NotARealMethod() would compile and throw at runtime.
        });

        LessonIo.Example("Covariance (out) and contravariance (in)", () =>
        {
            // Covariance: IEnumerable<out T> — you only produce T.
            // A sequence of strings is a sequence of objects.
            IEnumerable<string> names = new[] { "Ada", "Grace" };
            IEnumerable<object> objects = names; // legal since C# 4
            LessonIo.Result("covariant count", objects.Count());

            // Contravariance: Action<in T> — you only consume T.
            // An action that accepts object can accept a string.
            Action<object> write = o => LessonIo.Result("wrote", o);
            Action<string> writeString = write;
            writeString("safe");
        });

        LessonIo.Interview(
            "What does 'out T' mean on an interface?",
            "The type parameter is covariant: it appears only in output positions " +
            "(return types). IEnumerable<out T> lets you treat IEnumerable<Cat> as " +
            "IEnumerable<Animal>. 'in T' is contravariant (input positions) — " +
            "IComparer<Animal> can compare Cats. This is a compile-time guarantee " +
            "that you cannot insert a Dog into a list of Cats through a widened reference.");
    }

    private static string FormatOrder(string sku = "UNKNOWN", int qty = 1, string? note = null)
    {
        return note is null ? $"{qty} x {sku}" : $"{qty} x {sku} ({note})";
    }
}
