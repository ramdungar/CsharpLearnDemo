using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Fundamentals;

/// <summary>
/// Lesson 5 — memory model, boxing, GC, IDisposable, and exceptions.
/// This is the "how the CLR actually runs your code" chapter interviewers love.
/// </summary>
public sealed class MemoryAndExceptionsLesson : ILesson
{
    public string Key => "memory";
    public string Title => "Memory, GC, and exceptions";
    public string Category => "fundamentals";
    public string Summary => "Stack vs heap, boxing, IDisposable, using, throw/catch, and exception filters.";

    public void Run()
    {
        LessonIo.Why(
            "C# hides malloc/free, but you still pay for allocations. The CLR " +
            "garbage collector reclaims unused heap objects; unmanaged resources " +
            "(files, sockets, DB connections) are not memory — they need Dispose. " +
            "Exceptions exist because return codes were ignored in C; structured " +
            "exception handling forces a path for failure.");

        LessonIo.Principle(
            "Deterministic cleanup (IDisposable)",
            "If a type owns an unmanaged or limited resource, implement IDisposable " +
            "and consumers must use 'using' (or await using). Do not rely on the " +
            "finalizer for timely cleanup — GC timing is not guaranteed.");

        LessonIo.Example("Boxing and unboxing", () =>
        {
            // Boxing: a value type is wrapped in a heap object so it can be
            // treated as object / an interface. This allocates. Avoid it in hot loops.
            int number = 42;
            object boxed = number;     // box
            int copy = (int)boxed;     // unbox — wrong type throws InvalidCastException

            // ArrayList (C# 1) boxed every int. List<int> (C# 2 generics) does not.
            LessonIo.Result("boxed runtime type", boxed.GetType().FullName);
            LessonIo.Result("unboxed", copy);
        });

        LessonIo.Example("IDisposable and using (C# 8 using declaration)", () =>
        {
            // using statement (C# 1): Dispose is called in a finally.
            using (var clock = new FakeClock())
            {
                LessonIo.Result("inside using", clock.Now);
            }

            // using declaration (C# 8): Dispose at the end of the scope.
            using var clock2 = new FakeClock();
            LessonIo.Result("using declaration", clock2.Now);
        });

        LessonIo.Example("Exceptions: throw, catch, filter, rethrow", () =>
        {
            try
            {
                ParsePositive("-3");
            }
            catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "text")
            {
                // Exception filter (C# 6): the stack is NOT unwound unless
                // the filter is true. Useful for logging without catching everything.
                LessonIo.Result("filtered catch", ex.Message);
            }

            try
            {
                throw new InvalidOperationException("demo");
            }
            catch (Exception ex)
            {
                // 'throw;' preserves the stack trace. 'throw ex;' resets it — a
                // classic interview trap.
                LessonIo.Result("caught", ex.GetType().Name);
            }
        });

        LessonIo.Interview(
            "Stack vs heap in C#?",
            "Local value types and references live in the current stack frame. " +
            "Objects (instances of classes) live on the heap. A struct field " +
            "inside a class lives on the heap with that object. The GC collects " +
            "heap objects that are unreachable. Large objects (>= 85,000 bytes) " +
            "go on the Large Object Heap. Generations 0/1/2 exist so short-lived " +
            "objects are cheap to collect.");
    }

    private static int ParsePositive(string text)
    {
        if (!int.TryParse(text, out var n))
        {
            throw new FormatException($"'{text}' is not an int.");
        }

        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(text), "Must be positive.");
        }

        return n;
    }

    /// <summary>
    /// Stand-in for FileStream / HttpClient / DbConnection.
    /// Interviewers expect you to know the dispose pattern conceptually.
    /// </summary>
    private sealed class FakeClock : IDisposable
    {
        public DateTimeOffset Now { get; } = DateTimeOffset.UtcNow;
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
            LessonIo.Note("FakeClock.Dispose() ran — this is the using-pattern guarantee.");
        }
    }
}
