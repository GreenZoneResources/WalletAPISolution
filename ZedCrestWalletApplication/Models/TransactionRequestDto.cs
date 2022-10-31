using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    public class TransactionRequestDto
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class TransactionRequestDtoValidator : AbstractValidator<TransactionRequestDto>
    {
        public TransactionRequestDtoValidator()
        {
            RuleFor(t => t.TransactionAmount).NotEmpty()
                .WithMessage("Amount cannot be empty")
                .GreaterThan(0)
                .WithMessage("Amount must greater than zero");

            RuleFor(t => t.TransactionSourceAccount).NotEmpty()
                .WithMessage("Source Account cannot be empty")
                .Length(10)
                .WithMessage("Source Account must be 10 digits");

            RuleFor(t => t.TransactionDestinationAccount).NotEmpty()
                .WithMessage("Destination Account cannot be empty")
                .Length(10)
                .WithMessage("Destination Account must be 10 digits");

            RuleFor(t => t.TransactionDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Transaction Date cannot be in the past");
        }
    }
}
