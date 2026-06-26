using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Entities;

public class VendorUser : BaseAuditableEntity
{
    public Guid   VendorId    { get; set; }
    public string KeycloakId  { get; set; } = string.Empty;
    public string Email       { get; set; } = string.Empty;
    public string FullName    { get; set; } = string.Empty;
    public string Role        { get; set; } = string.Empty;
    public bool   IsActive    { get; set; } = true;

    public Vendor? Vendor { get; set; }
}
