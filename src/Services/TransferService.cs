using System.Diagnostics.CodeAnalysis;

using Moonad;

using PicPayChallenge.Models;

namespace PicPayChallenge.Services;

public interface ITransferService
{
    Task<Result<ITransferErrors>> HandleAsync(Account payer,
        Account payee,
        decimal amount,
        CancellationToken cancellationToken);
}

public class TransferService(
    IAuthorizationService authorizationService,
    INotificationService notificationService,
    ILogger<TransferService> logger) : ITransferService
{
    public async Task<Result<ITransferErrors>> HandleAsync(
        Account payer,
        Account payee,
        decimal amount,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payer);
        ArgumentNullException.ThrowIfNull(payee);

        if (payer.Type == TypeAccount.Shopkeeper)
        {
            logger.LogInformation("The payer is a shopkeeper, so it cannot make a transfer");
            return TransferErrors.InvalidTypeAccountError;
        }
        
        var resultWithdraw = payer.Withdraw(amount);

        if (resultWithdraw.IsError)
        {
            logger.LogInformation("The payer does not have enough balance to make the transfer");
            return TransferErrors.NoBalanceError;
        }
        
        payee.Deposit(amount);
        
        var authorized = await authorizationService.IsAuthorized(cancellationToken).ConfigureAwait(false);
        
        if (!authorized)
        {
            logger.LogInformation("Authorization failed");
            return TransferErrors.UnauthorizedError;
        }
        
        await notificationService.Notify(cancellationToken).ConfigureAwait(false);
        return Result<ITransferErrors>.Ok();
    }
}

#pragma warning disable CA1040
public interface ITransferErrors;
#pragma warning restore CA1040

[SuppressMessage("ReSharper", "UnassignedReadonlyField")]
public static class TransferErrors
{
    public static readonly InvalidTypeAccountError InvalidTypeAccountError;
    public static readonly NoBalanceError NoBalanceError;
    public static readonly UnauthorizedError UnauthorizedError;
}

public readonly struct InvalidTypeAccountError : ITransferErrors; 
public readonly struct NoBalanceError : ITransferErrors;

public readonly struct UnauthorizedError : ITransferErrors;
