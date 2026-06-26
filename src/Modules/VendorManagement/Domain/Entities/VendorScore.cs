using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.Modules.VendorManagement.Domain.Entities;

public class VendorScore : BaseEntity
{
    public Guid        VendorId      { get; set; }
    public int         PeriodYear    { get; set; }
    public int         PeriodQuarter { get; set; }
    public decimal?    DeliveryScore { get; set; }
    public decimal?    QualityScore  { get; set; }
    public decimal?    PriceScore    { get; set; }
    public decimal?    ResponseScore { get; set; }
    public decimal?    DocScore      { get; set; }
    public decimal?    TotalScore    { get; set; }
    public VendorTier? Tier          { get; set; }
    public string?     Notes         { get; set; }
    public DateTime    CalculatedAt  { get; set; }
    public DateTime    CreatedAt     { get; set; }

    public Vendor? Vendor { get; set; }
}
