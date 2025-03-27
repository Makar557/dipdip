using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Рестораны
{
    public int Id { get; set; }

    public int ПользовательId { get; set; }

    public string Название { get; set; } = null!;

    public string? Описание { get; set; }

    public string Адрес { get; set; } = null!;

    public string Телефон { get; set; } = null!;

    public decimal? Рейтинг { get; set; }

    public TimeOnly НачалоРаботы { get; set; }

    public int КоличествоЧасовВДень { get; set; }

    public string? Логотип { get; set; }

    public virtual ICollection<Заказы> Заказыs { get; set; } = new List<Заказы>();

    public virtual ICollection<Меню> Менюs { get; set; } = new List<Меню>();

    public virtual ICollection<Оценки> Оценкиs { get; set; } = new List<Оценки>();

    public virtual Пользователи Пользователь { get; set; } = null!;
}
