using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.DeleteVendorDocument;

public record DeleteVendorDocumentCommand(Guid DocumentId, Guid DeletedById) : ICommand;
