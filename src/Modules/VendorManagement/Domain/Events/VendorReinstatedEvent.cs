using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Events;

public record VendorReinstatedEvent(Guid VendorId, string LegalName) : IDomainEvent;
