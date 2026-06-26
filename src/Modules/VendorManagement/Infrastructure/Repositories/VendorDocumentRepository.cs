using Microsoft.EntityFrameworkCore;
using ProcureHub.Modules.VendorManagement.Domain.Entities;
using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;
using ProcureHub.SharedKernel.Database;

namespace ProcureHub.Modules.VendorManagement.Infrastructure.Repositories;

public class VendorDocumentRepository : IVendorDocumentRepository
{
    private readonly ApplicationDbContext _db;

    public VendorDocumentRepository(ApplicationDbContext db) => _db = db;

    public Task<List<VendorDocument>> GetByVendorIdAsync(Guid vendorId, CancellationToken ct = default)
        => _db.Set<VendorDocument>()
              .Where(d => d.VendorId == vendorId)
              .OrderByDescending(d => d.CreatedAt)
              .ToListAsync(ct);

    public Task<VendorDocument?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Set<VendorDocument>()
              .FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<List<VendorDocument>> GetExpiringAsync(int withinDays, CancellationToken ct = default)
    {
        var threshold = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(withinDays));
        return _db.Set<VendorDocument>()
                  .Include(d => d.Vendor)
                  .Where(d => d.Status == DocumentStatus.Active
                           && d.ExpiredAt.HasValue
                           && d.ExpiredAt.Value <= threshold)
                  .ToListAsync(ct);
    }

    public void Add(VendorDocument document)    => _db.Set<VendorDocument>().Add(document);
    public void Remove(VendorDocument document) => _db.Set<VendorDocument>().Remove(document);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
