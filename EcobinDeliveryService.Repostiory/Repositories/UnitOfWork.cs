using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Persistence;

namespace EcobinDeliveryService.Repostiory.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            await operation();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
