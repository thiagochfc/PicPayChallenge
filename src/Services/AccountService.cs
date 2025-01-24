using Microsoft.EntityFrameworkCore;

using Moonad;

using PicPayChallenge.Infrastructure;
using PicPayChallenge.Models;

namespace PicPayChallenge.Services;

public interface IAccountService
{
    Task<Option<Account>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task SaveAsync(Account account, CancellationToken cancellationToken);
}

public class AccountService(AppDbContext dbContext) : IAccountService
{
    public async Task<Option<Account>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await dbContext
            .Accounts
            .SingleOrDefaultAsync(account => account.Id.Equals(id), cancellationToken)
            .ConfigureAwait(false);

        return account.ToOption();
    }

    public async Task SaveAsync(Account account, CancellationToken cancellationToken)
    {
        dbContext.Accounts.Update(account);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
