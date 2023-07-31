using Dapper;
using DataLibrary.Interfaces;
using DataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace DataLibrary.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        #region " Constructors... "
        public TransactionRepository(IConfiguration configuration)
                : base(configuration)
        {
        }
        #endregion

        #region " Public Methods... "
        public async Task<int> Insert(Transaction transaction)
        {
            const string sql = @"INSERT INTO [dbo].[Transactions] ([PaymentTypeId], [TransactionDate], [CashierName], [RegisterNumber], [Total], [Description])
                                 OUTPUT INSERTED.[Id]
                                 VALUES (@PaymentTypeId, @TransactionDate, @CashierName, @RegisterNumber, @Total, @Description) ";

            using DbConnection connection = await CreateConnection();
            using IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                int id = await connection.QueryFirstOrDefaultAsync<int>(sql, transaction, dbTransaction);
                dbTransaction.Commit();
                return id;
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public async Task<Transaction> Get(int id)
        {
            const string sql = @"SELECT tr.[Id], tr.[PaymentTypeId], tr.[TransactionDate], tr.[CashierName], tr.[RegisterNumber], tr.[Total], tr.[Description],
                                        pt.[Id], pt.[Name]
                                 FROM   [dbo].[Transactions] AS tr
                                 INNER  JOIN [dbo].[PaymentTypes] AS pt ON tr.PaymentTypeId = pt.Id
                                 WHERE  tr.Id = @id ";

            using DbConnection connection = await CreateConnection();

            IEnumerable<Transaction> result = await connection.QueryAsync<Transaction, PaymentType, Transaction>(sql, (transaction, paymentType) =>
            {
                transaction.PaymentType = paymentType;  
                return transaction;
            }, new { id }, splitOn: "Id");

            return result.FirstOrDefault();
        }

        public async Task Update(Transaction transaction)
        {
            const string sql = @"UPDATE [dbo].[Transactions]
                                 SET    [PaymentTypeId] = @PaymentTypeId, 
                                        [TransactionDate] = @TransactionDate,
                                        [CashierName] = @CashierName,
                                        [RegisterNumber] = @RegisterNumber,
                                        [Total] = @Total,
                                        [Description] = @Description
                                 WHERE  Id = @Id ";

            using DbConnection connection = await CreateConnection();
            using IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                await connection.ExecuteAsync(sql, transaction, dbTransaction);
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            const string sql = @"DELETE FROM [dbo].[Transactions]
                                 WHERE  Id = @Id ";

            using DbConnection connection = await CreateConnection();
            using IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                await connection.ExecuteAsync(sql, new { id }, dbTransaction);
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public async Task<(IEnumerable<Transaction> Transactions, int TotalCount)> Get(string paymentType, int pageNumber, int pageSize)
        {
            string from = @"FROM   [dbo].[Transactions] AS tr
                            INNER  JOIN [dbo].[PaymentTypes] AS pt ON tr.PaymentTypeId = pt.Id ";

            string where = string.IsNullOrEmpty(paymentType) ? string.Empty : "WHERE pt.[Name] = @paymentType ";

            string sql = $@"SELECT tr.[Id], tr.[PaymentTypeId], tr.[TransactionDate], tr.[CashierName], tr.[RegisterNumber], tr.[Total], tr.[Description],
                                        pt.[Id], pt.[Name]
                           {from} {where} 
                           ORDER  BY tr.[Id] 
                           {GetPagedSql(pageNumber, pageSize)} ";

            string countSql = $@"SELECT COUNT(*) 
                                {from} {where} ";

            using DbConnection connection = await CreateConnection();

            IEnumerable<Transaction> result = await connection.QueryAsync<Transaction, PaymentType, Transaction>(sql, (transaction, paymentType) =>
            {
                transaction.PaymentType = paymentType;
                return transaction;
            }, new { paymentType }, splitOn: "Id");

            int count = await connection.QueryFirstOrDefaultAsync<int>(countSql, new { paymentType });

            return (result, count);
        }

        #endregion
    }
}
