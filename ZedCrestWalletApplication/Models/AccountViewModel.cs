using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    public class AccountViewModel
    {
        public string RequestId => $"{Guid.NewGuid().ToString()}";
        public string AccountName { get; set; }
        public string ProductName { get; set; }
        public string Balance { get; set; }
    }
    public class AccountModelParameters
    {
        public string AccountNumber { get; set; }
        public string TransactionPin { get; set; }
    }
    public class AccountModelParameterValidator : AbstractValidator<AccountModelParameters>
    {
        public AccountModelParameterValidator()
        {
            RuleFor(t => t.AccountNumber).NotEmpty().MaximumLength(10);

            When(t => t.AccountNumber.Length != 10, () =>
            {
                RuleFor(t => t.AccountNumber)
                 .NotNull()
                 .WithMessage("Account Number cannot be empty")
                 .Must(value => value.Length == 10)
                 .WithMessage("Account Number must contain 10 digits");
            });

            RuleFor(t => t.TransactionPin)
                .NotEmpty()
                .WithMessage("Transaction Pin cannot be empty")
                .MaximumLength(10)
                .WithMessage("Transaction Pin can only accept 4 characters");
        }
    }

}
