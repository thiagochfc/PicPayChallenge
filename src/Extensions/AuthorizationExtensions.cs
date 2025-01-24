using PicPayChallenge.Services;

namespace PicPayChallenge.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorizationServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddHttpClient("Authorization", options =>
        {
            string uri = builder.Configuration.GetSection("Authorization:UrlBase").Value ??
                         throw new ArgumentException("Authorization:UrlBase");
            options.BaseAddress = new Uri(uri);
            options.Timeout = TimeSpan.FromSeconds(1);
        });
    }
}
