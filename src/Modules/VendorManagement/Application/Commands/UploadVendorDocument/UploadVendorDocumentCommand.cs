using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.UploadVendorDocument;

public record UploadVendorDocumentCommand(
    Guid           VendorId,
    DocumentType   DocumentType,
    string?        DocumentNumber,
    Stream         FileStream,
    string         FileName,
    string         ContentType,
    DateOnly?      ExpiredAt,
    DateOnly?      IssuedAt,
    string?        Notes
) : ICommand<Guid>;
