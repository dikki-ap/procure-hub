using ProcureHub.Modules.VendorManagement.Domain.Entities;

namespace ProcureHub.Modules.VendorManagement.Domain.Repositories;

public interface IVendorDocumentRepository
{
    Task<List<VendorDocument>> GetByVendorIdAsync(Guid vendorId, CancellationToken ct = default);
    Task<VendorDocument?>      GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<VendorDocument>> GetExpiringAsync(int withinDays, CancellationToken ct = default);
    void                       Add(VendorDocument document);
    void                       Remove(VendorDocument document);
    Task<int>                  SaveChangesAsync(CancellationToken ct = default);
}
