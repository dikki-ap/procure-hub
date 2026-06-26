using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Entities;

public class VendorContact : BaseAuditableEntity
{
    public Guid    VendorId  { get; set; }
    public string  Name      { get; set; } = string.Empty;
    public string? Position  { get; set; }
    public string? Email     { get; set; }
    public string? Phone     { get; set; }
    public bool    IsPrimary { get; set; } = false;

    public Vendor? Vendor { get; set; }
}
