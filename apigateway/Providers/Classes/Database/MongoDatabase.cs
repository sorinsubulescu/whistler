using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public sealed class MongoDatabase : IDatabase
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDatabase(string connectionString = null)
        {
            connectionString ??= Environment.GetEnvironmentVariable("MONGO_URL");
            if (string.IsNullOrEmpty(connectionString))
                connectionString = "mongodb://127.0.0.1:27017";

            var client = new MongoClient(connectionString);
            _mongoDatabase = client.GetDatabase("whistler");
        }

        public async Task<bool> DeleteOne<T>(string itemId, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);
            var deleteResult = await collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", itemId), cancellationToken)
                .ConfigureAwait(false);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1;
        }

        public async Task<bool> DeleteOne<T>(FilterDefinition<T> filterDefinition, string collectionName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);
            var deleteResult = await collection.DeleteOneAsync(filterDefinition, cancellationToken)
                .ConfigureAwait(false);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1;
        }

        public async Task<bool> UpdateOne<T>(string itemId, UpdateDefinition<T> updateDefinition, string collectionName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);

            var updateResult = await collection
                .UpdateOneAsync(Builders<T>.Filter.Eq("Id", itemId), updateDefinition,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable &&
                   updateResult.ModifiedCount == 1;
        }

        public async Task<bool> UpdateOneByFilter<T>(string itemId, FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);
            var updateResult = await collection
                .UpdateOneAsync(Builders<T>.Filter.And(Builders<T>.Filter.Eq("Id", itemId), filterDefinition), updateDefinition,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable &&
                   updateResult.ModifiedCount == 1;
        }

        public async Task<bool> UpdateAll<T>(UpdateDefinition<T> updateDefinition, string collectionName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);

            var updateResult = await collection
                .UpdateManyAsync(EmptyFilter<T>(), updateDefinition, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable &&
                   updateResult.ModifiedCount > 0;
        }

        public async Task<bool> ReplaceOne<T>(string itemId, T replacement, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);

            var replaceResult = await collection
                .ReplaceOneAsync(Builders<T>.Filter.Eq("Id", itemId), replacement, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return replaceResult.IsAcknowledged && replaceResult.IsModifiedCountAvailable &&
                   replaceResult.ModifiedCount == 1;
        }

        public async Task<bool> InsertOne<T>(T item, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);
            await collection.InsertOneAsync(item, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return true;
        }

        public async Task InsertMany<T>(IEnumerable<T> items, string collectionName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var collection = GetCollection<T>(collectionName);
            await collection.InsertManyAsync(items, null, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAll<T>(string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var results = await GetCollection<T>(collectionName)
                .FindAsync(EmptyFilter<T>(), cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return results.ToList(cancellationToken);
        }

        public async Task<T> GetOneByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            FindOptions<T> findOptions = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await GetCollection<T>(collectionName)
                .FindAsync(filterDefinition, findOptions, cancellationToken)
                .ConfigureAwait(false);

            return await result.FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<long> CountByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await GetCollection<T>(collectionName)
                .CountDocumentsAsync(filterDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await GetCollection<T>(collectionName)
                .Find(filterDefinition).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllPagedByFilterAndSort<T>(FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int startIndex,
            int count, string collectionName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await GetCollection<T>(collectionName)
                .Find(filterDefinition).Sort(sortDefinition).Skip(startIndex).Limit(count)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ExistsByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            FindOptions<T> findOptions = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await GetCollection<T>(collectionName)
                .FindAsync(filterDefinition, findOptions, cancellationToken)
                .ConfigureAwait(false);

            return await result.AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName) =>
            _mongoDatabase.GetCollection<T>(collectionName);

        private static FilterDefinition<T> EmptyFilter<T>() => Builders<T>.Filter.Empty;
    }
}
