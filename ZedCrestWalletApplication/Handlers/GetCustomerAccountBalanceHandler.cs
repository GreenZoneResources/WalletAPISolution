using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Queries;
using ZedCrestWalletApplication.Services.Interfaces;

namespace ZedCrestWalletApplication.Handlers
{

    public class GetCustomerAccountBalanceHandler : IRequestHandler<GetCustomerAccountBalanceQuery, AccountViewModel>
    {
        private ITransactionService _transactionService;
        public GetCustomerAccountBalanceHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<AccountViewModel> Handle(GetCustomerAccountBalanceQuery request, CancellationToken cancellationToken)
        {
           var result = await Task.FromResult(_transactionService.GetCustomerBalance(request.AccountNumber, request.TransactionPin));
            return result == null ? null : result;
        }
    }
}
