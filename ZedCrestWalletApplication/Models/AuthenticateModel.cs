using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    public class AuthenticateModel
    {
        public string AccountNumber { get; set; }
        public string Pin { get; set; }
    }

    public class AuthenticateModelValidator : AbstractValidator<AuthenticateModel>
    {
        public AuthenticateModelValidator()
        {
            RuleFor(t => t.AccountNumber).NotEmpty();
            When(t => t.AccountNumber != null, () =>
             {
                 RuleFor(t => t.AccountNumber).MaximumLength(10);
             });

            RuleFor(t => t.Pin).NotEmpty();
            When(t => t.Pin != null, () =>
            {
                RuleFor(t => t.Pin).MaximumLength(4);
            });
        }
    }
}
