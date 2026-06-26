using FluentAssertions;
using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.UnitTests.Domain;

file sealed class ConcreteEntity : BaseEntity { }

public class BaseEntityTests
{
    [Fact]
    public void Id_ShouldBeUuidV7_OnCreate()
    {
        var entity = new ConcreteEntity();

        // UUID v7 format: xxxxxxxx-xxxx-7xxx-xxxx-xxxxxxxxxxxx
        // Version nibble is the first char of the 3rd group (position 14 in the string)
        entity.Id.ToString()[14].Should().Be('7');
    }

    [Fact]
    public void Id_ShouldBeUnique_ForEachInstance()
    {
        var ids = Enumerable.Range(0, 100)
            .Select(_ => new ConcreteEntity().Id)
            .ToHashSet();

        ids.Should().HaveCount(100);
    }

    [Fact]
    public void Id_ShouldBeTimeOrdered_WhenCreatedSequentially()
    {
        var first  = new ConcreteEntity();
        var second = new ConcreteEntity();

        // UUID v7 is time-ordered: lexicographic comparison reflects creation order
        string.Compare(second.Id.ToString(), first.Id.ToString(), StringComparison.Ordinal)
            .Should().BeGreaterThanOrEqualTo(0);
    }
}
