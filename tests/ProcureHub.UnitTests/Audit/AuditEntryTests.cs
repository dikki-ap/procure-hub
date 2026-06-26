using FluentAssertions;
using ProcureHub.SharedKernel.Audit;

namespace ProcureHub.UnitTests.Audit;

public class AuditEntryTests
{
    // AuditEntry.ToAuditLog() only accesses TemporaryProperties (initialized as []),
    // so passing null! for EntityEntry is safe when no temp properties are involved.

    [Fact]
    public void ToAuditLog_ForUpdatedAction_ShouldSerializeBeforeAndAfterValues()
    {
        var entry = new AuditEntry(null!)
        {
            EntityType = "Vendor",
            EntityId   = Guid.NewGuid(),
            Action     = "Updated",
        };
        entry.BeforeValues["name"] = "Old Corp";
        entry.AfterValues["name"]  = "New Corp";
        entry.ChangedColumns.Add("name");

        var log = entry.ToAuditLog(null, null, null, null, null, null);

        log.BeforeValues.Should().NotBeNull().And.Contain("Old Corp");
        log.AfterValues.Should().Contain("New Corp");
        log.ChangedColumns.Should().NotBeNull().And.Contain("name");
        log.EntityType.Should().Be("Vendor");
        log.Action.Should().Be("Updated");
    }

    [Fact]
    public void ToAuditLog_ForCreatedAction_BeforeValuesShouldBeNull()
    {
        var entityId = Guid.NewGuid();
        var entry = new AuditEntry(null!)
        {
            EntityType = "Vendor",
            EntityId   = entityId,
            Action     = "Created",
        };
        entry.AfterValues["name"] = "New Vendor";

        var log = entry.ToAuditLog(null, null, null, null, null, null);

        log.BeforeValues.Should().BeNull();
        log.AfterValues.Should().Contain("New Vendor");
        log.ChangedColumns.Should().BeNull();
        log.EntityId.Should().Be(entityId);
    }

    [Fact]
    public void ToAuditLog_ForDeletedAction_AfterValuesShouldBeEmpty()
    {
        var entry = new AuditEntry(null!)
        {
            EntityType = "Vendor",
            EntityId   = Guid.NewGuid(),
            Action     = "Deleted",
        };
        entry.BeforeValues["name"] = "Removed Corp";

        var log = entry.ToAuditLog(null, null, null, null, null, null);

        log.BeforeValues.Should().Contain("Removed Corp");
        log.AfterValues.Should().Be("{}");
    }

    [Fact]
    public void ToAuditLog_ShouldMapActorFields_WhenProvided()
    {
        var userId = Guid.NewGuid();
        var entry  = new AuditEntry(null!)
        {
            EntityType = "Vendor",
            EntityId   = Guid.NewGuid(),
            Action     = "Created",
        };
        entry.AfterValues["name"] = "Test";

        var log = entry.ToAuditLog(userId, "user@example.com", "John Doe",
            "127.0.0.1", "Mozilla/5.0", "corr-123");

        log.UserId.Should().Be(userId);
        log.UserEmail.Should().Be("user@example.com");
        log.UserFullName.Should().Be("John Doe");
        log.IpAddress.Should().Be("127.0.0.1");
        log.UserAgent.Should().Be("Mozilla/5.0");
        log.CorrelationId.Should().Be("corr-123");
    }
}
