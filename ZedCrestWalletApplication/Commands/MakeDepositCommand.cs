using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Commands
{
    public class MakeDepositCommand : IRequest<Response>
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
    public class MakeDepositModelValidator : AbstractValidator<MakeDepositCommand>
    {
        public MakeDepositModelValidator()
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

            RuleFor(t => t.Amount).NotEmpty()
               .WithMessage("Amount cannot be empty")
               .GreaterThan(0)
               .WithMessage("Amount must greater than zero");

            RuleFor(t => t.TransactionPin)
                .NotEmpty()
                .WithMessage("Transaction Pin cannot be empty")
                .MaximumLength(10)
                .WithMessage("Transaction Pin can only accept 4 characters");
        }
    }
}
