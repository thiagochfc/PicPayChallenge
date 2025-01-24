using PicPayChallenge.Services;

namespace PicPayChallenge.Extensions;

public static class TransferServiceExtensions
{
    public static void AddTransferServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddTransient<ITransferService, TransferService>();
    }
}
