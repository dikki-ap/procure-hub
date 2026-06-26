using MediatR;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;
using ProcureHub.SharedKernel.Caching;
using ProcureHub.SharedKernel.CQRS;
using ProcureHub.SharedKernel.Exceptions;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.ApproveVendor;

public class ApproveVendorCommandHandler : ICommandHandler<ApproveVendorCommand>
{
    private readonly IVendorRepository _repo;
    private readonly ICacheService     _cache;

    public ApproveVendorCommandHandler(IVendorRepository repo, ICacheService cache)
    {
        _repo  = repo;
        _cache = cache;
    }

    public async Task<Unit> Handle(ApproveVendorCommand command, CancellationToken ct)
    {
        var vendor = await _repo.GetByIdAsync(command.VendorId, ct)
            ?? throw new NotFoundException("Vendor", command.VendorId);

        vendor.Approve(command.ApprovedById);

        _repo.Update(vendor);
        await _repo.SaveChangesAsync(ct);

        _cache.RemoveByPrefix(CacheKeys.Vendors.Prefix);

        return Unit.Value;
    }
}
