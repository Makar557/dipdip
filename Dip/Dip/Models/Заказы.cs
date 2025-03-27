using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Заказы
{
    public int Id { get; set; }

    public int КлиентId { get; set; }

    public int РесторанId { get; set; }

    public int? КурьерId { get; set; }

    public string АдресДоставки { get; set; } = null!;

    public decimal ИтоговаяСтоимость { get; set; }

    public string Статус { get; set; } = null!;

    public string СпособОплаты { get; set; } = null!;

    public DateTime? ДатаСоздания { get; set; }

    public DateTime? ДатаДоставки { get; set; }

    public string Квартира { get; set; } = null!;

    public virtual Пользователи Клиент { get; set; } = null!;

    public virtual Пользователи? Курьер { get; set; }

    public virtual Рестораны Ресторан { get; set; } = null!;

    public virtual ICollection<СоставЗаказа> СоставЗаказаs { get; set; } = new List<СоставЗаказа>();
    public static decimal КомиссияБизнеса { get; } = 0.20m;

}
