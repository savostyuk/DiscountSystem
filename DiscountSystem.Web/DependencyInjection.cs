using DiscountSystem.Application.Common;
using DiscountSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddScoped<IUser, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        return services;
    }
}
