using AutoMapper;
using DataLibrary.Interfaces;
using DomainServices.DomainModels;
using DomainServices.Exceptions;
using DomainServices.Interfaces;
using Models = DataLibrary.Models;

namespace DomainServices.Services
{
    public class TransactionService : ITransactionService
    {
        #region " Properties / Constants... " 
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 5;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        #endregion
        
        #region " Constructors... "
        public TransactionService(IMapper mapper,
                                  ITransactionRepository transactionRepository) 
        { 
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }
        #endregion

        #region " Public Methods... "

        public async Task<Transaction> Create(Transaction transaction)
        {
            Models.Transaction dataModel = _mapper.Map<Models.Transaction>(transaction);
            transaction.Id = await _transactionRepository.Insert(dataModel);
            return transaction;
        }

        public async Task<Transaction> Get(int id)
        {
            Models.Transaction dataModel = await _transactionRepository.Get(id) ?? throw new NotFoundException($"Transaction with id {id} not found");
            Transaction transaction = _mapper.Map<Transaction>(dataModel);
            return transaction;
        }

        public async Task Update(Transaction transaction)
        {
            Models.Transaction dataModel = _mapper.Map<Models.Transaction>(transaction);
            await _transactionRepository.Update(dataModel);
        }

        public async Task Delete(int id)
        {
            await _transactionRepository.Delete(id);
        }

        public async Task<PagedList<Transaction>> GetTransactions(string paymentType, int? pageNumber, int? pageSize)
        {
            pageNumber ??= DEFAULT_PAGE_NUMBER;
            pageSize ??= DEFAULT_PAGE_SIZE;

            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Invalid PageNumber or PageSize");
            }

            (IEnumerable<Models.Transaction> transactions, int totalCount) = await _transactionRepository.Get(paymentType, pageNumber.Value, pageSize.Value);

            return new PagedList<Transaction>()
            {
                Items = _mapper.Map<List<Transaction>>(transactions),
                CurrentPage = pageNumber.Value,
                PageSize = pageSize.Value,
                TotalItemCount = totalCount
            };
        }

        #endregion
    }
}
