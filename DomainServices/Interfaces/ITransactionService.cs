using DomainServices.DomainModels;

namespace DomainServices.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> Create(Transaction transaction);

        Task<Transaction> Get(int id);

        Task Update(Transaction transaction);

        Task Delete(int id);

        Task<PagedList<Transaction>> GetTransactions(string paymentType, int? pageNumber, int? pageSize);
    }
}
