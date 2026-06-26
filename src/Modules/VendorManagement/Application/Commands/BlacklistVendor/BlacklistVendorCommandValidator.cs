using FluentValidation;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.BlacklistVendor;

public class BlacklistVendorCommandValidator : AbstractValidator<BlacklistVendorCommand>
{
    public BlacklistVendorCommandValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MinimumLength(10).MaximumLength(1000);
    }
}
