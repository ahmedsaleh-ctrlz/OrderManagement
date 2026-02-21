using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OrderManagement.Application.Interfaces.Global;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
        int.Parse(_httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public string Role =>
        _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

    public int? WarehouseId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirst("warehouseId")?.Value;

            return value == null ? null : int.Parse(value);
        }
    }
}
