namespace PerfumeStoreApi.Service.Interfaces;

public interface ITokenBlacklistService
{
    void RevokeToken(string jti, DateTimeOffset expiry);
    bool IsRevoked(string jti);
}
