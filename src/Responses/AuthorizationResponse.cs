namespace PicPayChallenge.Responses;

public record AuthorizationResponseData(bool Authorization);
public record AuthorizationResponse(string Status, AuthorizationResponseData Data);
