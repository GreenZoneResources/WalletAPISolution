using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Services.Interfaces
{
    public interface ITransactionService 
    {
        double CalculateInterestToCustomerAccountBalance(decimal PrincipalAmount, double Rate, double Tenor, DateTime EffectiveDate);
       AccountViewModel GetCustomerBalance(string AccountNumber, string TransactionPin);
       Response CreateNewTransaction(CounterpartyTransaction transaction);
       Response FindTransactionByDate(DateTime date);
       Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
       Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
       Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
