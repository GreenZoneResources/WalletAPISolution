using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.DAL;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Services.Interfaces;
using ZedCrestWalletApplication.Utils;

namespace ZedCrestWalletApplication.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private ZedCrestWalletContext _dbContext;
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _OurBankSettlementAccount;
        private readonly IAccountService _accountService;
        public TransactionService(ZedCrestWalletContext dbContext,ILogger<TransactionService> logger,IOptions<AppSettings> settings, IAccountService accountService
            )
        {
            _dbContext = dbContext;
            _logger = logger;
            _settings = settings.Value;
            _OurBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }
        public double CalculateInterestToCustomerAccountBalance(decimal PrincipalAmount, double Rate, double Tenor,DateTime EffectiveDate)
        {
            double interest = 0.0d;
            if (IsLeapYear(EffectiveDate) == 366)
            {
                 interest = (Convert.ToDouble(PrincipalAmount) * Rate * Tenor) / (366 * 100);

            }
            else
            {
                 interest = (Convert.ToDouble(PrincipalAmount) * Rate * Tenor) / (365 * 100);
            }
            return interest;
        }
        public int IsLeapYear(DateTime EffectDate)
        {
            if(EffectDate.Year % 4 == 0)
            {
                return 366;
            }
            return 365;
        }
        public Response CreateNewTransaction(CounterpartyTransaction transaction)
        {
            Response response = new Response();
            _dbContext.CounterpartyTransactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbContext.CounterpartyTransactions.Where(i => i.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction retrieved successfully";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make deposit
            Response response = new Response();
            CustomerAccountInformation sourceAccount;
            CustomerAccountInformation destinationAccount;
            //firsst the user--account number must be valid
            //we will need authenticate in AccountService, then inject it here
            CounterpartyTransaction transaction = new CounterpartyTransaction();
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            try
            {
                //for deposit, our BankSettlement is the source giving money to the user's account
                sourceAccount = _accountService.GetByAccountNumber(_OurBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);
                //now let's update their balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there's update
                if((_dbContext.Entry(sourceAccount).State == EntityState.Modified) && 
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    //if transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    //if transaction is not successful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... =>  {ex.Message}");
                throw;
            }
            //set other props of transaction here
            transaction.TransactionType = TransType.Deposit;
            transaction.TransactionSourceAccount = _OurBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE: => {transaction.TransactionDate} FOR AMOUNT => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS =>" +
                $"{transaction.TransactionStatus}";
            //commit to db
            _dbContext.CounterpartyTransactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;

        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            //Implement Withdrawal
            Response response = new Response();
            CustomerAccountInformation sourceAccount;
            CustomerAccountInformation destinationAccount;
            //firsst the user--account number must be valid
            //we will need authenticate in AccountService, then inject it here
            CounterpartyTransaction transaction = new CounterpartyTransaction();
            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            try
            {
                //for deposit, our BankSettlement is the source giving money to the user's account
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);
                //now let's update their balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there's update
                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    //if transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    //if transaction is not successful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... =>  {ex.Message}");
                throw;
            }
            //set other props of transaction here
            transaction.TransactionType = TransType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE: => {transaction.TransactionDate} FOR AMOUNT => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS =>" +
                $"{transaction.TransactionStatus}";
            //commit to db
            _dbContext.CounterpartyTransactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //Implement Withdrawal
            Response response = new Response();
            CustomerAccountInformation sourceAccount;
            CustomerAccountInformation destinationAccount;
            //firsst the user--account number must be valid
            //we will need authenticate in AccountService, then inject it here
            CounterpartyTransaction transaction = new CounterpartyTransaction();
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            try
            {
                //for deposit, our BankSettlement is the source giving money to the user's account
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_OurBankSettlementAccount);
                //now let's update their balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there's update
                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    //if transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    //if transaction is not successful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... =>  {ex.Message}");
                throw;
            }
            //set other props of transaction here
            transaction.TransactionType = TransType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _OurBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE: => {transaction.TransactionDate} FOR AMOUNT => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS =>" +
                $"{transaction.TransactionStatus}";
            //commit to db
            _dbContext.CounterpartyTransactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public AccountViewModel GetCustomerBalance(string AccountNumber, string TransactionPin)
        {
            AccountViewModel account = new AccountViewModel();
            if(AccountNumber != null && TransactionPin != null)
            {
                var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
                if (authUser == null) throw new ApplicationException("Invalid Credentials");
                //Implement Balance
                try
                {
                    var accountInfo = _accountService.GetByAccountNumber(AccountNumber);
                    account.AccountName = $"{accountInfo.LastName + " " + accountInfo.FirstName}";
                    account.ProductName = accountInfo.AccountType.ToString();
                    account.Balance = string.Format("{0:N}",accountInfo.CurrentAccountBalance);
                }
                catch (ApplicationException ex)
                {

                    throw ex;
                }
            }
            else
            {
                return account;
            }
            return account;
        }
    }
}
