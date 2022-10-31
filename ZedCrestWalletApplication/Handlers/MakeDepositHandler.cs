using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Commands;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Services.Interfaces;

namespace ZedCrestWalletApplication.Handlers
{
    public class MakeDepositHandler : IRequestHandler<MakeDepositCommand, Response>
    {
        private readonly ITransactionService _transactionService;
        public MakeDepositHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<Response> Handle(MakeDepositCommand request, CancellationToken cancellationToken)
        {
            var result = await Task.FromResult(_transactionService.MakeDeposit(request.AccountNumber, request.Amount, request.TransactionPin));
            return result == null ? null : result;
        }

       
    }
}
