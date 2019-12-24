using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public interface IRefreshTokenProvider
    {
        Task<AuthToken> GetByTokenString(string tokenString, CancellationToken cancellationToken = default);

        Task<IEnumerable<AuthToken>> GetAllExpiredByUserId(string userId, CancellationToken cancellationToken = default);

        Task<bool> Add(AuthToken token, CancellationToken cancellationToken = default);

        Task<bool> Update(string id, UpdateDefinition<AuthToken> updateDefinition,
            CancellationToken cancellationToken = default);

        Task<bool> Delete(string id, CancellationToken cancellationToken = default);

        Task<bool> DeleteByTokenString(string tokenString, CancellationToken cancellationToken = default);
    }
}