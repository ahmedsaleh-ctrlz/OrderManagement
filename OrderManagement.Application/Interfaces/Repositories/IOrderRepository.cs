using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderRepository 
    {
        Task AddAsync(Order order, CancellationToken ct = default!);
        Task<Order?> GetWithDetailsAsync(int id, CancellationToken ct = default!);
        Task<bool> ExistsAsync(Expression<Func<Order, bool>> expression, CancellationToken ct = default!);
        Task SaveChangesAsync(CancellationToken ct = default!);

        Task<int> CountAsync(CancellationToken ct = default!);

        IQueryable<Order> GetQueryable();
        //Task<List<Order>> GetPagedWithDetailsAsync(int pageNumber, int pageSize);
    }
}
