using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Registrars;

namespace Soenneker.Google.Credentials.Registrars;

/// <summary>
/// Registers the service-account credential cache.
/// </summary>
public static class GoogleCredentialsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGoogleCredentialsUtil"/> as a singleton service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddGoogleCredentialsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddFileUtilAsSingleton().TryAddSingleton<IGoogleCredentialsUtil, GoogleCredentialsUtil>();
        return services;
    }

    /// <summary>
    /// Adds <see cref="IGoogleCredentialsUtil"/> as a scoped service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddGoogleCredentialsUtilAsScoped(this IServiceCollection services)
    {
        services.AddFileUtilAsScoped().TryAddScoped<IGoogleCredentialsUtil, GoogleCredentialsUtil>();
        return services;
    }
}
