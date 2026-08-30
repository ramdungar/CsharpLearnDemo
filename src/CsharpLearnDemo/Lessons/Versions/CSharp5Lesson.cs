using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Lessons.Versions;

/// <summary>
/// C# 5.0 (2012) with .NET Framework 4.5 — async/await. The biggest
/// control-flow change since the language was invented.
/// </summary>
public sealed class CSharp5Lesson : ILesson
{
    public string Key => "csharp5";
    public string Title => "C# 5.0 (2012) — async / await";
    public string Category => "Versions";
    public string Summary => "Why async/await replaced callbacks, plus caller-info attributes.";

    public void Run()
    {
        LessonIo.Why(
            "Servers were running out of threads waiting on I/O (database, HTTP, " +
            "disk). UI apps froze because the UI thread blocked on the network. " +
            "The APM (BeginX/EndX) and EAP (XCompleted) patterns were unreadable. " +
            "F# already had asynchronous workflows. C# 5 borrowed the idea: the " +
            "compiler rewrites async methods into a state machine so you write " +
            "straight-line code and the thread is released while you await I/O. " +
            "Caller info attributes (CallerMemberName) were added so INotifyPropertyChanged " +
            "and logging did not need magic strings.");

        LessonIo.Principle(
            "Async all the way — never block on async",
            "If a method does I/O, return Task/Task<T>/ValueTask. Do not call " +
            ".Result or .Wait() on the UI or ASP.NET request context — that is " +
            "the classic deadlock. Use await. Libraries targeting older contexts " +
            "use ConfigureAwait(false) so they do not marshal back to a sync context.");

        // Console Main is synchronous. GetAwaiter().GetResult() is OK in a
        // console entry path; it is NOT OK on a UI/ASP.NET SynchronizationContext.
        RunAsyncDemo().GetAwaiter().GetResult();

        LessonIo.Example("CallerMemberName — no magic strings", () =>
        {
            Log("ready");
        });

        LessonIo.Interview(
            "What does await actually do?",
            "If the awaitable is already complete, execution continues synchronously. " +
            "Otherwise the remainder of the method is posted as a continuation and " +
            "the current thread is returned to the caller/pool. Exceptions are " +
            "captured and rethrown on the awaiting thread, wrapped in AggregateException " +
            "if you use .Wait()/.Result, or unwrapped if you await. async void is " +
            "only for event handlers — exceptions there crash the process.");
    }

    private static async Task RunAsyncDemo()
    {
        LessonIo.Subheading("Sequential awaits vs concurrent WhenAll");

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var a = await FakeIoAsync("A", 60);
        var b = await FakeIoAsync("B", 60);
        sw.Stop();
        LessonIo.Result("sequential", $"{a}, {b} in {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var t1 = FakeIoAsync("A", 60);
        var t2 = FakeIoAsync("B", 60);
        var both = await Task.WhenAll(t1, t2);
        sw.Stop();
        LessonIo.Result("WhenAll", $"{string.Join(", ", both)} in {sw.ElapsedMilliseconds}ms");
    }

    private static async Task<string> FakeIoAsync(string name, int delayMs)
    {
        // Task.Delay is the teaching stand-in for HttpClient.GetAsync / ReadAsync.
        await Task.Delay(delayMs);
        return name;
    }

    private static void Log(
        string message,
        [System.Runtime.CompilerServices.CallerMemberName] string member = "",
        [System.Runtime.CompilerServices.CallerFilePath] string file = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
    {
        var fileName = Path.GetFileName(file);
        LessonIo.Result("caller-info", $"{fileName}:{line} {member} — {message}");
    }
}
