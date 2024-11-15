using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser (ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Users.Any()) 
            {
                var adminId = Guid.NewGuid();
                _context.Users.AddRange(
                    new Domain.Entities.User
                    {
                        Id = adminId,
                        FirstName = "Natalya",
                        LastName = "Shulzhenka",
                        Email = "text@gmail.com",
                        Role = "admin",
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = null,
                        LastModifiedAt = DateTime.Now,
                    },
                    new Domain.Entities.User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Anton",
                        LastName = "Romashko",
                        Email = "example@gmail.com",
                        Role = "user",
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now,
                    },
                    new Domain.Entities.User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Roman",
                        LastName = "Domaracki",
                        Email = "user@gmail.com",
                        Role = "user",
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now,
                    });
            }

            if (!_context.Vendors.Any())
            {
                _context.Vendors.AddRange(
                    new Domain.Entities.Vendor
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "ABC",
                        Email = "abc@gmail.com",
                        Website = "abc.com",
                        WorkingHours = "24/7",
                        Phone = "454444",
                        Address = "Warsaw",
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = null,
                        LastModifiedAt = DateTime.Now
                    },
                    new Domain.Entities.Vendor
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "Google",
                        Email = "google@gmail.com",
                        Website = "google.com",
                        WorkingHours = "24/7",
                        Phone = "555555",
                        Address = "Minsk",
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = null,
                        LastModifiedAt = DateTime.Now
                    },
                    new Domain.Entities.Vendor
                    {
                        Id = Guid.NewGuid(),
                        VendorName = "First",
                        Email = "first@gmail.com",
                        Website = "first.com",
                        WorkingHours = "24/7",
                        Phone = "3333",
                        Address = "London",
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = null,
                        LastModifiedAt = DateTime.Now
                    });
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
        }
    }
}
