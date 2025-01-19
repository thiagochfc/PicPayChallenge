using Moonad;

using PicPayChallenge.Models;

using Shouldly;

namespace PicPayChallenge.UnitTests;

public class AccountTests
{

    private static Result<Account, IAccountErrors> CreateAccount(TypeAccount type, decimal initialBalance) =>
        Account.Create("John Doe", "cpf", "email", "password", type, initialBalance);
    
    
    [Theory]
    [InlineData(TypeAccount.User, 0.00)]
    [InlineData(TypeAccount.Shopkeeper, 500.00)]
    public void ShouldCreateAccountSuccessfully(TypeAccount type, decimal initialBalance)
    {
        // Act 
        var accountResult = CreateAccount(type, initialBalance);
        bool assert = accountResult;

        // Assert
        assert.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TypeAccount.User, -0.01)]
    [InlineData(TypeAccount.Shopkeeper, -1.00)]
    public void ShouldNotCreateAccountDueToInvalidBalance(TypeAccount type, decimal initialBalance)
    {
        // Act 
        var accountResult = CreateAccount(type, initialBalance);
        bool assert = accountResult;
        
        // Assert
        assert.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TypeAccount.User, 0.00, 0.01)]
    [InlineData(TypeAccount.Shopkeeper, 597.33, 1578.98)]
    public void ShouldDepositSuccessfully(TypeAccount type, decimal initialBalance, decimal deposit)
    {
        // Arrange
        Account account = CreateAccount(type, initialBalance);
        decimal expected = initialBalance + deposit;

        // Act
        var resultDeposit = account.Deposit(deposit);
        bool assert = resultDeposit;
        
        // Assert
        assert.ShouldBeTrue();
        account.Balance.ShouldBe(expected);
    }
    
    [Theory]
    [InlineData(TypeAccount.User, 0.00, -0.01)]
    [InlineData(TypeAccount.Shopkeeper, 597.33, -1578.98)]
    public void ShouldNotDepositDueToInvalidValue(TypeAccount type, decimal initialBalance, decimal deposit)
    {
        // Arrange
        Account account = CreateAccount(type, initialBalance);

        // Act
        var resultDeposit = account.Deposit(deposit);
        bool assert = resultDeposit;

        // Assert
        assert.ShouldBeFalse();
        account.Balance.ShouldBe(initialBalance);
    }

    [Theory]
    [InlineData(TypeAccount.User, 500.00, 250.00)]
    [InlineData(TypeAccount.Shopkeeper, 597.33, 0.01)]
    public void ShouldWithdrawSuccessfully(TypeAccount type, decimal initialBalance, decimal withdraw)
    {
        // Arrange
        Account account = CreateAccount(type, initialBalance);
        decimal expected = initialBalance - withdraw;

        // Act
        var resultDeposit = account.Withdraw(withdraw);
        bool assert = resultDeposit;
        
        // Assert
        assert.ShouldBeTrue();
        account.Balance.ShouldBe(expected);
    }
    
    [Theory]
    [InlineData(TypeAccount.User, 0.00, 0.01)]
    [InlineData(TypeAccount.User, 5.00, -0.01)]
    [InlineData(TypeAccount.Shopkeeper, 597.33, 1578.98)]
    public void ShouldNotWithdrawDueToInvalidValue(TypeAccount type, decimal initialBalance, decimal withdraw)
    {
        // Arrange
        Account account = CreateAccount(type, initialBalance);

        // Act
        var resultDeposit = account.Withdraw(withdraw);
        bool assert = resultDeposit;

        // Assert
        assert.ShouldBeFalse();
        account.Balance.ShouldBe(initialBalance);
    }
}
