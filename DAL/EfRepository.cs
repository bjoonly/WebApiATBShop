using Ardalis.Specification.EntityFrameworkCore;
using DAL.Data;
using DAL.Interfaces;


namespace DAL
{
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(AppEFContext dbContext) : base(dbContext)
        {
        }
    }
}
