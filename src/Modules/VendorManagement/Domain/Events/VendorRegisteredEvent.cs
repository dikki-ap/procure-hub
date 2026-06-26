using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Events;

public record VendorRegisteredEvent(Guid VendorId, string LegalName, string VendorCode) : IDomainEvent;
