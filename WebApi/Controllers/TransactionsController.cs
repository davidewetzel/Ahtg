using DomainServices.DomainModels;
using DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Classes;

namespace WebApi.Controllers
{
    [ApiVersion(Constants.VERSION_1_0)]
    [Produces(Constants.APPLICATION_JSON)]
    [Route(Constants.API + Constants.CONTROLLER)]
    [Route(Constants.API + Constants.API_VERSION + Constants.CONTROLLER)]
    [ApiController]
    public class TransactionsController : Controller
    {
        #region " Properties / Constants... " 

        private readonly ITransactionService _transactionService;

        #endregion

        #region " Constructors... "
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        #endregion

        #region " Public Methods... "

        [HttpGet]
        public async Task<IActionResult> Get(string paymentType = null, int? pageNumber = null, int? pageSize = null)
        {
            PagedList<Transaction> transactions = await _transactionService.GetTransactions(paymentType, pageNumber, pageSize);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Transaction transaction)
        {
            transaction = await _transactionService.Create(transaction);
            return Created(Url.Action("Get", new { id = transaction.Id }), transaction);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Transaction transaction = await _transactionService.Get(id);
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Transaction transaction)
        {
            await _transactionService.Update(transaction);
            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.Delete(id);
            return NoContent();
        }
        #endregion
    }
}
