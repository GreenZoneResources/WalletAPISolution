using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    public class RegisterAccountModel
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

    public class RegisterAccountModelValidator : AbstractValidator<RegisterAccountModel>
    {
        public RegisterAccountModelValidator()
        {
            RuleFor(t => t.FirstName).NotEmpty();
            RuleFor(t => t.LastName).NotEmpty();
            RuleFor(t => t.PhoneNumber).NotEmpty();
            RuleFor(t => t.AccountType).NotEmpty()
            .When(t => t.AccountType <= 0).WithMessage("Account type must be 1,2,3,4 range");
            RuleFor(x => x.Email)
            .EmailAddress()
            .When(e => e.Email != string.Empty)
            .WithMessage("Email not in the correct format");

            RuleFor(t => t.Pin).NotEmpty().MaximumLength(4)
                .WithMessage("Pin cannot be more than 4 digits");
            When(t => t.Pin != null, () =>
                {
                    RuleFor(t => t.Pin).Equal(t => t.ConfirmPin)
                    .WithMessage("Pin and confirm pin does not match");
                }
                );
           
        }
    }
}
