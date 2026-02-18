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
        Task AddAsync(Order order);
        Task<Order?> GetWithItemsAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<Order, bool>> expression);
        Task SaveChangesAsync();

        Task<int> CountAsync();
        Task<List<Order>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
