using MediatR;
using ProcureHub.Modules.VendorManagement.Domain.Repositories;
using ProcureHub.SharedKernel.Caching;
using ProcureHub.SharedKernel.CQRS;
using ProcureHub.SharedKernel.Exceptions;

namespace ProcureHub.Modules.VendorManagement.Application.Commands.BlacklistVendor;

public class BlacklistVendorCommandHandler : ICommandHandler<BlacklistVendorCommand>
{
    private readonly IVendorRepository _repo;
    private readonly ICacheService     _cache;

    public BlacklistVendorCommandHandler(IVendorRepository repo, ICacheService cache)
    {
        _repo  = repo;
        _cache = cache;
    }

    public async Task<Unit> Handle(BlacklistVendorCommand command, CancellationToken ct)
    {
        var vendor = await _repo.GetByIdAsync(command.VendorId, ct)
            ?? throw new NotFoundException("Vendor", command.VendorId);

        vendor.Blacklist(command.Reason, command.BlacklistedById);

        _repo.Update(vendor);
        await _repo.SaveChangesAsync(ct);

        _cache.RemoveByPrefix(CacheKeys.Vendors.Prefix);

        return Unit.Value;
    }
}
