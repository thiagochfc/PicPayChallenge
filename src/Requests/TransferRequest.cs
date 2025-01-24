using System.ComponentModel.DataAnnotations;

namespace PicPayChallenge.Requests;

public record TransferRequest(
    [Required]
    Guid Payer,
    [Required]
    Guid Payee,
    [Required]
    decimal Value);
