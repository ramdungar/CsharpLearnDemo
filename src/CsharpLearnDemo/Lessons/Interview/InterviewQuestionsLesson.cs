using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Interview;

/// <summary>
/// High-frequency interview questions with the answer you should actually say,
/// plus a live snippet when it helps. Pair this with docs/INTERVIEW_PREP.md.
/// </summary>
public sealed class InterviewQuestionsLesson : ILesson
{
    public string Key => "interview";
    public string Title => "Interview Q&A (runtime, language, OOP)";
    public string Category => "Interview";
    public string Summary => "CLR, GC, string, boxing, delegates, async pitfalls, and equality — spoken answers.";

    public void Run()
    {
        LessonIo.Why(
            "Interviewers probe whether you understand the CLR, not whether you " +
            "memorized a blog post. Each question below is one they actually ask " +
            "for mid-level C# roles, with the reason they ask it.");

        LessonIo.Interview(
            "string is a reference type. Why does == compare characters?",
            "System.String overloads == and Equals for value (ordinal) comparison. " +
            "The intern pool can make two literals the same reference, but you must " +
            "not rely on that. Use string.Equals with a StringComparison in APIs " +
            "that care about culture (names, UI) vs ordinal (IDs, paths on Linux).");

        LessonIo.Example("string intern vs new String", () =>
        {
            var a = "hello";
            var b = "hello";
            var c = new string("hello".ToCharArray());
            LessonIo.Result("literals same reference", ReferenceEquals(a, b));
            LessonIo.Result("new string different reference", ReferenceEquals(a, c));
            LessonIo.Result("== still true", a == c);
        });

        LessonIo.Interview(
            "What is the difference between const, static readonly, and readonly?",
            "const: compile-time, inlined into callers (binary breaking if you change " +
            "a public const). static readonly: runtime, assigned in a static ctor, " +
            "one per type. readonly instance: assigned in ctor, one per object. " +
            "Use const for true constants (days in a week); readonly for anything " +
            "that might be computed or might change in a later version.");

        LessonIo.Interview(
            "Equals / GetHashCode / == — what must stay in sync?",
            "If two instances are equal, they MUST have the same hash code (dictionary " +
            "buckets). Override all three together on value-like types. Records do this " +
            "for you. Mutable keys in a Dictionary are a footgun — the hash changes " +
            "and the entry is lost.");

        LessonIo.Interview(
            "async/await deadlock: how do you cause it and how do you avoid it?",
            "Cause: UI or ASP.NET classic SynchronizationContext + .Wait()/.Result " +
            "on a task that needs that same context to finish. Avoid: await all the " +
            "way, or ConfigureAwait(false) in library code so continuations run on " +
            "the pool. ASP.NET Core has no sync context — .Result is still a thread-pool " +
            "starvation smell, just not the old deadlock.");

        LessonIo.Interview(
            "IEnumerable vs IQueryable vs IList vs ICollection?",
            "IEnumerable: forward-only, deferred, in memory. IQueryable: expression " +
            "tree, provider translates (SQL). ICollection: Count + mutate. IList: " +
            "index access. Expose the smallest type the caller needs (ISP). Do not " +
            "return IQueryable from a public API — it leaks EF and cannot be versioned.");

        LessonIo.Interview(
            "What happens when you box an int?",
            "The CLR allocates a heap object that holds a copy of the value and a " +
            "type handle. Casting back to the wrong value type throws. Comparing " +
            "two boxed ints with == on object uses reference equality unless you " +
            "call Equals. Generics exist largely to avoid this.");

        LessonIo.Interview(
            "delegate vs event vs Action/Func vs interface?",
            "delegate: the type of a method. event: a multicast delegate with " +
            "restricted access (observers cannot clear or invoke). Action/Func: " +
            "generic delegates so you do not declare one per shape. interface: " +
            "when the contract has several methods or will have multiple implementations " +
            "with state. Use a delegate for one function; an interface for a role.");

        LessonIo.Interview(
            "Task vs Thread vs ValueTask?",
            "Thread is an OS thread. Task is a promise (may complete on any thread, " +
            "may never use a dedicated thread — I/O). ValueTask avoids allocating a " +
            "Task when the result is often already available; consume it once. " +
            "ThreadPool.QueueUserWorkItem / Task.Run is for CPU work, not for wrapping " +
            "sync I/O and hoping.");
    }
}
