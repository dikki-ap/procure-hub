using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.ReinstateVendor;

public record ReinstateVendorCommand(Guid VendorId) : ICommand;
