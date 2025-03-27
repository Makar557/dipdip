using System;
using System.Collections.Generic;

namespace Dip.Models;

public partial class Меню
{
    public int Id { get; set; }

    public int РесторанId { get; set; }

    public string Название { get; set; } = null!;

    public string? Описание { get; set; }

    public decimal Цена { get; set; }

    public string? СсылкаНаИзображение { get; set; }

    public bool Доступность { get; set; }

    public virtual Рестораны Ресторан { get; set; } = null!;

    public virtual ICollection<Скидки> Скидкиs { get; set; } = new List<Скидки>();

    public virtual ICollection<СоставЗаказа> СоставЗаказаs { get; set; } = new List<СоставЗаказа>();

    public virtual ICollection<СоставКорзины> СоставКорзиныs { get; set; } = new List<СоставКорзины>();

    public Скидки? АктуальнаяСкидка => Скидкиs
    .Where(s => s.ДатаНачала <= DateOnly.FromDateTime(DateTime.Today) && s.ДатаОкончания >= DateOnly.FromDateTime(DateTime.Today))
    .OrderByDescending(s => s.ДатаНачала) // Берем самую свежую скидку
    .FirstOrDefault();
}
