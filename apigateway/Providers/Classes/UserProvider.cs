using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public class UserProvider
    {
         private readonly IDatabase _database;

        public UserProvider(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<User> GetById(string userId, CancellationToken cancellationToken = default)
            => _database.GetOneByFilter(UserIdFilter(userId), CollectionName.Users,
                cancellationToken: cancellationToken);

        public Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default)
            => _database.GetOneByFilter(UserNameFilter(userName), CollectionName.Users,
                new FindOptions<User> {Collation = CollationHelper.EnUsInvariantCase},
                cancellationToken);

        public Task<User> GetByEmail(string emailAddress, CancellationToken cancellationToken = default)
            => _database.GetOneByFilter(EmailAddressFilter(emailAddress), CollectionName.Users,
                new FindOptions<User> {Collation = CollationHelper.EnUsInvariantCase},
                cancellationToken);

        public Task<bool> Add(User user, CancellationToken cancellationToken = default)
            => _database.InsertOne(user, CollectionName.Users, cancellationToken);

        public Task<bool> Update(string id, UpdateDefinition<User> updateDefinition,
            CancellationToken cancellationToken = default)
            => _database.UpdateOne(id, updateDefinition, CollectionName.Users, cancellationToken);

        private static FilterDefinition<User> UserIdFilter(string userId) =>
            Builders<User>.Filter.Eq(e => e.Id, userId);

        private static FilterDefinition<User> UserNameFilter(string userName) =>
            Builders<User>.Filter.Eq(e => e.UserName, userName);

        private static FilterDefinition<User> EmailAddressFilter(string emailAddress) =>
            Builders<User>.Filter.Eq(e => e.Email, emailAddress);

        public Task<long> CountAll(CancellationToken cancellationToken = default) =>
            _database.CountByFilter(Builders<User>.Filter.Empty, CollectionName.Users, cancellationToken);
    }
}