using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.RegisterVendor;

public record RegisterVendorCommand(
    Guid       CompanyId,
    string     LegalName,
    string?    TradeName,
    VendorType VendorType,
    string?    Npwp,
    string?    Siup,
    string?    Nib,
    string     ContactName,
    string?    ContactPosition,
    string     ContactEmail,
    string?    ContactPhone
) : ICommand<Guid>;
