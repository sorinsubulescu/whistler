using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public interface ICommand<T> where T : RestResponse
    {
        Task<T> Execute(CancellationToken cancellationToken = default);
    }
}