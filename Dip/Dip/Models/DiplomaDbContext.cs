using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dip.Models;

public partial class DiplomaDbContext : DbContext
{
    public DiplomaDbContext()
    {
    }

    public DiplomaDbContext(DbContextOptions<DiplomaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Заказы> Заказыs { get; set; }

    public virtual DbSet<Корзины> Корзиныs { get; set; }

    public virtual DbSet<Меню> Менюs { get; set; }

    public virtual DbSet<Оценки> Оценкиs { get; set; }

    public virtual DbSet<Пользователи> Пользователиs { get; set; }

    public virtual DbSet<Рестораны> Рестораныs { get; set; }

    public virtual DbSet<Роли> Ролиs { get; set; }

    public virtual DbSet<Скидки> Скидкиs { get; set; }

    public virtual DbSet<СоставЗаказа> СоставЗаказаs { get; set; }

    public virtual DbSet<СоставКорзины> СоставКорзиныs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9C7BP4G;Database=DiplomaDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Заказы>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Заказы__3213E83F211BDF89");

            entity.ToTable("Заказы");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.АдресДоставки)
                .HasMaxLength(255)
                .HasColumnName("адрес_доставки");
            entity.Property(e => e.ДатаДоставки)
                .HasColumnType("datetime")
                .HasColumnName("дата_доставки");
            entity.Property(e => e.ДатаСоздания)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("дата_создания");
            entity.Property(e => e.ИтоговаяСтоимость)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("итоговая_стоимость");
            entity.Property(e => e.Квартира)
                .HasMaxLength(10)
                .HasColumnName("квартира");
            entity.Property(e => e.КлиентId).HasColumnName("клиент_id");
            entity.Property(e => e.КурьерId).HasColumnName("курьер_id");
            entity.Property(e => e.РесторанId).HasColumnName("ресторан_id");
            entity.Property(e => e.СпособОплаты)
                .HasMaxLength(20)
                .HasColumnName("способ_оплаты");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .HasColumnName("статус");

            entity.HasOne(d => d.Клиент).WithMany(p => p.ЗаказыКлиентs)
                .HasForeignKey(d => d.КлиентId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Заказы__клиент_i__571DF1D5");

            entity.HasOne(d => d.Курьер).WithMany(p => p.ЗаказыКурьерs)
                .HasForeignKey(d => d.КурьерId)
                .HasConstraintName("FK__Заказы__курьер_i__59063A47");

            entity.HasOne(d => d.Ресторан).WithMany(p => p.Заказыs)
                .HasForeignKey(d => d.РесторанId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Заказы__ресторан__5812160E");
        });

        modelBuilder.Entity<Корзины>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Корзины__3213E83FFC536900");

            entity.ToTable("Корзины");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ДатаСоздания)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("дата_создания");
            entity.Property(e => e.ИтоговаяСтоимость)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("итоговая_стоимость");
            entity.Property(e => e.ПользовательId).HasColumnName("пользователь_id");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .HasColumnName("статус");

            entity.HasOne(d => d.Пользователь).WithMany(p => p.Корзиныs)
                .HasForeignKey(d => d.ПользовательId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Корзины__пользов__4AB81AF0");
        });

        modelBuilder.Entity<Меню>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Меню__3213E83FD0211FB5");

            entity.ToTable("Меню");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Доступность)
                .HasDefaultValue(true)
                .HasColumnName("доступность");
            entity.Property(e => e.Название)
                .HasMaxLength(100)
                .HasColumnName("название");
            entity.Property(e => e.Описание).HasColumnName("описание");
            entity.Property(e => e.РесторанId).HasColumnName("ресторан_id");
            entity.Property(e => e.СсылкаНаИзображение).HasColumnName("ссылка_на_изображение");
            entity.Property(e => e.Цена)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("цена");

            entity.HasOne(d => d.Ресторан).WithMany(p => p.Менюs)
                .HasForeignKey(d => d.РесторанId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Меню__ресторан_i__45F365D3");
        });

        modelBuilder.Entity<Оценки>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Оценки__3213E83F6CE05951");

            entity.ToTable("Оценки");

            entity.HasIndex(e => new { e.ПользовательId, e.РесторанId }, "UQ__Оценки__B568568A0A482E88").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Оценка).HasColumnName("оценка");
            entity.Property(e => e.ПользовательId).HasColumnName("пользователь_id");
            entity.Property(e => e.РесторанId).HasColumnName("ресторан_id");

            entity.HasOne(d => d.Пользователь).WithMany(p => p.Оценкиs)
                .HasForeignKey(d => d.ПользовательId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Оценки__пользова__70DDC3D8");

            entity.HasOne(d => d.Ресторан).WithMany(p => p.Оценкиs)
                .HasForeignKey(d => d.РесторанId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Оценки__ресторан__71D1E811");
        });

        modelBuilder.Entity<Пользователи>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Пользова__3213E83F0DD8ABE8");

            entity.ToTable("Пользователи");

            entity.HasIndex(e => e.Логин, "UQ__Пользова__5EB64DCC32DEFEC8").IsUnique();

            entity.HasIndex(e => e.ЭлектроннаяПочта, "UQ__Пользова__D3AA95DC4AD714DC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Баланс)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("баланс");
            entity.Property(e => e.ДатаРегистрации)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("дата_регистрации");
            entity.Property(e => e.Имя)
                .HasMaxLength(50)
                .HasColumnName("имя");
            entity.Property(e => e.КредитнаяКарта).HasColumnName("кредитная_карта");
            entity.Property(e => e.Логин)
                .HasMaxLength(50)
                .HasColumnName("логин");
            entity.Property(e => e.РолиId).HasColumnName("роли_id");
            entity.Property(e => e.Телефон)
                .HasMaxLength(20)
                .HasColumnName("телефон");
            entity.Property(e => e.Фамилия)
                .HasMaxLength(50)
                .HasColumnName("фамилия");
            entity.Property(e => e.ХэшПароля).HasColumnName("хэш_пароля");
            entity.Property(e => e.ЭлектроннаяПочта)
                .HasMaxLength(100)
                .HasColumnName("электронная_почта");

            entity.HasOne(d => d.Роли).WithMany(p => p.Пользователиs)
                .HasForeignKey(d => d.РолиId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Пользоват__роли___3C69FB99");
        });

        modelBuilder.Entity<Рестораны>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ресторан__3213E83F4FF6654C");

            entity.ToTable("Рестораны");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Адрес)
                .HasMaxLength(255)
                .HasColumnName("адрес");
            entity.Property(e => e.КоличествоЧасовВДень).HasColumnName("количество_часов_в_день");
            entity.Property(e => e.Логотип).HasColumnName("логотип");
            entity.Property(e => e.Название)
                .HasMaxLength(100)
                .HasColumnName("название");
            entity.Property(e => e.НачалоРаботы).HasColumnName("начало_работы");
            entity.Property(e => e.Описание).HasColumnName("описание");
            entity.Property(e => e.ПользовательId).HasColumnName("пользователь_id");
            entity.Property(e => e.Рейтинг)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("рейтинг");
            entity.Property(e => e.Телефон)
                .HasMaxLength(15)
                .HasColumnName("телефон");

            entity.HasOne(d => d.Пользователь).WithMany(p => p.Рестораныs)
                .HasForeignKey(d => d.ПользовательId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Рестораны__польз__403A8C7D");
        });

        modelBuilder.Entity<Роли>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Роли__3213E83F1A7FFFD3");

            entity.ToTable("Роли");

            entity.HasIndex(e => e.Роль, "UQ__Роли__B421FE333368280F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Роль)
                .HasMaxLength(20)
                .HasColumnName("роль");
        });

        modelBuilder.Entity<Скидки>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Скидки__3214EC07982AE2AF");

            entity.ToTable("Скидки");

            entity.Property(e => e.ПроцентСкидки).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Меню).WithMany(p => p.Скидкиs)
                .HasForeignKey(d => d.МенюId)
                .HasConstraintName("FK__Скидки__МенюId__2180FB33");
        });

        modelBuilder.Entity<СоставЗаказа>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Состав_з__3213E83F270B9BA0");

            entity.ToTable("Состав_заказа");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.БлюдоId).HasColumnName("блюдо_id");
            entity.Property(e => e.ЗаказId).HasColumnName("заказ_id");
            entity.Property(e => e.Количество).HasColumnName("количество");
            entity.Property(e => e.Цена)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("цена");

            entity.HasOne(d => d.Блюдо).WithMany(p => p.СоставЗаказаs)
                .HasForeignKey(d => d.БлюдоId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Состав_за__блюдо__17036CC0");

            entity.HasOne(d => d.Заказ).WithMany(p => p.СоставЗаказаs)
                .HasForeignKey(d => d.ЗаказId)
                .HasConstraintName("FK__Состав_за__заказ__160F4887");
        });

        modelBuilder.Entity<СоставКорзины>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Состав_к__3213E83F88219B2F");

            entity.ToTable("Состав_корзины");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.БлюдоId).HasColumnName("блюдо_id");
            entity.Property(e => e.Количество).HasColumnName("количество");
            entity.Property(e => e.КорзинаId).HasColumnName("корзина_id");
            entity.Property(e => e.Цена)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("цена");

            entity.HasOne(d => d.Блюдо).WithMany(p => p.СоставКорзиныs)
                .HasForeignKey(d => d.БлюдоId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Состав_ко__блюдо__52593CB8");

            entity.HasOne(d => d.Корзина).WithMany(p => p.СоставКорзиныs)
                .HasForeignKey(d => d.КорзинаId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Состав_ко__корзи__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
