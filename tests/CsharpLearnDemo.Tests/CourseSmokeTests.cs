using CsharpLearnDemo.Infrastructure;

namespace CsharpLearnDemo.Tests;

/// <summary>
/// Smoke tests: the catalog is complete and every lesson runs without throwing.
/// This is how you keep a teaching repo from rotting — CI runs the examples.
/// </summary>
public sealed class CourseSmokeTests
{
    [Fact]
    public void Catalog_has_unique_keys_and_expected_categories()
    {
        var lessons = LessonCatalog.All();
        Assert.Equal(lessons.Count, lessons.Select(l => l.Key).Distinct().Count());
        Assert.Contains(lessons, l => l.Category == "Fundamentals");
        Assert.Contains(lessons, l => l.Category == "Versions");
        Assert.Contains(lessons, l => l.Category == "Principles");
        Assert.Contains(lessons, l => l.Category == "Interview");
        Assert.Contains(lessons, l => l.Key == "csharp12");
        Assert.Contains(lessons, l => l.Key == "solid");
    }

    [Fact]
    public void CourseApp_list_succeeds()
    {
        Assert.Equal(0, CourseApp.Run(["list"]));
    }

    [Fact]
    public void CourseApp_unknown_key_fails()
    {
        Assert.Equal(1, CourseApp.Run(["not-a-lesson"]));
    }

    [Fact]
    public void Every_lesson_runs()
    {
        foreach (var lesson in LessonCatalog.All())
        {
            var ex = Record.Exception(lesson.Run);
            Assert.True(ex is null, $"{lesson.Key} threw {ex}");
        }
    }
}
