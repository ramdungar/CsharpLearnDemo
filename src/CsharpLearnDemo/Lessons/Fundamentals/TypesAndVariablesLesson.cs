using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Fundamentals;

/// <summary>
/// Lesson 1 — types, variables, and the type system.
/// Everything else in C# is built on these rules. Learn them once; they
/// still apply in C# 13.
/// </summary>
public sealed class TypesAndVariablesLesson : ILesson
{
    public string Key => "types";
    public string Title => "Types, variables, and values";
    public string Category => "Fundamentals";
    public string Summary => "Value vs reference types, literals, conversions, var, and const/readonly.";

    public void Run()
    {
        LessonIo.Why(
            "A language without a clear type system cannot scale: the compiler " +
            "must know the shape of data to generate IL, catch mistakes early, " +
            "and pick the right memory layout (stack vs heap). C# chose a static, " +
            "safe type system (like Java) plus value types (like C++) because " +
            "Windows and game/server code need both safety and performance.");

        LessonIo.Principle(
            "Fail fast / type safety",
            "Prefer catching errors at compile time. 'var' does not mean dynamic: " +
            "the type is still inferred and fixed. Dynamic (C# 4) is the escape hatch.");

        LessonIo.Example("Built-in value types (live on the stack when local)", () =>
        {
            // int is an alias for System.Int32. Prefer the alias in C# source.
            int age = 30;

            // double is the default floating literal. Use m suffix for decimal
            // (money): binary floating point cannot represent 0.1 exactly.
            double ratio = 0.1;
            decimal price = 19.99m;

            // bool is a real type, not an int, unlike C.
            bool isAdult = age >= 18;

            // char is a UTF-16 code unit. string is a sequence of chars.
            char grade = 'A';

            LessonIo.Result("age", age);
            LessonIo.Result("ratio (double, inexact 0.1)", ratio);
            LessonIo.Result("price (decimal, exact for base-10)", price);
            LessonIo.Result("isAdult", isAdult);
            LessonIo.Result("grade", grade);
        });

        LessonIo.Example("Reference types: the variable holds a pointer", () =>
        {
            // string is a reference type but immutable. Two variables can
            // point at the same instance; neither can mutate the characters.
            string name = "Ada";
            string also = name;
            name = "Grace"; // rebinds the variable; 'also' still points at "Ada"

            // Arrays are reference types. Copying the variable copies the pointer.
            int[] scores = { 10, 20, 30 };
            int[] alias = scores;
            alias[0] = 99; // mutates the one shared array

            LessonIo.Result("name", name);
            LessonIo.Result("also (unchanged)", also);
            LessonIo.Result("scores[0] after alias write", scores[0]);
        });

        LessonIo.Example("Conversions: implicit (safe) vs explicit (you accept risk)", () =>
        {
            int whole = 42;
            long wider = whole;          // implicit: every int fits in a long
            short narrower = (short)whole; // explicit cast: may truncate

            // Parse vs TryParse: production code prefers TryParse so bad
            // user input does not throw (robustness over convenience).
            bool ok = int.TryParse("123", out int parsed);
            bool bad = int.TryParse("nope", out int failed);

            LessonIo.Result("wider", wider);
            LessonIo.Result("narrower", narrower);
            LessonIo.Result("TryParse 123", $"{ok}, {parsed}");
            LessonIo.Result("TryParse nope", $"{bad}, {failed}");
        });

        LessonIo.Example("var, const, and readonly", () =>
        {
            // var (C# 3) is inferred from the right-hand side. Still static.
            var message = "inferred as string";

            // const must be a compile-time constant. It is substituted into
            // callers — changing a public const is a binary breaking change.
            const int MaxRetries = 3;

            var sample = new ConfigSample();
            LessonIo.Result("var type", message.GetType().Name);
            LessonIo.Result("const MaxRetries", MaxRetries);
            LessonIo.Result("readonly AppName", sample.AppName);
        });

        LessonIo.Interview(
            "What is the difference between a value type and a reference type?",
            "A value-type variable contains the data (copying copies bytes). " +
            "A reference-type variable contains a reference to an object on the heap " +
            "(copying copies the pointer). Structs, enums, and primitives are value " +
            "types; class, interface, delegate, array, and string are reference types.");
    }

    /// <summary>
    /// Tiny type so we can show 'readonly' on a field. readonly is assigned
    /// only in the declaration or the constructor — the CLR enforces it.
    /// </summary>
    private sealed class ConfigSample
    {
        public readonly string AppName = "CsharpLearnDemo";
    }
}
