using Microsoft.EntityFrameworkCore;

using Moonad;

using PicPayChallenge.Infrastructure;
using PicPayChallenge.Models;
using PicPayChallenge.Requests;
using PicPayChallenge.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddHttpClient("Authorization", options =>
{
    string uri = builder.Configuration.GetSection("Authorization:UrlBase").Value ?? throw new ArgumentException("Authorization:UrlBase"); 
    options.BaseAddress = new Uri(uri);
    options.Timeout = TimeSpan.FromSeconds(1);
});
builder.Services.AddHttpClient("Notification", options =>
{
    string uri = builder.Configuration.GetSection("Notification:UrlBase").Value ?? throw new ArgumentException("Notification:UrlBase"); 
    options.BaseAddress = new Uri(uri);
    options.Timeout = TimeSpan.FromSeconds(5);
});

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
    AppDbContext dbContext,
    IHttpClientFactory httpClientFactory,
    CancellationToken cancellationToken) =>
{
    var httpClientAuthorization = httpClientFactory.CreateClient("Authorization");
    var httpClientNotification = httpClientFactory.CreateClient("Notification");
    Option<Account> payerOption = await GetAccountByIdAsync(request.Payer).ConfigureAwait(false);
    
    if (payerOption.IsNone)
    {
        return Results.BadRequest();
    }

    var payer = payerOption.Get();
    if (payer.Type == TypeAccount.Shopkeeper)
    {
        return Results.Conflict();
    }
    
    Option<Account> payeeOption = await GetAccountByIdAsync(request.Payee).ConfigureAwait(false);
    
    if (payeeOption.IsNone)
    {
        return Results.BadRequest();
    }
    
    var payee = payeeOption.Get();

    var resultWithdraw = payer.Withdraw(request.Value);
    payee.Deposit(request.Value);

    if (resultWithdraw.IsError)
    {
        return Results.UnprocessableEntity();
    }

    var resultAuthorize = await httpClientAuthorization
        .GetFromJsonAsync<AuthorizationResponse>(httpClientAuthorization.BaseAddress, cancellationToken)
        .ConfigureAwait(false);

    if (!(resultAuthorize?.Data.Authorization ?? false))
    {
        return Results.Unauthorized();
    }
    
    dbContext.Accounts.Update(payer);
    dbContext.Accounts.Update(payee);
    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    await httpClientNotification.GetAsync(httpClientNotification.BaseAddress, cancellationToken).ConfigureAwait(false);
    
    return Results.Ok();

    Task<Account?> GetAccountByIdAsync(string id) =>
        dbContext
            .Accounts
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)), cancellationToken);
});

await app
    .RunAsync()
    .ConfigureAwait(false);
