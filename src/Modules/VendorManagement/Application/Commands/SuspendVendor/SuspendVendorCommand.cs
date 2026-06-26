using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.SuspendVendor;

public record SuspendVendorCommand(Guid VendorId) : ICommand;
