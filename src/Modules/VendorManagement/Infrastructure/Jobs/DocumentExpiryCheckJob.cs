using Hangfire;
using Microsoft.Extensions.Logging;
using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;

namespace ProcureHub.Modules.VendorManagement.Infrastructure.Jobs;

public class DocumentExpiryCheckJob
{
    private readonly IVendorDocumentRepository _docRepo;
    private readonly ILogger<DocumentExpiryCheckJob> _logger;

    private const int ExpiryWarningDays = 30;

    public DocumentExpiryCheckJob(
        IVendorDocumentRepository docRepo,
        ILogger<DocumentExpiryCheckJob> logger)
    {
        _docRepo = docRepo;
        _logger  = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var today  = DateOnly.FromDateTime(DateTime.UtcNow);
        var expiring = await _docRepo.GetExpiringAsync(ExpiryWarningDays, ct);

        foreach (var doc in expiring)
        {
            if (doc.ExpiredAt.HasValue && doc.ExpiredAt.Value < today)
            {
                doc.Status = DocumentStatus.Expired;
                _logger.LogInformation(
                    "Document expired — DocId: {DocId}, VendorId: {VendorId}, Type: {Type}, ExpiredAt: {ExpiredAt}",
                    doc.Id, doc.VendorId, doc.DocumentType, doc.ExpiredAt);
            }
            else
            {
                _logger.LogInformation(
                    "Document expiring soon — DocId: {DocId}, VendorId: {VendorId}, Type: {Type}, ExpiredAt: {ExpiredAt}",
                    doc.Id, doc.VendorId, doc.DocumentType, doc.ExpiredAt);
            }
        }

        if (expiring.Count > 0)
            await _docRepo.SaveChangesAsync(ct);

        _logger.LogInformation(
            "DocumentExpiryCheckJob completed. Processed: {Count} documents.", expiring.Count);
    }
}
