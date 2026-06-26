using Microsoft.EntityFrameworkCore;
using ProcureHub.Modules.MasterData.Domain.Entities;
using ProcureHub.SharedKernel.Database;
using ProcureHub.SharedKernel.Domain;

namespace ProcureHub.API.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedMasterDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db          = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await db.Companies.AnyAsync()) return;

        // ── Company ──────────────────────────────────────────────────────────

        var company = new Company
        {
            Name     = "PT ProcureHub Indonesia",
            Code     = "PRCH",
            Type     = "Internal",
            IsActive = true,
        };
        db.Companies.Add(company);
        await db.SaveChangesAsync();

        // ── Currencies (global — no CompanyId) ───────────────────────────────

        db.Set<Currency>().AddRange(
            new Currency { Code = "IDR", Name = "Indonesian Rupiah", Symbol = "Rp",  ExchangeRate = 1m,     IsBase = true  },
            new Currency { Code = "USD", Name = "US Dollar",         Symbol = "$",   ExchangeRate = 16000m, IsBase = false },
            new Currency { Code = "EUR", Name = "Euro",              Symbol = "€",   ExchangeRate = 17500m, IsBase = false },
            new Currency { Code = "SGD", Name = "Singapore Dollar",  Symbol = "S$",  ExchangeRate = 12000m, IsBase = false }
        );
        await db.SaveChangesAsync();

        // ── Units of Measure ─────────────────────────────────────────────────

        db.Set<UnitOfMeasure>().AddRange(
            new UnitOfMeasure { CompanyId = company.Id, Code = "KG",   Name = "Kilogram"  },
            new UnitOfMeasure { CompanyId = company.Id, Code = "TON",  Name = "Ton"       },
            new UnitOfMeasure { CompanyId = company.Id, Code = "PCS",  Name = "Pieces"    },
            new UnitOfMeasure { CompanyId = company.Id, Code = "LTR",  Name = "Liter"     },
            new UnitOfMeasure { CompanyId = company.Id, Code = "MTR",  Name = "Meter"     },
            new UnitOfMeasure { CompanyId = company.Id, Code = "BOX",  Name = "Box"       },
            new UnitOfMeasure { CompanyId = company.Id, Code = "SET",  Name = "Set"       },
            new UnitOfMeasure { CompanyId = company.Id, Code = "UNIT", Name = "Unit"      }
        );
        await db.SaveChangesAsync();

        // ── Payment Terms ────────────────────────────────────────────────────

        db.Set<PaymentTerm>().AddRange(
            new PaymentTerm { CompanyId = company.Id, Code = "COD",   Name = "Cash on Delivery",       Days = 0,  Description = "Payment upon delivery" },
            new PaymentTerm { CompanyId = company.Id, Code = "NET7",  Name = "Net 7 Days",             Days = 7                                         },
            new PaymentTerm { CompanyId = company.Id, Code = "NET14", Name = "Net 14 Days",            Days = 14                                        },
            new PaymentTerm { CompanyId = company.Id, Code = "NET30", Name = "Net 30 Days",            Days = 30                                        },
            new PaymentTerm { CompanyId = company.Id, Code = "NET60", Name = "Net 60 Days",            Days = 60                                        },
            new PaymentTerm { CompanyId = company.Id, Code = "DP50",  Name = "Down Payment 50%",       Days = 30, Description = "50% upfront, remainder on delivery" },
            new PaymentTerm { CompanyId = company.Id, Code = "DP30",  Name = "Down Payment 30%",       Days = 30, Description = "30% upfront, remainder on delivery" }
        );
        await db.SaveChangesAsync();

        // ── Material Categories ───────────────────────────────────────────────

        db.Set<MaterialCategory>().AddRange(
            new MaterialCategory { CompanyId = company.Id, Code = "RM",    Name = "Raw Material",        IsStrategic = true  },
            new MaterialCategory { CompanyId = company.Id, Code = "SP",    Name = "Spare Parts",         IsStrategic = false },
            new MaterialCategory { CompanyId = company.Id, Code = "MRO",   Name = "Maintenance & Repair",IsStrategic = false },
            new MaterialCategory { CompanyId = company.Id, Code = "CAPEX", Name = "Capital Expenditure", IsStrategic = true  },
            new MaterialCategory { CompanyId = company.Id, Code = "PKG",   Name = "Packaging",           IsStrategic = false },
            new MaterialCategory { CompanyId = company.Id, Code = "CONS",  Name = "Consumables",         IsStrategic = false }
        );
        await db.SaveChangesAsync();

        // ── Locations ────────────────────────────────────────────────────────

        db.Set<Location>().AddRange(
            new Location { CompanyId = company.Id, Name = "Main Warehouse",      Type = "warehouse", City = "Jakarta",        Country = "Indonesia" },
            new Location { CompanyId = company.Id, Name = "Production Plant A",  Type = "plant",     City = "Cikarang",       Country = "Indonesia" },
            new Location { CompanyId = company.Id, Name = "Head Office",         Type = "office",    City = "Jakarta Selatan", Country = "Indonesia" }
        );
        await db.SaveChangesAsync();
    }
}
