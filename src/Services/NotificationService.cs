namespace PicPayChallenge.Services;

public interface INotificationService
{
    Task Notify(CancellationToken cancellationToken);
}

public class NotificationService(
    IHttpClientFactory httpClientFactory,
    ILogger<NotificationService> logger) : INotificationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Notification");

    public async Task Notify(CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending the notification");
        
        await _httpClient
            .GetAsync(_httpClient.BaseAddress, cancellationToken)
            .ConfigureAwait(false);   
    }
}
