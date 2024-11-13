using Microsoft.Extensions.DependencyInjection;

namespace DiscountSystem.Infrastructure.Data;

public static class InitialiserExtentions
{
    public static async Task InitialiseDbAsync (this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope ();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync ();
        await initialiser.SeedAsync ();
    }
}
