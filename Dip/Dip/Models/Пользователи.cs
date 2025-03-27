using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Пользователи
{
    public int Id { get; set; }

    public string Имя { get; set; } = null!;

    public string Фамилия { get; set; } = null!;

    public string ЭлектроннаяПочта { get; set; } = null!;

    public string Логин { get; set; } = null!;

    public string ХэшПароля { get; set; } = null!;

    public int РолиId { get; set; }

    public DateTime? ДатаРегистрации { get; set; }

    public string? КредитнаяКарта { get; set; }

    public string Телефон { get; set; } = null!;

    public decimal? Баланс { get; set; }

    public virtual ICollection<Заказы> ЗаказыКлиентs { get; set; } = new List<Заказы>();

    public virtual ICollection<Заказы> ЗаказыКурьерs { get; set; } = new List<Заказы>();

    public virtual ICollection<Корзины> Корзиныs { get; set; } = new List<Корзины>();

    public virtual ICollection<Оценки> Оценкиs { get; set; } = new List<Оценки>();

    public virtual ICollection<Рестораны> Рестораныs { get; set; } = new List<Рестораны>();

    public virtual Роли Роли { get; set; } = null!;
}
