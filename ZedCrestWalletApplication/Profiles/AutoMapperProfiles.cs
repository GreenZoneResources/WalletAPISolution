using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterAccountModel, CustomerAccountInformation>();
            CreateMap<UpdateAccountModel, CustomerAccountInformation>();
            CreateMap<CustomerAccountInformation, GetAccountModel>();
            CreateMap<TransactionRequestDto, CounterpartyTransaction>();

        }
    }
}
