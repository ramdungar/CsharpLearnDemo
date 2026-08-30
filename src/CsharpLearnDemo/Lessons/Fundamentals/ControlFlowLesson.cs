using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Fundamentals;

/// <summary>
/// Lesson 2 — decisions and loops. Later versions add pattern matching
/// (C# 7+) and switch expressions (C# 8), but every program still uses these.
/// </summary>
public sealed class ControlFlowLesson : ILesson
{
    public string Key => "flow";
    public string Title => "Control flow";
    public string Category => "Fundamentals";
    public string Summary => "if/else, switch, loops, break/continue, and why later versions added patterns.";

    public void Run()
    {
        LessonIo.Why(
            "Programs are decision trees over data. Early C# copied C-family " +
            "if/switch/for so C/Java developers felt at home. Later versions " +
            "added pattern matching because nested ifs on types and properties " +
            "became the #1 source of noisy, bug-prone code in large codebases.");

        LessonIo.Principle(
            "Make control flow total",
            "Handle every case. Prefer switch expressions that do not compile " +
            "when a new enum value is added (C# 8+) over a chain of ifs that silently " +
            "falls through.");

        LessonIo.Example("if / else if / else", () =>
        {
            int temperature = 18;
            string band;
            if (temperature < 0)
            {
                band = "freezing";
            }
            else if (temperature < 20)
            {
                band = "cool";
            }
            else
            {
                band = "warm";
            }

            LessonIo.Result("band", band);
        });

        LessonIo.Example("Classic switch vs C# 8 switch expression", () =>
        {
            // Classic switch (C# 1): statement, can fall through only when empty.
            var day = DayOfWeek.Monday;
            string classic;
            switch (day)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    classic = "weekend";
                    break;
                default:
                    classic = "weekday";
                    break;
            }

            // Switch expression: an expression (returns a value). Exhaustiveness
            // is checked for enums. This is why C# 8 added it — less mutation.
            string modern = day switch
            {
                DayOfWeek.Saturday or DayOfWeek.Sunday => "weekend",
                _ => "weekday"
            };

            LessonIo.Result("classic", classic);
            LessonIo.Result("modern", modern);
        });

        LessonIo.Example("Loops: for, foreach, while", () =>
        {
            // for: you own the index. Use when you need the position.
            var squares = new List<int>();
            for (int i = 1; i <= 4; i++)
            {
                squares.Add(i * i);
            }

            // foreach: the CLR calls GetEnumerator(). You cannot assign the
            // iteration variable (it is foreach-readonly). Prefer this when
            // you only need each item — it states intent (KISS).
            int sum = 0;
            foreach (var square in squares)
            {
                sum += square;
            }

            // while: unknown iteration count, e.g. reading a stream.
            int n = 3;
            int factorial = 1;
            while (n > 1)
            {
                factorial *= n;
                n--;
            }

            LessonIo.Result("squares", string.Join(",", squares));
            LessonIo.Result("sum", sum);
            LessonIo.Result("3!", factorial);
        });

        LessonIo.Interview(
            "foreach vs for — when do you pick each?",
            "foreach when you only need elements (clearer, works on any IEnumerable). " +
            "for when you need the index, must mutate by position, or are walking a " +
            "span/array in reverse. Never mutate a List while foreach-ing it — that " +
            "throws InvalidOperationException.");
    }
}
