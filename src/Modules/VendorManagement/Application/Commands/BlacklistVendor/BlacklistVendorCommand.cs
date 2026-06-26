using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.BlacklistVendor;

public record BlacklistVendorCommand(Guid VendorId, string Reason, Guid BlacklistedById) : ICommand;
