using ProcureHub.Modules.VendorManagement.Application.DTOs;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;
using ProcureHub.SharedKernel.CQRS;
using ProcureHub.SharedKernel.Exceptions;

namespace ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorDocuments;

public class GetVendorDocumentsQueryHandler : IQueryHandler<GetVendorDocumentsQuery, List<VendorDocumentDto>>
{
    private readonly IVendorRepository         _vendorRepo;
    private readonly IVendorDocumentRepository _docRepo;

    public GetVendorDocumentsQueryHandler(
        IVendorRepository         vendorRepo,
        IVendorDocumentRepository docRepo)
    {
        _vendorRepo = vendorRepo;
        _docRepo    = docRepo;
    }

    public async Task<List<VendorDocumentDto>> Handle(GetVendorDocumentsQuery query, CancellationToken ct)
    {
        var vendor = await _vendorRepo.GetByIdAsync(query.VendorId, ct)
            ?? throw new NotFoundException("Vendor", query.VendorId);

        var docs = await _docRepo.GetByVendorIdAsync(vendor.Id, ct);

        return docs.Select(d => new VendorDocumentDto(
            d.Id, d.DocumentType, d.DocumentNumber, d.FileUrl, d.FileName,
            d.FileSize, d.ExpiredAt, d.IssuedAt, d.Status, d.Notes)).ToList();
    }
}
