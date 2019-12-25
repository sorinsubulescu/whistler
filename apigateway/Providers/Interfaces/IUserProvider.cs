using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public interface IUserProvider
    {
        Task<User> GetById(string userId, CancellationToken cancellationToken = default);

        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default);

        Task<User> GetByEmail(string emailAddress, CancellationToken cancellationToken = default);

        Task<bool> Add(User user, CancellationToken cancellationToken = default);

        Task<bool> Update(string id, UpdateDefinition<User> updateDefinition,
            CancellationToken cancellationToken = default);

        Task<long> CountAll(CancellationToken cancellationToken = default);
    }
}