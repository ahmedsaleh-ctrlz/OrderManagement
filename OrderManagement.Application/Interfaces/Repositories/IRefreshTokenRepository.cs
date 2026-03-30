using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token, CancellationToken ct);

        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
