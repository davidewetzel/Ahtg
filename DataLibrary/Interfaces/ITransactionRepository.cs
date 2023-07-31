using DataLibrary.Models;

namespace DataLibrary.Interfaces
{
    public interface ITransactionRepository
    {
        Task<int> Insert(Transaction transaction);

        Task<Transaction> Get(int id);

        Task Update(Transaction transaction);

        Task Delete(int id);

        Task<(IEnumerable<Transaction> Transactions, int TotalCount)> Get(string paymentType, int pageNumber, int pageSize);
    }
}
