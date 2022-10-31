using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    [Table("CounterpartyTransactions")]
    public class CounterpartyTransaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public CounterpartyTransaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-","").Substring(1,17)}";
        }

    }

    public enum TranStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TransType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public class MakeDepositModel
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
    public class MakeDepositModelValidator : AbstractValidator<MakeDepositModel>
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

    //Make withdrawal model validation using Fluent Validation
    public class MakeWithdrawalModel
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
    public class MakeWithdrawalModelValidator : AbstractValidator<MakeWithdrawalModel>
    {
        public MakeWithdrawalModelValidator()
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

     //Make funds transfer model validation using Fluent Validation
    public class MakeFundsTransferModel
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
    public class MakeFundsTransferModelValidator : AbstractValidator<MakeFundsTransferModel>
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

    public class CalculateInterestModel
    {
        public double Rate { get; set; }
        public double Tenor { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
