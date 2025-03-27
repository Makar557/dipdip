using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Оценки
{
    public int Id { get; set; }

    public int ПользовательId { get; set; }

    public int РесторанId { get; set; }

    public int Оценка { get; set; }

    public virtual Пользователи Пользователь { get; set; } = null!;

    public virtual Рестораны Ресторан { get; set; } = null!;
}
