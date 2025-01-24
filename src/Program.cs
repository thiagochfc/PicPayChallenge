using Microsoft.EntityFrameworkCore;

using Moonad;

using PicPayChallenge.Extensions;
using PicPayChallenge.Infrastructure;
using PicPayChallenge.Models;
using PicPayChallenge.Requests;
using PicPayChallenge.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.AddAuthorizationServices();
builder.AddNotificationServices();
builder.AddTransferServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/accounts", async (
    ILogger<Program> logger,
    AppDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    var existAccounts = await dbContext
        .Accounts
        .AsNoTrackingWithIdentityResolution()
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);

    if (!existAccounts)
    {
        logger.LogInformation("Account not exists!");

        IEnumerable<Account> accountsCreate =
        [
            Account.Create("Ariana Estellet Ferreira",
                "18771618295",
                "Ariana.estellet@gmail.com",
                "password",
                TypeAccount.User,
                500.50m),
            Account.Create("Caruso Monteiro Laboratório ME",
                "40885265000139",
                "caruso.monteiro@gmail.com",
                "password",
                TypeAccount.Shopkeeper,
                10_000.00m),
            Account.Create("Firmino Torres Barros",
                "64237451968",
                "firmino.barros@gmail.com",
                "password",
                TypeAccount.User,
                5000m),
        ];

        foreach (Account account in accountsCreate)
        {
            await dbContext.Accounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    else
    {
        logger.LogInformation("Accounts already exists!");
    }

    var accounts = await dbContext
        .Accounts
        .AsNoTrackingWithIdentityResolution()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    return TypedResults.Ok(accounts);
});

app.MapPost("/transfer", async (
    TransferRequest request,
    IAccountService accountService,
    ITransferService transferService,
    ILogger<Program> logger,
    CancellationToken cancellationToken) =>
{
    var payerOption = await accountService
        .GetByIdAsync(request.Payer, cancellationToken)
        .ConfigureAwait(false);

    if (payerOption.IsNone)
    {
        logger.LogInformation("The payer does not exist");
        return Results.BadRequest();
    }
    
    Option<Account> payeeOption = await accountService
        .GetByIdAsync(request.Payee, cancellationToken)
        .ConfigureAwait(false);

    if (payeeOption.IsNone)
    {
        logger.LogInformation("The payee does not exist");
        return Results.BadRequest();
    }
    
    var resultTransfer = await transferService
        .HandleAsync(payerOption, payeeOption, request.Value, cancellationToken)
        .ConfigureAwait(false);

    if (resultTransfer.IsError)
    {
        return resultTransfer.ErrorValue switch
        {
            InvalidTypeAccountError => Results.Conflict(),
            NoBalanceError => Results.UnprocessableEntity(),
            UnauthorizedError => Results.Unauthorized(),
            _ => Results.BadRequest()
        };
    }
    
    await accountService.SaveAsync(payerOption, cancellationToken).ConfigureAwait(false);
    await accountService.SaveAsync(payeeOption, cancellationToken).ConfigureAwait(false);

    return Results.Ok();
});

await app
    .RunAsync()
    .ConfigureAwait(false);
