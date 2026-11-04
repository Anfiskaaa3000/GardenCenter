using System;
using System.Collections.Generic;

namespace GardenCenter.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
    // Это свойство склеивает Фамилию, Имя и Отчество в одну строку
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

    // Этот метод говорит программе, как показывать пользователя в списке
    public override string ToString()
    {
        return $"{FullName} ({Login})";
    }
}
