using Domain.Dtos;
using Domain.Monitoring;

namespace Domain.UseCases.GetForums;

internal class GetForumsUseCase(IGetForumsStorage storage, DomainMetrics metrics) : IGetForumsUseCase
{
    public async Task<IEnumerable<ForumDto>> Execute(CancellationToken cancellationToken)
    {
        try
        {
            var result = await storage.GetForums(cancellationToken);
            metrics.ForumsFetched(true);
            return result;
        }
        catch
        {
            metrics.ForumsFetched(false);
            throw;
        }
    }
}