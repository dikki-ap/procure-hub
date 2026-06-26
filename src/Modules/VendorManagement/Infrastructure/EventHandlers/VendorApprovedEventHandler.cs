using MediatR;
using Microsoft.Extensions.Logging;
using ProcureHub.Modules.VendorManagement.Domain.Events;

namespace ProcureHub.Modules.VendorManagement.Infrastructure.EventHandlers;

public class VendorApprovedEventHandler : INotificationHandler<VendorApprovedEvent>
{
    private readonly ILogger<VendorApprovedEventHandler> _logger;

    public VendorApprovedEventHandler(ILogger<VendorApprovedEventHandler> logger)
        => _logger = logger;

    public Task Handle(VendorApprovedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation(
            "Vendor approved — VendorId: {VendorId}, LegalName: {LegalName}, ApprovedById: {ApprovedById}",
            notification.VendorId,
            notification.LegalName,
            notification.ApprovedById);

        // TODO: Send approval notification email to vendor primary contact.
        return Task.CompletedTask;
    }
}
