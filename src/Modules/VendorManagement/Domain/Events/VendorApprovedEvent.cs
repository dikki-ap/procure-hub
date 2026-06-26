using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Events;

public record VendorApprovedEvent(Guid VendorId, string LegalName, Guid ApprovedById) : IDomainEvent;
