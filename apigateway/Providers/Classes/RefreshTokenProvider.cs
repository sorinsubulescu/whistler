using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    internal sealed class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IDatabase _database;

        public RefreshTokenProvider(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<AuthToken> GetByTokenString(string tokenString, CancellationToken cancellationToken = default)
            => _database.GetOneByFilter(TokenStringFilter(tokenString), CollectionName.Token,
                cancellationToken: cancellationToken);

        public Task<IEnumerable<AuthToken>> GetAllExpiredByUserId(string userId, CancellationToken cancellationToken = default)
            => _database.GetAllByFilter(ExpiredTokensByUserIdFilter(userId), CollectionName.Token, cancellationToken);

        public Task<bool> Add(AuthToken token, CancellationToken cancellationToken = default)
            => _database.InsertOne(token, CollectionName.Token, cancellationToken);

        public Task<bool> Update(string id, UpdateDefinition<AuthToken> updateDefinition,
            CancellationToken cancellationToken = default)
            => _database.UpdateOne(id, updateDefinition, CollectionName.Token, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default)
            => _database.DeleteOne<AuthToken>(id, CollectionName.Token, cancellationToken);

        public Task<bool> DeleteByTokenString(string tokenString, CancellationToken cancellationToken = default)
            => _database.DeleteOne(TokenStringFilter(tokenString), CollectionName.Token, cancellationToken);

        private static FilterDefinition<AuthToken> UserIdFilter(string userId) =>
            Builders<AuthToken>.Filter.Eq(e => e.UserId, userId);

        private static FilterDefinition<AuthToken> RefreshTokenFilter() =>
            Builders<AuthToken>.Filter.Eq(e => e.TokenType, AuthTokenType.SessionToken);

        private static FilterDefinition<AuthToken> ExpiredTokenFilter() =>
            Builders<AuthToken>.Filter.Lt(e => e.Expires, DateTime.UtcNow);

        private static FilterDefinition<AuthToken> ExpiredTokensByUserIdFilter(string userId) =>
            Builders<AuthToken>.Filter.And(UserIdFilter(userId), RefreshTokenFilter(), ExpiredTokenFilter());

        private static FilterDefinition<AuthToken> TokenStringFilter(string tokenString) =>
            Builders<AuthToken>.Filter.Eq(e => e.TokenString, tokenString);
    }
}