using TweyesBackend.Persistence;

namespace TweyesBackend.Core.Contract.Repository
{
    public interface IQuery<TOut>
    {
        IQueryable<TOut> Run(ApplicationDbContext dbContext);
    }
}
