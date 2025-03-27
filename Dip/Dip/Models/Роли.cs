using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Роли
{
    public int Id { get; set; }

    public string Роль { get; set; } = null!;

    public virtual ICollection<Пользователи> Пользователиs { get; set; } = new List<Пользователи>();
}
