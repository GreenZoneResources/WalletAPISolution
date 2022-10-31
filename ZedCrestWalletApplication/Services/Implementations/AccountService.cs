using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedCrestWalletApplication.DAL;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Services.Interfaces;

namespace ZedCrestWalletApplication.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private ZedCrestWalletContext _dbContext;
        public AccountService(ZedCrestWalletContext dbContext)
        {
            _dbContext = dbContext;
        }
        public CustomerAccountInformation Authenticate(string AccountNumber, string Pin)
        {
            //does account exist for this number
            var account = _dbContext.CustomerAccountInformation.Where(e => e.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (account == null)
                return null;
            //Verify pinHash
            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;
            //Authentication passed

            return account;

        }
        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");
            //now let's verify pin
            using (var hash = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computePinHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for(int i = 0; i < computePinHash.Length; i++)
                {
                    if (computePinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }
        public CustomerAccountInformation Create(CustomerAccountInformation accountInformation, string Pin, string ConfirmPin)
        {
            if (_dbContext.CustomerAccountInformation.Any(e => e.Email == accountInformation.Email))
                throw new ApplicationException("An account already exists with this email");
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match","Pin");
            //now  
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            accountInformation.PinHash = pinHash;
            accountInformation.PinSalt = pinSalt;

            _dbContext.CustomerAccountInformation.Add(accountInformation);
            _dbContext.SaveChanges();

            return accountInformation;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using(var haac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = haac.Key;
                pinHash = haac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
        public void Delete(int id)
        {
            var account = _dbContext.CustomerAccountInformation.Find(id);
            if(account != null)
            {
                _dbContext.CustomerAccountInformation.Remove(account);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<CustomerAccountInformation> GetAllAccount()
        {
            return _dbContext.CustomerAccountInformation.ToList();
        }

        public CustomerAccountInformation GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.CustomerAccountInformation.Where(i => i.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null) return null;
            return account;
        }

        public CustomerAccountInformation GetById(int Id)
        {
            var account = _dbContext.CustomerAccountInformation.Where(i => i.Id == Id).FirstOrDefault();
            if (account == null) return null;
            return account;
        }

        public void Update(CustomerAccountInformation accountInformation, string Pin = null)
        {
            //Update is happening here
            var accountToBeUpdated = _dbContext.CustomerAccountInformation.Where(i => i.Email == accountInformation.Email).SingleOrDefault();
            if (accountToBeUpdated == null) throw new ApplicationException("Account does not exist");
            if (!string.IsNullOrWhiteSpace(accountInformation.Email))
            {
                //
                if (_dbContext.CustomerAccountInformation.Any(k => k.Email == accountInformation.Email)) throw new ApplicationException("This email " + accountInformation.Email + " already exists");
                // else changes
                accountToBeUpdated.Email = accountInformation.Email;
            }
            //actually we want to allow the user to be able to change only Email and PhoneNumber and Pin
            if (!string.IsNullOrWhiteSpace(accountInformation.PhoneNumber))
            {
                //
                if (_dbContext.CustomerAccountInformation.Any(k => k.PhoneNumber == accountInformation.PhoneNumber)) throw new ApplicationException("This phoneNo " + accountInformation.PhoneNumber + "already exists");
                // else changes
                accountToBeUpdated.PhoneNumber = accountInformation.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);
                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
                accountToBeUpdated.DateLastUpdated = DateTime.Now;
            }
            //
            _dbContext.CustomerAccountInformation.Update(accountToBeUpdated);
            _dbContext.SaveChanges();
        }

    }
}
