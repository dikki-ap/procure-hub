using FluentAssertions;
using FluentValidation;
using MediatR;
using ProcureHub.SharedKernel.Behaviors;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.UnitTests.Behaviors;

// ── Test doubles ────────────────────────────────────────────────────────────

file record CreateItemCommand(string Name) : ICommand<Unit>;

file sealed class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator() => RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
}

// ── Tests ────────────────────────────────────────────────────────────────────

public class ValidationBehaviorTests
{
    private static RequestHandlerDelegate<Unit> NextReturnsUnit(ref bool called)
    {
        var captured = false;
        called = captured;
        return () =>
        {
            captured = true;
            return Task.FromResult(Unit.Value);
        };
    }

    [Fact]
    public async Task Handle_WithNoValidators_ShouldPassThrough()
    {
        var behavior  = new ValidationBehavior<CreateItemCommand, Unit>([]);
        var nextCalled = false;
        RequestHandlerDelegate<Unit> next = () => { nextCalled = true; return Task.FromResult(Unit.Value); };

        await behavior.Handle(new CreateItemCommand("Test"), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldPassThrough()
    {
        var behavior  = new ValidationBehavior<CreateItemCommand, Unit>([new CreateItemValidator()]);
        var nextCalled = false;
        RequestHandlerDelegate<Unit> next = () => { nextCalled = true; return Task.FromResult(Unit.Value); };

        await behavior.Handle(new CreateItemCommand("Valid Name"), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithEmptyName_ShouldThrowValidationException()
    {
        var behavior = new ValidationBehavior<CreateItemCommand, Unit>([new CreateItemValidator()]);
        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        var act = () => behavior.Handle(new CreateItemCommand(""), next, CancellationToken.None);

        var ex = await act.Should()
            .ThrowAsync<ProcureHub.SharedKernel.Exceptions.ValidationException>();

        ex.Which.Errors.Should().ContainKey("Name");
    }

    [Fact]
    public async Task Handle_WithNameTooLong_ShouldThrowValidationException()
    {
        var behavior  = new ValidationBehavior<CreateItemCommand, Unit>([new CreateItemValidator()]);
        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);
        var tooLong   = new string('x', 101);

        var act = () => behavior.Handle(new CreateItemCommand(tooLong), next, CancellationToken.None);

        await act.Should().ThrowAsync<ProcureHub.SharedKernel.Exceptions.ValidationException>();
    }

    [Fact]
    public async Task Handle_WithMultipleErrors_ShouldCollectAll()
    {
        // Name empty AND length violation cannot both fire at once with the current validator,
        // so use two separate validators to produce two failures.
        var validator1 = new InlineValidator<CreateItemCommand>();
        validator1.RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        var validator2 = new InlineValidator<CreateItemCommand>();
        validator2.RuleFor(c => c.Name).Must(_ => false).WithMessage("Custom rule failed");

        var behavior = new ValidationBehavior<CreateItemCommand, Unit>(
            [validator1, validator2]);
        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        var ex = await Assert.ThrowsAsync<ProcureHub.SharedKernel.Exceptions.ValidationException>(
            () => behavior.Handle(new CreateItemCommand(""), next, CancellationToken.None));

        ex.Errors.Values.SelectMany(v => v).Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
