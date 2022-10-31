using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Queries
{
    public class GetCustomerAccountBalanceQuery : IRequest<AccountViewModel>
    {
        public string AccountNumber { get; set; }
        public string TransactionPin { get; set; }
        public GetCustomerAccountBalanceQuery(string _AccountNumber, string _TransactionPin)
        {
            AccountNumber = _AccountNumber;
            TransactionPin = _TransactionPin;
        }
    }
}
