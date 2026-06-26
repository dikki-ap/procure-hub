using ProcureHub.Modules.VendorManagement.Application.DTOs;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorList;

public record GetVendorListQuery(Guid CompanyId) : IQuery<List<VendorDto>>;
