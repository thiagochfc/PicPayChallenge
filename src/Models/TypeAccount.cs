using System.Text.Json.Serialization;

namespace PicPayChallenge.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeAccount
{
    User,
    Shopkeeper,
}
