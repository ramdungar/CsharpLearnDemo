using CsharpLearnDemo.Lessons.Fundamentals;
using CsharpLearnDemo.Lessons.Interview;
using CsharpLearnDemo.Lessons.Principles;
using CsharpLearnDemo.Lessons.Versions;

namespace CsharpLearnDemo.Infrastructure;

/// <summary>
/// Single list of lessons. Adding a class here is how the course grows
/// (Open/Closed: the menu does not change).
/// </summary>
public static class LessonCatalog
{
    public static IReadOnlyList<ILesson> All() =>
    [
        new TypesAndVariablesLesson(),
        new ControlFlowLesson(),
        new MethodsLesson(),
        new ObjectOrientedLesson(),
        new MemoryAndExceptionsLesson(),
        new CSharp1Lesson(),
        new CSharp2Lesson(),
        new CSharp3Lesson(),
        new CSharp4Lesson(),
        new CSharp5Lesson(),
        new CSharp6Lesson(),
        new CSharp7Lesson(),
        new CSharp8Lesson(),
        new CSharp9Lesson(),
        new CSharp10Lesson(),
        new CSharp11Lesson(),
        new CSharp12Lesson(),
        new CSharp13AndBeyondLesson(),
        new SolidLesson(),
        new CleanCodeLesson(),
        new DesignPatternsLesson(),
        new InterviewQuestionsLesson(),
        new CodingProblemsLesson()
    ];
}
