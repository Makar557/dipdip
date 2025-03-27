using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Корзины
{
    public int Id { get; set; }

    public int ПользовательId { get; set; }

    public string Статус { get; set; } = null!;

    public DateTime? ДатаСоздания { get; set; }

    public decimal? ИтоговаяСтоимость { get; set; }

    public virtual Пользователи Пользователь { get; set; } = null!;

    public virtual ICollection<СоставКорзины> СоставКорзиныs { get; set; } = new List<СоставКорзины>();
}
