using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class СоставКорзины
{
    public int Id { get; set; }

    public int КорзинаId { get; set; }

    public int БлюдоId { get; set; }

    public int Количество { get; set; }

    public decimal Цена { get; set; }

    public virtual Меню Блюдо { get; set; } = null!;

    public virtual Корзины Корзина { get; set; } = null!;
}
