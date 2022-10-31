using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Services.Interfaces
{
   public interface IAccountService
    {
        CustomerAccountInformation Authenticate(string AccountNumber, string Pin);
        IEnumerable<CustomerAccountInformation> GetAllAccount();

        CustomerAccountInformation Create(CustomerAccountInformation accountInformation, string Pin, string ConfirmPin);
        void Update(CustomerAccountInformation accountInformation, string Pin = null);
        void Delete(int id);
        CustomerAccountInformation GetById(int Id);
        CustomerAccountInformation GetByAccountNumber(string AccountNumber);
    }
}
