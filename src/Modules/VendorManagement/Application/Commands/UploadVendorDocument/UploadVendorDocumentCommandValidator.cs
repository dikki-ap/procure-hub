using FluentValidation;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.UploadVendorDocument;

public class UploadVendorDocumentCommandValidator : AbstractValidator<UploadVendorDocumentCommand>
{
    private static readonly HashSet<string> AllowedTypes =
        ["application/pdf", "image/jpeg", "image/png", "image/webp"];

    public UploadVendorDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentType).IsInEnum();
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ContentType)
            .Must(t => AllowedTypes.Contains(t))
            .WithMessage("Only PDF, JPEG, PNG, and WebP files are allowed.");
        RuleFor(x => x.FileStream)
            .Must(s => s.Length <= 10 * 1024 * 1024)
            .WithMessage("File size must not exceed 10 MB.");
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes != null);
    }
}
