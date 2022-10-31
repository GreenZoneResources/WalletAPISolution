using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Commands
{
    public class RegisterCommand : IRequest<Response>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        // public string AccountNumberGenerated { get; set; }

        //we will also store the hash and salt of the Account Transaction pin
        //public byte[] PinHash { get; set; }
        // public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        //let's add regular expression
        public string Pin { get; set; }
        public string ConfirmPin { get; set; }
    }
}
