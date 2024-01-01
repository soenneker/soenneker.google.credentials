using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Google.Credentials.Abstract;
using Soenneker.Utils.File.Registrars;

namespace Soenneker.Google.Credentials.Registrars;

/// <summary>
/// An async thread-safe singleton for Google OAuth credentials
/// </summary>
public static class GoogleCredentialsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGoogleCredentialsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static void AddGoogleCredentialsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddFileUtilAsSingleton();
        services.TryAddSingleton<IGoogleCredentialsUtil, GoogleCredentialsUtil>();
    }

    /// <summary>
    /// Adds <see cref="IGoogleCredentialsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddGoogleCredentialsUtilAsScoped(this IServiceCollection services)
    {
        services.AddFileUtilAsScoped();
        services.TryAddScoped<IGoogleCredentialsUtil, GoogleCredentialsUtil>();
    }
}
