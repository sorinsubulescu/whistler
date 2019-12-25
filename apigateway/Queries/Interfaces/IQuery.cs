using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public interface IQuery<T>
    {
        Task<T> Execute(CancellationToken cancellationToken = default);
    }
}