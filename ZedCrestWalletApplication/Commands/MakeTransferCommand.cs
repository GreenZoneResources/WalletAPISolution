using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Commands
{
    public class MakeTransferCommand : IRequest<Response>
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }

    public class MakeFundsTransferModelValidator : AbstractValidator<MakeTransferCommand>
    {
        public MakeFundsTransferModelValidator()
        {
            RuleFor(t => t.FromAccount).NotEmpty().MaximumLength(10);

            When(t => t.FromAccount.Length != 10, () =>
            {
                RuleFor(t => t.FromAccount)
                 .NotNull()
                 .WithMessage("Account Number cannot be empty")
                 .Must(value => value.Length == 10)
                 .WithMessage("Source Account Number must contain 10 digits");
            });

            RuleFor(t => t.ToAccount).NotEmpty().MaximumLength(10);

            When(t => t.ToAccount.Length != 10, () =>
            {
                RuleFor(t => t.ToAccount)
                 .NotNull()
                 .WithMessage("Account Number cannot be empty")
                 .Must(value => value.Length == 10)
                 .WithMessage("Destination Account Number must contain 10 digits");
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
