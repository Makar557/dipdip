using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class СоставЗаказа
{
    public int Id { get; set; }

    public int ЗаказId { get; set; }

    public int БлюдоId { get; set; }

    public int Количество { get; set; }

    public decimal Цена { get; set; }

    public virtual Меню Блюдо { get; set; } = null!;

    public virtual Заказы Заказ { get; set; } = null!;
}
