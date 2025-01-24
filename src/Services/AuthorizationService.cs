using Moonad;

using PicPayChallenge.Responses;

namespace PicPayChallenge.Services;

public interface IAuthorizationService
{
    Task<Result> IsAuthorized(CancellationToken cancellationToken);
}

public class AuthorizationService(
    IHttpClientFactory httpClientFactory,
    ILogger<AuthorizationService> logger) : IAuthorizationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Authorization");

    public async Task<Result> IsAuthorized(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Requesting the authorization");
            
            var resultAuthorize = await _httpClient
                .GetFromJsonAsync<AuthorizationResponse>(_httpClient.BaseAddress, cancellationToken)
                .ConfigureAwait(false);

            return (resultAuthorize?.Data.Authorization ?? false) 
                ? Result.Ok() 
                : Result.Error();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Requesting the authorization failed");
            return Result.Error();
        }
    }
}
