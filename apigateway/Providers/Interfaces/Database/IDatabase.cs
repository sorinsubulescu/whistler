using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public interface IDatabase
    {
        Task<bool> DeleteOne<T>(string itemId, string collectionName, CancellationToken cancellationToken = default);

        Task<bool> DeleteOne<T>(FilterDefinition<T> filterDefinition, string collectionName,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateOne<T>(string itemId, UpdateDefinition<T> updateDefinition, string collectionName,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateOneByFilter<T>(string itemId, FilterDefinition<T> filterDefinition,
            UpdateDefinition<T> updateDefinition, string collectionName, CancellationToken cancellationToken = default);

        Task<bool> UpdateAll<T>(UpdateDefinition<T> updateDefinition, string collectionName,
            CancellationToken cancellationToken = default);

        Task<bool> ReplaceOne<T>(string itemId, T replacement, string collectionName,
            CancellationToken cancellationToken = default);

        Task<bool> InsertOne<T>(T item, string collectionName, CancellationToken cancellationToken = default);

        Task InsertMany<T>(IEnumerable<T> items, string collectionName,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAll<T>(string collectionName, CancellationToken cancellationToken = default);

        Task<T> GetOneByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            FindOptions<T> findOptions = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllPagedByFilterAndSort<T>(FilterDefinition<T> filterDefinition,
            SortDefinition<T> sortDefinition, int startIndex,
            int count, string collectionName, CancellationToken cancellationToken = default);

        Task<long> CountByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByFilter<T>(FilterDefinition<T> filterDefinition, string collectionName,
            FindOptions<T> findOptions = null, CancellationToken cancellationToken = default);
    }
}