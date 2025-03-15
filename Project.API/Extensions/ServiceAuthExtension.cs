


namespace Project.API.Extensions
{
    public static class ServiceAuthExtension
    {
        public static IServiceCollection AddServiceAuth(this IServiceCollection services)
        {
            {
                services.AddTransient<IAuthenticationServices, AuthenticationServices>();
                services.AddTransient<IAuthorizationService, AuthorizationService>();
                services.AddTransient<IOfficesServices, OfficesServices>();
                services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
                services.AddTransient<IEmailServices, EmailServices>();
                services.AddTransient<IApplicationUserServices, ApplicationUserServices>();

                return services;
            }
        }

    }
}