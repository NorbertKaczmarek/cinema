using cinema.context.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace cinema.context;

public class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext dbContext = eventData.Context!;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        var entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(a => a.CreatedOnUtc).CurrentValue = DateTimeOffset.Now;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.Now;
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        DbContext dbContext = eventData.Context!;
        if (dbContext is null)
        {
            return base.SavingChanges(eventData, result);
        }
        var entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(a => a.CreatedOnUtc).CurrentValue = DateTimeOffset.Now;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.Now;
            }
        }
        return base.SavingChanges(eventData, result);
    }
}
