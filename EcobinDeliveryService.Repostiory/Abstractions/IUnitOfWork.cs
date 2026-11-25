using System;

namespace EcobinDeliveryService.Repostiory.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();

    Task ExecuteInTransactionAsync(Func<Task> operation);
}
