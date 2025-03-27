using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dip.Models;
using Dip.Repository;

public class DiscountCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DiscountCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await УдалитьПросроченныеСкидки();
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task УдалитьПросроченныеСкидки()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DiplomaDbContext>();

            var устаревшиеСкидки = dbContext.Скидкиs
                .Where(s => s.ДатаОкончания < DateOnly.FromDateTime(DateTime.UtcNow))
                .ToList();

            if (устаревшиеСкидки.Any())
            {
                dbContext.Скидкиs.RemoveRange(устаревшиеСкидки);
                await dbContext.SaveChangesAsync();

                var userIds = dbContext.Корзиныs
                    .Where(c => c.Статус == "активная")
                    .Select(c => c.ПользовательId)
                    .Distinct()
                    .ToList();

                var cartRepo = scope.ServiceProvider.GetRequiredService<КорзинаRepository>();

                foreach (var userId in userIds)
                {
                    cartRepo.UpdateCartPrices(userId);
                }
            }
        }
    }
}
