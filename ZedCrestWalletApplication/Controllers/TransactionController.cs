using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Commands;
using ZedCrestWalletApplication.DAL;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Queries;
using ZedCrestWalletApplication.Services.Interfaces;

namespace ZedCrestWalletApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        private IAccountService _accountService;
        IMapper _mapper;
        IMediator _mediator;
        private readonly IRabitMQProducer _rabitMQProducer;
        private readonly ZedCrestWalletContext _dbContext;
        public TransactionController(ZedCrestWalletContext dbContext, IMediator mediator,ITransactionService transactionService, IMapper mapper, IRabitMQProducer rabitMQProducer, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _mapper = mapper;
            _rabitMQProducer = rabitMQProducer;
            _dbContext = dbContext;
            _mediator = mediator;
        }
        [HttpPost]
        [Route("view_balance_by_accountnumber")]
        public IActionResult GetCustomerAccountBalance(AccountModelParameters account)
        {
            if (!ModelState.IsValid) return BadRequest("Parameters are not in correct format");
            var query = new GetCustomerAccountBalanceQuery(account.AccountNumber, account.TransactionPin);
            var result = _mediator.Send(query);
            return result != null ? (IActionResult) Ok(result) : NotFound();
        }
        //Add Interest to Customer Balances
        [HttpPost]
        [Route("add_interest_to_customer_balance")]
        public IActionResult AddInterestToCustomerAccountBalance(CalculateInterestModel interest)
        {
            if (!ModelState.IsValid) return BadRequest("Parameters are not in correct format");
            Response message = null;
            var dateOfDay = DateTime.Now;
            DateTime midnightTime = DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)
            .AddSeconds(-DateTime.Now.Second);
            if (dateOfDay == midnightTime)
            {
                var getAllAccount = _accountService.GetAllAccount().ToList();
                for(int i = 0; i < getAllAccount.Count; i++)
                {
                    var CalcInterest = _transactionService.CalculateInterestToCustomerAccountBalance(getAllAccount[i].CurrentAccountBalance, interest.Rate, interest.Tenor, interest.EffectiveDate);
                    getAllAccount[i].CurrentAccountBalance += Convert.ToDecimal(CalcInterest);
                    _dbContext.SaveChanges();
                     message = new Response() {
                       ResponseCode = "05",
                       ResponseMessage = getAllAccount[i].FirstName + "'s Account updated with interest of " + CalcInterest,
                       Data = null
                    };
                    _rabitMQProducer.SendInterestToAccountMessage(message);
                }
            }
            return Ok(message);
        }
        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            if (!ModelState.IsValid) return BadRequest(transactionRequest);

            var transaction = _mapper.Map<CounterpartyTransaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }
        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit([FromBody] MakeDepositCommand deposit)
        {
            if(!ModelState.IsValid) return BadRequest("Account number must be 10-digit");
            var result = _mediator.Send(deposit);
            return result == null ? null : Ok(result);
        }

        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(MakeWithdrawalCommand withdrawal)
        {
            if (!ModelState.IsValid) return BadRequest("Account number must be 10-digit");
            var result = _mediator.Send(withdrawal);
            return result == null ? null : Ok(result);
        }
        [HttpPost]
        [Route("make_fund_transfer")]
        public IActionResult MakeFundsTransfer(MakeTransferCommand transfer)
        {
            if (!ModelState.IsValid) return BadRequest("Account number must be 10-digit");
            var result = _mediator.Send(transfer);
            return result == null ? null : Ok(result);
        }
    }
}
