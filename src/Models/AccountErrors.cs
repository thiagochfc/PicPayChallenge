using System.Diagnostics.CodeAnalysis;

namespace PicPayChallenge.Models;

#pragma warning disable CA1040
public interface IAccountErrors;
#pragma warning restore CA1040

[SuppressMessage("ReSharper", "UnassignedReadonlyField")]
public static class AccountErrors
{
    public static readonly NegativeError NegativeError;
}

public readonly struct NegativeError : IAccountErrors;
