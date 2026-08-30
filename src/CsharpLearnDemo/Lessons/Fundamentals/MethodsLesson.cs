using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Fundamentals;

/// <summary>
/// Lesson 3 — methods, parameters, and overloading. Later versions add
/// optional/named args (C# 4), local functions (C# 7), and expression bodies (C# 6).
/// </summary>
public sealed class MethodsLesson : ILesson
{
    public string Key => "methods";
    public string Title => "Methods and parameters";
    public string Category => "Fundamentals";
    public string Summary => "Signatures, overloads, ref/out/in, params, and expression-bodied members.";

    public void Run()
    {
        LessonIo.Why(
            "Methods are the unit of reuse and the unit of testing. C# started " +
            "with C-style pass-by-value plus ref/out because interop and " +
            "multiple-return scenarios were common. Later versions reduced the " +
            "need for out (tuples in C# 7) and made intention clearer (in, named args).");

        LessonIo.Principle(
            "Command-Query Separation",
            "A method either does something (command) or returns something (query). " +
            "Mixing both — 'save and return a mutated global' — makes tests and " +
            "reasoning harder.");

        LessonIo.Example("Overloads and named / optional arguments (C# 4)", () =>
        {
            // One conceptual operation, several arities. The compiler picks
            // the best overload (overload resolution). This is why optional
            // parameters must come last.
            LessonIo.Result("Greet()", Greet());
            LessonIo.Result("Greet(name)", Greet("Linus"));
            LessonIo.Result("named title", Greet(name: "Ada", title: "Dr"));
        });

        LessonIo.Example("ref, out, in, and params", () =>
        {
            int value = 10;
            Triple(ref value); // ref: caller must have an assigned variable

            // out: the callee assigns. C# 7 lets you declare at the call site.
            bool parsed = TrySplit("a:b", out string left, out string right);

            // in: readonly reference — no copy of a large struct, no mutation.
            var box = new Dimensions(3, 4);
            double area = Area(in box);

            // params: syntactic sugar for an array argument.
            int total = Sum(1, 2, 3, 4);

            LessonIo.Result("Triple(10)", value);
            LessonIo.Result("TrySplit", $"{parsed} {left}/{right}");
            LessonIo.Result("Area", area);
            LessonIo.Result("Sum", total);
        });

        LessonIo.Example("Expression-bodied members (C# 6) and local functions (C# 7)", () =>
        {
            LessonIo.Result("IsEven(4)", IsEven(4));
            LessonIo.Result("Factorial(5)", Factorial(5));
        });

        LessonIo.Interview(
            "ref vs out vs in?",
            "ref: two-way, variable must be assigned before the call. " +
            "out: write-only from the callee; used for Try* patterns. " +
            "in: read-only reference, added in C# 7.2 to pass large structs " +
            "without copying. Prefer returning a tuple or a small record over out " +
            "in new public APIs.");
    }

    // Expression-bodied method: one expression, no braces. Same IL as a return.
    private static bool IsEven(int n) => n % 2 == 0;

    private static string Greet(string name = "world", string title = "")
    {
        return string.IsNullOrEmpty(title) ? $"Hello, {name}" : $"Hello, {title} {name}";
    }

    private static void Triple(ref int n) => n *= 3;

    private static bool TrySplit(string input, out string left, out string right)
    {
        var parts = input.Split(':');
        if (parts.Length != 2)
        {
            left = right = "";
            return false;
        }

        left = parts[0];
        right = parts[1];
        return true;
    }

    private static double Area(in Dimensions d) => d.Width * d.Height;

    private static int Sum(params int[] numbers)
    {
        var total = 0;
        foreach (var n in numbers)
        {
            total += n;
        }

        return total;
    }

    private static int Factorial(int n)
    {
        // Local function: visible only here, can capture locals, and can be
        // recursive without polluting the class. Added because nested helpers
        // were previously private methods that confused readers.
        int Inner(int x) => x <= 1 ? 1 : x * Inner(x - 1);
        return Inner(n);
    }

    private readonly struct Dimensions
    {
        public Dimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; }
        public double Height { get; }
    }
}
