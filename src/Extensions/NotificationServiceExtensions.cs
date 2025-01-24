using PicPayChallenge.Services;

namespace PicPayChallenge.Extensions;

public static class NotificationServiceExtensions
{
    public static void AddNotificationServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddHttpClient("Notification", options =>
        {
            string uri = builder.Configuration.GetSection("Notification:UrlBase").Value ??
                         throw new ArgumentException("Notification:UrlBase");
            options.BaseAddress = new Uri(uri);
            options.Timeout = TimeSpan.FromSeconds(5);
        });
    }
}
