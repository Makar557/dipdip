using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Скидки
{
    public int Id { get; set; }

    public int МенюId { get; set; }

    public decimal ПроцентСкидки { get; set; }

    public DateOnly ДатаНачала { get; set; }

    public DateOnly ДатаОкончания { get; set; }

    public virtual Меню Меню { get; set; } = null!;
}
