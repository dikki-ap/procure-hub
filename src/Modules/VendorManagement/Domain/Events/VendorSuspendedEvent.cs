using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Events;

public record VendorSuspendedEvent(Guid VendorId, string LegalName) : IDomainEvent;
