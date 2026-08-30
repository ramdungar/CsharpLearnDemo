namespace CsharpLearnDemo.Tests;

/// <summary>
/// Tiny checks that the language features we teach behave as described.
/// Useful as flash cards: read the test name, predict the assert, then look.
/// </summary>
public sealed class LanguageFeatureTests
{
    [Fact]
    public void Record_uses_value_equality_and_with_copies()
    {
        var ada = new Person("Ada", "Lovelace");
        var clone = new Person("Ada", "Lovelace");
        var grace = ada with { First = "Grace" };

        Assert.Equal(ada, clone);
        Assert.False(ReferenceEquals(ada, clone));
        Assert.Equal("Grace", grace.First);
        Assert.Equal("Lovelace", grace.Last);
        Assert.NotEqual(ada, grace);
    }

    [Fact]
    public void Range_and_index_slice_arrays()
    {
        int[] numbers = [10, 20, 30, 40, 50];
        Assert.Equal(50, numbers[^1]);
        Assert.Equal(new[] { 20, 30, 40 }, numbers[1..4]);
    }

    [Fact]
    public void Nullable_value_and_reference_behave_differently()
    {
        int? missing = null;
        Assert.False(missing.HasValue);
        Assert.Equal(-1, missing.GetValueOrDefault(-1));

        string? name = null;
        Assert.Null(name);
        Assert.Equal("anon", name ?? "anon");
    }

    [Fact]
    public void Deferred_linq_sees_latest_captured_variable()
    {
        var factor = 1;
        var query = Enumerable.Range(1, 3).Select(n => n * factor);
        factor = 10;
        Assert.Equal(new[] { 10, 20, 30 }, query.ToArray());
    }

    private sealed record Person(string First, string Last);
}
