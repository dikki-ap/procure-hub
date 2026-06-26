using MediatR;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;
using ProcureHub.Modules.VendorManagement.Infrastructure.Storage;
using ProcureHub.SharedKernel.Caching;
using ProcureHub.SharedKernel.CQRS;
using ProcureHub.SharedKernel.Exceptions;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.DeleteVendorDocument;

public class DeleteVendorDocumentCommandHandler : ICommandHandler<DeleteVendorDocumentCommand>
{
    private readonly IVendorDocumentRepository _docRepo;
    private readonly IStorageService           _storage;
    private readonly ICacheService             _cache;

    public DeleteVendorDocumentCommandHandler(
        IVendorDocumentRepository docRepo,
        IStorageService           storage,
        ICacheService             cache)
    {
        _docRepo = docRepo;
        _storage = storage;
        _cache   = cache;
    }

    public async Task<Unit> Handle(DeleteVendorDocumentCommand command, CancellationToken ct)
    {
        var doc = await _docRepo.GetByIdAsync(command.DocumentId, ct)
            ?? throw new NotFoundException("VendorDocument", command.DocumentId);

        await _storage.DeleteAsync("vendor-documents", doc.FileUrl, ct);

        _docRepo.Remove(doc);
        await _docRepo.SaveChangesAsync(ct);

        _cache.RemoveByPrefix(CacheKeys.Vendors.Prefix);

        return Unit.Value;
    }
}
