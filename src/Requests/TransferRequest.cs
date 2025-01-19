using System.ComponentModel.DataAnnotations;

namespace PicPayChallenge.Requests;

public record TransferRequest(
    [Required]
    string Payer,
    [Required]
    string Payee,
    [Required]
    decimal Value);
