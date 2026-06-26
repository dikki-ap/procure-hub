using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Events;

public record VendorBlacklistedEvent(Guid VendorId, string LegalName, string Reason, Guid BlacklistedById) : IDomainEvent;
