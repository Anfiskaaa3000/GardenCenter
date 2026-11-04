using GardenCenter.Models;

namespace GardenCenter.Services;

public static class Session
{
    public static User? CurrentUser { get; set; }
    public static bool IsAdmin => CurrentUser?.Role?.RoleName == "Администратор";
    public static bool IsStaff => IsAdmin || CurrentUser?.Role?.RoleName == "Менеджер";
}