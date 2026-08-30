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
        var keys = lessons.Select(l => l.Key).ToList();
        var categories = lessons.Select(l => l.Category).Distinct().ToList();

        Assert.Equal(lessons.Count, keys.Distinct().Count());
        Assert.Contains("Fundamentals", categories);
        Assert.Contains("Versions", categories);
        Assert.Contains("Principles", categories);
        Assert.Contains("Interview", categories);
        Assert.Contains("csharp12", keys);
        Assert.Contains("solid", keys);
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
