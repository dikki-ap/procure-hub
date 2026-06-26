using MediatR;
using Microsoft.Extensions.Logging;
using ProcureHub.Modules.VendorManagement.Domain.Events;

namespace ProcureHub.Modules.VendorManagement.Infrastructure.EventHandlers;

public class VendorBlacklistedEventHandler : INotificationHandler<VendorBlacklistedEvent>
{
    private readonly ILogger<VendorBlacklistedEventHandler> _logger;

    public VendorBlacklistedEventHandler(ILogger<VendorBlacklistedEventHandler> logger)
        => _logger = logger;

    public Task Handle(VendorBlacklistedEvent notification, CancellationToken ct)
    {
        _logger.LogWarning(
            "Vendor blacklisted — VendorId: {VendorId}, LegalName: {LegalName}, Reason: {Reason}, BlacklistedById: {BlacklistedById}",
            notification.VendorId,
            notification.LegalName,
            notification.Reason,
            notification.BlacklistedById);

        // TODO: Notify procurement team and revoke active PO/RFQ access.
        return Task.CompletedTask;
    }
}
