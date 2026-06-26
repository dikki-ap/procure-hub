using ProcureHub.Modules.VendorManagement.Application.DTOs;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorDocuments;

public record GetVendorDocumentsQuery(Guid VendorId) : IQuery<List<VendorDocumentDto>>;
