using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Commands
{
    //public record GetCustomerAccountBalanceCommand(string AccountNumber, string TransactionPin) : IRequest;
    public class GetCustomerAccountBalanceCommand : IRequest<AccountViewModel>
    {
        public string AccountNumber { get; set; }
        public string TransactionPin { get; set; }
        public GetCustomerAccountBalanceCommand()
        {
            this.AccountNumber = AccountNumber;
            this.TransactionPin = TransactionPin;
        }
    }

    
}
