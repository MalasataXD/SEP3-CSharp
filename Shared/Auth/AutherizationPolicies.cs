using Microsoft.Extensions.DependencyInjection;

namespace Shared.Auth;

public class AutherizationPolicies
{
    // NOTE: More Policies can be added here!
    public static void AddPolicies(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            // ¤ Must be Worker Policy
            options.AddPolicy("MustBeWorker", a
                => a.RequireAuthenticatedUser().RequireClaim("Role", "Worker", "Chef","Admin"));
            // ¤ Must be Chef Policy
            options.AddPolicy("MustBeWorker", a
                => a.RequireAuthenticatedUser().RequireClaim("Role", "Chef","Admin"));
            // ¤ Must be SysAdmin Policy
            options.AddPolicy("MustBeSysAdmin", a 
                => a.RequireAuthenticatedUser().RequireClaim("Role","Admin"));
        });
    }
}