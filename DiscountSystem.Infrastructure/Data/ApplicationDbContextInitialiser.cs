using DiscountSystem.Domain.Entities;
using DiscountSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscountSystem.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;

    public ApplicationDbContextInitialiser (ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<ApplicationDbContextInitialiser> logger
        )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
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
            foreach(var roleName in Enum.GetNames(typeof(ApplicationRole)))
            {
                if (!await _roleManager.RoleExistsAsync(roleName)) 
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
                }
            }

            var adminEmail = "admin@test.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    Email = adminEmail,
                    FirstName = "Administrator",
                    LastName = "User",
                    Location = "Belarus",
                    UserName = adminEmail
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123");

                if (result.Succeeded)
                { 
                    await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin.ToString());
                }
                else
                {
                    _logger.LogError("Admin creation failed");
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while seeding the database.");
        }
    }
}
