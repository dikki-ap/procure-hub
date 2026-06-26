using ProcureHub.SharedKernel.CQRS;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.ApproveVendor;

public record ApproveVendorCommand(Guid VendorId, Guid ApprovedById) : ICommand;
