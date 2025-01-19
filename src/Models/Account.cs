using Moonad;

namespace PicPayChallenge.Models;

public class Account
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string CpfCnpj { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public TypeAccount Type { get; init; }
    public decimal Balance { get; private set; }

    private Account(Guid id,
        string fullName,
        string cpfCnpj,
        string email,
        string password,
        TypeAccount type,
        decimal balance) =>
        (Id, FullName, CpfCnpj, Email, Password, Type, Balance) =
        (id, fullName, cpfCnpj, email, password, type, balance);

    public static Result<Account, IAccountErrors> Create(string fullname, string cpfCnpj, string email, string password, TypeAccount type,
        decimal initialBalance = 0.00m)
    {
        if (initialBalance < 0.00m)
        {
            return AccountErrors.NegativeError;
        }

        Account account = new(Guid.CreateVersion7(), fullname, cpfCnpj, email, password, type, initialBalance); 
        return account;
    }

    public Result<IAccountErrors> Deposit(decimal value)
    {
        if (value < 0.00m)
        {
            return AccountErrors.NegativeError;
        }
        
        Balance += value;
        return Result<IAccountErrors>.Ok();
    }

    public Result<IAccountErrors> Withdraw(decimal value)
    {
        if ((value < 0.00m) || (Balance - value < 0.00m)) 
        {
            return AccountErrors.NegativeError;
        }
        
        Balance -= value;
        return Result<IAccountErrors>.Ok();
    }
}
