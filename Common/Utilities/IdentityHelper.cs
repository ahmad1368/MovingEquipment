using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public static class IdentityHelper
{
    private static IHttpContextAccessor _httpContextAccessor;

    // مقداردهی اولیه IHttpContextAccessor
    public static void Initialize(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // دریافت یوزرنیم کاربر فعلی
    public static string? GetUsername()
    {
        return _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    }

    // دریافت شناسه (ID) کاربر فعلی
    public static string? GetUserId()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    // دریافت ایمیل کاربر فعلی
    public static string? GetUserEmail()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    // بررسی اینکه کاربر لاگین کرده یا نه
    public static bool IsAuthenticated()
    {
        return _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}