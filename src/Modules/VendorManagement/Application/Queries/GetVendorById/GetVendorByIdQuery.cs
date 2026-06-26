using ProcureHub.Modules.VendorManagement.Application.DTOs;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorById;

public record GetVendorByIdQuery(Guid Id) : IQuery<VendorDetailDto>;
