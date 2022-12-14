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
    public class MakeTransferHandler : IRequestHandler<MakeTransferCommand, Response>
    {
        private readonly ITransactionService _transactionService;
        public MakeTransferHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<Response> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
        {
            var result = await Task.FromResult(_transactionService.MakeFundsTransfer(request.FromAccount,request.ToAccount, request.Amount, request.TransactionPin));
            return result == null ? null : result;
        }


    }
}
